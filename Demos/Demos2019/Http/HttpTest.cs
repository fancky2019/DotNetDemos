using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Http
{
    class HttpTest
    {
        public async void Test()
        {
            //先启动WebApi
            string url = "https://localhost:44300/api/Product?ID=50";
            var result = await new Http().GetAsyncData(url);


            var result1 = new WebApiClientHelper("https://localhost:44300/").GetByUrl("api/Product?ID=50");
            var result11 = new WebApiClientHelper().GetByWholeUrlAsync("https://localhost:44300/api/Product?ID=50");
        }
    }
}
