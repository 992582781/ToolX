// Copyright 2014 The Authors Marx-Yu. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OOS_Wopi
{
    class Program
    {
        static void Main()
        {
            // docsPath parameter may change to the real local path that save demo documents(word or excel file)

            //m_docsPath = ConfigurationManager.AppSettings["LocalStoragePath"].ToString();

            //如果编辑的文件路径和文件有中文 请使用2次编码：解码也用两次解码；千万注意路径或者url中的 / 这个符号不能转码

            var url1 = HttpUtility.UrlEncode(HttpUtility.UrlEncode("副本"));//文件夹
            var url = HttpUtility.UrlEncode(HttpUtility.UrlEncode("test副本.docx"));//文件名称


            string m_host = ConfigurationManager.AppSettings["host"].ToString();
            string m_port = ConfigurationManager.AppSettings["post"].ToString();
            CobaltServer svr = new CobaltServer(m_host, Convert.ToInt32(m_port));
            svr.Start();
            Console.WriteLine("A simple wopi webserver. Press any key to quit.");
            Console.ReadKey();

            svr.Stop();
        }
    }
}
