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


        //    DefaultView();
        //    bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        //    if (login == false)
        //    {
        //        NavigationMenu.Visible = false;
        //    }
        //    else
        //    {
        //        connection.Open();
        //        Label3.Text = System.DateTime.Now.ToLongTimeString();
        //        string meep = Membership.GetUser().UserName;
        //        SqlCommand checker = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Membership.GetUser().UserName + "' AND END_USER.role_id = ROLE.role_id", connection);
        //            if (checker.ExecuteScalar().ToString() == "ADMIN")
        //            {
        //                AdminView();
        //            }
        //            else if (checker.ExecuteScalar().ToString() == "CLEARING DEPT")
        //            {
        //                ClearingDeptView();
        //            }
        //            else if (checker.ExecuteScalar().ToString() == "BANK BRANCH")
        //            {
        //                BankBranchView();
        //            }
        //            else if (checker.ExecuteScalar().ToString() == "OVERSEER")
        //            {
        //                OverseerView();
        //            }
        //            else
        //            {
        //                TBAView();
        //            }
        //        }

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

        //protected void BankBranchView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = true;
        //    NavigationMenu.FindItem("Confirmation").Enabled = true;
        //}
        //protected void ClearingDeptView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = true;
        //    NavigationMenu.FindItem("Signature Verification").Enabled = true;
        //}
        //protected void AdminView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = true;
        //    NavigationMenu.FindItem("Update Thresholds").Enabled = true;
        //    NavigationMenu.FindItem("User Maintenance").Enabled = true;
        //}
        //protected void TBAView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = true;
        //}
        //protected void OverseerView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = true;
        //    NavigationMenu.FindItem("Signature Verification").Enabled = true;
        //    NavigationMenu.FindItem("Confirmation").Enabled = true;
        //    NavigationMenu.FindItem("Update Thresholds").Enabled = true;
        //    NavigationMenu.FindItem("User Maintenance").Enabled = true;
        //}
        //protected void DefaultView()
        //{
        //    NavigationMenu.FindItem("Main Menu").Enabled = false;
        //    NavigationMenu.FindItem("Signature Verification").Enabled = false;
        //    NavigationMenu.FindItem("Confirmation").Enabled = false;
        //    NavigationMenu.FindItem("Update Thresholds").Enabled = false;
        //    NavigationMenu.FindItem("User Maintenance").Enabled = false;
        //}
    }

    
}
