using System;
using System.Collections.Generic;

namespace Tiexue.Framework.Cache {
    public interface ICacheClient : IDisposable {
        void Initialize();
        bool Initialized { get; }

        bool Add(string key, object data);
        bool Add(string key, object data, DateTime expiration);
        bool Add(string key, object data, TimeSpan validFor);

        bool Set(string key, object data);
        bool Set(string key, object data, DateTime expiration);
        bool Set(string key, object data, TimeSpan validFor);

        object Get(string key);
        IDictionary<string, object> Get(IEnumerable<string> keys);
        T Get<T>(string key);

        bool Remove(string key);
        bool RemoveAll();
    }
}
