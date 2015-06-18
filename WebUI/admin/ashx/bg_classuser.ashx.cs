using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace ZGZY.WebUI.admin.ashx
{
    /// <summary>
    /// bg_classuser 的摘要说明
    /// </summary>
    public class bg_classuser : IHttpHandler
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

                    case "getusercombo":

                        StringBuilder sb = new StringBuilder();
                        sb.Append("select u.Id id,u.UserName text,'1' ParentId,u.Id Sort from tbUser u");
                        DataTable usercombo_dt=ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr,CommandType.Text,sb.ToString());
                        string combotree = "[{\"id\":\"1\",\"ParentId\":\"0\",\"Sort\":\"1\",\"text\":\"所有员工\",\"children\":" + ZGZY.Common.JsonHelper.ToJson(usercombo_dt) + "}]";
                        context.Response.Write(combotree);
                        //context.Response.Write("test ok");
                        break;

                    case "getuserbyclassid":
                       string temp_classid=context.Request.Params["classid"] ?? "";
                       StringBuilder user_sb = new StringBuilder();
                       user_sb.Append("select uc.ClassId ClassId,uc.UserId Id,u.UserId UserId,u.UserName username,uc.score Score from tbUserClass uc LEFT JOIN tbUser u on u.Id=uc.UserId where uc.ClassId='" + temp_classid + "'");
                       DataTable user_dt = ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, user_sb.ToString());
                       context.Response.Write(ZGZY.Common.JsonHelper.ToJson(user_dt));
                       break;

                    case "setuser":
                        string userid = context.Request.Params["ui_class_setuser_user"] ?? "";
                        string classid = context.Request.Params["ui_class_setuser_classid"] ?? "";
                        if (classid != "" && new BLL.UserClass().SetUserSingle(Convert.ToInt32(classid), userid))
                        {
                            userOperateLog.OperateInfo = "设置课程参与者";
                            userOperateLog.IfSuccess = true;
                            userOperateLog.Description = "设置成功，课程主键：" + classid + " 用户主键：" + userid;
                            context.Response.Write("{\"msg\":\"设置成功！\",\"success\":true}");
                        }
                        else
                        {
                            userOperateLog.OperateInfo = "设置用户部门";
                            userOperateLog.IfSuccess = false;
                            userOperateLog.Description = "设置失败，课程主键：" + classid + " 用户主键：" + userid;
                            context.Response.Write("{\"msg\":\"设置失败！\",\"success\":true}");
                        }
                        //context.Response.Write("{\"msg\":\""+userid.ToString()+"\",\"success\":true}");
                        break;

                    case "updatescore":
                        
                        
                        if (user != null && new ZGZY.BLL.Authority().IfAuthority("department", "edit", user.Id))
                        {
                            string score_classid = context.Request.Params["ClassId"] ?? "";
                            string score_userid = context.Request.Params["Id"] ?? "";
                            string score_score = context.Request.Params["Score"] ?? "";

                            StringBuilder score_sb = new StringBuilder();
                            score_sb.Append("update tbUserClass set score='"+score_score+"' where UserId='"+score_userid+"' and ClassId='"+score_classid+"'");
                            int score_result = ZGZY.Common.SqlHelper.ExecuteNonQuery(ZGZY.Common.SqlHelper.connStr, CommandType.Text, score_sb.ToString());
                            if (score_result!=0)
                            {
                                userOperateLog.OperateInfo = "编辑分数";
                                userOperateLog.IfSuccess = true;
                                userOperateLog.Description = "编辑分数成功，课程主键：" + score_classid + " 用户主键：" + score_userid;
                                context.Response.Write("{\"msg\":\"设置成功！\",\"success\":true}");
                            }
                            else
                            {
                                userOperateLog.OperateInfo = "编辑分数";
                                userOperateLog.IfSuccess = false;
                                userOperateLog.Description = "编辑分数失败，课程主键：" + score_classid + " 用户主键：" + score_userid;
                                context.Response.Write("{\"msg\":\"设置失败！\",\"success\":true}");
                            }
                        }
                        else
                        {
                            userOperateLog.OperateInfo = "编辑分数";
                            userOperateLog.IfSuccess = false;
                            userOperateLog.Description = "无权限，请联系管理员";
                            context.Response.Write("{\"msg\":\"无权限，请联系管理员！\",\"success\":true}");
                        }
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