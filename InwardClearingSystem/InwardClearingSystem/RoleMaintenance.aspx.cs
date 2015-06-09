using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace InwardClearingSystem
{
    public partial class RoleMaintenance : BasePage
    {
        String query;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        string function = "Role Maintenance";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == 1)
            {
                Response.Redirect("~/FailurePage.aspx");
            }
            if (!Page.IsPostBack)
            {
                FillDataTable();
            }
        }

        private DataTable FillDataTable()
        {
            query = "FillRoleMaintenanceDataTable";
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                RoleView.DataSource = dt;
                RoleView.DataBind();
                return dt;
            }
        }

        protected void addRole_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddRole.aspx");
        }

        protected void RowSelect_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            int id = (int)RoleView.DataKeys[row.RowIndex].Value;
            Response.Redirect("~/AddRole.aspx?Role=" + id);
        }
    }
}