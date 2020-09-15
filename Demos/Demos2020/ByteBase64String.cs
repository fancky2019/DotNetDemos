using Demos.OpenResource.DotNettyDemo.Model;
using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    class ByteBase64String
    {
        public void Test()
        {
            Fun();
        }

        public void Fun()
        {
            try
            {


                MessageInfo msg = new MessageInfo()
                {
                    MessageType = MessageType.HeartBeat,
                    Body = "dsdssd"
                };
                MessagePackSerializer.DefaultOptions = ContractlessStandardResolver.Options;
                byte[] bytes = MessagePackSerializer.Serialize<MessageInfo>(msg);

                string base64Str = Convert.ToBase64String(bytes);
                byte[] fromBase64StringByte = Convert.FromBase64String(base64Str);
                MessageInfo mc2 = MessagePackSerializer.Deserialize<MessageInfo>(bytes);
                int m = 0;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
