using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MulticastNetWork
{
    public class NetWorkClient
    {
    	public int port {private set; get; }
        public String host_ip { set; get; }

        public NetWorkClient(String _ip, int _port)
    	{
    		host_ip = _ip;
    		port = _port;
    	}
        NetworkStream ns;
        BinaryFormatter formatter = new BinaryFormatter();
        TcpClient tclient;
        Thread answerThread;
    	public bool Connect(String _ip = null)
    	{
            if (_ip != null)
                host_ip = _ip;
            tclient = new TcpClient();

            try
            {
                tclient.Connect(host_ip, port);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            ns = tclient.GetStream();
            
            return true;
    	}

        public void BeginListener()
        {
            answerThread = new Thread(Answer);
            answerThread.IsBackground = true;
            answerThread.Start();
        }

        public void AbortConnection()
        {
            if (tclient!=null)
            {
                tclient.Close();
            }
        }
    	public bool Send(object thing)
    	{
            if (ns == null) return false;
            try{
                formatter.Serialize(ns, thing);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return true;
    	}

        public bool Send(object[] things)
    	{
            if (ns == null) return false;
            try
            {
                foreach (var item in things)
                {
                    formatter.Serialize(ns, item);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return true;
    	}

        public event AnswerEventHandler answer;

        void Answer()
        {
            try
            {
                while(true)
                {
                    var data = formatter.Deserialize(ns);
                    answer.Invoke(this, new AnswerEventArgs(data));
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }

    }
}
