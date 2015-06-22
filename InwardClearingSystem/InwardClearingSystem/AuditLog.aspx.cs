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
        SqlCommand cmd;
        String function = "Audit Log";

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
                ViewState["myDataTable"] = FillDataTable("FillAuditLogDataTable", activeConnectionOpen(), LogView);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkBxUser.Checked)
                {
                    if (!drpDwnUserSearch.SelectedValue.Equals("None"))
                    {

                        //username search
                        using (cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SearchAuditLogsByUserID";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = activeConnectionOpen();
                            cmd.Parameters.AddWithValue("@id", drpDwnUserSearch.SelectedValue);
                            cmd.ExecuteNonQuery();
                            dt = new DataTable();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            LogView.DataSource = dt;
                            LogView.DataBind();
                        }
                    }
                }
                else
                {
                    //date search
                    if (String.IsNullOrWhiteSpace(txtBxDateFrom.Text) && String.IsNullOrWhiteSpace(txtBxDateTo.Text))
                    {
                        //wala laman
                    }
                    else
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.CommandText = "SearchAuditLogsByDateLogged";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = activeConnectionOpen();

                            if (!String.IsNullOrWhiteSpace(txtBxDateFrom.Text) && !String.IsNullOrWhiteSpace(txtBxDateTo.Text))
                            {
                                cmd.Parameters.AddWithValue("@datefrom", txtBxDateFrom.Text + " 12:00:00 AM");
                                cmd.Parameters.AddWithValue("@dateto", txtBxDateTo.Text + " 11:59:59 PM");
                            }

                            //if date from is null. shows only date in date to
                            if (String.IsNullOrWhiteSpace(txtBxDateFrom.Text) && !String.IsNullOrWhiteSpace(txtBxDateTo.Text))
                            {
                                cmd.Parameters.AddWithValue("@datefrom", txtBxDateTo.Text + " 12:00:00 AM");
                                cmd.Parameters.AddWithValue("@dateto", txtBxDateTo.Text + " 11:59:59 PM");
                            }
                            //if Date to is Null shows date from to current date
                            if (String.IsNullOrWhiteSpace(txtBxDateTo.Text) && !String.IsNullOrWhiteSpace(txtBxDateFrom.Text))
                            {
                                cmd.Parameters.AddWithValue("@datefrom", txtBxDateFrom.Text + " 12:00:00 AM");
                                cmd.Parameters.AddWithValue("@dateto", DateTime.Now);
                            }
                            cmd.ExecuteNonQuery();
                            dt = new DataTable();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            LogView.DataSource = dt;
                            LogView.DataBind();
                        }
                    }
                }
            }
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        protected void btnViewAll_Click(object sender, EventArgs e)
        {
            FillDataTable("FillAuditLogDataTable", activeConnectionOpen(), LogView);
        }

        protected void LogView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {

                LogView.PageIndex = e.NewPageIndex;
                if (ViewState["SortExpression"].ToString() == null)
                {
                    FillDataTable("FillAuditLogDataTable", activeConnectionOpen(), LogView);
                }
                else
                {
                    dt = ViewState["myDataTable"] as DataTable;
                    dt.DefaultView.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
                    LogView.DataSource = dt;
                    LogView.DataBind();
                }
            }
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        protected void LogView_PageSize(object sender, EventArgs e)
        {
            LogView.PageSize = Convert.ToInt32(pgSizeDrpDwn.SelectedValue);
            FillDataTable("FillAuditLogDataTable", activeConnectionOpen(), LogView);
        }

        protected void LogView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            LogView.DataSource = dt;
            LogView.DataBind();
        }
    }
}