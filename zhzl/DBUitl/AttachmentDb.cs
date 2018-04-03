using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBUitl
{
    public class AttachmentDb
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(Attachment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Attachment(");
            strSql.Append("Title,Content,Path,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@title,@content,@path,@createtime)");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@content", OleDbType.LongVarChar,0),
					new OleDbParameter("@path", OleDbType.LongVarChar,0),
					new OleDbParameter("@createtime", OleDbType.Date)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Content;
            parameters[2].Value = model.Path;
            parameters[3].Value = model.CreateTime;
            int id = DbHelperOleDb.ExecuteSql2(strSql.ToString(), parameters);
            return id;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(Attachment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Attachment set ");
            strSql.Append("title=@title,");
            strSql.Append("content=@content,");
            strSql.Append("path=@path,");
            strSql.Append("modifytime=@modifytime");
            strSql.Append(" where id=@id");
            OleDbParameter[] parameters = {
					new OleDbParameter("@title", OleDbType.VarChar,0),
					new OleDbParameter("@content", OleDbType.LongVarChar,0),
					new OleDbParameter("@path", OleDbType.VarChar,0),
                    new OleDbParameter("@id", OleDbType.VarChar,0),
					new OleDbParameter("@modifytime", OleDbType.Date,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Content;
            parameters[2].Value = model.Path;
            parameters[3].Value = model.Id;
            parameters[4].Value = model.ModifyTime;

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
            strSql.Append("delete from Attachment ");
            strSql.Append(" where  id=@id");
            OleDbParameter[] parameters = {
                    new OleDbParameter("@id", OleDbType.VarChar,0)};
            parameters[0].Value = id;
            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString(),parameters);
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
            strSql.Append(" FROM Attachment order by CreateTime desc ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperOleDb.Query(strSql.ToString());
        }
    }
}
