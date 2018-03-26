using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBUitl
{
   public  class BannerDb
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static bool Add(Banner model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Banner(");
            strSql.Append("Title,ImgPath,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@title,@imgpath,@createtime)");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@imgpath", OleDbType.VarChar,0),
					new OleDbParameter("@createtime", OleDbType.Date)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.ImgPath;
            parameters[2].Value = model.CreateTime;
            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(Banner model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Banner set ");
            strSql.Append("title=@title,");
            strSql.Append("ImgPath=@imgpath,");
            strSql.Append("modifytime=@modifytime");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@imgpath", OleDbType.VarChar,0),
					new OleDbParameter("@modifytime", OleDbType.Date,4),
                    new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.ImgPath;
            parameters[2].Value = model.ModifyTime;
            parameters[3].Value = model.Id;

            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public static string GetSingle(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Banner where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = id;
            DataSet dataSet = DbHelperOleDb.Query(strSql.ToString(), parameters);
            DataTable table= dataSet.Tables[0];
            string result= JsonConvert.SerializeObject(table); 
            return result;
        }

    }
}
