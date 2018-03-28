using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBUitl
{
    public class ProductDb
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Product(");
            strSql.Append("Title,Content,Remark,ImgPath,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@title,@content,@remark,@imgpath,@createtime)");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@content", OleDbType.LongVarChar,0),
					new OleDbParameter("@remark", OleDbType.LongVarChar,0),
					new OleDbParameter("@imgpath", OleDbType.VarChar,0),
					new OleDbParameter("@createtime", OleDbType.Date)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Content;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.ImgPath;
            parameters[4].Value = model.CreateTime;
            int id = DbHelperOleDb.ExecuteSql2(strSql.ToString(), parameters);
            return id;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(Product model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Product set ");
            strSql.Append("title=@title,");
            strSql.Append("content=@content,");
            strSql.Append("ImgPath=@imgpath,");
            strSql.Append("remark=@remark,");
            strSql.Append("modifytime=@modifytime");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@content", OleDbType.LongVarChar,0),
					new OleDbParameter("@imgpath", OleDbType.VarChar,0),
					new OleDbParameter("@remark", OleDbType.LongVarChar,0),
					new OleDbParameter("@modifytime", OleDbType.Date,4),
                    new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Content;
            parameters[2].Value = model.ImgPath;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.ModifyTime;
            parameters[5].Value = model.Id;

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
        /// 删除一条数据
        /// </summary>
        public static bool Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Product ");
            strSql.Append(" where  id=@id");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = id;
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
        public static DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Product ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperOleDb.Query(strSql.ToString());
        }



        /// <summary>
        /// 获得数据列表
        /// </summary>
        public static string GetSingle(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Product where id=@id ");
            OleDbParameter[] parameters = {
					new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = id;
            DataSet dataSet = DbHelperOleDb.Query(strSql.ToString(), parameters);
            DataTable table = dataSet.Tables[0];
            string result = JsonConvert.SerializeObject(table);
            return result;
        }

    }
}
