using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace ZGZY.WebUI.admin.html
{
    public partial class pkt_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string itemid;
            if (Request.Params["pkt_nbr"] != null)
            {
                itemid = Request.Params["pkt_nbr"].ToString();
                ReportDocument classrpt = new ReportDocument();
                classrpt.Load(Server.MapPath("pkt.rpt"));
                
                classrpt.SetDatabaseLogon("wmrdc", "wmrdc");
                ParameterValues pv;
                ParameterDiscreteValue pdv = new ParameterDiscreteValue();
                pv = classrpt.DataDefinition.ParameterFields["pkt_nbr"].CurrentValues;
                pdv.Value = itemid;
                pv.Add(pdv);
                classrpt.DataDefinition.ParameterFields["pkt_nbr"].ApplyCurrentValues(pv);

                //classrpt.PrintOptions.PrinterName = "doPDF v7";
                //classrpt.PrintToPrinter(1, true, 0, 0);

                this.CrystalReportViewer1.ReportSource = classrpt;
                this.CrystalReportViewer1.DataBind();
                
            }
            else
            {
                Response.Write("no id");
            }
        }
    }
}