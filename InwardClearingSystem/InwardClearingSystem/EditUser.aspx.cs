using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace InwardClearingSystem
{
    public partial class EditUser : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            unTxtBx.Text = Session["TBEusername"].ToString();
            fnTxtBx.Text = Session["TBEfirstname"].ToString();
            mnTxtBx.Text = Session["TBEmiddlename"].ToString();
            lnTxtBx.Text = Session["TBElastname"].ToString();
            emTxtBx.Text = Session["TBEemail"].ToString();
            passTxtBx.Text = Session["TBEpassword"].ToString();
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            using (connection)
            {
                connection.Open();
                using (SqlCommand usernameChecker = new SqlCommand("SELECT username FROM [User] WHERE username = @username", connection))
                {
                    usernameChecker.Parameters.AddWithValue("@username", unTxtBx.Text);
                    using (SqlCommand emailChecker = new SqlCommand("SELECT email FROM [User] WHERE email = @email", connection))
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
                                        using (SqlCommand update = new SqlCommand())
                                        {
                                            update.CommandText = "UpdateUser";
                                            update.CommandType = CommandType.StoredProcedure;
                                            update.Connection = connection;
                                            update.Parameters.AddWithValue("@username", unTxtBx.Text);
                                            update.Parameters.AddWithValue("@password", passTxtBx.Text);
                                            update.Parameters.AddWithValue("@f_name", fnTxtBx.Text);
                                            update.Parameters.AddWithValue("@m_name", mnTxtBx.Text);
                                            update.Parameters.AddWithValue("@l_name", lnTxtBx.Text);
                                            update.Parameters.AddWithValue("@email", emTxtBx.Text);
                                            update.ExecuteNonQuery();
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