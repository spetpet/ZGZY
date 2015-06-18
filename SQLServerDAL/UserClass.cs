using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace ZGZY.SQLServerDAL
{
    /// <summary>
    /// 用户部门（SQL Server数据库实现）
    /// </summary>
    public class UserClass : ZGZY.IDAL.IUserClass
    {

        /// <summary>
        /// 设置课程参与者
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        public bool SetUserSingle(List<Model.UserClass> user_addList, List<Model.UserClass> user_deleteList)
        {
            Hashtable sqlStringList = new Hashtable();
            for (int i = 0; i < user_deleteList.Count; i++)  //删除的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from tbUserClass ");
                sb.Append("where UserId=@UserId and ClassId=@ClassId");
                SqlParameter[] para1 = {
					                   new SqlParameter("@UserId", SqlDbType.Int,10),
                                       new SqlParameter("@ClassId", SqlDbType.Int,10)
                                       };
                para1[0].Value = user_deleteList[i].UserId;
                para1[1].Value = user_deleteList[i].ClassId;
                sqlStringList.Add(sb, para1);
            }
            for (int i = 0; i < user_addList.Count; i++)  //新增的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into tbUserClass(");
                sb.Append("UserId,ClassId)");
                sb.Append(" values (");
                sb.Append("@UserId,@ClassId)");
                sb.Append(";select @@IDENTITY");
                SqlParameter[] para2 = { 
                                       new SqlParameter("@UserId", SqlDbType.Int,10),
                                       new SqlParameter("@ClassId", SqlDbType.Int,10)
                                       };
                para2[0].Value = user_addList[i].UserId;
                para2[1].Value = user_addList[i].ClassId;
                sqlStringList.Add(sb, para2);    //【【sb不能ToString() 否则报hashtable不能有相同键的错误】】
            }
            try
            {
                ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, sqlStringList);   //批量插入（事务）
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 设置用户课程（单个用户）
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        public bool SetClassSingle(List<Model.UserClass> class_addList, List<Model.UserClass> class_deleteList)
        {
            Hashtable sqlStringList = new Hashtable();
            for (int i = 0; i < class_deleteList.Count; i++)  //删除的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from tbUserClass ");
                sb.Append("where UserId=@UserId and ClassId=@ClassId");
                SqlParameter[] para1 = {
					                   new SqlParameter("@UserId", SqlDbType.Int,10),
                                       new SqlParameter("@ClassId", SqlDbType.Int,10)
                                       };
                para1[0].Value = class_deleteList[i].UserId;
                para1[1].Value = class_deleteList[i].ClassId;
                sqlStringList.Add(sb, para1);
            }
            for (int i = 0; i < class_addList.Count; i++)  //新增的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into tbUserClass(");
                sb.Append("UserId,ClassId)");
                sb.Append(" values (");
                sb.Append("@UserId,@ClassId)");
                sb.Append(";select @@IDENTITY");
                SqlParameter[] para2 = { 
                                       new SqlParameter("@UserId", SqlDbType.Int,10),
                                       new SqlParameter("@ClassId", SqlDbType.Int,10)
                                       };
                para2[0].Value = class_addList[i].UserId;
                para2[1].Value = class_addList[i].ClassId;
                sqlStringList.Add(sb, para2);    //【【sb不能ToString() 否则报hashtable不能有相同键的错误】】
            }
            try
            {
                ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, sqlStringList);   //批量插入（事务）
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置用户课程（批量设置）
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        public bool SetClassBatch(List<Model.UserClass> class_addList, List<Model.UserClass> class_deleteList)
        {
            Hashtable sqlStringListDelete = new Hashtable();
            Hashtable sqlStringListAdd = new Hashtable();
            for (int i = 0; i < class_deleteList.Count; i++)  //删除的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from tbUserClass ");
                sb.Append("where UserId=@UserId");
                SqlParameter[] para1 = {
                                       new SqlParameter("@UserId", SqlDbType.Int,10)   //批量设置先删除当前用户的所有课程
                                       };
                para1[0].Value = class_deleteList[i].UserId;
                sqlStringListDelete.Add(sb, para1);
            }
            for (int i = 0; i < class_addList.Count; i++)  //新增的用户课程
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into tbUserClass(");
                sb.Append("UserId,ClassId)");
                sb.Append(" values (");
                sb.Append("@UserId,@ClassId)");
                sb.Append(";select @@IDENTITY");
                SqlParameter[] para2 = { 
                                       new SqlParameter("@UserId", SqlDbType.Int,10),
                                       new SqlParameter("@ClassId", SqlDbType.Int,10)
                                       };
                para2[0].Value = class_addList[i].UserId;
                para2[1].Value = class_addList[i].ClassId;
                sqlStringListAdd.Add(sb, para2);    //【【sb不能ToString() 否则报hashtable不能有相同键的错误】】
            }
            try
            {
                ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, sqlStringListDelete);   //先删
                ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, sqlStringListAdd);   //再加
                return true;
            }
            catch
            {
                return false;
            }
        }

        
       

        /// <summary>
        /// 获取用户下的课程数量
        /// </summary>
        public int GetUserClassCount(string userIds)
        {
            string sql = "select COUNT(*) from tbUserClass uc join tbUser u on uc.UserId = u.Id where u.Id = " + userIds;
            object count = ZGZY.Common.SqlHelper.ExecuteScalar(ZGZY.Common.SqlHelper.connStr, CommandType.Text, sql, null);
            return Convert.ToInt32(count);
        }

        /// <summary>
        /// 获取用户下的课程（分页）
        /// </summary>
        public DataTable GetPagerUserClass(string userIds, string order, int pageSize, int pageIndex)
        {
            int beginIndex = (pageIndex - 1) * pageSize + 1;   //分页开始页码
            int endIndex = pageIndex * pageSize;   //分页结束页码
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct(T.Id),T.className,T.AddDate,T.ClassStartDate,t.score from (");
            strSql.Append(" select row_number() over(order by c." + order + ")");
            strSql.Append(" as Rownum,c.Id,c.Classname,c.AddDate,c.ClassStartDate,uc.score from tbClass c");
            strSql.Append(" join tbUserClass uc on c.Id = uc.ClassId");
            strSql.Append(" join tbUser u on uc.UserId = u.Id");
            strSql.Append(" where u.Id in (" + userIds + ")) as T");
            strSql.Append(" where T.Rownum between " + beginIndex + " and " + endIndex + "");
            return ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, strSql.ToString(), null);
        }

    }
}
