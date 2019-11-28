using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthCommon
{
    /*
     * 私钥包含公钥。
     * 公钥用于对数据进行加密，私钥用于对数据进行解密。 
     * 私钥用于对数据进行签名，公钥用于对签名进行验证
     */

    /*
     * 通过RSA加密将AES的key发给对方，通过AES加密、解密通信。
     */
    /// <summary>
    ///非对称加密： 公钥加密私钥解密
    /// </summary>

    public class RSAUtil : IDisposable
    {
        private RSACryptoServiceProvider _rsa = null;
        private string _modulus;
        private string _exponent;

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrimaryKey
        {
            get
            {
                return _rsa.ToXmlString(true); ;
            }
        }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey
        {
            get
            {
                return _rsa.ToXmlString(false); ;
            }
        }

        /// <summary>
        /// 公钥模数
        /// </summary>
        public string Modulus
        {
            get
            {
                return _modulus;
            }
        }

        /// <summary>
        /// 公钥指数
        /// </summary>
        public string Exponent
        {
            get
            {
                return _exponent;
            }
        }

        /// <summary>
        /// 生成默认密钥加密类
        /// </summary>
        public RSAUtil(int keySize = 1024)
        {
            _rsa = new RSACryptoServiceProvider(keySize);
            ExportParameters();
        }



        /// <summary>
        /// 根据密钥或者密钥文件路径生成加密类（公钥、私钥加密类）
        /// </summary>
        /// <param name="primaryKeyOrPublicKeyOrKeyXMLFilePath">Key（私钥、公钥key）还是XML文件路径</param>
        /// <param name="primaryKey">公钥还是私钥加解密类</param>
        public RSAUtil(string primaryKeyOrPublicKeyOrKeyXMLFilePath, bool isPrivateKey = true)
        {
            _rsa = new RSACryptoServiceProvider();
            string key = primaryKeyOrPublicKeyOrKeyXMLFilePath;
            if (File.Exists(primaryKeyOrPublicKeyOrKeyXMLFilePath))
            {
                List<string> keys = XMLHelper.Instance.ReadXMLData(primaryKeyOrPublicKeyOrKeyXMLFilePath);
                key = isPrivateKey ? keys[0] : keys[1];
            }
            _rsa.FromXmlString(key);
            ExportParameters();
        }


        /// <summary>
        /// 公钥生成加密类
        /// </summary>
        /// <param name="modulus">公钥模数</param>
        /// <param name="exponent">公钥指数</param>
        public RSAUtil(string modulus, string exponent)
        {
            RSAParameters param = new RSAParameters()
            {
                Modulus = Convert.FromBase64String(modulus),
                Exponent = Convert.FromBase64String(exponent)
            };
            _rsa = new RSACryptoServiceProvider();
            _rsa.ImportParameters(param);
            ExportParameters();

        }

        private void ExportParameters()
        {
            RSAParameters param = _rsa.ExportParameters(false);
            this._exponent = Convert.ToBase64String(param.Exponent);
            this._modulus = Convert.ToBase64String(param.Modulus);
        }

        /// <summary>
        /// 保存公钥、私钥到xml文件
        /// </summary>
        /// <param name="path"></param>
        public void SaveKeys(string path)
        {
            string primaryKey = _rsa.ToXmlString(true);
            string publicKey = _rsa.ToXmlString(false);
            XMLHelper.Instance.WriteDatas(primaryKey, publicKey, path);
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="DataToEncrypt">待加密字节数组</param>
        /// <param name="DoOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] RSAEncrypt(byte[] dataToEncrypt, bool doOAEPPadding = false)
        {
            return _rsa.Encrypt(dataToEncrypt, doOAEPPadding);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataToEncrypt">待加密字符串</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] RSAEncrypt(string dataToEncrypt, bool doOAEPPadding = false)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(dataToEncrypt);
            return _rsa.Encrypt(byteData, doOAEPPadding);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataToEncrypt">待加密字符串</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public string RSAEncryptToBase64String(string dataToEncrypt, bool doOAEPPadding = false)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(dataToEncrypt);
            return Convert.ToBase64String(_rsa.Encrypt(byteData, doOAEPPadding));
        }



        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataToEncrypt">待加密数组</param>
        /// <param name="index">开始位置</param>
        /// <param name="count">长度</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] RSAEncrypt(byte[] dataToEncrypt, int index, int count, bool doOAEPPadding = false)
        {
            return _rsa.Encrypt(dataToEncrypt.Skip(index).Take(count).ToArray(), doOAEPPadding);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt">待解密字节数组</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] RSADecrypt(byte[] dataToDecrypt, bool doOAEPPadding = false)
        {
            return _rsa.Decrypt(dataToDecrypt, doOAEPPadding);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt">加密后的Base64String</param>
        /// <param name="doOAEPPadding">true 若要直接执行 System.Security.Cryptography.RSA 使用 OAEP 填充 （仅可在运行 Windows XP 的计算机上或更高版本）
        /// 的加密; 否则为 false 使用 PKCS #1 v1.5 填充。</param>
        /// <returns></returns>
        public byte[] RSADecrypt(string dataToDecrypt, bool doOAEPPadding = false)
        {
            return _rsa.Decrypt(Convert.FromBase64String(dataToDecrypt), doOAEPPadding);
        }



        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="doOAEPPadding"></param>
        /// <returns></returns>
        public byte[] RSADecrypt(byte[] dataToDecrypt, int index, int count, bool doOAEPPadding = false)
        {
            return _rsa.Decrypt(dataToDecrypt.Skip(index).Take(count).ToArray(), doOAEPPadding);
        }




        /// <summary>
        /// 数字签名（私钥签名）
        /// 注意：用私钥初始化RSAUtil类
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <returns>签名后的字节数组</returns>
        public byte[] Sign(string dataToSign)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(dataToSign);
            //使用SHA1进行摘要算法，生成签名
            return _rsa.SignData(byteData, new SHA1CryptoServiceProvider());
        }

        /// <summary>
        /// 数字签名（私钥签名）
        /// 注意：用私钥初始化RSAUtil类
        /// </summary>
        /// <param name="dataToSign">原文</param>
        /// <returns>签名后的字节数组</returns>
        public string SignedToString(string dataToSign)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(dataToSign);
            //使用SHA1进行摘要算法，生成签名
            byte[] encryptedData = _rsa.SignData(byteData, new SHA1CryptoServiceProvider());
            return Convert.ToBase64String(encryptedData);
        }


        /// <summary>
        /// 验证签名（公钥验证签名）
        /// 注意：用公钥初始化RSAUtil类
        /// </summary>
        /// <param name="plaintext">原文</param>
        /// <param name="SignedData">签名后的字节数组</param>
        /// <returns></returns>
        public bool VerifySigned(byte[] dataToVerify, byte[] signedData)
        {
            return _rsa.VerifyData(dataToVerify, new SHA1CryptoServiceProvider(), signedData);
        }


        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
