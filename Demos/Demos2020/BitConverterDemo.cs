using Demos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2020
{
    public class BitConverterDemo
    {

        public void  Test()
        {
            ByteConvert();
        }

        private void ByteConvert()
        {
            var msg = "test";
            var bytes = Encoding.UTF8.GetBytes(msg);
            var receivedBytes = new byte[bytes.Length];
            Array.Copy(bytes, receivedBytes, bytes.Length);
            string byteStr = BitConverter.ToString(receivedBytes, 0);
            var hexStr = Hex.HexString(receivedBytes);
        }
        public void Fun()
        {

            string s = "";
            //sByte长度为0
            var sByte = Encoding.UTF8.GetBytes(s).ToList();
            //string ss = null;
            ////null不能序列化
            //var ssByte = Encoding.UTF8.GetBytes(ss).ToList();
            char m = '男';
            BitConverterModel model = new BitConverterModel()
            {
                Married = false,
                Sex = '男'
                //Sex = 'M'
            };
            var bytes = model.Serialize();
            var mo = new BitConverterModel().Deserialize(bytes);
            char sex = mo.Sex;
            var re = string.IsNullOrEmpty(mo.Name);
        }


    }

    public class BitConverterModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public byte ChildCount { get; set; }
        public DateTime Birthday { get; set; }
        //public ulong BirthdayTimestamp => Birthday.GetTimeStamp();
        public ulong BirthdayTimestamp { get; set; }
        /// <summary>
        /// bool:true-1,false-0
        /// </summary>
        public bool Married { get; set; }
        /// <summary>
        /// 男：汉字占两个字节
        /// 字母：占两个字节，byte[0]是值，byte[1]是0
        ///char 16 位 Unicode 字符：两个字节
        /// </summary>
        public char Sex { get; set; }


        public byte[] Serialize()
        {
            var nameBytes = new byte[20];
            if (!string.IsNullOrEmpty(Name))
            {
                var nameByteArray = Encoding.UTF8.GetBytes(Name);
                Array.Copy(nameByteArray, 0, nameBytes, 0, nameByteArray.Length);
            }
            var ageBytes = BitConverter.GetBytes(Age);
            var marriedBytes = BitConverter.GetBytes(Married);
            var sexBytes = BitConverter.GetBytes(Sex);
            Birthday =  Birthday==default(DateTime)  ? DateTime.Now : Birthday;
            var birthdayTimestampBytes =  BitConverter.GetBytes(Birthday.GetTimeStamp());
            var byteLength = nameBytes.Length + ageBytes.Length + 1 + birthdayTimestampBytes.Length
                            + marriedBytes.Length + sexBytes.Length;
            var bytes = new byte[byteLength];

            Array.Copy(nameBytes, 0, bytes, 0, nameBytes.Length);
            Array.Copy(ageBytes, 0, bytes, nameBytes.Length, ageBytes.Length);
            bytes[nameBytes.Length + ageBytes.Length] = ChildCount;
            Array.Copy(birthdayTimestampBytes, 0, bytes, nameBytes.Length + ageBytes.Length + 1, birthdayTimestampBytes.Length);
            Array.Copy(marriedBytes, 0, bytes, nameBytes.Length + ageBytes.Length + 1 + birthdayTimestampBytes.Length,
                marriedBytes.Length);
            Array.Copy(sexBytes, 0, bytes,
                nameBytes.Length + ageBytes.Length + 1 + birthdayTimestampBytes.Length + marriedBytes.Length,
              sexBytes.Length);


            return bytes;
        }

        public BitConverterModel Deserialize(byte[] bytes)
        {
            //name
            var nameBytes = new byte[20];
            var ageBytes = new byte[4];
            var marriedBytes = new byte[1];
            var sexBytes = new byte[2];
            var birthdayTimestampBytes = new byte[8];
            
            Array.Copy(bytes, 0, nameBytes, 0, nameBytes.Length);
            Array.Copy(bytes, nameBytes.Length, ageBytes, 0, ageBytes.Length);
            this.ChildCount = bytes[20 + 4];
            Array.Copy(bytes,25, birthdayTimestampBytes, 0, birthdayTimestampBytes.Length);
            Array.Copy(bytes, 33, marriedBytes, 0, marriedBytes.Length);
            Array.Copy(bytes, 34, sexBytes, 0, sexBytes.Length);
            this.Name = EmptyBytes(nameBytes)?null: Encoding.UTF8.GetString(nameBytes);// BitConverter.ToString(nameBytes, 0);
            this.Age = BitConverter.ToInt32(ageBytes, 0);
            this.BirthdayTimestamp = BitConverter.ToUInt64(birthdayTimestampBytes,0);
            this.Married = BitConverter.ToBoolean(marriedBytes, 0);
            this.Sex = BitConverter.ToChar(sexBytes,0);
     
            return this;
        }

        private bool EmptyBytes (byte[] bytes)
        {
            //if(bytes[0]==0)
            //{

            //}
          return  !bytes.ToList().Exists(p => p != 0);
        }
    }

}
