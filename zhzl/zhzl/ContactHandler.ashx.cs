using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUitl;

namespace zhzl
{
    /// <summary>
    /// ContactHandler 的摘要说明
    /// </summary>
    public class ContactHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool result = false;
            string action = context.Request["action"] ?? "";

            try
            {
                if (action == "select")
                {
                    string con = ContactDb.GetSingle("1");
                    context.Response.Write(con);
                    return;
                }
                string title = context.Request["title"] ?? "";
                string id = context.Request["id"] ?? "";
                string content = context.Request["content"] ?? "";

                Contact contact = new Contact()
                {
                    Title = title,
                    Id = id,
                    Content = content,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now
                };
                if (action == "update")
                {
                    result = ContactDb.Update(contact);
                }
                else
                {
                    result = ContactDb.Add(contact);
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }

            String message = result == true ? "操作成功" : "操作失败";
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
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