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
    /// bg_combobox 的摘要说明
    /// </summary>
    public class bg_combobox : IHttpHandler
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
                    case "get_party_data"://获取承运商

                        StringBuilder party_data_sb = new StringBuilder();
                        party_data_sb.Append("select t.party_pk,t.party_name from Spl_Fnd_Party_Prof t");
                        DataTable party_data_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, party_data_sb.ToString());
                        context.Response.Write(ZGZY.Common.JsonHelper.ToJson(party_data_dt));
                        //context.Response.Write("test");
                        break;

                    case "get_reject_data":
                        StringBuilder reject_data_sb = new StringBuilder();
                        reject_data_sb.Append("select t.fnd_reject_pk reject_pk,t.reject_name from SPL_FND_Reject t");
                        DataTable reject_data_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, reject_data_sb.ToString());
                        context.Response.Write(ZGZY.Common.JsonHelper.ToJson(reject_data_dt));
                        break;

                    case "get_problem_data":
                        StringBuilder problem_data_sb = new StringBuilder();
                        problem_data_sb.Append("select t.fnd_reject_pk problem_pk,t.reject_name problem_name from SPL_FND_Reject t");
                        DataTable problem_data_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, problem_data_sb.ToString());
                        context.Response.Write(ZGZY.Common.JsonHelper.ToJson(problem_data_dt));
                        break;

                    case "getall"://获取pkt
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        string strWhere = "";


                        string ui_pkt_pktnbr = context.Request.Params["ui_pkt_pktnbr"] ?? "";
                        string ui_pkt_season = context.Request.Params["ui_pkt_season"] ?? "";
                        string ui_pkt_batch = context.Request.Params["ui_pkt_batch"] ?? "";
                        string ui_pkt_adddatestart = context.Request.Params["ui_pkt_adddatestart"] ?? "";
                        string ui_pkt_adddateend = context.Request.Params["ui_pkt_adddateend"] ?? "";

                        if (ui_pkt_pktnbr.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_pkt_pktnbr))   //防止sql注入
                            strWhere += string.Format(" and pd.pkt_ctrl_nbr like '%{0}%'", ui_pkt_pktnbr.Trim());
                        if (ui_pkt_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_pkt_season))
                            strWhere += string.Format(" and im.season like '%{0}%'", ui_pkt_season.Trim());
                        if (ui_pkt_batch.Trim() != "")
                            strWhere += " and pd.batch_nbr = '" + ui_pkt_batch.Trim() + "'";
                        if (ui_pkt_adddatestart.Trim() != "")
                            strWhere += " and ph.create_date_time > to_date('" + ui_pkt_adddatestart.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if (ui_pkt_adddateend.Trim() != "")
                            strWhere += " and ph.create_date_time < to_date('" + ui_pkt_adddateend.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if ((ui_pkt_season.Trim() != "" || ui_pkt_batch.Trim() != "") && ui_pkt_adddatestart.Trim() == "" && ui_pkt_adddateend.Trim() == "")
                            strWhere += " and to_char(ph.create_date_time,'yyyymmdd')='" + DateTime.Now.ToString("yyyyMMdd") + "'";
                        if (strWhere == "")
                        {
                            strWhere = " and to_char(ph.create_date_time,'yyyymmdd')='" + DateTime.Now.ToString("yyyyMMdd") + "'";
                        }
                        //string strwhere = "and 1=1";
                        StringBuilder pkt_getall_sb = new StringBuilder();
                        StringBuilder pkt_count_sb = new StringBuilder();
                        pkt_getall_sb.Append("select rownum id,ph.create_date_time,pd.pkt_ctrl_nbr,im.season,im.sku_id,im.sku_desc,im.std_pack_qty,ceil(pd.orig_pkt_qty/im.std_pack_qty) pak_qty,pd.orig_pkt_qty,pd.batch_nbr from pkt_hdr ph inner join pkt_dtl pd on pd.pkt_ctrl_nbr=ph.pkt_ctrl_nbr left join item_master im on im.sku_id=pd.sku_id where ph.whse='S00' ");
                        pkt_getall_sb.Append(strWhere);
                        pkt_getall_sb.Append(" order by im.season");
                        pkt_count_sb.Append("select count(*),sum(ceil(pd.orig_pkt_qty/im.std_pack_qty)) sum_pak_qty,sum(pd.orig_pkt_qty) sum_orig_pkt_qty from pkt_hdr ph inner join pkt_dtl pd on pd.pkt_ctrl_nbr=ph.pkt_ctrl_nbr left join item_master im on im.sku_id=pd.sku_id where ph.whse='S00' ");
                        pkt_count_sb.Append(strWhere);
                        //context.Response.Write(pkt_count_sb.ToString());
                        DataTable pkt_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, pkt_getall_sb.ToString());
                        DataTable pkt_count_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, pkt_count_sb.ToString());
                        DataRow pkt_count_dr = pkt_count_dt.Rows[0];

                        string pkt_getall = ZGZY.Common.JsonHelper.ToJson(pkt_getall_dt);
                        //context.Response.Write("{\"total\":" + pkt_count_dr[0].ToString() + ",\"rows\":" + pkt_getall + ",\"footer\":[{\"PKT_CTRL_NBR\":\"合计\",\"PAK_QTY\":" + pkt_count_dr[1].ToString() + ",\"ORIG_PKT_QTY\":" + pkt_count_dr[2].ToString() + "}]}");
                        //context.Response.Write("{\"total\":" + pkt_count_dr[0].ToString() + ",\"rows\":" + pkt_getall + ",\"footer\":[{\"PKT_CTRL_NBR\":\"合计:\",\"PAK_QTY\":" + pkt_count_dr[1].ToString() + ",\"ORIG_PKT_QTY\":" + pkt_count_dr[2].ToString() + "}]}");        
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));

                        // }
                        context.Response.Write("test2");
                        break;

                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "PKT功能异常";
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