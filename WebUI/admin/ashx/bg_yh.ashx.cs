using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace ZGZY.WebUI.admin.ashx
{
    /// <summary>
    /// bg_yh 的摘要说明
    /// </summary>
    public class bg_yh : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=UTF-8";


            //EF.MAEntities ma_ef = new EF.MAEntities();
            //List<EF.YH_LOCN> yh_list = new List<EF.YH_LOCN>();
            //var var_yh_list = from yh_rc in ma_ef.YH_LOCN select yh_rc;
            //foreach (var v in var_yh_list)
            //{
            //    yh_list.Add(v);
            //}
            //context.Response.Write(ZGZY.Common.JsonHelper.Obj2Json(yh_list));

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
                    case "getall":
                        string strWhere = "";
                        //string ui_yh_id = context.Request.Params["ui_yh_id"] ?? "";
                        string ui_yh_batch = context.Request.Params["ui_yh_batch"] ?? "";
                        string ui_yh_season = context.Request.Params["ui_yh_season"] ?? "";
                        string ui_yh_createdatestart = context.Request.Params["ui_yh_createdatestart"] ?? "";
                        string ui_yh_createdateend = context.Request.Params["ui_yh_createdateend"] ?? "";
                        string ui_yh_createdate = context.Request.Params["ui_yh_createdate"] ?? "";
                        //if (ui_yh_id.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_yh_id))   //防止sql注入
                        //    strWhere += string.Format(" and ah.shpmt_nbr='{0}'", ui_yh_id.Trim());
                        if (ui_yh_batch.Trim() != "")
                            strWhere += " and y.batch_nbr='" + ui_yh_batch.Trim() + "'";
                        if (ui_yh_season.Trim() != "")
                            strWhere += " and y.season='" + ui_yh_season.Trim() + "'";
                        if (ui_yh_createdatestart.Trim() != "")
                            strWhere += " and y.sysdate > cast('" + ui_yh_createdatestart.Trim() + "' as datetime)";
                        if (ui_yh_createdateend.Trim() != "")
                            strWhere += " and y.sysdate < cast('" + ui_yh_createdateend.Trim() + "' as datetime)";
                        if (ui_yh_createdate.Trim() != "")
                            strWhere += " and y.sysdate = cast('" + ui_yh_createdate.Trim() + "' as datetime)";
                        if (strWhere == "")
                        {
                            strWhere = " and y.sysdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            //strWhere = "and  1=1";
                        }
                        strWhere += " order by y.sysdate,y.season";
                        //string strwhere = "and 1=1";
                        StringBuilder yh_getall_sb = new StringBuilder();
                        StringBuilder asn_count_sb = new StringBuilder();
                        yh_getall_sb.Append("select * from yh_locn y where y.whse='S00' ");
                        yh_getall_sb.Append(strWhere);
                        //asn_count_sb.Append("select count(*),sum(ceil(ad.units_rcvd/im.std_pack_qty)) sum_pak_qty,sum(ad.units_rcvd) sum_units_rcvd from asn_hdr ah left join asn_dtl ad on ad.shpmt_nbr=ah.shpmt_nbr left join item_master im on im.sku_id=ad.sku_id left join batch_master bm on bm.batch_nbr=ad.batch_nbr and bm.sku_id=ad.sku_id where ah.to_whse='S00' ");
                        //asn_count_sb.Append(strWhere);
                        //context.Response.Write(asn_count_sb.ToString());
                        DataTable asn_getall_dt = ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.connStr153, CommandType.Text, yh_getall_sb.ToString());
                        //DataTable asn_count_dt = ZGZY.Common.SqlHelper.GetDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, asn_count_sb.ToString());
                        //DataRow asn_count_dr = asn_count_dt.Rows[0];

                        string yh_getall = ZGZY.Common.JsonHelper.ToJson(asn_getall_dt);
                        context.Response.Write(yh_getall);
                        break;
                        
                    default:
                        context.Response.Write("参数错误！");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "养护台账功能异常";
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