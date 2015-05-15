using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace WebApplication1.Account
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            
            RegisterHyperLink.NavigateUrl = "SignUp.aspx";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Membership.ValidateUser(TextBox1.Text, TextBox2.Text))
            {
                connection.Open();
                SqlCommand usernameChecker = new SqlCommand("SELECT username FROM END_USER WHERE username = '" + TextBox1.Text + "'", connection);
                SqlCommand passwordChecker = new SqlCommand("SELECT password FROM END_USER WHERE username = '" + TextBox1.Text + "'", connection);
                if (usernameChecker.ExecuteScalar() != null && passwordChecker.ExecuteScalar() != null)
                {
                    if (TextBox1.Text == usernameChecker.ExecuteScalar().ToString() && TextBox2.Text == passwordChecker.ExecuteScalar().ToString())
                    {
                        FormsAuthentication.RedirectFromLoginPage(TextBox1.Text, CheckBox1.Checked);
                    }
                    else
                    {
                        Label2.Visible = true;
                    }
                }
                else
                {
                    Label2.Visible = true;
                }
            }
        }
    }
}
