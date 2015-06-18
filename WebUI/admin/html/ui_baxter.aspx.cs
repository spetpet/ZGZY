using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using System.Data.OleDb;

namespace ZGZY.WebUI.admin.html
{
    public partial class ui_baxter : System.Web.UI.Page
    {
        public static string oraconnectstr = "Provider=OraOLEDB.Oracle;Data Source=wmrdc;Persist Security Info=True;User ID=wmrdc;Password=wmrdc;Unicode=True";
        public static string sqlconnectstr = "Provider=sqloledb;Data Source=173.5.28.153;Persist Security Info=True;Password=sa123456;User ID=sa;Initial Catalog=MA";
        public OleDbConnection oraconn = new OleDbConnection(oraconnectstr);
        public OleDbConnection sqlconn = new OleDbConnection(sqlconnectstr);
        public StringBuilder date_str = new StringBuilder();
        public StringBuilder inv_str = new StringBuilder();
        public StringBuilder in_str = new StringBuilder();
        public StringBuilder out_str = new StringBuilder();


        protected void Page_Load(object sender, EventArgs e)
        {

            //int i=0;
            //string temp_str = "";
            string data_sql = "select * from baxter_inv order by recdate";
            OleDbCommand data_command = new OleDbCommand(data_sql, sqlconn);
            sqlconn.Open();
            OleDbDataReader data_reader = data_command.ExecuteReader();





            while (data_reader.Read())
            {
                inv_str.Append("[");
                inv_str.Append(MilliTimeStamp(Convert.ToDateTime(data_reader[0].ToString())));
                inv_str.Append(",");
                inv_str.Append(data_reader[1].ToString());
                inv_str.Append("]");
                inv_str.Append(",");
                in_str.Append("[");
                in_str.Append(MilliTimeStamp(Convert.ToDateTime(data_reader[0].ToString())));
                in_str.Append(",");
                in_str.Append(data_reader[2].ToString());
                in_str.Append("]");
                in_str.Append(",");
                out_str.Append("[");
                out_str.Append(MilliTimeStamp(Convert.ToDateTime(data_reader[0].ToString())));
                out_str.Append(",");
                out_str.Append(data_reader[3].ToString());
                out_str.Append("]");
                out_str.Append(",");



            }

            inv_str.Replace(",", "", inv_str.Length - 1, 1);
            in_str.Replace(",", "", in_str.Length - 1, 1);
            out_str.Replace(",", "", out_str.Length - 1, 1);



        }

        public long MilliTimeStamp(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1969, 12, 31);
            DateTime d2 = TheDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            //TimeSpan ts = d2 - d1.Ticks;
            return (long)ts.TotalMilliseconds;
        }


    }
}