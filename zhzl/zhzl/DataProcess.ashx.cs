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
            bool result = false;
            string action = context.Request["action"] ?? "";
            if (action == "del")
            {
                string id = context.Request["id"] ?? "";
                result = DbUtil.Delete(Convert.ToInt32(id));
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
                    result = DbUtil.Add(product);
                }
                else
                {
                    result = DbUtil.Update(product);
                }
            }
           
            String message = result == true ? "新增成功" : "新增失败";
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