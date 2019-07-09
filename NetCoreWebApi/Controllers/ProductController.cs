using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreWebApi.Controllers
{
    /*
     * 访问Swagger
     * https://localhost:8464/swagger
     */
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private WMSDbContext _WMSDbContext;
        public ProductController(WMSDbContext WMSDbContext)
        {
            this._WMSDbContext = WMSDbContext;
        }

        //一个Controller只能有一个默认的Get，URL不用写Action名字
        //http://localhost:8464/api/Product?id=50
        /// <summary>
        /// 一个Controller只能有一个默认的Get，URL不用写Action名字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] Product model)
        {
            var product = this._WMSDbContext.Product.Where(p => p.ID == model.ID).FirstOrDefault();
            return Json(product);
        }

        //http://localhost:8464/api/Product/GetProductList?id=50
        [HttpGet("GetProductList")]
        public IActionResult GetProductList([FromQuery] Product model)
        {
            var product = this._WMSDbContext.Product.Where(p => p.ID == model.ID).FirstOrDefault();
            return Json(product);
        }

        //http://localhost:8464/api/Product/GetProductListByProQuery?ProductName=fancky1&price=13
        [HttpGet("GetProductListByProcedure")]
        public IActionResult GetProductListByProcedure([FromQuery] Product model)
        {
            SqlParameter[] sqlParms = new SqlParameter[2];
            sqlParms[0] = new SqlParameter("@productName", model.ProductName);
            sqlParms[1] = new SqlParameter("@price", model.Price);
            var list = _WMSDbContext.QueryByProcedure<Product>("GetProductProc", sqlParms);
            return Json(list);
        }

        //http://localhost:8464/api/Product
        //Content-Type application/json
        [HttpPost]
        public IActionResult Post([FromBody] Product model)
        {
            var product = this._WMSDbContext.Product.Where(p => p.ID == model.ID).FirstOrDefault();
            return Json(product);
        }

        //http://localhost:8464/api/Product/AddProduct
        //Content-Type application/json
        [HttpPost("AddProduct")]
        public IActionResult AddProduct([FromBody] Product model)
        {
            var product = this._WMSDbContext.Product.Where(p => p.ID == model.ID).FirstOrDefault();
            return Json(product);
        }


        [HttpPost("UpdateProductByProcedure")]
        public IActionResult UpdateProductByProcedure([FromBody] Product model)
        {
            SqlParameter[] sqlParms = new SqlParameter[2];
            //sqlParms[0] = new SqlParameter("@productName", System.Data.SqlDbType.NVarChar, 100);
            //sqlParms[0].Value = model.ProductName;
            //sqlParms[1] = new SqlParameter("@id", System.Data.SqlDbType.Int);
            //sqlParms[1].Value = model.ID;

            //参数最好和存储过程内声明的顺序一直
            sqlParms[0] = new SqlParameter("@productName", model.ProductName);
            sqlParms[1] = new SqlParameter("@id", model.ID);

            var count = _WMSDbContext.ExecuteScalarByProcedure<int>("UpdateProductProc", sqlParms);
            return Json(count);
        }


        [HttpGet("AsyncTest")]
        public async Task<IActionResult> AsyncTest()
        {
            return await Task.Run(() =>
            {
                Thread.Sleep(10 * 1000);
                return Json("Async");
            });
        }

        [HttpGet("SyncTest")]
        public IActionResult SyncTest()
        {
            Thread.Sleep(10 * 1000);
            return Json("Sync");
        }

        [HttpGet("SyncTest1")]
        public IActionResult SyncTest1()
        {
            Thread.Sleep(10 * 1000);
            return Json("Sync1");
        }

    }
}
