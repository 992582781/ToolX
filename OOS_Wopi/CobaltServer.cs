// Copyright 2014 The Authors Marx-Yu. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Configuration;

namespace OOS_Wopi
{
    public class CobaltServer
    {
        private HttpListener m_listener;
        private string m_docsPath = ConfigurationManager.AppSettings["LocalStoragePath"].ToString();
        private string m_host;
        private int m_port;

        public CobaltServer(string host, int port = 8080)
        {
            m_host = host;
            m_port = port;
        }

        public void Start()
        {
            m_listener = new HttpListener();
            // localhost may change to the real hostname or IP
            m_listener.Prefixes.Add(String.Format("http://{0}:{1}/wopi/", m_host, m_port));
            m_listener.Start();
            m_listener.BeginGetContext(ProcessRequest, m_listener);
            Console.WriteLine(@"WopiServer Started");
        }

        public void Stop()
        {
            m_listener.Stop();
        }

        private void ErrorResponse(HttpListenerContext context, string errmsg)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(errmsg);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = @"application/json";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Close();
        }

        private void ProcessRequest(IAsyncResult result)
        {
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);
                try
                {
                    Console.WriteLine(context.Request.HttpMethod + @" " + context.Request.Url.AbsolutePath);
                    //如果编辑的文件路径和文件有中文 请使用2次编码：解码也用两次解码
                    var AbsolutePath = HttpUtility.UrlDecode(HttpUtility.UrlDecode(context.Request.Url.AbsolutePath));

                    var stringarr = AbsolutePath.Split('/');
                    var access_token = context.Request.QueryString["access_token"];

                    if (stringarr.Length < 3)
                    {
                        Console.WriteLine(@"Invalid request");

                        ErrorResponse(context, @"Invalid request parameter");
                        m_listener.BeginGetContext(ProcessRequest, m_listener);
                        return;
                    }
                    Console.WriteLine(@"EditSession0000");
                    //var filename = HttpUtility.UrlDecode(stringarr[3]);
                    var filename = string.Empty;

                    for (int i = m_docsPath.Split('\\').Length - 1; i < stringarr.Length; i++)
                    {
                        filename = filename + stringarr[i] + "/";
                    }
                    filename = filename.TrimEnd('/');
                    if (filename.Contains("contents"))
                        filename = filename.Replace("contents", "").TrimEnd('/');
                    Console.WriteLine(@"EditSession1111");
                    Console.WriteLine(@"filename:" + filename);
                    //use filename as session id just test, recommend use file id and lock id as session id
                    EditSession editSession = EditSessionManager.Instance.GetSession(filename);
                    if (editSession == null)
                    {
                        var fileExt = filename.Substring(filename.LastIndexOf('.') + 1);
                        editSession = new FileSession(filename, m_docsPath + "/" + filename, @"marx.yu", @"marx yu", @"marx.yuf@gmail.com", false);

                        EditSessionManager.Instance.AddSession(editSession);
                    }

                    if (stringarr.Length == 4 + (filename.Length - filename.Replace("/", "").Length)
                        && context.Request.HttpMethod.Equals(@"GET"))
                    {
                        //request of checkfileinfo, will be called first
                        var memoryStream = new MemoryStream();
                        var json = new DataContractJsonSerializer(typeof(WopiCheckFileInfo));
                        json.WriteObject(memoryStream, editSession.GetCheckFileInfo());
                        memoryStream.Flush();
                        memoryStream.Position = 0;
                        StreamReader streamReader = new StreamReader(memoryStream);
                        var jsonResponse = Encoding.UTF8.GetBytes(streamReader.ReadToEnd());

                        context.Response.ContentType = @"application/json";
                        context.Response.ContentLength64 = jsonResponse.Length;
                        context.Response.OutputStream.Write(jsonResponse, 0, jsonResponse.Length);
                        context.Response.Close();
                    }
                    else if (stringarr.Length == 5 + +(filename.Length - filename.Replace("/", "").Length)
                        && stringarr[4 + (filename.Length - filename.Replace("/", "").Length)].Equals(@"contents"))
                    {
                        // get and put file's content
                        if (context.Request.HttpMethod.Equals(@"POST"))
                        {
                            var ms = new MemoryStream();
                            context.Request.InputStream.CopyTo(ms);
                            editSession.Save(ms.ToArray());
                            context.Response.ContentLength64 = 0;
                            context.Response.ContentType = @"text/html";
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        }
                        else
                        {
                            var content = editSession.GetFileContent();
                            context.Response.ContentType = @"application/octet-stream";
                            context.Response.ContentLength64 = content.Length;
                            context.Response.OutputStream.Write(content, 0, content.Length);
                        }
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod.Equals(@"POST") &&
                        (context.Request.Headers["X-WOPI-Override"].Equals("LOCK") ||
                        context.Request.Headers["X-WOPI-Override"].Equals("UNLOCK") ||
                        context.Request.Headers["X-WOPI-Override"].Equals("REFRESH_LOCK"))
                        )
                    {
                        //lock, 
                        Console.WriteLine("request lock: " + context.Request.Headers["X-WOPI-Override"]);
                        context.Response.ContentLength64 = 0;
                        context.Response.ContentType = @"text/html";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.Close();
                    }
                    else
                    {
                        Console.WriteLine(@"Invalid request parameters");
                        ErrorResponse(context, @"Invalid request cobalt parameter");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"process request exception:" + ex.Message);
                }
                m_listener.BeginGetContext(ProcessRequest, m_listener);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"get request context:" + ex.Message);
                return;
            }
        }
    }
}
