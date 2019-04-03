using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace WinformFormUpdate
{
    public class FtpHelper
    {

        /// <summary>
        /// ftp 上传
        /// </summary>
        /// <param name="filePath">"D:\\Test\\file"</param>
        /// <param name="filename">"计算机网络（第7版）-谢希仁.pdf"</param>
        /// <param name="ftpServerIP">192.168.1.105:21（ftp默认端口21，可不写）</param>
        /// <param name="ftpUserID">administrator</param>
        /// <param name="ftpPassword">系统密码</param>
        /// <param name="progress">显示进度回调</param>
        /// <returns></returns>
        public static int UploadFtp(string filePath, string filename, string ftpServerIP, string ftpUserID, string ftpPassword, Action<int> progress)
        {
            FileInfo fileInf = new FileInfo(filePath + "\\" + filename);
            string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
            FtpWebRequest reqFTP;
            // Create FtpWebRequest object from the Uri provided 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
            try
            {
                // Provide the WebPermission Credintials 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // By default KeepAlive is true, where the control connection is not closed 
                // after a command is executed. 
                reqFTP.KeepAlive = false;

                // Specify the command to be executed. 
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type. 
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file 
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                long readLength = 0;
                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
                //FileStream fs = fileInf.OpenRead(); 
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Stream to which the file to be upload is written 
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time 
                contentLen = fs.Read(buff, 0, buffLength);
                readLength += contentLen;

                // Till Stream content ends 
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream 
                    strm.Write(buff, 0, contentLen);
                    CalculateProcess(fileInf.Length, readLength, progress);
                    contentLen = fs.Read(buff, 0, buffLength);
                    readLength += contentLen;
                }

                // Close the file stream and the Request Stream 
                strm.Close();
                fs.Close();
                return 0;
            }
            catch (Exception ex)
            {
                reqFTP.Abort();
                //  Logging.WriteError(ex.Message + ex.StackTrace);
                return -2;
            }
        }

        static void CalculateProcess(long totalLength, long readLength, Action<int> process)
        {
            int percent = (int)(((double)readLength / (double)totalLength) * 100);
            process?.Invoke(percent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件保存路径</param>
        /// <param name="fileName">文件要保存文件名</param>
        /// <param name="ftpFileName">ftp服务器上文件名</param>
        /// <param name="ftpServerIP">192.168.1.105:21（ftp默认端口21，可不写）</param>
        /// <param name="ftpUserID">administrator</param>
        /// <param name="ftpPassword">系统密码</param>
        /// <param name="progress">显示进度回调</param>
        /// <returns></returns>
        public static int DownloadFtp(string filePath, string fileName, string ftpFileName, string ftpServerIP, string ftpUserID, string ftpPassword, Action<int> progress)
        {
            FtpWebRequest reqFTP;
            try
            {
                long fileSize = GetFileSize(ftpFileName, ftpServerIP, ftpUserID, ftpPassword);
                  //filePath = < <The full path where the file is to be created.>>, 
                  //fileName = < <Name of the file to be created(Need not be the name of the file on FTP server).>> 
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ftpFileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                long readLength = 0;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                readLength += readCount;
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    CalculateProcess(fileSize, readLength, progress);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    readLength += readCount;
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return 0;
            }
            catch (Exception ex)
            {
                // Logging.WriteError(ex.Message + ex.StackTrace);
                // System.Windows.Forms.MessageBox.Show(ex.Message);
                return -2;
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">ip服务器下的相对路径</param>
        /// <returns>文件大小</returns>
        public static long GetFileSize(string ftpFileName, string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ftpFileName));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                long dataLength =request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">服务器下的相对路径 包括文件名</param>
        public static void DeleteFileName(string path,string ftpip,string username, string password, string fileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(ftpip + "" + fileName);
                string uri = path + fileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除文件出错：" + ex.Message);
            }
        }

        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;
        /// <summary>
        /// 判断本机是否联网
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectNetwork()
        {
            try
            {
                int dwFlag = 0;

                //false表示没有连接到任何网络,true表示已连接到网络上
                if (!InternetGetConnectedState(ref dwFlag, 0))
                {

                    //if (!InternetGetConnectedState(ref dwFlag, 0))
                    //     Console.WriteLine("未连网!");
                    //else if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                    //    Console.WriteLine("采用调治解调器上网。");
                    //else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                    //    Console.WriteLine("采用网卡上网。"); 
                    return false;
                }

                //判断当前网络是否可用
                IPAddress[] addresslist = Dns.GetHostAddresses("www.baidu.com");
                if (addresslist[0].ToString().Length <= 6)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}