using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace InwardClearingSystem.Account
{
    public partial class SignUp : BasePage
    {
        BasePage bp = new BasePage();
        String function = "User Maintenance";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (bp.checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

        }

        /// <summary>
        /// Cancels the add user operation.
        /// </summary>
        protected void canceBtn_Click(object sender, EventArgs e)
        {
            if (this.Context != null)
            {
                Response.Redirect("~/UserMaintenance.aspx");
            }
        }

        /// <summary>
        /// Registers the new user.
        /// </summary>
        protected void regBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (bp.checkForDuplicateData("username", "[User]", unTxtBx.Text) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "The username already exists. Please choose another.";
                }
                else if (bp.checkForDuplicateData("email", "[User]", emTxtBx.Text) == true)
                {
                    lblError.Visible = true;
                    lblError.Text = "The email is taken. Please choose another.";
                }
                else if (passTxtBx.Text != cpassTxtBx.Text)
                {
                    lblError.Visible = true;
                    lblError.Text = "Passwords do not match.";
                }
                else if (passTxtBx.Text.Count() < 8)
                {
                    lblError.Visible = true;
                    lblError.Text = "Password must have at least 8 characters.";
                }
                else
                {
                    using (activeConnectionOpen())
                    {
                        SqlCommand insert = new SqlCommand();
                        insert.CommandText = "InsertUser";
                        insert.CommandType = CommandType.StoredProcedure;
                        insert.Connection = activeConnection;
                        insert.Parameters.AddWithValue("@username", unTxtBx.Text);
                        insert.Parameters.AddWithValue("@password", passTxtBx.Text);
                        insert.Parameters.AddWithValue("@f_name", fnTxtBx.Text);
                        insert.Parameters.AddWithValue("@m_name", mnTxtBx.Text);
                        insert.Parameters.AddWithValue("@l_name", lnTxtBx.Text);
                        insert.Parameters.AddWithValue("@email", emTxtBx.Text);
                        insert.ExecuteNonQuery();
                        if (this.Context != null)
                        {
                            Response.Redirect("~/UserMaintenance.aspx");
                        }
                    }
                }
            }
            catch
            {
                bp.Message("An error has occurred. Please try again.");
            }
        }
    }
}