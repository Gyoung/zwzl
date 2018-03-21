using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBUitl
{
    public class DbUtil
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static bool Add(Product model)
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
					new OleDbParameter("@imgpath", OleDbType.VarChar,4),
					new OleDbParameter("@createtime", OleDbType.Date)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Content;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.ImgPath;
            parameters[4].Value = model.CreateTime;
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
    }
}
