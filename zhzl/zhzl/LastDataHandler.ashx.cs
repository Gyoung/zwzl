using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace zhzl
{
    /// <summary>
    /// LastDataHandler 的摘要说明
    /// </summary>
    public class LastDataHandler : IHttpHandler
    {

        static List<string> list = new List<string>();
        private static object o = new object();
        static Queue queue = new Queue();
        static LeafUDPClient udpserver = new LeafUDPClient();
        int port = 20002;


        public void ProcessRequest(HttpContext context)
        {
            if (udpserver.NetWork == null)
            {
                try
                {
                    //HttpApplication happ = (HttpApplication)sender;
                    IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Any, port);


                    udpserver.NetWork = new UdpClient(ipLocalEndPoint);
                    udpserver.ipLocalEndPoint = ipLocalEndPoint;
                    udpserver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), udpserver);
                    EventLog.WriteEntry("LastDataHandler.ashx","socket启动成功!");
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("LastDataHandler.ashx","port:"+port+" "+ ex.ToString());
                }
            }
           

            string data = "";
            lock (o)
            {
                //object ob = queue.Dequeue();
                //data = ob == null ? "" : ob.ToString();
                if (list.Count > 0)
                {
                    data = list[0];
                    list.Remove(data);
                }
            }
          
            context.Response.ContentType = "text/plain";
            context.Response.Write(data);
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveCallback(IAsyncResult ar)
        {
            LeafUDPClient userver = (LeafUDPClient)ar.AsyncState;
            string ConnName = "";
            try
            {
                if (userver.NetWork.Client != null)
                {
                    IPEndPoint fclient = userver.ipLocalEndPoint;
                    Byte[] recdata = userver.NetWork.EndReceive(ar, ref fclient);
                    
                    ConnName = userver.ipLocalEndPoint.Port + "->" + fclient.ToString();
                    string data = new ASCIIEncoding().GetString((recdata));
                    //
                    lock (o)
                    {
                        list.Add("时间:"+DateTime.Now+" 数据:"+data);
                        //queue.Enqueue(data);
                    }
                   
                  
                }
            }
            catch (ObjectDisposedException ex) {
                EventLog.WriteEntry("LastDataHandler.ashx", "port:" + port + " " + ex.ToString());
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("LastDataHandler.ashx", "port:" + port + " " + ex.ToString());
            }
            finally
            {
                if (userver.NetWork.Client != null)
                {
                    userver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), userver);//继续异步接收数据
                }
            }
        }

        private byte[] trans(byte[] bytes)
        {
            byte[] newBytes=new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                newBytes[i] = (byte)(Convert.ToInt32(bytes[i].ToString(), 16));
            }
            return newBytes;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}