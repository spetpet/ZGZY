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
    public partial class yh_ni_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sysdate;
            if (Request.Params["sysdate"] != null)
            {
                sysdate = Request.Params["sysdate"].ToString();
                if (sysdate.Length < 10)
                    sysdate = sysdate.Substring(0, 5) + "0" + sysdate.Substring(5, 4);
                ReportDocument classrpt = new ReportDocument();
                classrpt.Load(Server.MapPath("yh_ni.rpt"));

                classrpt.SetDatabaseLogon("sa", "sa123456");
                ParameterValues pv;
                ParameterDiscreteValue pdv = new ParameterDiscreteValue();
                pv = classrpt.DataDefinition.ParameterFields["Date"].CurrentValues;
                pdv.Value = sysdate;
                pv.Add(pdv);
                classrpt.DataDefinition.ParameterFields["Date"].ApplyCurrentValues(pv);

                //classrpt.PrintOptions.PrinterName = "doPDF v7";
                //classrpt.PrintToPrinter(1, true, 0, 0);

                this.CrystalReportViewer1.ReportSource = classrpt;
                this.CrystalReportViewer1.DataBind();

            }
            else
            {
                Response.Write("no date");
            }
        }
    }
}