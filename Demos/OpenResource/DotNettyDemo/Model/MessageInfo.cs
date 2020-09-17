using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.DotNettyDemo.Model
{
    /// <summary>
    /// C#属性首字母大写，java字段首字母是小写的。否则反序列化报错。
    /// </summary>
    //[MessagePackObject(keyAsPropertyName: true)]
    [MessagePackObject]
    public class MessageInfo
    {
        //[Key(0)]
        [Key("messageType")]
        public MessageType MessageType { get; set; }
        //public string MessageType { get; set; }
        //[Key(1)]
        [Key("body")]
        public string Body { get; set; }

        public override string ToString()
        {
            return $"MessageType:{this.MessageType}, Body:{this.Body}";
        }
    }

    public enum MessageType
    {
        HeartBeat,
        Data
    }
}
