using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthCommon
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public class AESUtil : IDisposable
    {

        private byte[] _key = null;
        private byte[] _iv = null;
        private AesCryptoServiceProvider _aes = null;
        private ICryptoTransform _encryptor = null;
        private ICryptoTransform _decryptor = null;
        public string Key
        {
            get
            {
                return Convert.ToBase64String(_key);
            }
        }
        public string IV
        {
            get
            {
                return Convert.ToBase64String(_iv);
            }

        }

        public AESUtil(int keySize=256)
        {
            _aes = new AesCryptoServiceProvider();
            _aes.KeySize = keySize;
            _key = _aes.Key;
            _iv = _aes.IV;
            _encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            _decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);
        }

        public AESUtil(string key, string iv, int keySize = 256)
        {
            _aes = new AesCryptoServiceProvider();
            _aes.KeySize = keySize;
            _key = Convert.FromBase64String(key);
            _iv = Convert.FromBase64String(iv);
            _aes.Key = _key;
            _aes.IV = _iv;

            _encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            _decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="DoOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] AESEncrypt(string text, bool DoOAEPPadding = false)
        {

            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, _encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt">待解密字节数组</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public string AESDecrypt(byte[] dataToDecrypt, bool doOAEPPadding = false)
        {
            string str = null;
            using (MemoryStream msDecrypt = new MemoryStream(dataToDecrypt))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, _decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        str = srDecrypt.ReadToEnd();
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt">待解密数组</param>
        /// <param name="index">开始位置</param>
        /// <param name="count">长度</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public string AESDecrypt(byte[] dataToDecrypt,int index, int count , bool doOAEPPadding = false)
        {
            string str = null;
            using (MemoryStream msDecrypt = new MemoryStream(dataToDecrypt,  index, count))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, _decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        str = srDecrypt.ReadToEnd();
                    }
                }
            }
            return str;
        }

        public void Dispose()
        {
            if (_aes != null)
                _aes.Dispose();

            if (_encryptor != null)
                _encryptor.Dispose();

            if (_decryptor != null)
                _decryptor.Dispose();
        }
    }
}
