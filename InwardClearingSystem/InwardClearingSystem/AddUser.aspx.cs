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
    public partial class SignUp : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void regBtn_Click(object sender, EventArgs e)
        {
            using (connection)
            {
                connection.Open();
                using (SqlCommand usernameChecker = new SqlCommand())
                {
                    usernameChecker.CommandText = "UsernameDuplicateChecker";
                    usernameChecker.CommandType = CommandType.StoredProcedure;
                    usernameChecker.Connection = connection;
                    usernameChecker.Parameters.AddWithValue("@username", unTxtBx.Text);

                    using (SqlCommand emailChecker = new SqlCommand())
                    {
                        emailChecker.CommandText = "EMailDuplicateChecker";
                        emailChecker.CommandType = CommandType.StoredProcedure;
                        emailChecker.Connection = connection;
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
                                        using (SqlCommand insert = new SqlCommand())
                                        {
                                            insert.CommandText = "InsertUser";
                                            insert.CommandType = CommandType.StoredProcedure;
                                            insert.Connection = connection;
                                            insert.Parameters.AddWithValue("@username", unTxtBx.Text);
                                            insert.Parameters.AddWithValue("@password", passTxtBx.Text);
                                            insert.Parameters.AddWithValue("@f_name", fnTxtBx.Text);
                                            insert.Parameters.AddWithValue("@m_name", mnTxtBx.Text);
                                            insert.Parameters.AddWithValue("@l_name", lnTxtBx.Text);
                                            insert.Parameters.AddWithValue("@email", emTxtBx.Text);
                                            insert.ExecuteNonQuery();
                                            Response.Redirect("~/UserMaintenance.aspx");
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
}