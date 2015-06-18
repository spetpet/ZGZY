using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ZGZY.BLL
{
    /// <summary>
    /// 用户课程（BLL）
    /// </summary>
    public class UserClass
    {
        private static readonly ZGZY.IDAL.IUserClass dal = ZGZY.DALFactory.Factory.GetUserClassDAL();

        /// <summary>
        /// 设置用户课程（单个用户）
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <param name="classIds">课程id，多个用逗号隔开</param>
        public bool SetClassSingle(int userId, string classIds)
        {
            DataTable dt_user_class_old = new ZGZY.BLL.Class().GetClassByUserId(userId);  //用户之前拥有的课程
            List<ZGZY.Model.UserClass> class_addList = new List<ZGZY.Model.UserClass>();     //需要插入课程的sql语句集合
            List<ZGZY.Model.UserClass> class_deleteList = new List<ZGZY.Model.UserClass>();     //需要删除课程的sql语句集合

            string[] str_class = classIds.Trim(',').Split(',');    //传过来用户勾选的课程（有去勾的也有新勾选的）

            ZGZY.Model.UserClass userclassdelete = null;
            ZGZY.Model.UserClass userclassadd = null;
            for (int i = 0; i < dt_user_class_old.Rows.Count; i++)
            {
                //等于-1说明用户去掉勾选了某个课程 需要删除
                if (Array.IndexOf(str_class, dt_user_class_old.Rows[i]["Classid"].ToString()) == -1)
                {
                    userclassdelete = new ZGZY.Model.UserClass();
                    userclassdelete.ClassId = Convert.ToInt32(dt_user_class_old.Rows[i]["Classid"].ToString());
                    userclassdelete.UserId = userId;
                    class_deleteList.Add(userclassdelete);
                }
            }

            if (!string.IsNullOrEmpty(classIds))
            {
                for (int j = 0; j < str_class.Length; j++)
                {
                    //等于0那么原来的课程没有 是用户新勾选的
                    if (dt_user_class_old.Select("Classid = '" + str_class[j] + "'").Length == 0)
                    {
                        userclassadd = new ZGZY.Model.UserClass();
                        userclassadd.UserId = userId;
                        userclassadd.ClassId = Convert.ToInt32(str_class[j]);
                        class_addList.Add(userclassadd);
                    }
                }
            }
            if (class_addList.Count == 0 && class_deleteList.Count == 0)
                return true;
            else
                return dal.SetClassSingle(class_addList, class_deleteList);
        }

        /// <summary>
        /// 设置用户课程（批量设置）
        /// </summary>
        /// <param name="userIds">用户主键，多个用逗号隔开</param>
        /// <param name="classIds">课程id，多个用逗号隔开</param>
        public bool SetClassBatch(string userIds, string classIds)
        {
            List<ZGZY.Model.UserClass> class_addList = new List<ZGZY.Model.UserClass>();     //需要插入课程的sql语句集合
            List<ZGZY.Model.UserClass> class_deleteList = new List<ZGZY.Model.UserClass>();     //需要删除课程的sql语句集合
            string[] str_userid = userIds.Trim(',').Split(',');
            string[] str_class = classIds.Trim(',').Split(',');

            ZGZY.Model.UserClass userclassdelete = null;
            ZGZY.Model.UserClass userclassadd = null;
            for (int i = 0; i < str_userid.Length; i++)
            {
                //批量设置先删除当前用户的所有课程
                userclassdelete = new ZGZY.Model.UserClass();
                userclassdelete.UserId = Convert.ToInt32(str_userid[i]);
                class_deleteList.Add(userclassdelete);

                if (!string.IsNullOrEmpty(classIds))
                {
                    //再添加设置的课程
                    for (int j = 0; j < str_class.Length; j++)
                    {
                        userclassadd = new ZGZY.Model.UserClass();
                        userclassadd.UserId = Convert.ToInt32(str_userid[i]);
                        userclassadd.ClassId = Convert.ToInt32(str_class[j]);
                        class_addList.Add(userclassadd);
                    }
                }
            }
            return dal.SetClassBatch(class_addList, class_deleteList);
        }

        /// <summary>
        /// 设置课程参与者
        /// </summary>
        /// <param name="classId">用户主键</param>
        /// <param name="userIds">课程id，多个用逗号隔开</param>
        public bool SetUserSingle(int classId, string userIds)
        {
            DataTable dt_class_user_old = new ZGZY.BLL.User().GetUserByClassId(classId);  //课程之前的参与者
            List<ZGZY.Model.UserClass> user_addList = new List<ZGZY.Model.UserClass>();     //需要插入用户的sql语句集合
            List<ZGZY.Model.UserClass> user_deleteList = new List<ZGZY.Model.UserClass>();     //需要删除用户的sql语句集合

            string[] str_user = userIds.Trim(',').Split(',');    //传过来勾选的用户（有去勾的也有新勾选的）

            ZGZY.Model.UserClass classuserdelete = null;
            ZGZY.Model.UserClass classuseradd = null;
            for (int i = 0; i < dt_class_user_old.Rows.Count; i++)
            {
                //等于-1说明用户去掉勾选了某个课程 需要删除
                if (Array.IndexOf(str_user, dt_class_user_old.Rows[i]["id"].ToString()) == -1)
                {
                    classuserdelete = new ZGZY.Model.UserClass();
                    classuserdelete.UserId = Convert.ToInt32(dt_class_user_old.Rows[i]["id"].ToString());
                    classuserdelete.ClassId = classId;
                    user_deleteList.Add(classuserdelete);
                }
            }

            if (!string.IsNullOrEmpty(userIds))
            {
                for (int j = 0; j < str_user.Length; j++)
                {
                    //等于0那么原来的课程没有 是用户新勾选的
                    if (dt_class_user_old.Select("id = '" + str_user[j] + "'").Length == 0)
                    {
                        classuseradd = new ZGZY.Model.UserClass();
                        classuseradd.UserId = Convert.ToInt32(str_user[j]);
                        classuseradd.ClassId = classId;
                        user_addList.Add(classuseradd);
                    }
                }
            }
            if (user_addList.Count == 0 && user_deleteList.Count == 0)
                return true;
            else
                return dal.SetUserSingle(user_addList, user_deleteList);
        }


        public string GetPagerUserClass(string userIds, string order, int pageSize, int pageIndex)
        {
            if (ZGZY.Common.SqlInjection.GetString(userIds))   //简单sql防注入
                userIds = "";
            if (ZGZY.Common.SqlInjection.GetString(order))
                order = "AddDate asc";
            int totalCount = dal.GetUserClassCount(userIds);
            DataTable dt = dal.GetPagerUserClass(userIds, order, pageSize, pageIndex);

            string strjson = ZGZY.Common.JsonHelper.ToJson(dt);
            return "{\"total\": " + totalCount.ToString() + ",\"rows\":" + strjson + "}";
        }

    }
}
