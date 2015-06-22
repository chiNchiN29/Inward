using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InwardClearingSystem
{
    public partial class UpdateThreshold : BasePage
    {
        string function = "Update Threshold";
   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                using (SqlCommand select = new SqlCommand())
                {
                    select.CommandText = "GetBypassValueStandard";
                    select.CommandType = CommandType.StoredProcedure;
                    select.Connection = activeConnection;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PH", false);
                    Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
                }
                using (SqlCommand select2 = new SqlCommand())
                {
                    select2.CommandText = "GetHighValueStandard";
                    select2.CommandType = CommandType.StoredProcedure;
                    select2.Connection = activeConnection;
                    Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
                }
            }
     
        }

        protected void setThresholds_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox1.Text != "" || TextBox2.Text != "")
                {
                    int num1;
                    if (TextBox1.Text != "")
                    {
                        bool boop = int.TryParse(TextBox1.Text, out num1);
                        if (boop == true)
                        {
                            using (activeConnectionOpen())
                            {
                                SqlCommand updateMin = new SqlCommand();
                                updateMin.CommandText = "UpdateBypassValueStandard";
                                updateMin.CommandType = CommandType.StoredProcedure;
                                updateMin.Connection = activeConnection;
                                updateMin.Parameters.AddWithValue("@thresh", TextBox1.Text);
                                updateMin.ExecuteNonQuery();

                                SqlCommand select = new SqlCommand();
                                select.CommandText = "GetBypassValueStandard";
                                select.CommandType = CommandType.StoredProcedure;
                                select.Connection = activeConnection;
                                Label2.Text = String.Format("{0:C}", select.ExecuteScalar());

                            }
                        }
                    }

                    if (TextBox2.Text != "")
                    {
                        bool poob = int.TryParse(TextBox2.Text, out num1);
                        if (poob == true)
                        {
                            using (activeConnectionOpen())
                            {
                                SqlCommand updateMax = new SqlCommand();
                                updateMax.CommandText = "UpdateHighValueStandard";
                                updateMax.CommandType = CommandType.StoredProcedure;
                                updateMax.Connection = activeConnection;
                                updateMax.Parameters.AddWithValue("@thresh", TextBox2.Text);
                                updateMax.ExecuteNonQuery();

                                SqlCommand select2 = new SqlCommand();
                                select2.CommandText = "GetHighValueStandard";
                                select2.CommandType = CommandType.StoredProcedure;
                                select2.Connection = activeConnection;
                                Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());

                            }
                        }
                    }

                }
                else
                {
                    if (this.Context != null)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }
    }
}