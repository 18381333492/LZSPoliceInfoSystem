using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Models;
using Common;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Server;
using RazorBase;

namespace Web.Controllers
{

    /// <summary>
    /// 模板控制器
    /// </summary>
    public class TempletController : BaseController
    {
        // GET: Templet
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 添加模板视图
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Add(int ? ID)
        {
            var templet = new TG_Templet();
            templet.sTempletUrl = ConfigHelper.ReadAppSetting("sTemplatePath");
            if (ID != null)
            {
                templet = mangae.db.TG_Templet.Find(ID);
                //读取模板文件内容
                var path = AppDomain.CurrentDomain.BaseDirectory + ConfigHelper.ReadAppSetting("sTemplatePath")+"\\";
                string sFileName =templet.sTempletEnName + ".cshtml";
                if (System.IO.File.Exists(path+sFileName))
                {//文件存在
                    string Content = System.IO.File.ReadAllText(path + sFileName);
                    templet.sTempletContent = Content;
                }
                else
                {//不存在
                    templet.sTempletContent = templet.sTempletContent;
                }
            } 
            return View(templet);

        }
    
        /// <summary>
        /// 分页获模板取数据列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public ActionResult List(PageInfo pageInfo)
        {
            var query = from m in mangae.db.TG_Templet
                        orderby m.dInsertTime descending
                        select new
                        {
                            m.ID,
                            m.sTempletName,
                            m.sTempletUrl,
                            m.sTempletEnName,
                            m.dInsertTime,
                            m.bIsCompile
                        };
            if (!string.IsNullOrEmpty(pageInfo.sKeyWord))
            {//模糊查询
                query = query.Where(m => m.sTempletName.Contains(pageInfo.sKeyWord) || m.sTempletEnName.Contains(pageInfo.sKeyWord));
            }
            var total = query.Count();
            query = query.Skip((pageInfo.page - 1) * pageInfo.rows).Take(pageInfo.rows);

            result.pageResult.total = total;
            result.pageResult.rows = query;
            return Content(result.pageResult.toJson());
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="templet"></param>
        [ValidateInput(false)]
        public void Insert(TG_Templet templet)
        {
            if (templet.bIsCompile == null) templet.bIsCompile = false;
            templet.sTempletEnName = templet.sTempletEnName.Trim();
            if (mangae.db.TG_Templet.Any(m => m.sTempletEnName == templet.sTempletEnName || m.sTempletName == templet.sTempletName))
            {//存在重复模板名称或者模板标识
                result.info = "存在重复模板名称或者模板标识";
            }
            else
            {//不存在添加
                templet.dInsertTime = DateTime.Now;
                mangae.Add<TG_Templet>(templet);
                result.success = mangae.SaveChange();
                //生成模板文件
                MakeTempletHtml(templet);
            }
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="templet"></param>
        [ValidateInput(false)]
        public void Update(TG_Templet templet)
        {
            if (templet.bIsCompile == null) templet.bIsCompile = false;
            templet.sTempletEnName = templet.sTempletEnName.Trim();
            if (mangae.db.TG_Templet.Where(m => m.ID != templet.ID).Any(m => m.sTempletEnName == templet.sTempletEnName || m.sTempletName == templet.sTempletName))
            {//存在重复模板名称或者模板标识
                result.info = "存在重复模板名称或者模板标识";
            }
            else
            {
                mangae.Edit<TG_Templet>(templet);
                result.success = mangae.SaveChange();
                //生成模板文件
                MakeTempletHtml(templet);
            }
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="Ids"></param>
        public void Cancel(string Ids)
        {
            List<int> IdList = Ids.Split(',').Select(m => Convert.ToInt32(m)).ToList();
            if (mangae.db.TG_Category.Any(m => IdList.Contains(m.iTemplateId.Value) || IdList.Contains(m.iArticleTemplateId.Value)))
            {
                result.info = "该模板绑定有栏目,不能进行删除操作？";
            }
            else
            {
                result.success = mangae.Delete<TG_Templet>(Ids);
            }
        }


        /// <summary>
        /// 生成模板文件
        /// </summary>
        private void MakeTempletHtml(TG_Templet templet)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + ConfigHelper.ReadAppSetting("sTemplatePath")+"\\";
            if (!Directory.Exists(path))
            {//不存在该目录
                Directory.CreateDirectory(path);
            }
            else
            {
                string sFileName = templet.sTempletEnName + ".cshtml";
                System.IO.File.WriteAllText(path + sFileName, templet.sTempletContent);
                //预编译模板
                RazorHelper.PrevCompileTemplate(templet.sTempletEnName,templet.sTempletContent);
            }
        }

        /// <summary>
        /// 生成所有的模板文件
        /// </summary>
        public void MakeAllTempletHtml()
        {
            var list = mangae.db.TG_Templet.ToList();
            var path = AppDomain.CurrentDomain.BaseDirectory + ConfigHelper.ReadAppSetting("sTemplatePath")+ "\\";
            if (!Directory.Exists(path))
            {//不存在该目录
                Directory.CreateDirectory(path);
            }
            else
            {
                foreach (var item in list)
                {
                    string sFileName = item.sTempletEnName + ".cshtml";
                    System.IO.File.WriteAllText(path + sFileName, item.sTempletContent);
                    //预编译模板
                    RazorHelper.PrevCompileTemplate(item.sTempletEnName,item.sTempletContent);
                }
            }
        }


        /// <summary>
        /// 获取combox下拉模板列表
        /// </summary>
        /// <returns></returns>
        public ActionResult comboxData()
        {
            var query =from m in mangae.db.TG_Templet 
                       orderby m.ID descending
                       select new
                       {
                           m.ID,
                           m.sTempletName
                       };
            JArray array = JArray.Parse(JsonHelper.toJson(query));
            JObject job = new JObject();
            job.Add(new JProperty("ID",0));
            job.Add(new JProperty("sTempletName", "请选择模板"));
            array.AddFirst(job);
            return Content(JsonHelper.toJson(array));
        }
    }
}