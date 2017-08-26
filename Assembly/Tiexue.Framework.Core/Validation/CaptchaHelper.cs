using System;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.ServiceLocation;
using Tiexue.Framework.Cache;
using Tiexue.Framework.Web;
using log4net;

namespace Tiexue.Framework.Validation {
    public static class CaptchaHelper {

        private static readonly ILog _log = LogManager.GetLogger(typeof(CaptchaHelper));

        private static ICacheClient GetCacheClient() {
            var client = ServiceLocator.Current.GetInstance<ICacheClient>();
            if(client == null)
                _log.WarnFormat("Not found an instance of type ICacheClient");
            return client;
        }

        public static void SetCache(CaptchaImage image, DateTime? expires) {
            if(image == null || String.IsNullOrWhiteSpace(image.UniqueId)) return;

            var cache = GetCacheClient();
            var exp = expires ?? DateTime.Now.AddSeconds(CaptchaImage.CacheTimeOut);
            if (cache != null && cache.Set(image.UniqueId, image, exp)) return;

            _log.WarnFormat("Failed to set cache for CaptchaImage [key: {0}] via cache client", image.UniqueId);
            HttpRuntime.Cache.Add(image.UniqueId, image, null, exp, System.Web.Caching.Cache.NoSlidingExpiration,
                                  CacheItemPriority.NotRemovable, null);
        }

        public static CaptchaImage GetCaptchaImageFromCache(string guid) {
            if(String.IsNullOrWhiteSpace(guid)) return null;
            var cache = GetCacheClient();
            if(cache != null) {
                var image = cache.Get<CaptchaImage>(guid);
                if(image != null) return image;
                _log.WarnFormat("CaptchaImage [key: {0}] not found via cache client.", guid);
            }
            return CaptchaImage.GetCachedCaptcha(guid);
        }

        public static void RemoveCachedCaptchaImage(string guid) {
            if (String.IsNullOrWhiteSpace(guid)) return;
            var cache = GetCacheClient();
            if (cache != null && cache.Remove(guid)) return;
            _log.WarnFormat("Failed to remove cached CaptchaImage [key: {0}] via cache client", guid);
            HttpRuntime.Cache.Remove(guid);
        }

        public static bool Validate(string guid, string text) {
            if (String.IsNullOrWhiteSpace(guid)) return false;

            var image = GetCaptchaImageFromCache(guid);
            var actualValue = text;
            var expectedValue = image == null ? String.Empty : image.Text;
            RemoveCachedCaptchaImage(guid);

            return !String.IsNullOrEmpty(actualValue) &&
                   !String.IsNullOrEmpty(expectedValue) &&
                   String.Equals(actualValue, expectedValue, StringComparison.OrdinalIgnoreCase);
        }

        public static string CreateCaptchaImage(int width, int height) {
            var image = new CaptchaImage { Width = width, Height = height };
            SetCache(image, DateTime.UtcNow.AddSeconds(CaptchaImage.CacheTimeOut + 86400));
            return image.UniqueId;
        }
    }
}
