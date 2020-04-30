using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApplication.Models
{
    public class MessageResult<T>
    {

        /**
         * 执行结果（true:成功，false:失败）
         */
        public Boolean Success { get; set; }
        public String Message { get; set; }
        public T Data { get; set; }
    }
}
