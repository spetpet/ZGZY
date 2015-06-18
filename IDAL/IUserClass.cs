using System.Collections.Generic;
using System;
using System.Data;

namespace ZGZY.IDAL
{
    /// <summary>
    /// 用户课程接口（不同的数据库访问类实现接口达到多数据库的支持）
    /// </summary>
    public interface IUserClass
    {
        /// <summary>
        /// 设置课程参与者
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        bool SetUserSingle(List<ZGZY.Model.UserClass> user_addList, List<ZGZY.Model.UserClass> user_deleteList);
        
        /// <summary>
        /// 设置用户课程（单个用户）
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        bool SetClassSingle(List<ZGZY.Model.UserClass> class_addList, List<ZGZY.Model.UserClass> class_deleteList);

        /// <summary>
        /// 设置用户课程（批量设置）
        /// </summary>
        /// <param name="class_addList">要增加的</param>
        /// <param name="class_deleteList">要删除的</param>
        bool SetClassBatch(List<ZGZY.Model.UserClass> class_addList, List<ZGZY.Model.UserClass> class_deleteList);

        
        /// <summary>
        /// 获取用户下的课程数量
        /// </summary>
        int GetUserClassCount(string userIds);

        /// <summary>
        /// 获取用户下的课程（分页）
        /// </summary>
        DataTable GetPagerUserClass(string userIds, string order, int pageSize, int pageIndex);

    }
}
