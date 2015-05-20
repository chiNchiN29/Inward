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
        HttpCookie newUserSession = new HttpCookie("Username");
        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();

            RegisterHyperLink.NavigateUrl = "SignUp.aspx";
            try
            {
                if (!IsPostBack)
                {
                    if ((Request.Cookies["Username"] != null))
                    {
                        Login1.UserName = Request.Cookies["Username"].Value;
                        Login1.RememberMeSet = true;
                    }
                }
            }
            catch
            {
                Response.Write("Unable to process request at this time.");
            }
        }

        protected void Button1_Click(object sender, AuthenticateEventArgs e)
        {
            SqlCommand usernameChecker = new SqlCommand("SELECT username FROM END_USER WHERE username = '" + Login1.UserName + "'", connection);
            SqlCommand passwordChecker = new SqlCommand("SELECT password FROM END_USER WHERE username = '" + Login1.UserName + "'", connection);

            string username = usernameChecker.ExecuteScalar().ToString();
            string password = passwordChecker.ExecuteScalar().ToString();

            if (usernameChecker.ExecuteScalar() != null && passwordChecker.ExecuteScalar() != null)
            {
                if (Login1.UserName == usernameChecker.ExecuteScalar().ToString() && Login1.Password == passwordChecker.ExecuteScalar().ToString())
                {
                    //FormsAuthentication.RedirectFromLoginPage(TextBox1.Text, CheckBox1.Checked);
                    SqlCommand cmd = new SqlCommand("select * from END_USER where username = @userName " +
                    "AND password = @password", connection);
                    cmd.Parameters.AddWithValue("@userName", Login1.UserName);
                    cmd.Parameters.AddWithValue("@password", Login1.Password);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Session["UserName"] = (string)dr["username"];
                        Session["FirstName"] = dr["first_name"].ToString();
                        Session["LastName"] = dr["last_name"].ToString();
                        Session["Email"] = dr["email"].ToString();
                        Session["RoleID"] = Convert.ToInt32(dr["role_id"]);
                    }
                    e.Authenticated = true;
                    Login1.DestinationPageUrl = "~/Default.aspx";

                    connection.Close();
                }
            }

        }
    }
}
