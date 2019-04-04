using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Http
{
    public class Http
    {
        public event Action<bool, string> OnAsyncResponseArrived ;

        public bool Get(string url, out string data, string encode = "utf-8")
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            Stream stream = null;
            StreamReader reader = null;
            data = string.Empty;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Timeout = 10000;
                resp = (HttpWebResponse)req.GetResponse();
                stream = resp.GetResponseStream();
                reader = new StreamReader(stream, Encoding.GetEncoding(encode));
                data = reader.ReadToEnd();
                return true;
            }
            catch (Exception ex)
            {
                string error = string.Concat(
                    "request error!\r\n",
                    "msg:", ex.Message, "\r\n",
                    "encode:", encode, "\r\n",
                    "url:", url
                    );
                data = error;
                return false;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (resp != null) resp.Close();
            }
        }

        public void GetAsync(string url, string encode = "utf-8")
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;

            req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = 10000;
            AsyncCallback callback = new AsyncCallback((ar) =>
            {
                Stream stream = null;
                StreamReader reader = null;
                try
                {
                    resp = (HttpWebResponse)req.EndGetResponse(ar);
                    stream = resp.GetResponseStream();
                    reader = new StreamReader(stream, Encoding.GetEncoding(encode));

                    bool isSuccess = true;
                    string data = reader.ReadToEnd();
                    this.OnAsyncResponseArrived(isSuccess, data);
                }
                catch (Exception ex)
                {
                    bool isSuccess = false;
                    string error = string.Concat(
                    "request error!\r\n",
                    "msg:", ex.Message, "\r\n",
                    "encode:", encode, "\r\n",
                    "url:", url
                    );

                    this.OnAsyncResponseArrived(isSuccess, error);
                }
                finally
                {
                    if (reader != null) reader.Dispose();
                    if (stream != null) stream.Dispose();
                    if (resp != null) resp.Close();
                }
            });
            req.BeginGetResponse(callback, req);
        }

        public async  Task<(bool Success,string Data)>  GetAsyncData(string url, string encode = "utf-8")
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;

            req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = 10000;

            resp = (HttpWebResponse) await req.GetResponseAsync();

            Stream stream = null;
            StreamReader reader = null;
            try
            {
                
                stream = resp.GetResponseStream();
                reader = new StreamReader(stream, Encoding.GetEncoding(encode));

                bool isSuccess = true;
                string data = reader.ReadToEnd();
                // this.OnAsyncResponseArrived(isSuccess, data);
                return (isSuccess, data);
            }
            catch (Exception ex)
            {
                bool isSuccess = false;
                string error = string.Concat(
                "request error!\r\n",
                "msg:", ex.Message, "\r\n",
                "encode:", encode, "\r\n",
                "url:", url
                );

                //this.OnAsyncResponseArrived(isSuccess, error);
                return (isSuccess, error);
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (stream != null) stream.Dispose();
                if (resp != null) resp.Close();
            }
        }

        /// <summary>
        /// 返回JSon数据
        /// </summary>
        /// <param name="JSONData">要处理的JSON数据</param>
        /// <param name="Url">要提交的URL</param>
        /// <returns>返回的JSON处理字符串</returns>
        public string GetResponseData(string JSONData, string Url)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.KeepAlive = false;  //设置不建立持久性连接连接
                request.ProtocolVersion = HttpVersion.Version10; //http的版本有2个,一个是1.0,一个是1.1 具体更具实际情况测试替换
               

                request.Method = "POST";
                request.ContentLength = bytes.Length;
                //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据  
                request.ServicePoint.Expect100Continue = false;  
                //request.ContentType = "text/xml";
                request.ContentType = "application/json";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                //声明一个HttpWebRequest请求
                request.Timeout = 90000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                string strResult = streamReader.ReadToEnd();

                request.Abort();
                streamReceive.Dispose();
                streamReader.Dispose();
                response.Close();
                reqstream.Close();
                reqstream.Dispose();

                return strResult;
            }
            catch(Exception ex)
            {
              //  LogManagement.Log("Http", ex.ToString());
                return null;
              
            }
        }

    }
}

