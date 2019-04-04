using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace Demos.Demos2019.Http
{
    public class WebApiClientHelper
    {
        /// <summary>
        /// 完整URL: "https://localhost:44300/api/Product/AddProduct
        /// baseUrl: "https://localhost:44300"
        /// url:/api/Product/AddProduct
        /// </summary>
        private string _baseUrl;

        public WebApiClientHelper()
        {
        }

        /// <summary>
        /// baseUrl: "https://localhost:44300"
        /// </summary>
        /// <param name="baseUrl"></param>
        public WebApiClientHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }


        //public string GetByUrl(string url, ICredentials credentials)
        /// <summary>
        /// http get
        /// </summary>
        /// <param name="url">/api/Product/AddProduct</param>
        /// <returns>返回的JSON字符串</returns>
        public string GetByUrl(string url)
        {
            string result = string.Empty;
            //Microsoft.AspNet.WebApi.Client
            //var handler = new HttpClientHandler { Credentials = credentials };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">https://localhost:44300/api/Product?ID=50</param>
        /// <returns></returns>
        public string GetByWholeUrlAsync(string url)
        {
            string result = string.Empty;
            //Microsoft.AspNet.WebApi.Client
            //var handler = new HttpClientHandler { Credentials = credentials };
            HttpClient client = new HttpClient();
          //  client.BaseAddress = new Uri(_baseUrl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">/api/Product/AddProduct</param>
        /// <param name="model">参数</param>
        /// <returns></returns>
        public string Put<T>(string url, T model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, data);
            response.Wait();
            var res = response.Result.Content.ReadAsStringAsync();
            res.Wait();
            return res.Result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="url">请求的URL（/api/Product/AddProduct）</param>
        /// <param name="model">提交的参数</param>
        /// <returns></returns>
        public string Post<T>(string url, T model)
        {
            string result = string.Empty;
            var handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8);
            var response = client.PostAsync(url, data).Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }

 

        ////old  废弃
        //public string PostT<T>(string url, T model)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(_baseUrl);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        //    var response = client.PostAsync(url, data);
        //    response.Wait();
        //    var res = response.Result.Content.ReadAsStringAsync();
        //    res.Wait();
        //    return res.Result;
        //}
    }
}
