using AuthCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2019.Encrypt
{
    public class EncryptDemo
    {
        /*
         * 私钥不公开，公钥公开。私钥包含公钥。
         */

        /*
         * 数据加密：
         * 公钥加密，私钥解密。拥有私钥的才能解密。如：客户端发送用公钥加密的数据，服务端用私钥解密。
         * 私钥加密，公钥解密。有公钥的都可解密。如服务器广播用私钥加密数据，客户端用公钥解密。
         * 
         * 
         * 
         * 
         * 数据签名：数字签名技术是将摘要信息用发送者的私钥加密，与原文一起传送给接收者。接收者只有用发送者的公钥
         * 才能解密被加密的摘要信息，然后用HASH函数对收到的原文产生一个摘要信息，与解密的摘要信息对比。如果相同，
         * 则说明收到的信息是完整的，在传输过程中没有被修改，否则说明信息被修改过，因此数字签名能够验证信息的完整
         * 性。
         * 
         * 
         * 整个加密签名过程：
         * A向B发送信息的整个签名和加密的过程如下：
         * 1、A先用自己的私钥（PRI_A）对信息（一般是信息的摘要）进行签名。
         * 2、A接着使用B的公钥（PUB_B）对信息内容和签名信息进行加密。
         * 这样当B接收到A的信息后，获取信息内容的步骤如下：
         * 1、用自己的私钥（PRI_B）解密A用B的公钥（PUB_B）加密的内容；
         * 2、得到解密后的明文后用A的公钥（PUB_A）解签A用A自己的私钥（PRI_A）的签名。
         */


        /*
        * 公钥用于对数据进行加密，私钥用于对数据进行解密。 
        * 私钥用于对数据进行签名，公钥用于对签名进行验证
        */

        /*
         * 通过RSA加密将AES的key发给对方，通过AES加密、解密通信。
         */
        public void Test()
        {

            RSATest();
            AESTest();
        }

        private void RSATest()
        {
            string str = "fancky";
            //生成密钥对（私钥和公钥）
            RSAUtil rSAUtil = new RSAUtil();
            rSAUtil.SaveKeys("keys.xml");



            //加密:公钥加密私钥解密
            //生成公钥加密类
            RSAUtil rSAUtilEncrypt = new RSAUtil(rSAUtil.PublicKey, false);
            var encryptBytes = rSAUtilEncrypt.RSAEncrypt(str);
            //生成私钥解密类
            RSAUtil rSAUtilDecrypt = new RSAUtil(rSAUtil.PrimaryKey, false);
            var decryptText = Encoding.UTF8.GetString(rSAUtilDecrypt.RSADecrypt(encryptBytes));


            ////
            //var b1 = rSAUtilDecrypt.RSAEncrypt(str);
            ////公钥不能解密，报异常
            //var b2 = rSAUtilEncrypt.RSADecrypt(b1);
            //var publicKeyEncryptPrimaryKeyDecrypt = Encoding.UTF8.GetString(b2);





            //签名：私钥签名公钥验证签名
            //生成私钥签名类
            RSAUtil rSAUtilSign = new RSAUtil(rSAUtil.PrimaryKey, false);
            var singedData = rSAUtil.Sign(str);
            //生成公钥验证签名类
            var dataToVerify = Encoding.UTF8.GetBytes(str);
            RSAUtil rSAUtilVerifySigned = new RSAUtil(rSAUtil.PublicKey, false);
            var verifyResult = rSAUtilVerifySigned.VerifySigned(dataToVerify, singedData);

            //F12参见源码：由于公钥没有一下信息。报异常。
            /*
             *     if (includePrivateParameters)
            {
                stringBuilder.Append("<P>" + Convert.ToBase64String(rSAParameters.P) + "</P>");
                stringBuilder.Append("<Q>" + Convert.ToBase64String(rSAParameters.Q) + "</Q>");
                stringBuilder.Append("<DP>" + Convert.ToBase64String(rSAParameters.DP) + "</DP>");
                stringBuilder.Append("<DQ>" + Convert.ToBase64String(rSAParameters.DQ) + "</DQ>");
                stringBuilder.Append("<InverseQ>" + Convert.ToBase64String(rSAParameters.InverseQ) + "</InverseQ>");
                stringBuilder.Append("<D>" + Convert.ToBase64String(rSAParameters.D) + "</D>");
            }
             */
            // rSAUtilVerifySigned.SaveKeys("publickeys.xml");

        }

        private void AESTest()
        {
            AESUtil aes = new AESUtil();
            byte[] a = aes.AESEncrypt(@"123  Source=mscorlib
  StackTrace:
   在 System.Security.Cryptography.CryptographicException.ThrowCryptographicException(Int32 hr)
   在 System.Security.Cryptography.RSACryptoServiceProvider.EncryptKey(SafeKeyHandle pKeyContext, Byte[] pbKey, Int32 cbKey, Boolean fOAEP, ObjectHandleOnStack ohRetEncryptedKey)
   在 System.Security.Cryptography.RSACryptoServiceProvider.Encrypt(Byte[] rgb, Boolean fOAEP)
   在 EncryptServer.frmServer.<>c__DisplayClass20_0.<btnSend_Click>b__0(ClientInfo p) 在 C:\Users\Administrator\Desktop\EncryptServer\EncryptServer\FrmServer.cs 中: 第 301 行
   在 System.Collections.Generic.List`1.ForEach(Action`1 action)
   在 EncryptServer.frmServer.btnSend_Click(Object sender, EventArgs e) 在 C:\Users\Administrator\Desktop\EncryptServer\EncryptServer\FrmServer.cs 中: 第 294 行
   在 System.Windows.Forms.Control.OnClick(EventArgs e)
   在 System.Windows.Forms.Button.OnClick(EventArgs e)
   在 System.Windows.Forms.Button.OnMouseUp(MouseEventArgs mevent)
   在 System.Windows.Forms.Control.WmMouseUp(Message& m, MouseButtons button, Int32 clicks)
   在 System.Windows.Forms.Control.WndProc(Message& m)
   在 System.Windows.Forms.ButtonBase.WndProc(Message& m)
   在 System.Windows.Forms.Button.WndProc(Message& m)
   在 System.Windows.Forms.Control.ControlNativeWindow.OnMessage(Message& m)
   在 System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
   在 System.Windows.Forms.NativeWindow.DebuggableCallback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)
   在 System.Windows.Forms.UnsafeNativeMethods.DispatchMessageW(MSG& msg)
   在 System.Windows.Forms.Application.ComponentManager.System.Windows.Forms.UnsafeNativeMethods.IMsoComponentManager.FPushMessageLoop(IntPtr dwComponentID, Int32 reason, Int32 pvLoopData)
   在 System.Windows.Forms.Application.ThreadContext.RunMessageLoopInner(Int32 reason, ApplicationContext context)
   在 System.Windows.Forms.Application.ThreadContext.RunMessageLoop(Int32 reason, ApplicationContext context)
   在 System.Windows.Forms.Application.Run(Form mainForm)
   在 EncryptServer.Program.Main() 在 C:\Users\Administrator\Desktop\EncryptServer\EncryptServer\Program.cs 中: 第 19 行");
            AESUtil aesD = new AESUtil(aes.Key, aes.IV);
            string s = aesD.AESDecrypt(a);
        }
    }
}
