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
    public partial class userclassreport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int itemid;
            if (Request.Params["Id"] != null)
            {
                itemid = Int32.Parse(Request.Params["Id"].ToString());
                ReportDocument classrpt = new ReportDocument();
                classrpt.Load(Server.MapPath("./CrystalReport1.rpt"));
                classrpt.SetDatabaseLogon("sa", "sa123456", "173.5.28.153", "ZGZY");
                ParameterValues pv;
                ParameterDiscreteValue pdv = new ParameterDiscreteValue();
                pv = classrpt.DataDefinition.ParameterFields["@classid"].CurrentValues;
                pdv.Value = itemid;
                pv.Add(pdv);
                classrpt.DataDefinition.ParameterFields["@classid"].ApplyCurrentValues(pv);

                this.CrystalReportViewer1.ReportSource = classrpt;
                this.CrystalReportViewer1.DataBind();
            }
            else
            {
                Response.Write("no id");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ReportDocument classrpt = new ReportDocument();
            classrpt.Load(Server.MapPath("./CrystalReport1.rpt"));
            classrpt.SetDatabaseLogon("sa", "sa123456", "173.5.28.153", "ZGZY");
            ParameterValues pv;
            ParameterDiscreteValue pdv = new ParameterDiscreteValue();
            pv = classrpt.DataDefinition.ParameterFields["@classid"].CurrentValues;
            pdv.Value = this.DropDownList1.SelectedItem.Value;
            pv.Add(pdv);
            classrpt.DataDefinition.ParameterFields["@classid"].ApplyCurrentValues(pv);

            this.CrystalReportViewer1.ReportSource = classrpt;
            this.CrystalReportViewer1.DataBind();
            

        }
    }
}