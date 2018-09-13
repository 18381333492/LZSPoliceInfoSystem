using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web
{
    /// <summary>
    /// 文件的上传摘要说明
    /// </summary>
    public class FileUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                HttpFileCollection ImgList = context.Request.Files;
                HttpPostedFile file = ImgList[0];
                if (file != null && file.ContentLength > 0)
                {
                    //string[] sExtension = { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };

                    /*文件保存路径*/
                    string sPath = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\";
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }

                    //string format = System.IO.Path.GetExtension(file.FileName).ToLower();//获取文件后缀名
                    /*组装文件名*/
                    string sFileName = file.FileName;

                    /*保存图片到本地*/
                    file.SaveAs(sPath + sFileName);

                    context.Response.Write(JsonHelper.toJson(new
                    {
                        error = 0,
                        name=sFileName
                    }));
                }
            }
            catch (Exception e)
            {
                context.Response.Write(JsonHelper.toJson(new
                {
                    error = 1,
                    message = "上传失败"
                }));
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