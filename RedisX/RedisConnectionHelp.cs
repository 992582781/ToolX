using ConfigX;
using NLog;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisX
{
    /// ConnectionMultiplexer对象管理帮助类
    /// </summary>
    public static class RedisConnectionHelp
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string[] RedisConnectionString = ConfigUtil.GetConnectionString("RedisConnectionString").Split(',');

        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;
        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();



        public static ConfigurationOptions ConfigOptions
        {
            get
            {
                ConfigurationOptions config = new ConfigurationOptions()
                {
                    //是一个列表，一个复杂的的场景中可能包含有主从复制 ， 对于这种情况，只需要指定所有地址在连接字符串中
                    //（它将会自动识别出主服务器 set值得时候用的主服务器）假设这里找到了两台主服务器，将会对两台服务进行裁决选出一台作为主服务器
                    //来解决这个问题 ， 这种情况是非常罕见的 ，我们也应该避免这种情况的发生。
                    //
                    AbortOnConnectFail = false,
                    AllowAdmin = true,
                    ConnectTimeout = 150000,
                    SyncTimeout = 150000,
                    ResponseTimeout = 150000
                };

                foreach (var EndPoint in RedisConnectionString)
                {
                    config.EndPoints.Add(EndPoint.ToString().Split(':')[0], Convert.ToInt16(EndPoint.ToString().Split(':')[1]));
                }

                ////服务器秘密
                //config.Password = "123456";
                ////客户端名字
                //config.ClientName = "127.0.0.1";
                ////服务器名字
                //config.ServiceName = "127.0.0.1";
                ////true表示管理员身份，可以用一些危险的指令。
                //config.AllowAdmin = true;
                return config;
            }
        }



        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            _instance = GetManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            var connect = ConnectionMultiplexer.Connect(ConfigOptions);
            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;


            return connect;
        }

        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            logger.Error("RedisConnectionHelp Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            logger.Error("RedisConnectionHelp ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            logger.Error("RedisConnectionHelp ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            logger.Error("RedisConnectionHelp 重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            logger.Error("RedisConnectionHelp HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            logger.Error("RedisConnectionHelp InternalError:Message" + e.Exception.Message);
        }

        #endregion 事件
    }
}