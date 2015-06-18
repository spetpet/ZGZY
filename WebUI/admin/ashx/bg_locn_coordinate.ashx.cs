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
    /// bg_locn_coordinate 的摘要说明
    /// </summary>
    public class bg_locn_coordinate : IHttpHandler
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
                    case "getall"://获取pkt
                        
                        StringBuilder inv_getall_sb = new StringBuilder();

                        inv_getall_sb.Append("select * from v_locn_coor_s00");
                        
                        //inv_getall_sb.Append(" order by im.season");
                        
                        DataTable inv_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, inv_getall_sb.ToString());
                        string inv_getall = ZGZY.Common.JsonHelper.ToJsonLower(inv_getall_dt);
                        context.Response.Write("{\"heatmap\": {\"binSize\": 0.5,\"units\": \"t\",\"map\": " + inv_getall + "}}");
                        break;

                   

                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "库位位置查询功能异常";
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