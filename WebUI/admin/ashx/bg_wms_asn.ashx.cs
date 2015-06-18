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
    /// bg_wms_asn 的摘要说明
    /// </summary>
    public class bg_wms_asn : IHttpHandler
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
                    case "getall"://获取asn
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("asn", "getall",    ))
                        // {
                        string strWhere = "";


                        string ui_asn_asnnbr = context.Request.Params["ui_asn_asnnbr"] ?? "";
                        string ui_asn_season = context.Request.Params["ui_asn_season"] ?? "";
                        string ui_asn_batch = context.Request.Params["ui_asn_batch"] ?? "";
                        string ui_asn_adddatestart = context.Request.Params["ui_asn_adddatestart"] ?? "";
                        string ui_asn_adddateend = context.Request.Params["ui_asn_adddateend"] ?? "";
                        string ui_asn_createdatestart = context.Request.Params["ui_asn_createdatestart"] ?? "";
                        string ui_asn_createdateend = context.Request.Params["ui_asn_createdateend"] ?? "";

                        if (ui_asn_asnnbr.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_asn_asnnbr))   //防止sql注入
                            strWhere += string.Format(" and ah.shpmt_nbr='{0}'", ui_asn_asnnbr.Trim());
                        if (ui_asn_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_asn_season))
                            strWhere += string.Format(" and im.season='{0}'", ui_asn_season.Trim());
                        if (ui_asn_batch.Trim() != "")
                            strWhere += " and ad.batch_nbr = '" + ui_asn_batch.Trim() + "'";
                        if (ui_asn_adddatestart.Trim() != "")
                            strWhere += " and ah.first_rcpt_date_time > to_date('" + ui_asn_adddatestart.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if (ui_asn_adddateend.Trim() != "")
                            strWhere += " and ah.first_rcpt_date_time < to_date('" + ui_asn_adddateend.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if (ui_asn_createdatestart.Trim() != "")
                            strWhere += " and ah.create_date_time > to_date('" + ui_asn_createdatestart.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if (ui_asn_createdateend.Trim() != "")
                            strWhere += " and ah.create_date_time < to_date('" + ui_asn_createdateend.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        //if ((ui_asn_season.Trim() != "" || ui_asn_batch.Trim() != "") && ui_asn_adddatestart.Trim() == "" && ui_asn_adddateend.Trim() == "")
                        if ((ui_asn_season.Trim() != "") && ui_asn_adddatestart.Trim() == "" && ui_asn_adddateend.Trim() == "")
                            strWhere += " and to_char(ah.create_date_time,'yyyymmdd')>='" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd") + "'";
                        if (strWhere == "")
                        {
                            strWhere = " and to_char(ah.create_date_time,'yyyymmdd')>='" + DateTime.Now.AddDays(-3).ToString("yyyyMMdd") + "'";
                        }
                        strWhere += " order by ah.first_rcpt_date_time";
                        //string strwhere = "and 1=1";
                        StringBuilder asn_getall_sb = new StringBuilder();
                        StringBuilder asn_count_sb = new StringBuilder();
                        asn_getall_sb.Append("select im.sku_brcd,im.season,im.std_pack_qty,ah.shpmt_nbr,ah.manif_nbr,ad.batch_nbr,im.sku_desc,ah.units_shpd units_total_shpd,ad.units_shpd,ad.units_rcvd,ceil(ad.units_rcvd/im.std_pack_qty) pack_qty,ah.create_date_time,ah.mod_date_time,ah.first_rcpt_date_time,bm.xpire_date,bm.misc_instr_code_1 made_date from asn_hdr ah left join asn_dtl ad on ad.shpmt_nbr=ah.shpmt_nbr left join item_master im on im.sku_id=ad.sku_id left join batch_master bm on bm.batch_nbr=ad.batch_nbr and bm.sku_id=ad.sku_id where ah.to_whse='S00' ");
                        asn_getall_sb.Append(strWhere);
                        asn_count_sb.Append("select count(*),sum(ceil(ad.units_rcvd/im.std_pack_qty)) sum_pak_qty,sum(ad.units_rcvd) sum_units_rcvd from asn_hdr ah left join asn_dtl ad on ad.shpmt_nbr=ah.shpmt_nbr left join item_master im on im.sku_id=ad.sku_id left join batch_master bm on bm.batch_nbr=ad.batch_nbr and bm.sku_id=ad.sku_id where ah.to_whse='S00' ");
                        asn_count_sb.Append(strWhere);
                        //context.Response.Write(asn_count_sb.ToString());
                        DataTable asn_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, asn_getall_sb.ToString());
                        DataTable asn_count_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, asn_count_sb.ToString());
                        DataRow asn_count_dr = asn_count_dt.Rows[0];

                        string asn_getall = ZGZY.Common.JsonHelper.ToJson(asn_getall_dt);
                        
                        context.Response.Write("{\"total\":" + asn_count_dr[0].ToString() + ",\"rows\":" + asn_getall + ",\"footer\":[{\"SHPMT_NBR\":\"合计:\",\"PACK_QTY\":" + asn_count_dr[1].ToString() + ",\"UNITS_RCVD\":" + asn_count_dr[2].ToString() + "}]}");
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));
                        // }
                        //context.Response.Write(strWhere);
                        break;

                    case "add":
                        string ui_asn_asnid = context.Request.Params["ui_asn_asnid"] ?? "";
                        //string ui_asn_sku = context.Request.Params["ui_asn_sku"] ?? "";
                        //string ui_asn_ord_qty = context.Request.Params["ui_asn_ord_qty"] ?? "";
                        string ui_asn_skubrcd = context.Request.Params["ui_asn_skubrcd"] ?? "";
                        string ui_asn_shp_qty = context.Request.Params["ui_asn_shp_qty"] ?? "";
                        string ui_asn_rcv_qty = context.Request.Params["ui_asn_rcv_qty"] ?? "";
                        string ui_asn_rjt_qty = context.Request.Params["ui_asn_rjt_qty"] ?? null;
                        string ui_asn_reject = context.Request.Params["ui_asn_reject"] ?? "";
                        string ui_asn_prob_qty = context.Request.Params["ui_asn_prob_qty"] ?? "";
                        string ui_asn_problem = context.Request.Params["ui_asn_problem"] ?? "";
                        string ui_asn_party = context.Request.Params["ui_asn_party"] ?? "";
                        string ui_asn_shptype = context.Request.Params["ui_asn_shptype"] ?? "";
                        string ui_asn_carnbr = context.Request.Params["ui_asn_carnbr"] ?? "";
                        string ui_asn_rcvtemp = context.Request.Params["ui_asn_rcvtemp"] ?? null;
                        string ui_asn_memo = context.Request.Params["ui_asn_memo"] ?? "";
                        string ui_asn_startdate = context.Request.Params["ui_asn_startdate"] ?? "1900-1-1 00:00:00";
                        string ui_asn_enddate = context.Request.Params["ui_asn_enddate"] ?? null;
                        StringBuilder add_sql_sb = new StringBuilder();
                        DateTime today=DateTime.Now;

                        //add_sql_sb.Append("insert into SPL_ASN_HDR(SPL_ASN_HDR_PK,Status,Entry_Method,sys_crt_by,sys_crt_dtm,sys_mdf_by,sys_mdf_dtm,sys_voided_f,SHPMT_NBR,UNITS_SHPD,UNITS_RCVD,UNITS_REJECT)");
                        //add_sql_sb.Append(",REJECT_PK,PARTY_CARRIER,TRANSPORT,VEHICLE_NBR,TEMP_RCVD,REJECT_REMARK,DTM_SHPD,DTM_ARVL)");
                        //add_sql_sb.Append(" values(SPL_ASN_Hdr_Seq.nextval,20,20,'GZ015',:tday,'GZ015',:tday,0,:SHPMT_NBR,:UNITS_SHPD,:UNITS_RCVD,:UNITS_REJECT)");
                        //add_sql_sb.Append(",:REJECT_PK,:PARTY_CARRIER,:TRANSPORT,:VEHICLE_NBR,:TEMP_RCVD,:REJECT_REMARK,:DTM_SHPD,:DTM_ARVL)");
                        add_sql_sb.Append("insert into SPL_ASN_HDR(SPL_ASN_HDR_PK,Status,Entry_Method,sys_crt_by,sys_crt_dtm,sys_mdf_by,sys_mdf_dtm,sys_voided_f,SHPMT_NBR,UNITS_SHPD,UNITS_RCVD,UNITS_REJECT,REJECT_PK,UNITS_PROBLEM,PROBLEM_PK,PARTY_CARRIER,TRANSPORT,VEHICLE_NBR,TEMP_RCVD,REJECT_REMARK,DTM_SHPD,DTM_ARVL,SKU_BRCD) values(SPL_ASN_Hdr_Seq.nextval,20,20,'" + user.UserId + "',:tday,'" + user.UserId + "',:tday,0,:SHPMT_NBR,:UNITS_SHPD,:UNITS_RCVD,:UNITS_REJECT,:REJECT_PK,:UNITS_PROBLEM,:PROBLEM_PK,:PARTY_CARRIER,:TRANSPORT,:VEHICLE_NBR,:TEMP_RCVD,:REJECT_REMARK,:DTM_SHPD,:DTM_ARVL,:SKU_BRCD)");
                        OracleParameter[] add_sql_par={
                                                          new OracleParameter(":tday",today),
                                                          new OracleParameter(":SHPMT_NBR",ui_asn_asnid),
                                                          new OracleParameter(":UNITS_SHPD",Convert.ToInt32(ui_asn_shp_qty)),
                                                          new OracleParameter(":UNITS_RCVD",Convert.ToInt32(ui_asn_rcv_qty)),
                                                          new OracleParameter(":UNITS_REJECT",Convert.ToInt32(ui_asn_rjt_qty)),
                                                          new OracleParameter(":REJECT_PK",Convert.ToInt32(ui_asn_reject)),
                                                          new OracleParameter(":UNITS_PROBLEM",Convert.ToInt32(ui_asn_prob_qty)),
                                                          new OracleParameter(":PROBLEM_PK",Convert.ToInt32(ui_asn_problem)),
                                                          new OracleParameter(":PARTY_CARRIER",Convert.ToInt32(ui_asn_party)),
                                                          new OracleParameter(":TRANSPORT",ui_asn_shptype),
                                                          new OracleParameter(":VEHICLE_NBR",ui_asn_carnbr),
                                                          new OracleParameter(":TEMP_RCVD",ui_asn_rcvtemp),
                                                          new OracleParameter(":REJECT_REMARK",ui_asn_memo),
                                                          new OracleParameter(":DTM_SHPD",Convert.ToDateTime(ui_asn_startdate)),
                                                          new OracleParameter(":DTM_ARVL",Convert.ToDateTime(ui_asn_enddate)),
                                                          new OracleParameter(":SKU_BRCD",ui_asn_skubrcd)
                                                          //new OracleParameter(":",),
                                                          
                                                      };

                        int result= ZGZY.Common.SqlHelper.ExecuteOracleNonQuery(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, add_sql_sb.ToString(),add_sql_par);
                        if (result != 0) context.Response.Write("{\"result\":\"录入成功！成功"+result.ToString()+"条\",\"success\":true}");
                        
                        break;

                    case "get_inbound":
                        string strWhere_inbound = "";
                        string ui_inbound_asnnbr = context.Request.Params["ui_inbound_asnnbr"] ?? "";
                        string ui_inbound_season = context.Request.Params["ui_inbound_season"] ?? "";
                        string ui_inbound_skudesc = context.Request.Params["ui_inbound_skudesc"] ?? "";
                        string ui_inbound_adddatestart = context.Request.Params["ui_inbound_adddatestart"] ?? "";
                        string ui_inbound_adddateend = context.Request.Params["ui_inbound_adddateend"] ?? "";
                        if (ui_inbound_asnnbr.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_inbound_asnnbr))   //防止sql注入
                            strWhere_inbound += string.Format(" and ah.shpmt_nbr='{0}'", ui_inbound_asnnbr.Trim());
                        if (ui_inbound_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_inbound_season))
                            strWhere_inbound += string.Format(" and im.season='{0}'", ui_inbound_season.Trim());
                        if (ui_inbound_skudesc.Trim() != "")
                            strWhere_inbound += " and im.sku_desc like '%" + ui_inbound_skudesc.Trim() + "%'";
                        if (ui_inbound_adddatestart.Trim() != "")
                            strWhere_inbound += " and sah.dtm_arvl > to_date('" + ui_inbound_adddatestart.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if (ui_inbound_adddateend.Trim() != "")
                            strWhere_inbound += " and sah.dtm_arvl < to_date('" + ui_inbound_adddateend.Trim() + "','yyyy-mm-dd hh24:mi:ss')";
                        if ((ui_inbound_season.Trim() != "" || ui_inbound_skudesc.Trim() != "") && ui_inbound_adddatestart.Trim() == "" && ui_inbound_adddateend.Trim() == "")
                            strWhere_inbound += " and to_char(sah.dtm_arvl,'yyyymmdd')='" + DateTime.Now.ToString("yyyyMMdd") + "'";
                        if (strWhere_inbound == "")
                        {
                            strWhere_inbound = " and to_char(sah.dtm_arvl,'yyyymmdd')='" + DateTime.Now.ToString("yyyyMMdd") + "'";
                        }

                        StringBuilder get_inbound_sb = new StringBuilder();
                        get_inbound_sb.Append("select sah.units_problem,sah.problem_pk,r1.reject_name problem_name,sah.sys_crt_dtm,im.STD_PACK_QTY,im.season,im.sku_desc,im.sku_id,sah.spl_asn_hdr_pk,sah.shpmt_nbr,ah.units_shpd units_total_shpd,sah.units_shpd,sah.units_rcvd,sah.units_reject,r.reject_name,sah.units_arvl,sah.vehicle_nbr,sah.transport,sah.party_carrier,pp.party_name,sah.dtm_shpd,sah.dtm_rcvd,sah.temp_rcvd,sah.dtm_arvl,sah.reject_pk,sah.reject_remark from spl_asn_hdr sah left join asn_hdr ah on ah.shpmt_nbr=sah.shpmt_nbr left join item_master im on im.sku_brcd=sah.sku_brcd left join Spl_Fnd_Party_Prof pp on pp.party_pk=sah.party_carrier left join SPL_FND_Reject r on r.fnd_reject_pk=sah.reject_pk left join SPL_FND_Reject r1 on r1.fnd_reject_pk=sah.problem_pk where ah.to_whse='S00' ");
                        get_inbound_sb.Append(strWhere_inbound);
                        get_inbound_sb.Append(" order by sah.spl_asn_hdr_pk");
                        DataTable get_inbound_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, get_inbound_sb.ToString());
                        string get_inbound_jsonstr = ZGZY.Common.JsonHelper.ToJson(get_inbound_dt);
                        context.Response.Write(get_inbound_jsonstr);
                        break;

                    case "edit":
                        string ui_edit_pk = context.Request.Params["ui_asn_hdr_pk"] ?? "";
                        string ui_edit_asnid = context.Request.Params["ui_asn_asnid"] ?? "";
                        //string ui_asn_sku = context.Request.Params["ui_asn_sku"] ?? "";
                        //string ui_asn_ord_qty = context.Request.Params["ui_asn_ord_qty"] ?? "";
                        string ui_edit_skubrcd = context.Request.Params["ui_asn_skubrcd"] ?? "";
                        string ui_edit_shp_qty = context.Request.Params["ui_asn_shp_qty"] ?? "";
                        string ui_edit_rcv_qty = context.Request.Params["ui_asn_rcv_qty"] ?? "";
                        string ui_edit_rjt_qty = context.Request.Params["ui_asn_rjt_qty"] ?? null;
                        string ui_edit_prob_qty = context.Request.Params["ui_asn_prob_qty"] ?? "";
                        string ui_edit_problem = context.Request.Params["ui_asn_problem"] ?? "";
                        string ui_edit_reject = context.Request.Params["ui_asn_reject"] ?? "";
                        string ui_edit_party = context.Request.Params["ui_asn_party"] ?? "";
                        string ui_edit_shptype = context.Request.Params["ui_asn_shptype"] ?? "";
                        string ui_edit_carnbr = context.Request.Params["ui_asn_carnbr"] ?? "";
                        string ui_edit_rcvtemp = context.Request.Params["ui_asn_rcvtemp"] ?? null;
                        string ui_edit_memo = context.Request.Params["ui_asn_memo"] ?? "";
                        string ui_edit_startdate = context.Request.Params["ui_asn_startdate"] ?? "1900-1-1 00:00:00";
                        string ui_edit_enddate = context.Request.Params["ui_asn_enddate"] ?? null;
                        StringBuilder edit_sql_sb = new StringBuilder();
                        DateTime edit_today = DateTime.Now;
                        edit_sql_sb.Append("update spl_asn_hdr s set s.units_rcvd=:UNITS_RCVD,s.units_shpd=:UNITS_SHPD,s.vehicle_nbr=:VEHICLE_NBR,s.transport=:TRANSPORT,s.party_carrier=:PARTY_CARRIER,s.dtm_shpd=:DTM_SHPD,s.dtm_arvl=:DTM_ARVL,s.temp_rcvd=:TEMP_RCVD,s.units_reject=:UNITS_REJECT,s.reject_remark=:REJECT_REMARK,s.reject_pk=:REJECT_PK,s.units_problem=:UNITS_PROBLEM,s.problem_pk=:PROBLEM_PK,s.sys_mdf_dtm=:tday where s.spl_asn_hdr_pk=:SPL_ASN_HDR_PK");
                        OracleParameter[] edit_sql_par ={
                                                          new OracleParameter(":tday",edit_today),
                                                          
                                                          new OracleParameter(":UNITS_SHPD",Convert.ToInt32(ui_edit_shp_qty)),
                                                          new OracleParameter(":UNITS_RCVD",Convert.ToInt32(ui_edit_rcv_qty)),
                                                          new OracleParameter(":UNITS_REJECT",Convert.ToInt32(ui_edit_rjt_qty)),
                                                          new OracleParameter(":REJECT_PK",Convert.ToInt32(ui_edit_reject)),
                                                          new OracleParameter(":PARTY_CARRIER",Convert.ToInt32(ui_edit_party)),
                                                          new OracleParameter(":TRANSPORT",ui_edit_shptype),
                                                          new OracleParameter(":VEHICLE_NBR",ui_edit_carnbr),
                                                          new OracleParameter(":TEMP_RCVD",ui_edit_rcvtemp),
                                                          new OracleParameter(":REJECT_REMARK",ui_edit_memo),
                                                          new OracleParameter(":DTM_SHPD",  Convert.ToDateTime(ui_edit_startdate)),
                                                          new OracleParameter(":DTM_ARVL",Convert.ToDateTime(ui_edit_enddate)),
                                                          new OracleParameter(":SPL_ASN_HDR_PK",Convert.ToInt32(ui_edit_pk)),
                                                          new OracleParameter(":UNITS_PROBLEM",Convert.ToInt32(ui_edit_prob_qty)),
                                                           new OracleParameter(":PROBLEM_PK",Convert.ToInt32(ui_edit_problem))
                                                            
                                                          //new OracleParameter(":",),
                                                          
                                                      };
                        int edit_result= ZGZY.Common.SqlHelper.ExecuteOracleNonQuery(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, edit_sql_sb.ToString(),edit_sql_par);
                        if (edit_result != 0) context.Response.Write("{\"result\":\"更新记录成功！成功"+edit_result.ToString()+"条\",\"success\":true}");

                        break;

                    case "delete":
                        string ui_delete_pk = context.Request.Params["ui_asn_hdr_pk"] ?? "";
                        StringBuilder delete_sql_sb = new StringBuilder();
                        delete_sql_sb.Append("delete from spl_asn_hdr s where s.spl_asn_hdr_pk=:spl_asn_hdr_pk");
                        OracleParameter[] delete_sql_paras = { 
                                                            new OracleParameter(":spl_asn_hdr_pk",Convert.ToInt32(ui_delete_pk))
                                                         };
                        int delete_result = ZGZY.Common.SqlHelper.ExecuteOracleNonQuery(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, delete_sql_sb.ToString(), delete_sql_paras);
                        if (delete_result != 0) context.Response.Write("{\"result\":\"删除记录成功！成功删除" + delete_result.ToString() + "条\",\"success\":true}");
 
                        break;

                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                //userOperateLog.OperateInfo = "asn功能异常";
                //userOperateLog.IfSuccess = false;
                //userOperateLog.Description = ZGZY.Common.JsonHelper.StringFilter(ex.Message);
                //ZGZY.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
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