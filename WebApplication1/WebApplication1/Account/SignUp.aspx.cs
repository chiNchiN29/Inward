using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Account
{
    public partial class SignUp : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void regBtn_Click(object sender, EventArgs e)
        {
            connection.Open();
            using (SqlCommand usernameChecker = new SqlCommand("SELECT username FROM END_USER WHERE username = @username", connection))
            {
                usernameChecker.Parameters.AddWithValue("@username", unTxtBx.Text);
                using (SqlCommand emailChecker = new SqlCommand("SELECT email FROM END_USER WHERE email = @email", connection))
                {
                    emailChecker.Parameters.AddWithValue("@email", emTxtBx.Text);
                    if (usernameChecker.ExecuteScalar() != null)
                    {
                        Response.Write("The username is taken. Please choose another.");
                    }
                    else
                    {
                        if (emailChecker.ExecuteScalar() != null)
                        {
                            Response.Write("The email is taken. Please choose another.");
                        }
                        else
                        {
                            if (passTxtBx.Text == cpassTxtBx.Text)
                            {
                                if (passTxtBx.Text.Count() < 8)
                                {
                                    Label4.Visible = true;
                                    Label4.Text = "You must have at least 8 characters for your password.";
                                }
                                else
                                {
                                    using (SqlCommand insert = new SqlCommand("insert into END_USER(username, first_name, last_name, password, email) values (@user, @first, @last, @pass, @mail)", connection))
                                    {
                                        insert.Parameters.AddWithValue("@user", unTxtBx.Text);
                                        insert.Parameters.AddWithValue("@first", fnTxtBx.Text);
                                        insert.Parameters.AddWithValue("@last", lnTxtBx.Text);
                                        insert.Parameters.AddWithValue("@pass", passTxtBx.Text);
                                        insert.Parameters.AddWithValue("@mail", emTxtBx.Text);
                                        insert.ExecuteNonQuery();
                                        Response.Redirect("~/Account/Login.aspx");
                                    }
                                }
                            }
                            else
                            {
                                Response.Write("The passwords do not match. Please try again.");
                            }
                        }
                    }
                }
            }
        }
    }
}