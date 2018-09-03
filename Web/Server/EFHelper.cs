using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Server
{
    public class EFHelper
    {

        private Entities instance;

        public Entities db
        {
            get
            {
                if (instance == null)
                    instance = new Entities();
                return instance;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        public void Add<T>(T entry) where T : class, new()
        {
            this.db.Set<T>().Add(entry);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        public void Edit<T>(T entry) where T : class, new()
        {
            this.db.Set<T>().Attach(entry);
            this.db.Entry<T>(entry).State = EntityState.Modified;//编辑
        }


        /// <summary>
        /// 物理删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        public void Delete<T>(T entry) where T : class, new()
        {
            this.db.Set<T>().Attach(entry);
            this.db.Entry<T>(entry).State = EntityState.Deleted;//删除
        }


        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Ids">主键Ids集合</param>
        public int Cancel<T>(string Ids) where T : class, new()
        {
            int res = 0;
            res = this.db.Database.
                ExecuteSqlCommand(string.Format(@"UPDATE
                                        [{0}] SET bIsDeleted = 1 
                                        WHERE ID IN({1})", typeof(T).Name, Ids));
            return res;
        }

        /// <summary>
        /// 物理删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Ids">主键Ids集合,以逗号隔开</param>
        public bool Delete<T>(string Ids) where T : class, new()
        {
            int res = 0;
            res = this.db.Database.
                ExecuteSqlCommand(string.Format(@"DELETE [{0}] WHERE ID IN({1})", typeof(T).Name, Ids));
            return res>0?true:false;
        }


        /// <summary>
        /// 根据Sql语句执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExcuteBySql(string sql, params object[] param)
        {
            int res = 0;
            res = this.db.Database.ExecuteSqlCommand(sql, param);
            return res;
        }


        /// <summary>
        /// 无操作日志提交操作
        /// </summary>
        /// <returns></returns>
        public bool SaveChange()
        {
            return this.db.SaveChanges()>0?true:false;
        }
    }
}