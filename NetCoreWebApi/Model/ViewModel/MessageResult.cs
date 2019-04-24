using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApi.Model.ViewModel
{
    public class MessageResult<T>
    {

        /**
         * 执行结果（true:成功，false:失败）
         */
        private Boolean _success;
        private String _message;
        private List<T> _data;

        public bool Success { get => _success; set => _success = value; }
        public string Message { get => _message; set => _message = value; }
        public List<T> Data { get => _data; set => _data = value; }
    }
}
