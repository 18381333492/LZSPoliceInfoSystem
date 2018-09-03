using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SecurityHelper
    {

        /// <summary>
        /// Md5大写32加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5data.Length; i++)
            {
                sBuilder.Append(md5data[i].ToString("X2"));
                //X代表十六进制
                //2:代表每个数字2位
            }
            return sBuilder.ToString();
        }


        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SHA1(string input)
        {
            SHA1 shaHash = System.Security.Cryptography.SHA1.Create();
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString().ToUpper();
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "SHA1");
        }


        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="express">要加密的字符串</param>
        /// <returns></returns>
        public static string RSAEncryption(string express)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "oa_erp_dowork";//密匙容器的名称，保持加密解密一致才能解密成功
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(express);//将要加密的字符串转换为字节数组
                return Convert.ToBase64String(plaindata);//将加密后的字节数组转换为字符串
            }
        }

        /// <summary>
        ///  RSA解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public static string RSADecrypt(string ciphertext)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = "oa_erp_dowork";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                byte[] encryptdata = Convert.FromBase64String(ciphertext);
                return Encoding.Default.GetString(encryptdata);
            }
        }

    }
}
