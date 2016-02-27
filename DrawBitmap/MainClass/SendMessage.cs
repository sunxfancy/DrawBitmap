using DrawBitmap.MainClass;
using MulticastNetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
namespace DrawBitmap
{
    public class SendMessage
    {
        public void StartSending()
        {
            MyID = App.data.Me.user_id;
        	SendThread= new Thread(Sending);
            SendThread.IsBackground = true;
            SendThread.Start();
        }
        public void EndSending()
        {
            SendThread.Abort();
        }

        Thread SendThread;

        int MyID;
        List<UserMessage> message;

        private void Sending()
        {
            while(true)
            {
                try
                {
                    message = ServerAPI.Checking(MyID);
                    if (message != null)
                    {
                        foreach (var item in message)
                        {
                            GetReturn.ParseMessage(item);
                        }             
                    }
                }
                catch (Exception e)
                {
                    Console.Error.Write(e.ToString());
                    return;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
