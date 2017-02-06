using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LiXinCommon;

namespace LiXinLMS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var loginFrom = ConfigurationManager.AppSettings["loginFrom"];
            var token = BaseEncode.GetMd5Str(TextBox1.Text.Trim() + DateTime.Now.ToString("yyyyMMdd") + loginFrom);

            Response.Redirect(string.Format("/Login/SSO?token={0}&loginid={1}", token, TextBox1.Text.Trim()));
        }
    }
}