using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    
    public class SendingClient
    {
        
        public int port {private set; get; }
        public String host_ip { set; get; }

        public SendingClient(String _ip, int _port)
    	{
    		host_ip = _ip;
    		port = _port;
    	}
        NetworkStream ns;
        BinaryFormatter formatter = new BinaryFormatter();
        public TcpClient tclient;
       
        public bool Connect(String _ip = null)
        {
            if (_ip != null)
                host_ip = _ip;
            tclient = new TcpClient();

            try
            {
                tclient.Connect(host_ip, port);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            ns = tclient.GetStream();
            
            return true;
        }

       
        public object Sending(object thing)
        {
            object obj;
            if (ns == null) throw new NotSupportedException("NetworkStream Error");
            try
            {
                formatter.Serialize(ns, thing);
                obj = formatter.Deserialize(ns);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return -1;
            }
            return obj;
        }

        public static Socket GetSocket(TcpClient cln)
        {
            PropertyInfo pi = cln.GetType().GetProperty("Client", BindingFlags.NonPublic | BindingFlags.Instance);
            Socket sock = (Socket)pi.GetValue(cln, null);
            return sock;
        }

        public static string GetRemoteIP(TcpClient cln)
        {
            string ip = GetSocket(cln).RemoteEndPoint.ToString().Split(':')[0];
            return ip;
        }

        public static int GetRemotePort(TcpClient cln)
        {
            string temp = GetSocket(cln).RemoteEndPoint.ToString().Split(':')[1];
            int port = Convert.ToInt32(temp);
            return port;
        }


        [System.Obsolete("GetLocalIP已过时，请使用GetRemoteIP从服务端获取")]
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork )
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("获取本机IP出错:" + ex.Message);
                return "";
            }
        }


        public void Close()
        {
            tclient.Close();
        }
    }
}
