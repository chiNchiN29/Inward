using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand usernameChecker = new SqlCommand("SELECT username FROM END_USER WHERE username = '" + TextBox1.Text + "'", connection);
            SqlCommand emailChecker = new SqlCommand("SELECT email FROM END_USER WHERE email = '" + TextBox2.Text + "'", connection);
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
                    if (TextBox3.Text == TextBox4.Text)
                    {
                        SqlCommand insert = new SqlCommand("insert into END_USER(username, password, email) values (@user, @pass, @mail)", connection);

                        insert.Parameters.AddWithValue("@user", TextBox1.Text);
                        insert.Parameters.AddWithValue("@pass", TextBox3.Text);
                        insert.Parameters.AddWithValue("@mail", TextBox2.Text);
                        insert.ExecuteNonQuery();
                        Response.Redirect("~/Account/Login.aspx");
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