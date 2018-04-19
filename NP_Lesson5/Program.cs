using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NP_Lesson5
{
    class Program
    {
        static void Main(string[] args)
        {
            var ftpClient = new FtpClient();
            ftpClient.Url = "ftp://ftp.archive.ubuntu.com/ubuntu/project/ubuntu-archive-keyring.gpg";

            //Console.WriteLine(ftpClient.GetDir());
            //foreach(string str in ftpClient.GetDirectoryList())
            //{
            //    Console.WriteLine(str);
            //}

            ftpClient.DownloadFile(@"c:\tmp\ubuntu-archive-keyring.gpg");
            Console.ReadLine();
        }
    }

    public class FtpClient
    {
        public string Url { get; set; }

        public FtpWebRequest InitRequest(string method)
        {
            var rsit = (FtpWebRequest)WebRequest.Create(this.Url);
            rsit.Method = method;
            rsit.KeepAlive = true;
            rsit.UsePassive = true;
            rsit.UseBinary = true;
            return rsit;
        }
        public IEnumerable<string> GetDirectoryList()
        {
            var req = InitRequest(WebRequestMethods.Ftp.ListDirectoryDetails);
            var resp = (FtpWebResponse)req.GetResponse();
            var sr = new StreamReader(resp.GetResponseStream());
            while (sr.Peek() >= 0)
            {
                yield return sr.ReadLine();
            }
            sr.Close();
            resp.Close();
        }

        public string GetDir()
        {
            var req = InitRequest(WebRequestMethods.Ftp.PrintWorkingDirectory);
            var resp = (FtpWebResponse)req.GetResponse();
            var sr = new StreamReader(resp.GetResponseStream());

            return sr.ReadToEnd();
        }
        public void DownloadFile(string path)
        {
            var req = InitRequest(WebRequestMethods.Ftp.DownloadFile);
            var resp = (FtpWebResponse)req.GetResponse();
            var sr = resp.GetResponseStream();
            var ms = new FileStream(path, FileMode.CreateNew);
            sr.CopyTo(ms);
            ms.Flush();
            ms.Close();
        }
    }
}
