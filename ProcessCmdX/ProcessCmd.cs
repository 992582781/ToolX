using System;
using System.Diagnostics;

namespace ProcessCmdX
{
    public class ProcessCmd
    {
        public static bool Connect(string Path, string UserName, string PassWord)
        {
            bool flag = false;
            Process process = new Process();
            try
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                string dosLine = @"net use " + "" + Path + " " + "/user:" + UserName + " " + PassWord;
                // MessageBox.Show(dosLine);
                process.StandardInput.WriteLine(dosLine);
                process.StandardInput.WriteLine("exit");
                while (!process.HasExited)
                {
                    process.WaitForExit(1000);
                }
                string errormsg = process.StandardError.ReadToEnd();
                process.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    flag = true;
                }
                else
                {
                    throw new Exception(errormsg);

                }
            ;
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
            finally
            {
                process.Close();
                process.Dispose();
            }

            return flag;
        }
    }
}
