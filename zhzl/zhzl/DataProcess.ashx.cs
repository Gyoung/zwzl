using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUitl;

namespace zhzl.code
{
    /// <summary>
    /// DataProcess 的摘要说明
    /// </summary>
    public class DataProcess : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string title = context.Request["title"];
            string remark = context.Request["remark"];
            string content = context.Request["content"];
            string imgPath = context.Request["img"];

            Product product = new Product()
            {
                Title=title,Content=content,
                Remark=remark,ImgPath=imgPath
            };


            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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