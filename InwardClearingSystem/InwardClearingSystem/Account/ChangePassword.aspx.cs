using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InwardClearingSystem.Account
{
    public partial class ChangePassword : BasePage
    {
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void ChangeUserPassword_ChangingPassword(object sender, LoginCancelEventArgs e)
        {
            using (activeConnection)
            {
                activeConnection.Open();
                cmd = new SqlCommand("update END_USER SET password = @newpass where END_USER.username = @user", activeConnection);
                cmd.Parameters.AddWithValue("@newpass", ChangeUserPassword.NewPassword);
                cmd.Parameters.AddWithValue("@user", Session["UserName"]);
                cmd.ExecuteNonQuery();
            }
        }

        protected void ChangeUserPassword_ContinueButtonClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}
