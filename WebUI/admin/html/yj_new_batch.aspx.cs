using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ZGZY.WebUI.admin.html
{
    public partial class yj_new_batch : System.Web.UI.Page
    {
        public string batch_nbr, season, size_desc,url;

        protected void Page_Load(object sender, EventArgs e)
        {
            batch_nbr = Request.Params["BATCH"].ToString() ?? "";
            season = Request.Params["SEASON"].ToString() ?? "";
            size_desc = Request.Params["SIZE_DESC"].ToString() ?? "";
            FileInfo file = new FileInfo(Server.MapPath("~/admin/bayer_yj/") + season + "/" +size_desc+ batch_nbr + ".jpg");
            if (file.Exists)
                url = "~/admin/bayer_yj/" + season + "/" +size_desc+batch_nbr + ".jpg";
            else
                url = "~/admin/yj/nopic.png";
            Image1.ImageUrl = url;

        }
    }
}