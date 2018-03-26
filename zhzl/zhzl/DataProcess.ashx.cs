using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUitl;
using System.Data;
using Newtonsoft.Json;

namespace zhzl.code
{
    /// <summary>
    /// DataProcess 的摘要说明
    /// </summary>
    public class DataProcess : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool result = false;
            string action = context.Request["action"] ?? "";
            if (action == "select")
            {
                DataSet ds= ProductDb.GetList("");
                DataTable table = ds.Tables[0];
                Page page=new Page();
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
                result = ProductDb.Delete(Convert.ToInt32(id));
            }
            else
            {
                string title = context.Request["title"] ?? "";
                string remark = context.Request["remark"] ?? "";
                string content = context.Request["content"] ?? "";
                string imgPath = context.Request["img"] ?? "";

                Product product = new Product()
                {
                    Title = title,
                    Content = content,
                    Remark = remark,
                    ImgPath = imgPath,
                    CreateTime = DateTime.Now,
                    ModifyTime=DateTime.Now
                };
                if (action == "add")
                {
                    result = ProductDb.Add(product);
                }
                else
                {
                    result = ProductDb.Update(product);
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