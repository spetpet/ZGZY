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
    /// bg_pkt_gsp 的摘要说明
    /// </summary>
    public class bg_pkt_gsp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
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
                            string ui_gsp_pktnbr = context.Request.Params["ui_gsp_pktnbr"] ?? "";
                            string ui_gsp_pktctlnbr = context.Request.Params["ui_gsp_pktctlnbr"] ?? "";
                            string ui_pkt_createdate = context.Request.Params["ui_pkt_createdate"] ?? "";
                            if (ui_gsp_pktctlnbr.Trim() != "")
                                strWhere += string.Format(" and phd.pkt_ctrl_nbr like '%{0}%'", ui_gsp_pktctlnbr.Trim());
                            if (ui_gsp_pktnbr.Trim() != "")
                                strWhere += string.Format(" and phd.ftsr_nbr like '%{0}%' ", ui_gsp_pktnbr.Trim());
                            if (ui_pkt_createdate.Trim() != "")
                                strWhere += string.Format(" and to_char(phd.create_date_time,'yyyy-mm-dd')= '{0}' ", ui_pkt_createdate.Trim());
                            if (strWhere == "")
                            {
                                strWhere = " and to_char(phd.create_date_time,'yyyymmdd')=to_char(sysdate,'yyyymmdd') ";
                            }

                            StringBuilder gsp_getall_sql = new StringBuilder();
                            //gsp_getall_sql.Append("select pkt,bm,sum(zys) zys,sum(lys) lys,sum(yis) yis,sum(zys)+sum(lys)-sum(yis) ws from (select ph.pkt_ctrl_nbr pkt,im.size_desc bm,sum(floor(cd.to_be_pakd_units / im.std_pack_qty ))zys,mod(cd.to_be_pakd_units,im.std_pack_qty) lys,floor(0) yis from carton_dtl cd left join item_master im on im.sku_id = cd.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = cd.pkt_ctrl_nbr left join carton_hdr ch on ch.carton_nbr=cd.carton_nbr where ph.whse='S00' and to_char(ph.mod_date_time,'yyyymmdd') = '20150422' group by  ph.pkt_ctrl_nbr,im.size_desc,mod(cd.to_be_pakd_units,im.std_pack_qty) union all select c.pkt_ctrl_nbr pkt,im.size_desc bm,floor(0) zys,mod(0,0) lys,count(c.gsp_nbr) yis from c_gsp_nbr_trkg c left join item_master im on im.sku_id = c.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = c.pkt_ctrl_nbr where c.stat_code=0 and ph.whse='S00' and to_char(ph.mod_date_time,'yyyymmdd') = '20150422' group by c.pkt_ctrl_nbr,im.size_desc union all select cd.pkt_ctrl_nbr pkt,im.size_desc bm,floor(0) zys,mod(0,0) lys,count(bc.national_barcode_no) yis from bayer_case bc left join (select distinct carton_nbr,create_date_time,pkt_ctrl_nbr,sku_id from carton_dtl) cd on bc.carton_nbr=cd.carton_nbr left join item_master im on im.sku_id = cd.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = cd.pkt_ctrl_nbr where  to_char(ph.mod_date_time,'yyyymmdd') = '20150422' group by cd.pkt_ctrl_nbr,im.size_desc,bc.carton_nbr) group by pkt,bm order by pkt");
                            gsp_getall_sql.Append("select to_char(phd.create_date_time,'yyyymmdd') cdate,t.pkt,phd.ftsr_nbr,sum(zys) zys,sum(lys) lys,sum(yis) yis,sum(zys)+sum(lys)-sum(yis) ws from ");
                            gsp_getall_sql.Append("(select ph.pkt_ctrl_nbr pkt,sum(floor(cd.to_be_pakd_units / im.std_pack_qty ))zys,mod(cd.to_be_pakd_units,im.std_pack_qty) lys,floor(0) yis from carton_dtl cd left join item_master im on im.sku_id = cd.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = cd.pkt_ctrl_nbr left join carton_hdr ch on ch.carton_nbr=cd.carton_nbr where ph.whse='S00' group by  ph.pkt_ctrl_nbr,mod(cd.to_be_pakd_units,im.std_pack_qty) ");
                            gsp_getall_sql.Append("union all select c.pkt_ctrl_nbr pkt,floor(0) zys,mod(0,0) lys,count(c.gsp_nbr) yis from c_gsp_nbr_trkg c left join item_master im on im.sku_id = c.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = c.pkt_ctrl_nbr where c.stat_code=0 and ph.whse='S00' group by c.pkt_ctrl_nbr,im.size_desc ");
                            gsp_getall_sql.Append("union all select cd.pkt_ctrl_nbr pkt,floor(0) zys,mod(0,0) lys,count(bc.national_barcode_no) yis from bayer_case bc left join (select distinct carton_nbr,create_date_time,pkt_ctrl_nbr,sku_id from carton_dtl) cd on bc.carton_nbr=cd.carton_nbr left join item_master im on im.sku_id = cd.sku_id left join pkt_hdr ph on ph.pkt_ctrl_nbr = cd.pkt_ctrl_nbr group by cd.pkt_ctrl_nbr,bc.carton_nbr) t ");
                            gsp_getall_sql.Append("left join pkt_hdr phd on phd.pkt_ctrl_nbr=t.pkt where 1=1 ");
                            //gsp_getall_sql.Append("where to_char(phd.create_date_time,'yyyymmdd')='20150504' ");
                            gsp_getall_sql.Append(strWhere);
                            gsp_getall_sql.Append(" group by pkt,to_char(phd.create_date_time,'yyyymmdd'),phd.ftsr_nbr order by pkt");
                            DataTable v_gsp_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, gsp_getall_sql.ToString());
                            //DataRow pkt_count_dr = pkt_count_dt.Rows[0];
                            string gsp_getall_json = ZGZY.Common.JsonHelper.ToJson(v_gsp_dt);
                            context.Response.Write(gsp_getall_json);
                            break;

                        case "by_pktctlnbr":
                            string pkt_ctrl_nbr = context.Request.Params["pkt_ctrl_nbr"] ?? "none";
                            StringBuilder gsp_bypktctrlnbr_sql = new StringBuilder();
                            gsp_bypktctrlnbr_sql.Append("select carton_nbr,sum(to_be_scan) to_be_scan,sum(scaned) scaned from ( ");
                            gsp_bypktctrlnbr_sql.Append(" select ch.carton_nbr,case ch.carton_creation_code when 5 then ch.total_qty else floor(ch.total_qty/im.std_pack_qty)+mod(ch.total_qty,im.std_pack_qty) end to_be_scan,0 scaned from carton_hdr ch ");
                            gsp_bypktctrlnbr_sql.Append(" left join item_master im on im.sku_id=ch.sku_id where ch.whse='S00' and ch.pkt_ctrl_nbr='"+pkt_ctrl_nbr.Trim()+"' ");
                            gsp_bypktctrlnbr_sql.Append(" union all select c.cntr_nbr,0 to_be_scan,count(*) scaned from c_gsp_nbr_trkg c where c.whse='S00' and c.pkt_ctrl_nbr='"+pkt_ctrl_nbr.Trim()+"' group by c.cntr_nbr");
                            gsp_bypktctrlnbr_sql.Append(" union all select b.carton_nbr,0,count(*) from bayer_case b left join carton_hdr ch on ch.carton_nbr=b.carton_nbr where ch.pkt_ctrl_nbr='" + pkt_ctrl_nbr.Trim() + "' group by b.carton_nbr ) t group by carton_nbr");
                            DataTable gsp_bypktctrlnbr_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, gsp_bypktctrlnbr_sql.ToString());
                            string gsp_bypktctrlnbr_json = ZGZY.Common.JsonHelper.ToJson(gsp_bypktctrlnbr_dt);
                            context.Response.Write(gsp_bypktctrlnbr_json);

                            break;

                        default:
                            context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                            break;
                    }

            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + ZGZY.Common.JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "扫码校验功能异常";
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