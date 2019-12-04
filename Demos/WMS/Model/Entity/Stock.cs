using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Model.EntityModels.WMS
{
    public class Stock
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid GUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 0、删除 1、正常
        /// </summary>
        public short Status { get; set; }
    }
}