using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace ZGZY.BLL
{
    public class Class
    {
        private static readonly ZGZY.IDAL.IClass dal = ZGZY.DALFactory.Factory.GetClassDAL();

        public DataTable GetClassByUserId(int id)
        {
            return dal.GetClassByUserId(id);
        }

        public string GetAllClass(string name)
        {
            return ZGZY.Common.JsonHelper.ToJson(dal.GetAllClass(null));
        }

        public string GetClassCombotree(string name)
        {
            string combotree = "[{\"id\":\"1\",\"ParentId\":\"0\",\"Sort\":\"1\",\"text\":\"所有课程\",\"children\":" + ZGZY.Common.JsonHelper.ToJson(dal.GetClassCombotree(null))+"}]";
            return combotree;
        }

        public int AddClass(ZGZY.Model.Class Classes)
        {
            return dal.AddClass(Classes);
        }

        public bool EditClass(ZGZY.Model.Class Classes)
        {
            return dal.EditClass(Classes);
        }

        public bool DeleteClass(string ClassIds)
        {
            return dal.DeleteClass(ClassIds);
        }

        



    }
}
