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

namespace InwardClearingSystem
{
    public partial class UpdateThreshold : BasePage
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnectionOpen();
            SqlCommand cmd = new SqlCommand("SELECT role_desc FROM [User] u, Role r WHERE username = @name AND u.role_id = r.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@name", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "ADMIN" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                Message("You are not authorized to view this page");
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                SqlCommand select = new SqlCommand("select minimum from Threshold", activeConnection);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PH", false);
                Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
                SqlCommand select2 = new SqlCommand("select maximum from Threshold", activeConnection);
                Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
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
                            SqlCommand updateMin = new SqlCommand("update Threshold SET minimum = @thresh", activeConnection);
                            updateMin.Parameters.AddWithValue("@thresh", TextBox1.Text);
                            updateMin.ExecuteNonQuery();
                            SqlCommand select = new SqlCommand("select minimum from Threshold", activeConnection);
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
                            SqlCommand updateMax = new SqlCommand("update Threshold SET maximum = @thresh", activeConnection);
                            updateMax.Parameters.AddWithValue("@thresh", TextBox2.Text);
                            updateMax.ExecuteNonQuery();
                            SqlCommand select2 = new SqlCommand("select maximum from Threshold", activeConnection);
                            Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
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