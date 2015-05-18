using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.Globalization;

namespace WebApplication1
{
    public partial class UpdateThreshold : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand select = new SqlCommand("select minimum from THRESHOLD", connection);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PH", false);
            Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
            
            SqlCommand select2 = new SqlCommand("select maximum from THRESHOLD", connection);
            Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
            

                  connection.Close();
        }

        protected void SetThresholds(object sender, EventArgs e)
        {
            if (TextBox1.Text != "" || TextBox2.Text != "")
            {
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    connection.Open();
                    int num1;
                    if (TextBox1.Text != "")
                    {
                        bool boop = int.TryParse(TextBox1.Text, out num1);
                        if (boop == true)
                        {
                            SqlCommand updateMin = new SqlCommand("update THRESHOLD SET minimum = @thresh", connection);
                            updateMin.Parameters.AddWithValue("@thresh", TextBox1.Text);
                            updateMin.ExecuteNonQuery();
                            SqlCommand select = new SqlCommand("select minimum from THRESHOLD", connection);
                            Label2.Text = String.Format("{0:C}", select.ExecuteScalar());
                        }
                    }

                    if (TextBox2.Text != "")
                    {
                        bool poob = int.TryParse(TextBox2.Text, out num1);
                        if (poob == true)
                        {
                            SqlCommand updateMax = new SqlCommand("update THRESHOLD SET maximum = @thresh", connection);
                            updateMax.Parameters.AddWithValue("@thresh", TextBox2.Text);
                            updateMax.ExecuteNonQuery();
                            SqlCommand select2 = new SqlCommand("select maximum from THRESHOLD", connection);
                            Label5.Text = String.Format("{0:C}", select2.ExecuteScalar());
                        }
                    }
                    connection.Close();
                }
            
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}