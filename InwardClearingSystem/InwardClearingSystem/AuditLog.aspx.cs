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
    public partial class AuditLog : BasePage
    {
        DataTable dt;
        StringBuilder query;
        SqlCommand cmd;
        SqlDataAdapter da;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
            }
        }

        private DataTable FillDataTable()
        {
            using (cmd = new SqlCommand())
            {
                cmd.CommandText = "FillAuditLogDataTable";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                LogView.DataSource = dt;
                LogView.DataBind();
                return dt;
            }
        }

        protected void LogView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            LogView.DataSource = dt;
            LogView.DataBind();
        }

        private string GetSortDirection(string column)
        {
            string sortDirection = "DESC";
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "DESC"))
                    {
                        sortDirection = "ASC";
                    }
                }
            }
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void LogView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            LogView.PageIndex = e.NewPageIndex;
            if (ViewState["SortExpression"].ToString() == null)
            {
                FillDataTable();
            }
            else
            {
                dt = ViewState["myDataTable"] as DataTable;
                dt.DefaultView.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
                LogView.DataSource = dt;
                LogView.DataBind();
            }
        }

        protected void LogView_PageSize(object sender, EventArgs e)
        {
            LogView.PageSize = Convert.ToInt32(pgSizeDrpDwn.SelectedValue);
            FillDataTable();
        }
    }
}