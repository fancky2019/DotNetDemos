using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Model
{
    public class MessageInfo
    {
        public MessageType messageType { get; set; }
        public string body { get; set; }
    }

    public enum MessageType
    {
        HeartBeat,
        Data
    }
}
