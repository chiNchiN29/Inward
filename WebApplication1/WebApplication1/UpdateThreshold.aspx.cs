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

namespace WebApplication1
{
    public partial class UpdateThreshold : BasePage
    {
        SqlCommand cmd;
   
        protected void Page_Load(object sender, EventArgs e)
        {
            cmd = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Session["UserName"] + "' AND END_USER.role_id = ROLE.role_id", activeConnection);
            if (cmd.ExecuteScalar().ToString() != "ADMIN" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                ErrorMessage("You are not authorized to view this page");
                Response.Redirect("Default.aspx");
            }
            else
            {
                SqlCommand select = new SqlCommand("select minimum from THRESHOLD", activeConnection);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PH", false);
                Label2.Text = String.Format("{0:C}", select.ExecuteScalar());

                SqlCommand select2 = new SqlCommand("select maximum from THRESHOLD", activeConnection);
                Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
            }
            activeConnection.Close();
        }

        protected void SetThresholds(object sender, EventArgs e)
        {
            activeConnection.Open();
            if (TextBox1.Text != "" || TextBox2.Text != "")
            {
                    int num1;
                    if (TextBox1.Text != "")
                    {
                        bool boop = int.TryParse(TextBox1.Text, out num1);
                        if (boop == true)
                        {
                            SqlCommand updateMin = new SqlCommand("update THRESHOLD SET minimum = @thresh", activeConnection);
                            updateMin.Parameters.AddWithValue("@thresh", TextBox1.Text);
                            updateMin.ExecuteNonQuery();
                            SqlCommand select = new SqlCommand("select minimum from THRESHOLD", activeConnection);
                            Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
                        }
                    }

                    if (TextBox2.Text != "")
                    {
                        bool poob = int.TryParse(TextBox2.Text, out num1);
                        if (poob == true)
                        {
                            SqlCommand updateMax = new SqlCommand("update THRESHOLD SET maximum = @thresh", activeConnection);
                            updateMax.Parameters.AddWithValue("@thresh", TextBox2.Text);
                            updateMax.ExecuteNonQuery();
                            SqlCommand select2 = new SqlCommand("select maximum from THRESHOLD", activeConnection);
                            Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
                        }
                    }
                    
                }
           
            else
            {
                Response.Redirect("Default.aspx");
            }
            activeConnection.Close();
        }
    }
}