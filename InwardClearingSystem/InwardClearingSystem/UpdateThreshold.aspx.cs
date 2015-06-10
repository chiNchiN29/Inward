using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace InwardClearingSystem
{
    public partial class UpdateThreshold : BasePage
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnectionOpen();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "CheckUserRole";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnection;
                cmd.Parameters.AddWithValue("@username", Session["UserName"]);
                if (cmd.ExecuteScalar().ToString() != "ADMIN" && cmd.ExecuteScalar().ToString() != "OVERSEER")
                {
                    Message("You are not authorized to view this page");
                    Response.Redirect("~/Default.aspx");
                }
                else
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
            activeConnectionClose();
        }

        protected void SetThresholds(object sender, EventArgs e)
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
                            using (SqlCommand updateMin = new SqlCommand())
                            {
                                updateMin.CommandText = "UpdateBypassValueStandard";
                                updateMin.CommandType = CommandType.StoredProcedure;
                                updateMin.Connection = activeConnection;
                                updateMin.Parameters.AddWithValue("@thresh", TextBox1.Text);
                                updateMin.ExecuteNonQuery();
                            }
                            using (SqlCommand select = new SqlCommand())
                            {
                                select.CommandText = "GetBypassValueStandard";
                                select.CommandType = CommandType.StoredProcedure;
                                select.Connection = activeConnection;
                                Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
                            }
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
                            using (SqlCommand updateMax = new SqlCommand())
                            {
                                updateMax.CommandText = "UpdateHighValueStandard";
                                updateMax.CommandType = CommandType.StoredProcedure;
                                updateMax.Connection = activeConnection;
                                updateMax.Parameters.AddWithValue("@thresh", TextBox2.Text);
                                updateMax.ExecuteNonQuery();
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
                }
                    
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}