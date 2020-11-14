using Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallEr
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            OpreatQueue.queue.ProcessItemFunction += OpreatQueue.A;
            OpreatQueue.queue.ProcessException += OpreatQueue.C; //new EventHandler<EventArgs<Exception>>(C);
            Application.Run(new Form1());
        }
    }
}
