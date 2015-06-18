using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data;


namespace ZGZY.WebUI.admin.ashx
{
    /// <summary>
    /// bg_pkt 的摘要说明
    /// </summary>
    public class bg_checkin : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"];
            ZGZY.Model.UserOperateLog userOperateLog = null;   //操作日志对象
            try
            {
                ZGZY.Model.User user = ZGZY.Common.UserHelper.GetUser(context);   //获取cookie里的用户对象
                userOperateLog = new Model.UserOperateLog();
                userOperateLog.UserIp = context.Request.UserHostAddress;
                userOperateLog.UserName = user.UserId;

                switch (action)
                {
                    case "getall"://获取checkin
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        string strWhere = "";


                        string ui_checkin_user = context.Request.Params["ui_checkin_user"] ?? "";
                        string ui_checkin_content = context.Request.Params["ui_checkin_content"] ?? "";
                        string ui_checkin_adddatestart = context.Request.Params["ui_checkin_adddatestart"] ?? "";
                        string ui_checkin_adddateend = context.Request.Params["ui_checkin_adddateend"] ?? "";

                        if (ui_checkin_user.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_checkin_user))   //防止sql注入
                            strWhere += string.Format(" and u.UserName like '%{0}%'", ui_checkin_user.Trim());
                        if (ui_checkin_content.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_checkin_content))
                            strWhere += string.Format(" and ci.memo like '%{0}%'", ui_checkin_content.Trim());
                        if (ui_checkin_adddatestart.Trim() != "")
                            strWhere += " and ci.sign_date_time > cast('" + ui_checkin_adddatestart.Trim() + "' as datetime)";
                        if (ui_checkin_adddateend.Trim() != "")
                            strWhere += " and ci.sign_date_time < cast('" + ui_checkin_adddateend.Trim() + "' as datetime)";
                        if (strWhere == "")
                        {
                            strWhere = " and CONVERT(varchar(100), ci.sign_date_time, 112)='" + DateTime.Now.ToString("yyyyMMdd") + "'";
                        }
                        //string strwhere = "and 1=1";
                        StringBuilder checkin_getall_sb = new StringBuilder();
                        StringBuilder checkin_count_sb = new StringBuilder();
                        checkin_getall_sb.Append("select ci.user_id,u.UserName,ci.sign_date_time,ci.memo from tbcheckin ci LEFT JOIN tbUser u on u.UserId=ci.user_id where 1=1 ");
                        checkin_getall_sb.Append(strWhere);
                        checkin_getall_sb.Append(" order by ci.sign_date_time desc");
                        //checkin_count_sb.Append("select count(*) from tbcheckin ci ");
                        //checkin_count_sb.Append(strWhere);
                        //context.Response.Write(pkt_count_sb.ToString());
                        DataTable checkin_getall_dt = ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, checkin_getall_sb.ToString());
                        //DataTable checkin_count_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.connStr, CommandType.Text, checkin_count_sb.ToString());
                       // DataRow checkin_count_dr = checkin_count_dt.Rows[0];

                        string checkin_getall = ZGZY.Common.JsonHelper.ToJson(checkin_getall_dt);
                        //context.Response.Write("{\"total\":" + pkt_count_dr[0].ToString() + ",\"rows\":" + pkt_getall + ",\"footer\":[{\"PKT_CTRL_NBR\":\"合计\",\"PAK_QTY\":" + pkt_count_dr[1].ToString() + ",\"ORIG_PKT_QTY\":" + pkt_count_dr[2].ToString() + "}]}");
                        //context.Response.Write("{\"total\":" + checkin_count_dr[0].ToString() + ",\"rows\":" + checkin_getall + ",\"footer\":[{\"PKT_CTRL_NBR\":\"合计:\",\"PAK_QTY\":" + checkin_count_dr[1].ToString() + ",\"ORIG_PKT_QTY\":" + checkin_count_dr[2].ToString() + "}]}");
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));
                        // }
                        context.Response.Write(checkin_getall);
                        break;

                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "checkin功能异常";
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