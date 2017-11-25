using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace zhzl
{
    /// <summary>
    /// DownLoad 的摘要说明
    /// </summary>
    public class DownLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                String fileName= context.Request["fileName"];
                string strFilePath = context.Server.MapPath("~") + "/files/" + fileName;//服务器文件路径
                FileInfo fileInfo = new FileInfo(strFilePath);
                context.Response.Clear();
                context.Response.Charset = "GB2312";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AddHeader("Content-Disposition", "attachment;filename=" + context.Server.UrlEncode(fileInfo.Name));
                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                context.Response.ContentType = "application/x-bittorrent";
                context.Response.WriteFile(fileInfo.FullName);
                context.Response.End();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //不做处理
            }
            catch (Exception ex)
            {
                //做处理
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