using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OracleClient;
using System.Data;
using System.Text;

namespace ZGZY.WebUI.admin.html
{
    public partial class ui_floorview : System.Web.UI.Page
    {
        public int f1=0, f2=0, f3=0,fc=0;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sql_sb = new StringBuilder();
            sql_sb.Append("select * from s00_realtime_inv_byfloor");
            DataTable inv_dt = ZGZY.Common.SqlHelper.GetOracleDataTable(ZGZY.Common.SqlHelper.wmrdc_connStr, CommandType.Text, sql_sb.ToString());
            
            if (inv_dt.Rows.Count != 0)
            {
                foreach (DataRow inv_dr in inv_dt.Rows)
                {
                    if (inv_dr[2].ToString() == "SW") fc += Convert.ToInt32(inv_dr[4].ToString());
                    else 
                    {
                        switch (inv_dr[0].ToString())
                        {
                            case "1": f1 += Convert.ToInt32(inv_dr[4].ToString());
                                break;
                            case "2": f2 += Convert.ToInt32(inv_dr[4].ToString());
                                break;
                            case "3": f3 += Convert.ToInt32(inv_dr[4].ToString());
                                break;
                            default: break;

                        }

                    }
                }
            }
        }
    }
}