using DBUitl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace zhzl
{
    /// <summary>
    /// AttachmentHandler 的摘要说明
    /// </summary>
    public class AttachmentHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool result = false;
            string action = context.Request["action"] ?? "";
            if (action == "list")
            {
                DataSet ds = AttachmentDb.GetList("");
                DataTable table = ds.Tables[0];
                Page page = new Page();
                page.count = table.Rows.Count;
                page.data = table;
                page.code = 0;
                page.msg = "";
                string data = JsonConvert.SerializeObject(page);
                context.Response.ContentType = "application/json";
                context.Response.Write(data);
                return;
            }
            else if (action == "del")
            {
                string id = context.Request["id"] ?? "";
                result = AttachmentDb.Delete(Convert.ToInt32(id));
            }
            else
            {
                string title = context.Request["title"] ?? "";
                string path = context.Request["path"] ?? "";
                string content = context.Request["content"] ?? "";

                Attachment product = new Attachment()
                {
                    Title = title,
                    Content = content,
                    Path=path,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now
                };
                if (action == "add")
                {
                    result = AttachmentDb.Add(product)>0;
                }
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