using CallBackX;
using CallEr.Extension;
using Extension;
using LinqX;
using Queue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallEr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void EnumExtension_Click(object sender, EventArgs e)
        {
            ResultMsg resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)StatusCodeEnum.Error;
            resultMsg.Info = StatusCodeEnum.Error.GetEnumText();
            resultMsg.total = 0;
            resultMsg.rows = "";

            resultMsg = new ResultMsg();
            resultMsg.StatusCode = (int)Control_Type.UrlText;
            resultMsg.Info = Control_Type.UrlText.GetEnumDescriptio();
            resultMsg.total = 0;
            resultMsg.rows = "";
        }

        private void LinqEx_Click(object sender, EventArgs e)
        {

            List<Student> list = new List<Student>()
            {
                new Student() { ID = 1, Name = "A" },
                new Student() { ID = 2, Name = "B" },
                new Student() { ID = 1, Name = "A" },
                new Student() { ID = 4, Name = "D" }
            };

            //使用方法如下（针对ID，和Name进行Distinct）：
            var query = list.DistinctBy(p => new { p.ID, p.Name });
            //若仅仅针对ID进行distinct：
             query = list.DistinctBy(p => p.ID);
        }

        private async void async_Click(object sender, EventArgs e)
        {
            Task<int> downloading = DownloadDocsMainPageAsync();
            MessageBox.Show($" Launched downloading.");

            int bytesLoaded = await downloading;//等待结果  
            MessageBox.Show(bytesLoaded + " Downloaded  bytes.");
        }


        private static async Task<int> DownloadDocsMainPageAsync()
        {
            MessageBox.Show($"{nameof(DownloadDocsMainPageAsync)}: About to start downloading.");

            var client = new HttpClient();
            byte[] content = await client.GetByteArrayAsync("https://blog.csdn.net/");

            MessageBox.Show($"{nameof(DownloadDocsMainPageAsync)}: Finished downloading.");
            return content.Length;
        }

        private void Queue_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 50000; i++)
            {
                Task.Factory.StartNew((param) =>
                {
                    ComInfo info = new ComInfo();
                    info.ComId = Guid.NewGuid().GetHashCode();
                    info.InfoText = param.ToString();
                    OpreatQueue.queue.Enqueue(info);
                }, i);
            }
        }

        private void Callback_Click(object sender, EventArgs e)
        {
            CallBack.PostAsync(1,2);
            new CallBack1().PostAsync(1, 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] str = { "1", "2" };

            int[] intlist = Array.ConvertAll(str, int.Parse);
        }
    }


    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}
