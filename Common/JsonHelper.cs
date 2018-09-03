using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class JsonHelper
    {

        /// <summary>
        /// json序列化为字符串
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string toJson(object m)
        {
            try
            {
                return JsonConvert.SerializeObject(m, new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 反序列化JArray
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static JArray Array(string sJson)
        {
            return JArray.Parse(sJson);
        }

        /// <summary>
        /// 反序列化成JObject
        /// </summary>
        /// <param name="sJson"></param>
        /// <returns></returns>
        public static JObject Object(string sJson)
        {
            return JObject.Parse(sJson);
        }

        /// <summary>
        /// 根据传来的字典组装json数据 
        /// </summary>
        /// <param name="collect"></param>
        /// <returns></returns>
        public static string JsonString(Dictionary<string, object> collect)
        {
            JObject job = new JObject();
            foreach (var prop in collect)
            {
                job.Add(new JProperty(prop.Key, prop.Value));
            }
            return job.ToString();
        }
    }
}
