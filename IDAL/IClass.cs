using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZGZY.IDAL
{
    /// <summary>
    /// 课程Class接口
    /// </summary>
    public interface IClass
    {
        DataTable GetClassByUserId(int id);

        DataTable GetAllClass(string name);

        DataTable GetClassCombotree(string name);

        int AddClass(ZGZY.Model.Class classes);

        bool EditClass(ZGZY.Model.Class classes);

        bool DeleteClass(string classes);



        //int GetClassUserCount(string classes);

        //DataTable GetPageUserClass(string classes, string order, int pagesize, int pageindex);
            


    }
}
