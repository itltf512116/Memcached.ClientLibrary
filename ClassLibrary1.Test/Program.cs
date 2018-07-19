using System;
using Memcached.ClientLibrary;
using Microsoft.Extensions.Logging;

namespace ClassLibrary1.Test
{
    class Program
    {
        static NLog.ILogger log;
        static void Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            log = NLog.LogManager.GetCurrentClassLogger();
            log.Fatal("sdfdf");
            Console.WriteLine("Hello World!");
            MemcachedPoolinitialize("127.0.0.1:11211");
            var key = "key_001";
            var v = "ttt_oooo";
            SetCache(key, v);
            v = "ddd";
            log.Error(v);
            v = GetCacheByKey<string>(key);
            log.Error(v);
            Console.ReadLine();
        }


        public static T GetCacheByKey<T>(string key, bool addKeyPrefix = true)
        {
            try
            {
                var mc = new MemcachedClient();
                if (mc.KeyExists(key))
                    return (T)mc.Get(key);
                return default(T);
            }
            catch (Exception ex)
            {
                log.Fatal($"GetCacheByKey error,key:{key} ex:{ex}" );
                return default(T);
            }
        }

        public static void SetCache<T>(string key, T value, bool addKeyPrefix = true)
        {
            try
            {
                MemcachedClient mc = new MemcachedClient();
                mc.EnableCompression = false;
                if (value != null)
                    mc.Set(key, value);
            }
            catch (Exception ex)
            {
                log.Fatal($"SetCache error,key:{key} ex:{ex}");
            }
        }



        public static void MemcachedPoolinitialize(string servers)
        {
            char[] separator = { ',' };
            string[] serverlist = servers.Split(separator);

            // initialize the pool for memcache servers  
            try
            {
                SockIOPool pool = SockIOPool.GetInstance();
                if (pool != null)
                {
                    pool.SetServers(serverlist);

                    pool.InitConnections = 3;
                    pool.MinConnections = 3;
                    pool.MaxConnections = 50;

                    pool.SocketConnectTimeout = 1000;
                    pool.SocketTimeout = 3000;

                    pool.MaintenanceSleep = 30;
                    pool.Failover = true;
                    pool.Nagle = false;
                    pool.Initialize();
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex,"MemcachedPoolinitialize error");
            }
        }

    }



    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogDebug(20, "Doing hard work! {Action}", name);
        }


    }
}
