using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class UpdateThreshold : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool login = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (login == false)
                Response.Redirect("~/Account/LogIn.aspx");

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand select = new SqlCommand("select minimum from THRESHOLD", connection);
            Label2.Text = select.ExecuteScalar().ToString();
            SqlCommand select2 = new SqlCommand("select maximum from THRESHOLD", connection);
            Label5.Text = select2.ExecuteScalar().ToString();
            connection.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update THRESHOLD SET minimum = @thresh", connection);
            update.Parameters.AddWithValue("@thresh", TextBox1.Text);
            update.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("Verification.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update THRESHOLD SET maximum = @thresh", connection);
            update.Parameters.AddWithValue("@thresh", TextBox2.Text);
            update.ExecuteNonQuery();
            connection.Close();
            Response.Redirect("Confirmation.aspx");
        }
    }
}