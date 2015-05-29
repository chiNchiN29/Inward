using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Data;

namespace InwardClearingSystem.Account
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        HttpCookie UserCookie = new HttpCookie("Username");
        protected void Page_Load(object sender, EventArgs e)
        {


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
            int role_id;
            using (connection)
            {
                connection.Open();
                SqlCommand usernameChecker = new SqlCommand("SELECT username FROM [User] WHERE username = @username", connection);
                usernameChecker.Parameters.AddWithValue("@username", Login1.UserName);
                SqlCommand passwordChecker = new SqlCommand("SELECT password FROM [User] WHERE username = @username", connection);
                passwordChecker.Parameters.AddWithValue("@username", Login1.UserName);
                if (usernameChecker.ExecuteScalar() != null && passwordChecker.ExecuteScalar() != null)
                {
                    string username = usernameChecker.ExecuteScalar().ToString();
                    string password = passwordChecker.ExecuteScalar().ToString();
                    if (Login1.UserName == username && Login1.Password == password)
                    {
                        SqlCommand cmd = new SqlCommand("select * from [User] where username = @userName AND password = @password", connection);
                        cmd.Parameters.AddWithValue("@userName", Login1.UserName);
                        cmd.Parameters.AddWithValue("@password", Login1.Password);
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Session["UserName"] = (string)dr["username"];
                            Session["FirstName"] = dr["f_name"].ToString();
                            Session["LastName"] = dr["l_name"].ToString();
                            Session["Email"] = dr["email"].ToString();
                            Session["RoleID"] = Convert.ToInt32(dr["role_id"]);
                            Session["UserID"] = Convert.ToInt32(dr["user_id"]);
                        }
                        e.Authenticated = true;
                        role_id = Convert.ToInt32(Session["RoleID"]);
                        switch (role_id)
                        {
                            default:
                                Login1.DestinationPageUrl = "~/Default.aspx";
                                connection.Close();
                                break;

                            case 2:
                                Login1.DestinationPageUrl = "~/Verification.aspx";
                                connection.Close();
                                break;

                            case 3:
                                Login1.DestinationPageUrl = "~/Confirmation.aspx";
                                connection.Close();
                                break;
                        }
                    }
                }
                else
                {
                    e.Authenticated = false;
                }

                if (Login1.RememberMeSet)
                {
                    Response.Cookies["Username"].Value = Login1.UserName.ToString();
                    Response.Cookies["Username"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    if (Response.Cookies["Username"] != null)
                    {
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-30);
                    }
                }
            }
        }
    }
}
