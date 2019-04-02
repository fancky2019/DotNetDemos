using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreWebApi
{
    public class Product
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public int ID { get; set; }
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
        //[ConcurrencyCheck] 单列并发控制
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
        public decimal? Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 0、删除 1、正常
        /// </summary>
        public short Status { get; set; }
        //[Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}