using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void passChange(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand checker = new SqlCommand("SELECT password FROM END_USER WHERE username = '" + Membership.GetUser().UserName + "'", con);
            if (ChangeUserPassword.CurrentPassword == checker.ExecuteScalar().ToString())
            {
                SqlCommand addnew = new SqlCommand("UPDATE END_USER SET password = @pass WHERE username = '" + Membership.GetUser().UserName + "'", con);
                addnew.Parameters.AddWithValue("@pass", ChangeUserPassword.NewPassword);
                addnew.ExecuteNonQuery();
                Response.Redirect("/");
            }
            else
            {
                Response.Write("Current password is invalid.");
            }
        }
    }
}
