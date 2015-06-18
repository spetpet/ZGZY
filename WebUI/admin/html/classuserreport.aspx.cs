using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Insus.NET;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ZGZY.WebUI.admin.html
{
    public partial class classuserreport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int itemid;
            if (Request.Params["userid"] != null)
            {
                itemid = Int32.Parse(Request.Params["userid"].ToString());
                ReportDocument classrpt = new ReportDocument();
                classrpt.Load(Server.MapPath("./getclassbyuser.rpt"));
                classrpt.SetDatabaseLogon("sa", "sa123456", "173.5.28.153", "ZGZY");
                ParameterValues pv;
                ParameterDiscreteValue pdv = new ParameterDiscreteValue();
                pv = classrpt.DataDefinition.ParameterFields["@userid"].CurrentValues;
                pdv.Value = itemid;
                pv.Add(pdv);
                classrpt.DataDefinition.ParameterFields["@userid"].ApplyCurrentValues(pv);

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