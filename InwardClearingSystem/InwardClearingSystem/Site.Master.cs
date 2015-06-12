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
    }
}
