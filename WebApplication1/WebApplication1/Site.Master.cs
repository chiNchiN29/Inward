using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            //bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            //if (login == false)
            //{
            //    NavigationMenu.Visible = false;
            //}
            //else
            //{
            //    connection.Open();
            //    Label3.Text = System.DateTime.Now.ToLongTimeString();
            //    string meep = Membership.GetUser().UserName;
            //    SqlCommand checker = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Membership.GetUser().UserName + "' AND END_USER.role_id = ROLE.role_id", connection);
            //        if (checker.ExecuteScalar().ToString() == "ADMIN")
            //        {
            //            NavigationMenu.FindItem("Signature Verification").Enabled = false;
            //            NavigationMenu.FindItem("Confirmation").Enabled = false;
            //        }
            //        else if (checker.ExecuteScalar().ToString() == "CLEARING DEPT")
            //        {
            //            NavigationMenu.FindItem("User Maintenance").Enabled = false;
            //            NavigationMenu.FindItem("Confirmation").Enabled = false;
            //            NavigationMenu.FindItem("Update Thresholds").Enabled = false;
            //        }
            //        else if (checker.ExecuteScalar().ToString() == "BANK BRANCH")
            //        {
            //            NavigationMenu.FindItem("User Maintenance").Enabled = false;
            //            NavigationMenu.FindItem("Signature Verification").Enabled = false;
            //            NavigationMenu.FindItem("Update Thresholds").Enabled = false;
            //        }
            //        else
            //        {
            //            NavigationMenu.FindItem("User Maintenance").Enabled = false;
            //            NavigationMenu.FindItem("Confirmation").Enabled = false;
            //            NavigationMenu.FindItem("Signature Verification").Enabled = false;
            //            NavigationMenu.FindItem("Update Thresholds").Enabled = false;
            //        }
            //    }
            }
        protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Server.Transfer(e.Item.NavigateUrl);
        }

        private void UpdateTimer()
        {
            Label3.Text = System.DateTime.Now.ToLongTimeString();
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
        }

    }

    
}
