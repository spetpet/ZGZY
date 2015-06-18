using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ZGZY.WebUI.admin.ashx
{
    /// <summary>
    /// bg_class 的摘要说明
    /// </summary>
    public class bg_class : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"];
            ZGZY.Model.UserOperateLog userOperateLog = null;   //操作日志对象
            try
            {
                ZGZY.Model.User user = ZGZY.Common.UserHelper.GetUser(context);   //获取cookie里的课程对象
                userOperateLog = new Model.UserOperateLog();
                userOperateLog.UserIp = context.Request.UserHostAddress;
                userOperateLog.UserName = user.UserId;
                switch (action)
                {
                    case "getall":
                        
                        context.Response.Write(new ZGZY.BLL.Class().GetAllClass("1=1"));
                        //context.Response.Write("test ok");
                        break;

                    case "getcombotree":

                        context.Response.Write(new ZGZY.BLL.Class().GetClassCombotree("1=1"));
                        //context.Response.Write("test ok");
                        break;
                    case "searchUserClass":
                        int classIds = int.Parse(context.Request.Params["ClassId"]);
                       // string sortUserClass = context.Request.Params["sort"];  //排序列
                       // string orderUserClass = context.Request.Params["order"];  //排序方式 asc或者desc
                       // int pageindexUserClass = int.Parse(context.Request.Params["page"]);
                       // int pagesizeUserClass = int.Parse(context.Request.Params["rows"]);

                        string strJsonUserClass = new ZGZY.BLL.User().GetUserByClassIdtojson(classIds);
                        context.Response.Write(strJsonUserClass);
                        userOperateLog.OperateInfo = "查询用户课程";
                        userOperateLog.IfSuccess = true;
                        userOperateLog.Description = "查询课程Id：" + classIds ;
                        ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;

                    case "add":
                        if (user != null && new ZGZY.BLL.Authority().IfAuthority("class", "add", user.Id))
                        {
                            string ui_class_classkind_add = context.Request.Params["ui_class_classkind_add"] ?? "";
                            string ui_class_classname_add = context.Request.Params["ui_class_classname_add"] ?? "";
                            string ui_class_classaddress_add = context.Request.Params["ui_class_classaddress_add"] ?? "";
                            string ui_class_classabout_add = context.Request.Params["ui_class_classabout_add"] ?? "";
                            string ui_class_checktype_edit = context.Request.Params["ui_class_checktype_edit"] ?? "";
                            string ui_class_teacher_edit = context.Request.Params["ui_class_teacher_edit"] ?? "";
                            string ui_class_uploadfile_add = context.Request.Params["ui_class_uploadfile"] ?? "";
                            DateTime ui_class_classstartdate_add = Convert.ToDateTime(context.Request.Params["ui_class_classstartdate_add"] ?? null);
                            DateTime ui_class_classfinishdate_add = Convert.ToDateTime(context.Request.Params["ui_class_classfinishdate_add"] ?? null);
                            

                            ZGZY.Model.Class ClassAdd = new Model.Class();
                            ClassAdd.ClassName=ui_class_classname_add.Trim();
                            ClassAdd.ClassKind=ui_class_classkind_add.Trim();
                            ClassAdd.ClassAddress=ui_class_classaddress_add.Trim();
                            ClassAdd.ClassAbout=ui_class_classabout_add.Trim();
                            ClassAdd.ClassStartDate = ui_class_classstartdate_add;
                            ClassAdd.ClassFinishDate = ui_class_classfinishdate_add;
                            ClassAdd.CheckType = ui_class_checktype_edit;
                            ClassAdd.Teacher = ui_class_teacher_edit;
                            ClassAdd.uploadfile = ui_class_uploadfile_add;

                            int ClassId = new ZGZY.BLL.Class().AddClass(ClassAdd);
                            if (ClassId > 0)
                            {
                                userOperateLog.OperateInfo = "添加课程";
                                userOperateLog.IfSuccess = true;
                                userOperateLog.Description = "添加成功，课程主键：" + ClassId;
                                context.Response.Write("{\"msg\":\"添加成功！\",\"success\":true}");
                            }
                            else
                            {
                                userOperateLog.OperateInfo = "添加课程";
                                userOperateLog.IfSuccess = false;
                                userOperateLog.Description = "添加失败";
                                context.Response.Write("{\"msg\":\"添加失败！\",\"success\":false}");
                            }
                        }
                        else
                        {
                            userOperateLog.OperateInfo = "添加课程";
                            userOperateLog.IfSuccess = false;
                            userOperateLog.Description = "无权限，请联系管理员";
                            context.Response.Write("{\"msg\":\"无权限，请联系管理员！\",\"success\":false}");
                        }
                        ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;

                    case "edit":
                        if (user != null && new ZGZY.BLL.Authority().IfAuthority("class", "edit", user.Id))
                        {
                            int id = Convert.ToInt32(context.Request.Params["ui_class_classid_edit"]??"0");
                            string ui_class_classkind_edit = context.Request.Params["ui_class_classkind_edit"] ?? "";
                            string ui_class_classname_edit = context.Request.Params["ui_class_classname_edit"] ?? "";
                            string ui_class_classaddress_edit = context.Request.Params["ui_class_classaddress_edit"] ?? "";
                            string ui_class_classabout_edit = context.Request.Params["ui_class_classabout_edit"] ?? "";
                            string ui_class_checktype_edit = context.Request.Params["ui_class_checktype_edit"] ?? "";
                            string ui_class_teacher_edit = context.Request.Params["ui_class_teacher_edit"] ?? "";
                            string ui_class_uploadfile_edit = context.Request.Params["ui_class_uploadfile"] ?? "";
                            DateTime ui_class_classstartdate_edit = Convert.ToDateTime(context.Request.Params["ui_class_classstartdate_edit"] ?? null);
                            DateTime ui_class_classfinishdate_edit = Convert.ToDateTime(context.Request.Params["ui_class_classfinishdate_edit"] ?? null);

                            ZGZY.Model.Class ClassEdit = new Model.Class();
                            ClassEdit.Id = id;
                            ClassEdit.ClassName = ui_class_classname_edit.Trim();
                            ClassEdit.ClassKind = ui_class_classkind_edit.Trim();
                            ClassEdit.ClassAddress = ui_class_classaddress_edit.Trim();
                            ClassEdit.ClassAbout = ui_class_classabout_edit.Trim();
                            ClassEdit.ClassStartDate = ui_class_classstartdate_edit;
                            ClassEdit.ClassFinishDate = ui_class_classfinishdate_edit;
                            ClassEdit.CheckType = ui_class_checktype_edit;
                            ClassEdit.Teacher = ui_class_teacher_edit;
                            ClassEdit.uploadfile = ui_class_uploadfile_edit;

                            if (new ZGZY.BLL.Class().EditClass(ClassEdit))
                            {
                                userOperateLog.OperateInfo = "修改课程";
                                userOperateLog.IfSuccess = true;
                                userOperateLog.Description = "修改成功，课程主键：" + ClassEdit.Id;
                                context.Response.Write("{\"msg\":\"修改成功！\",\"success\":true}");
                            }
                            else
                            {
                                userOperateLog.OperateInfo = "修改课程";
                                userOperateLog.IfSuccess = false;
                                userOperateLog.Description = "修改失败";
                                context.Response.Write("{\"msg\":\"修改失败！\",\"success\":false}");
                            }
                        }
                        else
                        {
                            userOperateLog.OperateInfo = "修改课程";
                            userOperateLog.IfSuccess = false;
                            userOperateLog.Description = "无权限，请联系管理员";
                            context.Response.Write("{\"msg\":\"无权限，请联系管理员！\",\"success\":false}");
                        }
                        ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;

                    case "delete":
                        if (user != null && new ZGZY.BLL.Authority().IfAuthority("class", "delete", user.Id))
                        {
                            string ids = context.Request.Params["id"].Trim(',');
                            if (new ZGZY.BLL.Class().DeleteClass(ids))
                            {
                                userOperateLog.OperateInfo = "删除课程";
                                userOperateLog.IfSuccess = true;
                                userOperateLog.Description = "删除成功，课程主键：" + ids;
                                context.Response.Write("{\"msg\":\"删除成功！\",\"success\":true}");
                            }
                            else
                            {
                                userOperateLog.OperateInfo = "删除课程";
                                userOperateLog.IfSuccess = false;
                                userOperateLog.Description = "删除失败";
                                context.Response.Write("{\"msg\":\"删除失败！\",\"success\":false}");
                            }
                        }
                        else
                        {
                            userOperateLog.OperateInfo = "删除课程";
                            userOperateLog.IfSuccess = false;
                            userOperateLog.Description = "无权限，请联系管理员";
                            context.Response.Write("{\"msg\":\"无权限，请联系管理员！\",\"success\":false}");
                        }
                        ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;

                    case "searchuserclass":
                        int UserIds = int.Parse(context.Request.Params["userId"]);
                        //string sortUserClass = context.Request.Params["sort"];  //排序列
                        //string orderUserClass = context.Request.Params["order"];  //排序方式 asc或者desc
                        //int pageindexUserClass = int.Parse(context.Request.Params["page"]);
                        //int pagesizeUserClass = int.Parse(context.Request.Params["rows"]);

                        //string strJsonDepartmentUser = new ZGZY.BLL.Class().GetClassByUserId(int.Parse(UserIds));
                       string strJsonDepartmentUser= Common.JsonHelper.ToJson(new ZGZY.BLL.Class().GetClassByUserId(UserIds));
                       context.Response.Write(strJsonDepartmentUser);
                        userOperateLog.OperateInfo = "查询课程用户";
                        userOperateLog.IfSuccess = true;
                        //userOperateLog.Description = "查询课程Id：" + UserIds + " 排序：" + sortDepartmentUser + " " + orderUserClass + " 页码/每页大小：" + pageindexUserClass + " " + pagesizeUserClass;
                        userOperateLog.Description = "查询课程Id：" + UserIds;
                        ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;

                    default:
                        context.Response.Write("{\"msg\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "课程功能异常";
                userOperateLog.IfSuccess = false;
                userOperateLog.Description = ZGZY.Common.JsonHelper.StringFilter(ex.Message);
                ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}