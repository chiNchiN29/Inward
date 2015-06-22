using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InwardClearingSystem
{
    public partial class RoleMaintenance : BasePage
    {
        String function = "Role Maintenance";

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
                FillDataTable("FillRoleMaintenanceDataTable", activeConnectionOpen(), RoleView);
            }
        }

        protected void addRole_Click(object sender, EventArgs e)
        {
            if (this.Context != null)
            {
                Response.Redirect("~/AddRole.aspx");
            }
        }

        protected void RowSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            int id = (int)RoleView.DataKeys[row.RowIndex].Value;
            if (this.Context != null)
            {
                Response.Redirect("~/AddRole.aspx?Role=" + id);
            }
        }
    }
}