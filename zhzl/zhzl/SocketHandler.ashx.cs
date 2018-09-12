using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace zhzl
{
    /// <summary>
    /// SocketHandler 的摘要说明
    /// </summary>
    public class SocketHandler : IHttpHandler
    {
        LeafUDPClient udpserver = new LeafUDPClient();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //HttpApplication happ = (HttpApplication)sender;
                IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Any, 6000);


                udpserver.NetWork = new UdpClient(ipLocalEndPoint);
                udpserver.ipLocalEndPoint = ipLocalEndPoint;
                udpserver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), udpserver);
            }
            catch (Exception ex)
            {

            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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
                    string data = new ASCIIEncoding().GetString(recdata);
                    HttpContext.Current.Session["currentData"] = data;
                }
            }
            catch (ObjectDisposedException ex) { }
            catch (Exception ex)
            {

            }
            finally
            {
                if (userver.NetWork.Client != null)
                {
                    userver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), userver);//继续异步接收数据
                }
            }
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