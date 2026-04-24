using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stand
{
    public class ProductsDto
    {


        public ProductsDto()
        {
            this.ID = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }//{ get=> 1; } 
        /// <summary>
        /// 
        /// </summary>
        public Guid GUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? StockID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BarCodeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SkuID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProductStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 0、删除 1、正常
        /// </summary>
        public short Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] TimeStamp { get; set; }

    }
}
