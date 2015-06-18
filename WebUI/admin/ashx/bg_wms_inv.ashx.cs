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
    /// bg_wms_inv 的摘要说明
    /// </summary>
    public class bg_wms_inv : IHttpHandler
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
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        string strWhere = "";


                        string ui_inv_skudesc = context.Request.Params["ui_inv_skudesc"] ?? "";
                        string ui_inv_season = context.Request.Params["ui_inv_season"] ?? "";
                        string ui_inv_batch = context.Request.Params["ui_inv_batch"] ?? "";
                        

                        if (ui_inv_skudesc.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_inv_skudesc))   //防止sql注入
                            strWhere += string.Format(" and im.sku_desc like '%{0}%'", ui_inv_skudesc.Trim());
                        if (ui_inv_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_inv_season))
                            strWhere += " and im.season = '"+ ui_inv_season.Trim()+ "'";
                        if (ui_inv_batch.Trim() != "")
                            strWhere += " and si.batch_nbr = '" + ui_inv_batch.Trim() + "'";
                        if (strWhere == "")
                            strWhere = "1=0";
                        
                        //string strwhere = "and 1=1";
                        StringBuilder inv_getall_sb = new StringBuilder();
                        StringBuilder inv_count_sb = new StringBuilder();
                        inv_getall_sb.Append("select bm.xpire_date,bm.misc_instr_code_1 made_date,im.std_pack_qty,im.season,si.SKU_ID,im.sku_desc,si.batch_nbr,si.qty_on_hand,ceil(si.qty_on_hand/im.std_pack_qty) pack_qty from sku_invn si left join item_master im on im.sku_id=si.sku_id left join batch_master bm on bm.batch_nbr=si.BATCH_NBR and si.sku_id=bm.sku_id where si.whse='S00' ");
                        inv_getall_sb.Append(strWhere);
                        inv_getall_sb.Append(" order by im.season");
                        inv_count_sb.Append("select count(*) total,sum(si.qty_on_hand) sum_qty_on_hand,sum(ceil(si.qty_on_hand/im.std_pack_qty)) sum_pack_qty from sku_invn si left join item_master im on im.sku_id=si.sku_id where si.whse='S00' ");
                        inv_count_sb.Append(strWhere);
                        //context.Response.Write(inv_count_sb.ToString());
                        DataTable inv_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, inv_getall_sb.ToString());
                        DataTable inv_count_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, inv_count_sb.ToString());
                        DataRow inv_count_dr = inv_count_dt.Rows[0];

                        string inv_getall = ZGZY.Common.JsonHelper.ToJson(inv_getall_dt);
                        //context.Response.Write("{\"total\":" + inv_count_dr[0].ToString() + ",\"rows\":" + inv_getall + ",\"footer\":[{\"inv_CTRL_NBR\":\"合计\",\"PAK_QTY\":" + inv_count_dr[1].ToString() + ",\"ORIG_inv_QTY\":" + inv_count_dr[2].ToString() + "}]}");
                        context.Response.Write("{\"total\":" + inv_count_dr[0].ToString() + ",\"rows\":" + inv_getall + ",\"footer\":[{\"SEASON\":\"合计:\",\"QTY_ON_HAND\":" + inv_count_dr[1].ToString() + ",\"PACK_QTY\":" + inv_count_dr[2].ToString() + "}]}");
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));
                        // }

                        break;

                    case "get_locn_by_batch":
                        string row_batch = context.Request.Params["batch_nbr"] ?? "";
                        string row_season= context.Request.Params["season"] ?? "";
                        StringBuilder get_locn_sb = new StringBuilder();
                        get_locn_sb.Append("select t.locn_brcd,substr(t.locn_brcd,1,1) floor,t.QTY from exp_inv_s00 t ");
                        get_locn_sb.Append("where t.season='" + row_season + "' and t.batch_nbr='" + row_batch + "'");
                        DataTable get_locn_dt = Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, get_locn_sb.ToString());
                        string get_locn_jsonstr = ZGZY.Common.JsonHelper.ToJson(get_locn_dt);
                        context.Response.Write(get_locn_jsonstr);
                        break;

                    case "getbayer"://获取pkt
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        string strbayerWhere = "";
                        StringBuilder bayerinv_getall_sb = new StringBuilder();

                        string ui_bayerinv_skudesc = context.Request.Params["ui_inv_skudesc"] ?? "";
                        string ui_bayerinv_season = context.Request.Params["ui_inv_season"] ?? "";
                        string ui_bayerinv_batch = context.Request.Params["ui_inv_batch"] ?? "";


                        if (ui_bayerinv_skudesc.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_bayerinv_skudesc))   //防止sql注入
                            strbayerWhere += string.Format(" and t2.sku_desc like '%{0}%'", ui_bayerinv_skudesc.Trim());
                        if (ui_bayerinv_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_bayerinv_season))
                            strbayerWhere += " and t2.season = '" + ui_bayerinv_season.Trim() + "'";
                        if (ui_bayerinv_batch.Trim() != "")
                            strbayerWhere += " and t2.batch_nbr = '" + ui_bayerinv_batch.Trim() + "'";
                        if (strbayerWhere == "")
                            bayerinv_getall_sb.Append("select '请先输入查询条件' sku_desc from dual");
                        else
                        {
                            bayerinv_getall_sb.Append("select ' ' memo,t1.whse,t1.area,t1.zone,t1.posn,t2.season,sc.code_desc,t2.size_desc,t1.locn_brcd,t2.case_nbr,t2.sku_desc,(t2.commodity_code || t2.commodity_level_desc) commodity,round(t2.std_pack_qty) pack_qty,t2.dw,t2.misc_instr_code_1 ,to_char(t2.xpire_date,'yyyy-mm-dd') xpire_date,t2.batch_nbr,round(t2.actl_qty/t2.std_pack_qty,4) as js,round(t2.actl_qty,3) as xs,t2.aisle ");
                            bayerinv_getall_sb.Append(" from ( select lh.locn_id,ch.case_nbr,lh.aisle,lh.lvl,lh.posn,lh.locn_brcd,lh.area,lh.zone,lh.whse from case_hdr ch,locn_hdr lh where  lh.locn_id = ch.locn_id and lh.locn_class='R') t1, ");
                            bayerinv_getall_sb.Append(" (select lh.locn_id,ch.case_nbr,cd.case_seq_nbr,cd.batch_nbr,cd.actl_qty,im.season,im.size_desc,im.sku_desc,im.commodity_code,im.commodity_level_desc,im.std_pack_qty,sys_code.code_desc dw,bm.misc_instr_code_1,bm.xpire_date,lh.aisle,lh.zone");
                            bayerinv_getall_sb.Append(" from  case_hdr ch,case_dtl cd, locn_hdr lh, item_whse_master iwm,item_master im, batch_master bm,sys_code where lh.locn_id = ch.locn_id and ch.case_nbr=cd.case_nbr and lh.locn_class='R' and ch.stat_code<'90' and cd.sku_id =iwm.sku_id(+) and cd.sku_id =im.sku_id(+) and cd.sku_id = bm.sku_id(+) and cd.batch_nbr = bm.batch_nbr(+) and lh.whse = iwm.whse and im.dsp_qty_uom = sys_code.code_id(+) and sys_code.code_type = '004' and im.sku_brcd<>'KTP') t2,sys_code sc ");
                            bayerinv_getall_sb.Append(" where t1.locn_id=t2.locn_id and sc.code_type='910' and t2.season = sc.code_id and t1.case_nbr=t2.case_nbr and t1.whse='S00' ");
                            bayerinv_getall_sb.Append(strbayerWhere);
                            bayerinv_getall_sb.Append(" order by t1.locn_brcd, t2.case_nbr ");
                        }
                        DataTable bayerinv_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, bayerinv_getall_sb.ToString());
                        string bayerinv_getall = ZGZY.Common.JsonHelper.ToJson(bayerinv_getall_dt);
                        //context.Response.Write("{\"total\":" + inv_count_dr[0].ToString() + ",\"rows\":" + inv_getall + ",\"footer\":[{\"inv_CTRL_NBR\":\"合计\",\"PAK_QTY\":" + inv_count_dr[1].ToString() + ",\"ORIG_inv_QTY\":" + inv_count_dr[2].ToString() + "}]}");
                        context.Response.Write(bayerinv_getall);
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));
                        // }

                        break;

                    case "getbayer_act"://获取pkt
                        // if (user != null && new ZGZY.BLL.Authority().IfAuthority("pkt", "getall", user.Id))
                        // {
                        string strbayeractWhere = "";
                        StringBuilder bayerinvact_getall_sb = new StringBuilder();

                        string ui_bayerinvact_skudesc = context.Request.Params["ui_inv_skudesc"] ?? "";
                        string ui_bayerinvact_season = context.Request.Params["ui_inv_season"] ?? "";
                        string ui_bayerinvact_batch = context.Request.Params["ui_inv_batch"] ?? "";


                        if (ui_bayerinvact_skudesc.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_bayerinvact_skudesc))   //防止sql注入
                            strbayeractWhere += string.Format(" and t2.sku_desc like '%{0}%'", ui_bayerinvact_skudesc.Trim());
                        if (ui_bayerinvact_season.Trim() != "" && !ZGZY.Common.SqlInjection.GetString(ui_bayerinvact_season))
                            strbayeractWhere += " and t2.season = '" + ui_bayerinvact_season.Trim() + "'";
                        if (ui_bayerinvact_batch.Trim() != "")
                            strbayeractWhere += " and t2.batch_nbr = '" + ui_bayerinvact_batch.Trim() + "'";
                        if (strbayeractWhere == "")
                            bayerinvact_getall_sb.Append("select '请先输入查询条件' sku_desc from dual");
                        else
                        {
                            bayerinvact_getall_sb.Append("select ' ' memo,t1.whse,t1.area,t1.zone,t2.season,sc.code_desc,t2.size_desc,t1.locn_brcd,t2.sku_desc,(t2.commodity_code || t2.commodity_level_desc) commodity,round(t2.std_pack_qty) pack_qty,t2.dw,t2.misc_instr_code_1,to_char(t2.xpire_date,'yyyy-mm-dd') xpire_date,t2.batch_nbr,' ' js,t2.actl_invn_qty as xs from (select lh.whse,lh.area,lh.zone,lh.locn_id,pld.locn_seq_nbr,lh.aisle,lh.lvl,lh.posn,lh.locn_brcd,pld.batch_nbr,pld.actl_invn_qty,pld.to_be_pikd_qty,pld.to_be_filld_qty from pick_locn_dtl pld, locn_hdr lh where  lh.locn_id = pld.locn_id and lh.locn_class='A') t1,(select lh.locn_id,pld.locn_seq_nbr,im.season,im.size_desc,im.sku_desc,im.commodity_code,im.commodity_level_desc,pld.actl_invn_qty,im.std_pack_qty,sys_code.code_desc as dw,bm.misc_instr_code_1,bm.xpire_date,pld.batch_nbr from pick_locn_dtl pld, locn_hdr lh, item_whse_master iwm, item_master im, batch_master bm,sys_code where lh.locn_id = pld.locn_id and lh.locn_class='A' and pld.sku_id =iwm.sku_id(+) and pld.sku_id =im.sku_id(+) and pld.sku_id = bm.sku_id(+) and pld.batch_nbr = bm.batch_nbr(+) and lh.whse = iwm.whse and im.dsp_qty_uom = sys_code.code_id(+) and sys_code.code_type = '004') t2,sys_code sc where t1.locn_id=t2.locn_id and sc.code_type='910' and t2.season = sc.code_id and t1.locn_seq_nbr=t2.locn_seq_nbr and (t1.actl_invn_qty is null or t1.actl_invn_qty>0) and t1.whse='S00' ");
                            //bayerinvact_getall_sb.Append(" from ( select lh.locn_id,ch.case_nbr,lh.aisle,lh.lvl,lh.posn,lh.locn_brcd,lh.area,lh.zone,lh.whse from case_hdr ch,locn_hdr lh where  lh.locn_id = ch.locn_id and lh.locn_class='R') t1, ");
                           // bayerinvact_getall_sb.Append("(select lh.locn_id,pld.locn_seq_nbr,im.season,im.size_desc,im.sku_desc,im.commodity_code,im.commodity_level_desc,pld.actl_invn_qty,im.std_pack_qty,sys_code.code_desc as dw,bm.misc_instr_code_1,bm.xpire_date,pld.batch_nbr from pick_locn_dtl pld, locn_hdr lh, item_whse_master iwm, item_master im, batch_master bm,sys_code where lh.locn_id = pld.locn_id and lh.locn_class='A' and pld.sku_id =iwm.sku_id(+) and pld.sku_id =im.sku_id(+) and pld.sku_id = bm.sku_id(+) and pld.batch_nbr = bm.batch_nbr(+) and lh.whse = iwm.whse and im.dsp_qty_uom = sys_code.code_id(+) and sys_code.code_type = '004') t2,");
                            // bayerinvact_getall_sb.Append(" sys_code sc where t1.locn_id=t2.locn_id and sc.code_type='910' and t2.season = sc.code_id and t1.locn_seq_nbr=t2.locn_seq_nbr and (t1.actl_invn_qty is null or t1.actl_invn_qty>0) and t1.whse='S00' ");
                            bayerinvact_getall_sb.Append(strbayeractWhere);
                            bayerinvact_getall_sb.Append(" order by t1.locn_brcd ");
                        }
                        DataTable bayerinvact_getall_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, bayerinvact_getall_sb.ToString());
                        string bayerinvact_getall = ZGZY.Common.JsonHelper.ToJson(bayerinvact_getall_dt);
                        //context.Response.Write("{\"total\":" + inv_count_dr[0].ToString() + ",\"rows\":" + inv_getall + ",\"footer\":[{\"inv_CTRL_NBR\":\"合计\",\"PAK_QTY\":" + inv_count_dr[1].ToString() + ",\"ORIG_inv_QTY\":" + inv_count_dr[2].ToString() + "}]}");
                        context.Response.Write(bayerinvact_getall);
                        //context.Response.Write(new ZGZY.BLL.Menu().GetUserMenu(user.Id));
                        // }

                        break;

                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "库存查询功能异常";
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