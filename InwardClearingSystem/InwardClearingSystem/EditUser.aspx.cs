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
        BasePage bp = new BasePage();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        String function = "User Maintenance";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (bp.checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    unTxtBx.Text = Session["TBEusername"].ToString();
                    fnTxtBx.Text = Session["TBEfirstname"].ToString();
                    mnTxtBx.Text = Session["TBEmiddlename"].ToString();
                    lnTxtBx.Text = Session["TBElastname"].ToString();
                    emTxtBx.Text = Session["TBEemail"].ToString();
                    passTxtBx.Text =Session["TBEpassword"].ToString();
                }
                catch
                {
                    Server.Transfer("~/UserMaintenance.aspx");
                }
            }
        }

        protected void editBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection)
                {
                    connection.Open();

                    SqlCommand update = new SqlCommand();

                    update.CommandText = "UpdateUser";
                    update.CommandType = CommandType.StoredProcedure;
                    update.Connection = connection;
                    update.Parameters.AddWithValue("@username", unTxtBx.Text);
                    update.Parameters.AddWithValue("@password", passTxtBx.Text);
                    update.Parameters.AddWithValue("@f_name", fnTxtBx.Text);
                    update.Parameters.AddWithValue("@m_name", mnTxtBx.Text);
                    update.Parameters.AddWithValue("@l_name", lnTxtBx.Text);
                    update.Parameters.AddWithValue("@email", emTxtBx.Text);
                    update.Parameters.AddWithValue("@referenceID", Convert.ToInt32(Session["TBEuserID"]));
                    update.ExecuteNonQuery();
                    if (this.Context != null)
                    {
                        Response.Redirect("~/UserMaintenance.aspx");
                    }

                }
            }
            catch
            {
                throw;
            }
        }

        protected void canceBtn_Click(object sender, EventArgs e)
        {
            Server.Transfer("~/UserMaintenance.aspx");
        }
    }
}