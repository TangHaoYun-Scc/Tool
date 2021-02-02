using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Basics_Tool.RSA
{
    /// <summary>
    /// RSA加密解密及RSA签名和验证
    /// </summary>
    public class RSACryption
    {
        #region RSA 的密钥产生

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public static void RSAKey(out string xmlKeys , out string xmlPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //私钥
            xmlKeys = RSAKeyConvert.RSAPrivateKeyDotNet2Java(rsa.ToXmlString(true));
            //公钥
            xmlPublicKey = RSAKeyConvert.RSAPublicKeyDotNet2Java(rsa.ToXmlString(false));
        }

        #endregion RSA 的密钥产生

        #region RSA的加密函数

        //##############################################################################
        //RSA 方式加密
        //说明KEY必须是XML的行式,返回的是字符串
        //在有一点需要说明！！该加密方式有 长度 限制的！！
        //##############################################################################

        //RSA的加密函数
        /// <summary>
        /// RSA加密函数
        /// </summary>
        /// <param name="rawInput"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string rawInput , string publicKey)
        {
            if (string.IsNullOrEmpty(rawInput))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(publicKey))
            {
                throw new ArgumentException("Invalid Public Key");
            }

            publicKey = RSAKeyConvert.RSAPublicKeyJava2DotNet(publicKey);

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(rawInput);//有含义的字符串转化为字节流
                rsaProvider.FromXmlString(publicKey);//载入公钥
                int bufferSize = (rsaProvider.KeySize / 8) - 11;//单块最大长度
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    { //分段加密
                        int readSize = inputStream.Read(buffer , 0 , bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer , 0 , temp , 0 , readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp , false);
                        outputStream.Write(encryptedBytes , 0 , encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray());//转化为字节流方便传输
                }
            }
        }

        #endregion RSA的加密函数

        #region RSA的解密函数

        /// <summary>
        /// RSA的解密函数
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string encryptedInput , string privateKey)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedInput))
                {
                    return string.Empty;
                }

                if (string.IsNullOrWhiteSpace(privateKey))
                {
                    throw new ArgumentException("Invalid Private Key");
                }

                privateKey = RSAKeyConvert.RSAPrivateKeyJava2DotNet(privateKey);

                using (var rsaProvider = new RSACryptoServiceProvider())
                {
                    var inputBytes = Convert.FromBase64String(encryptedInput);
                    rsaProvider.FromXmlString(privateKey);
                    int bufferSize = rsaProvider.KeySize / 8;
                    var buffer = new byte[bufferSize];
                    using (MemoryStream inputStream = new MemoryStream(inputBytes),
                         outputStream = new MemoryStream())
                    {
                        while (true)
                        {
                            int readSize = inputStream.Read(buffer , 0 , bufferSize);
                            if (readSize <= 0)
                            {
                                break;
                            }

                            var temp = new byte[readSize];
                            Array.Copy(buffer , 0 , temp , 0 , readSize);
                            var rawBytes = rsaProvider.Decrypt(temp , false);
                            outputStream.Write(rawBytes , 0 , rawBytes.Length);
                        }
                        return System.Web.HttpUtility.UrlDecode(Encoding.UTF8.GetString(outputStream.ToArray()));
                    }
                }
            }
            catch
            {
                //ex.ToErrorLog();
                throw new Exception("校验报文失败");
            }
        }

        #endregion RSA的解密函数
    }
}
