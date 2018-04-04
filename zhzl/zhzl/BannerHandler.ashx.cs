using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUitl;

namespace zhzl
{
    /// <summary>
    /// BannerHandler 的摘要说明
    /// </summary>
    public class BannerHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            bool result = false;
            string action = context.Request["action"] ?? "";

            try
            {
                if (action == "select")
                {
                    string ban = BannerDb.GetSingle("1");
                    context.Response.Write(ban);
                    return;
                }
                string title = context.Request["title"] ?? "";
                string id = context.Request["id"] ?? "";
                string content = context.Request["content"] ?? "";
                string imgPath = context.Request["img"] ?? "";

                Banner banner = new Banner()
                {
                    Title = title,
                    Id = id,
                    Content=content,
                    ImgPath = imgPath,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now
                };
                if (action == "update")
                {
                    result = BannerDb.Update(banner);
                }
                else
                {
                    result = BannerDb.Add(banner);
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