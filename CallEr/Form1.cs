using CallEr.Extension;
using Extension;
using LinqX;
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
    }


    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}
