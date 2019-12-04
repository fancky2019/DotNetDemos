using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Model.EntityModels.WMS
{
    public class Check
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
        public int ProductID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 0、删除 1、正常
        /// </summary>
        public short Status { get; set; }
    }
}