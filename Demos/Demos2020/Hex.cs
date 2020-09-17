using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    class Hex
    {
        public static string HexString(byte[] bytes,bool uperCase=false)
        {
            var formatter = uperCase ? "X2" : "x2";
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var byt in bytes)
            {
                var str = $"0x{Convert.ToString(byt, 16)}";

                sb.Append($"0x{byt.ToString(formatter)},");
            }
            var hexStr = sb.ToString().TrimEnd(',');
            return hexStr + "}";
        }

        public static byte[] HexStringToByte(string hexString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(hexString);
            return bytes;
        }
    }
}
