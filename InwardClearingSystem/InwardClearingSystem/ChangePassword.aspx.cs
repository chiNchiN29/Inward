using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace InwardClearingSystem
{
    public partial class ChangePassword : BasePage
    {
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void cancelChange_Click(object sender, EventArgs e)
        {
            if (this.Context != null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void confirmChange_Click(object sender, EventArgs e)
        {
            using (activeConnection)
            {
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdatePassword";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@newpass", newPassword.Text);
                    cmd.Parameters.AddWithValue("@user", Session["UserName"]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}