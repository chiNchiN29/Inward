﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace InwardClearingSystem
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dateToday.Text = DateTime.Today.ToString("D");
            DefaultView();
            bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (login == false)
            {
                skipLink.Visible = false;
            }
            else
            {
                try
                {
                    Label3.Text = System.DateTime.Now.ToLongTimeString();
                    //string pool = Session["UserName"].ToString();
                    using (connection)
                    {
                        connection.Open();
                        using (SqlCommand checker = new SqlCommand())
                        {
                            checker.CommandText = "CheckUserRole";
                            checker.CommandType = CommandType.StoredProcedure;
                            checker.Connection = connection;
                            checker.Parameters.AddWithValue("@username", Session["UserName"]);
                            if (checker.ExecuteScalar().ToString() == "ADMIN")
                            {
                                AdminView();
                            }
                            else if (checker.ExecuteScalar().ToString() == "OVERSEER")
                            {
                                OverseerView();
                            }
                        }
                    }
                }
                catch(Exception b)
                {
                    Response.Write(b);
                }
            }
        }
        
        protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Server.Transfer(e.Item.NavigateUrl);
        }

        /// <summary>
        /// Updates timer each second.
        /// </summary>
        private void UpdateTimer()
        {
            Label3.Text = System.DateTime.Now.ToLongTimeString();
        }
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
        }

        protected void AdminView()
        {
			NavigationMenu.Visible = true;
            NavigationMenu.FindItem("Main Menu").Enabled = true;
            NavigationMenu.FindItem("Update Thresholds").Enabled = true;
            NavigationMenu.FindItem("User Maintenance").Enabled = true;
        }

        protected void OverseerView()
        {
			NavigationMenu.Visible = true;
            NavigationMenu.FindItem("Main Menu").Enabled = true;
            NavigationMenu.FindItem("Signature Verification").Enabled = true;
            NavigationMenu.FindItem("Confirmation").Enabled = true;
            NavigationMenu.FindItem("Update Thresholds").Enabled = true;
            NavigationMenu.FindItem("User Maintenance").Enabled = true;
        }
        
        protected void DefaultView()
        {
			NavigationMenu.Visible = false; 
            NavigationMenu.FindItem("Signature Verification").Enabled = false;
            NavigationMenu.FindItem("Confirmation").Enabled = false;
            NavigationMenu.FindItem("Update Thresholds").Enabled = false;
            NavigationMenu.FindItem("User Maintenance").Enabled = false;
        }
    }

    
}
