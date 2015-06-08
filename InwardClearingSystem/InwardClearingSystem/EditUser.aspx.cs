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
        string usernamePH;
        string firstPH;
        string middlePH;
        string lastPH;
        string emailPH;
        string passwordPH;
        protected void Page_Load(object sender, EventArgs e)
        {
            usernamePH = Session["TBEusername"].ToString();
            firstPH = Session["TBEfirstname"].ToString();
            middlePH = Session["TBEmiddlename"].ToString();
            lastPH = Session["TBElastname"].ToString();
            emailPH = Session["TBEemail"].ToString();
            passwordPH = Session["TBEpassword"].ToString();
            try
            {
                unTxtBx.Text = usernamePH;
                fnTxtBx.Text = firstPH;
                mnTxtBx.Text = middlePH;
                lnTxtBx.Text = lastPH;
                emTxtBx.Text = emailPH;
                passTxtBx.Text = passwordPH;
            }
            catch
            {
                Server.Transfer("~/UserMaintenance.aspx");
            }
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            using (connection)
            {
                connection.Open();
   
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
                    string moop = fnTxtBx.Text;
                    update.Parameters.AddWithValue("@referenceUserName", usernamePH);
                    update.ExecuteNonQuery();
                    Response.Redirect("~/UserMaintenance.aspx");
                }
            }
        }
    }
}