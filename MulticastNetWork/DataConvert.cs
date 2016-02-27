using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    public class DataConvert
    {
        /// <summary>
        /// 串行化任意一个可串行化数据
        /// </summary>
        /// <param name="data">要串行化的数据</param>
        /// <returns>数据字符串</returns>
        public static string Serialize(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream rems = new MemoryStream();
            formatter.Serialize(rems, data);
            string ans = Encoding.Default.GetString(rems.GetBuffer());
            return ans;
        }
        /// <summary> /// 反序列化 /// </summary> 
        /// <param name="data">数据字符串</param>
        /// <returns>对象</returns> 
        public static object Deserialize(string data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            byte[] bData = Encoding.Default.GetBytes(data);
            MemoryStream rems = new MemoryStream(bData);
            return formatter.Deserialize(rems);
        }
    }
}
