using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace ZGZY.SQLServerDAL
{
    public class Class : ZGZY.IDAL.IClass
    {
        public DataTable GetClassByUserId(int id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select c.Id ClassId,c.ClassName ClassName from tbUserClass uc");
            sb.Append(" join tbClass c on c.Id=uc.ClassId");
            sb.Append(" where uc.UserId=@Id");

            return ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, sb.ToString(), new SqlParameter("@Id", id));
        }

        public DataTable GetAllClass(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select t.*,dbo.getuserbyclass(t.id) userids from tbClass t");
            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" where " + name);
            }
            sb.Append(" order by t.ClassStartDate");
            return ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, sb.ToString(), null);
                            
        }

        public DataTable GetClassCombotree(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select t.Id id,t.ClassName text,t.ParentId ParentId,t.Id Sort  from tbClass t");
            if (!string.IsNullOrEmpty(name))
            {
                sb.Append(" where " + name);
            }
            sb.Append(" order by AddDate");
            return ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, sb.ToString(), null);

        }

        

        /// <summary>
        /// 添加课程
        /// </summary>
        public int AddClass(Model.Class Classes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tbClass(ClassName,ClassStartDate,ClassFinishDate,ClassKindId,ClassAddress,ClassAbout,checktype,teacher,uploadfile)");
            strSql.Append(" values ");
            strSql.Append("(@ClassName,@ClassStartDate,@ClassFinishDate,@ClassKind,@ClassAddress,@ClassAbout,@CheckType,@Teacher,@uploadfile)");
            strSql.Append(";SELECT @@IDENTITY");   //返回插入课程的主键
            SqlParameter[] paras = { 
                                   new SqlParameter("@ClassName",Classes.ClassName),
                                   new SqlParameter("@ClassStartDate",Classes.ClassStartDate),
                                   new SqlParameter("@ClassFinishDate",Classes.ClassFinishDate),
                                   new SqlParameter("@ClassKind",Classes.ClassKind),
                                   new SqlParameter("@ClassAddress",Classes.ClassAddress),
                                   new SqlParameter("@ClassAbout",Classes.ClassAbout),
                                   new SqlParameter("@CheckType",Classes.CheckType),
                                   new SqlParameter("@Teacher",Classes.Teacher),
                                   new SqlParameter("@uploadfile",Classes.uploadfile)
                                   };
            return Convert.ToInt32(ZGZY.Common.SqlHelper.ExecuteScalar(ZGZY.Common.SqlHelper.connStr, CommandType.Text, strSql.ToString(), paras));
        }

        /// <summary>
        /// 修改培训课程
        /// </summary>
        public bool EditClass(Model.Class Class)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tbClass set ");
            strSql.Append("ClassName=@ClassName,ClassKindId=@ClassKind,ClassAbout=@ClassAbout,ClassAddress=@ClassAddress,ClassStartDate=@ClassStartDate,ClassFinishDate=@ClassFinishDate,checktype=@CheckType,teacher=@Teacher,uploadfile=@uploadfile ");
            strSql.Append("where Id=@Id");

            SqlParameter[] paras = { 
                                   new SqlParameter("@ClassName",Class.ClassName),
                                   new SqlParameter("@ClassKind",Class.ClassKind),
                                   new SqlParameter("@ClassAbout",Class.ClassName),
                                   new SqlParameter("@ClassAddress",Class.ClassAddress),
                                   new SqlParameter("@ClassStartDate",Class.ClassStartDate),
                                   new SqlParameter("@ClassFinishDate",Class.ClassFinishDate),
                                   new SqlParameter("@Id",Class.Id),
                                   new SqlParameter("@CheckType",Class.CheckType),
                                   new SqlParameter("@Teacher",Class.Teacher),
                                   new SqlParameter("@uploadfile",Class.uploadfile)
                                   };
            object obj = ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, CommandType.Text, strSql.ToString(), paras);
            if (Convert.ToInt32(obj) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除培训课程
        /// </summary>
        public bool DeleteClass(string ClassIds)
        {
            List<string> list = new List<string>();
            list.Add("delete from tbClass where Id in (" + ClassIds + ")");
           // list.Add("delete from tbUserClass where ClassId in (" + ClassIds + ")");

            try
            {
                ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, list);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
