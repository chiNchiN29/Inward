using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool val1 = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (val1 == false)
            {
                NavigationMenu.Visible = false;
            }
        }

        protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            Server.Transfer(e.Item.NavigateUrl);
        }
    }
}
