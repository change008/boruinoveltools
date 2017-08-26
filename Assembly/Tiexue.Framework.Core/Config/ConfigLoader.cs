using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Threading;
using System.Configuration;
using System.Xml.Serialization;
using ServiceStack.Text;

namespace Tiexue.Framework.Config
{
    public class ConfigLoader
    {
        private ConfigLoader() { }

        public static T GetConfigSection<T>(string sectionName) where T : class
        {
            if (string.IsNullOrEmpty(sectionName)) return null;
            return System.Configuration.ConfigurationManager.GetSection(sectionName) as T;
        }


        public static string GetAppSetting(string fullpathFileName, string key, bool cacheConfig = true, FileSystemEventHandler onConfigChangedEventHandler = null)
        {
            if (string.IsNullOrEmpty(fullpathFileName))
            {
                throw new FileNotFoundException("Cannot Load Config:", fullpathFileName);
            }

            if (!cacheConfig)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }

            string hashcode = (fullpathFileName + key).GetHashCode().ToString();
            string configObj = HttpRuntime.Cache[hashcode] as string;
            if (configObj == null)
            {
                configObj = ConfigurationManager.AppSettings[key].ToString();

                HttpRuntime.Cache.Insert(hashcode, configObj, new System.Web.Caching.CacheDependency(fullpathFileName), DateTime.MaxValue, TimeSpan.Zero);
                if (onConfigChangedEventHandler != null)
                {
                    new LazyFileSystemWatcher(fullpathFileName, onConfigChangedEventHandler);
                }

            }
            return configObj;
        }

        public static T LoadXmlConfig<T>(string fullpathFileName, bool cacheConfig = true, FileSystemEventHandler onConfigChangedEventHandler = null) where T : class
        {
            return LoadConfigFromFile<T>(fullpathFileName, LoadFromXml<T>, cacheConfig, onConfigChangedEventHandler);
        }


        public static T LoadJsonConfig<T>(string fullpathFileName, bool cacheConfig = true, FileSystemEventHandler onConfigChangedEventHandler = null) where T : class
        {
            return LoadConfigFromFile<T>(fullpathFileName, LoadFromJson<T>, cacheConfig, onConfigChangedEventHandler);
        }


        private static T LoadConfigFromFile<T>(string fullpathFileName, Func<string, T> LoadFrom, bool cacheConfig = true, FileSystemEventHandler onConfigChangedEventHandler = null) where T : class
        {
            if (string.IsNullOrEmpty(fullpathFileName))
            {
                throw new FileNotFoundException("Cannot Load Config:", fullpathFileName);
            }
            if (!cacheConfig)
            {
                return LoadFrom(fullpathFileName);
            }
            string hashcode = fullpathFileName.GetHashCode().ToString();
            T configObj = HttpRuntime.Cache[hashcode] as T;
            if (configObj == null)
            {
                configObj = LoadFrom(fullpathFileName);
                HttpRuntime.Cache.Insert(hashcode, configObj, new System.Web.Caching.CacheDependency(fullpathFileName), DateTime.MaxValue, TimeSpan.Zero);
                if (onConfigChangedEventHandler != null)
                {
                    new LazyFileSystemWatcher(fullpathFileName, onConfigChangedEventHandler);
                }

            }
            return configObj;
        }

        private static T LoadFromJson<T>(string fileName) where T : class
        {
            return LoadFromFileInternal<T>(fileName, (fs) =>
            {
                return JsonSerializer.DeserializeFromStream<T>(fs);
            });
        }

        private static T LoadFromXml<T>(string fileName) where T : class
        {
            return LoadFromFileInternal<T>(fileName, (fs) =>
                   {
                       System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                       return (T)serializer.Deserialize(fs);
                   });
        }

        private static T LoadFromFileInternal<T>(string fileName, Func<FileStream, T> Fs2TFunc) where T : class
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return Fs2TFunc(fs);
            }
            catch (Exception ex)
            {
                throw new FormatException(string.Format(" Load XML Config File Fail:{0}", fileName), ex);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        class LazyFileSystemWatcher
        {
            private readonly Timer m_Timer;
            private readonly Int32 m_TimerInterval;
            private readonly FileSystemWatcher m_FileSystemWatcher;
            private readonly FileSystemEventHandler m_FileSystemEventHandler;
            private readonly Dictionary<String, FileSystemEventArgs> m_ChangedFiles = new Dictionary<string, FileSystemEventArgs>();


            /// <summary>
            /// Sets up a file system watcher to watch for changes to the config file.
            /// </summary>
            public LazyFileSystemWatcher(string filename, FileSystemEventHandler OnConfigChanged)
            {
                FileInfo configFile = new FileInfo(filename);
                try
                {
                    m_Timer = new Timer(OnTimer, null, Timeout.Infinite, Timeout.Infinite);
                    m_FileSystemWatcher = new FileSystemWatcher(configFile.DirectoryName, configFile.Name);
                    m_FileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    m_FileSystemWatcher.Changed += fileSystemWatcher_Changed;
                    m_FileSystemWatcher.EnableRaisingEvents = true;
                    m_FileSystemEventHandler = OnConfigChanged;
                    m_TimerInterval = 1500;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("An error occurred while attempting to watch for file system changes.", ex);
                }
            }



            public void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
            {
                lock (m_ChangedFiles)
                {
                    if (!m_ChangedFiles.ContainsKey(e.Name))
                    {
                        m_ChangedFiles.Add(e.Name, e);
                    }
                }
                m_Timer.Change(m_TimerInterval, Timeout.Infinite);
            }

            private void OnTimer(object state)
            {
                Dictionary<String, FileSystemEventArgs> tempChangedFiles = new Dictionary<String, FileSystemEventArgs>();

                lock (m_ChangedFiles)
                {
                    foreach (KeyValuePair<string, FileSystemEventArgs> changedFile in m_ChangedFiles)
                    {
                        tempChangedFiles.Add(changedFile.Key, changedFile.Value);
                    }
                    m_ChangedFiles.Clear();
                }

                foreach (KeyValuePair<string, FileSystemEventArgs> changedFile in tempChangedFiles)
                {
                    m_FileSystemEventHandler(this, changedFile.Value);
                }
            }
        }
    }
}
