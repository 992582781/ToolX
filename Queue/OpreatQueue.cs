using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    public static class OpreatQueue
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static MyAsyncQueue<ComInfo> queue = new MyAsyncQueue<ComInfo>();
        public static void A(ComInfo info)
        {
            Thread.Sleep(1);
            logger.Info(info.ComId + "----" + info.InfoText + "\r\n");
        }

        public static void C(object ex, EventArgs<Exception> args)
        {
            logger.Info("异常" + ex.ToString() + "----" + args.Argument.ToString() + "\r\n");
        }
    }

    public class ComInfo
    {
        public int ComId { get; set; }
        public string InfoText { get; set; }
    }
}
