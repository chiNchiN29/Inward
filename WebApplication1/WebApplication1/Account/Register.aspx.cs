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
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();

            SqlCommand addnew = new SqlCommand("INSERT INTO END_USER(username, password, email) VALUES (@user, @pass, @mail)", con);
            addnew.Parameters.AddWithValue("@user", RegisterUser.UserName);
            addnew.Parameters.AddWithValue("@pass", RegisterUser.Password);
            addnew.Parameters.AddWithValue("@mail", RegisterUser.Email);
            addnew.ExecuteNonQuery();
            Response.Redirect("/");
        }

    }
}
