using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 配置文件的相关操作的封装
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取连接字符串(从WebConfig配置文件中获取) 
        /// </summary>
        /// <param name="ConnStrinName"></param>
        /// <returns></returns>
        public static string ReadConnString(string ConnStrinName)
        {
            string result = string.Empty;
            try
            {
                ConnectionStringSettings cConstrset = ConfigurationManager.ConnectionStrings[ConnStrinName];
                result = cConstrset.ConnectionString;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }


        /// <summary>
        /// 读取配置文件中的appSetting
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string ReadAppSetting(string appName)
        {
            string result = string.Empty;
            try
            {
                result = ConfigurationManager.AppSettings[appName];
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }


    }
}
