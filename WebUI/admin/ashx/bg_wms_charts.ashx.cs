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
    /// bg_wms_charts 的摘要说明
    /// </summary>
    public class bg_wms_charts : IHttpHandler
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
                    case "get_realtime_inv"://获取实时库存
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        

                        //string strwhere = "and 1=1";
                        int f1_count=0, f2_count=0, f3_count=0;
                        List<string> f1_qty = new List<string>(), f2_qty = new List<string>(), f3_qty = new List<string>();
                        List<string> f1_season = new List<string>(), f2_season = new List<string>(), f3_season = new List<string>();
                        StringBuilder inv_getall_sb = new StringBuilder();
                        inv_getall_sb.Append("select * from case_count_byfloor_s00");
                        DataTable inv_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, inv_getall_sb.ToString());
                        DataRow inv_getall_dr;
                        for (int i = 0; i < inv_getall_dt.Rows.Count; i++)
                        {
                            inv_getall_dr=inv_getall_dt.Rows[i];
                            switch (inv_getall_dr[0].ToString())
                            {
                                case "1":
                                    f1_count += int.Parse(inv_getall_dr[4].ToString());
                                    f1_qty.Add(inv_getall_dr[4].ToString());
                                    f1_season.Add( inv_getall_dr[2].ToString());
                                    break;
                                case "2":
                                    f2_count += int.Parse(inv_getall_dr[4].ToString());
                                    f2_qty.Add(inv_getall_dr[4].ToString());
                                    f2_season.Add (inv_getall_dr[2].ToString());
                                    break;
                                case "3":
                                    f3_count += int.Parse(inv_getall_dr[4].ToString());
                                    f3_qty.Add (inv_getall_dr[4].ToString());
                                    f3_season.Add (inv_getall_dr[2].ToString());
                                    break;
                                default:
                                    break;

                            }
                        }
                        StringBuilder json_sb = new StringBuilder();

                        json_sb.Append("[{y: " + f1_count.ToString() + ",color: colors[0],drilldown: {name: '一楼库存构成',categories: " + ZGZY.Common.JsonHelper.StringArrayToJson(f1_season.ToArray()) + ",data: " + ZGZY.Common.JsonHelper.StringArrayToJsonInt(f1_qty.ToArray()) + ",color: colors[0]}},");
                        json_sb.Append("{y: " + f2_count.ToString() + ",color: colors[0],drilldown: {name: '二楼库存构成',categories: " + ZGZY.Common.JsonHelper.StringArrayToJson(f2_season.ToArray()) + ",data: " + ZGZY.Common.JsonHelper.StringArrayToJsonInt(f2_qty.ToArray()) + ",color: colors[0]}},");
                        json_sb.Append("{y: " + f3_count.ToString() + ",color: colors[0],drilldown: {name: '三楼库存构成',categories: " + ZGZY.Common.JsonHelper.StringArrayToJson(f3_season.ToArray()) + ",data: " + ZGZY.Common.JsonHelper.StringArrayToJsonInt(f3_qty.ToArray()) + ",color: colors[0]}}]");
                        context.Response.Write(json_sb);
                       // string[] test = f2_season.ToArray();
                        //context.Response.Write(ZGZY.Common.JsonHelper.StringArrayToJson(f2_season.ToArray()));
                        
                        // }

                        break;

                    case "get_locn_by_batch":
                        
                        break;


                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "可视化查询功能异常";
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