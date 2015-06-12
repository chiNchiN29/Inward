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
        SqlDataReader dr;
        string function = "Audit Log";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                Response.Redirect("~/NoAccess.aspx");
            }
  
            if (!Page.IsPostBack)
            {
                query = new StringBuilder();
                query.Append("Select username FROM [User]");
                cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
                dr = cmd.ExecuteReader();
                drpDwnUserSearch.Items.Insert(0, new ListItem("<Select User>", "None"));
                while (dr.Read())
                {
                    drpDwnUserSearch.Items.Add(dr["username"].ToString());
                }
                dr.Close();

                ViewState["myDataTable"] = FillDataTable();
            }
        }

        private DataTable FillDataTable()
        {
            try
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
            catch
            {
                throw;
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
            try
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
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        protected void LogView_PageSize(object sender, EventArgs e)
        {
            LogView.PageSize = Convert.ToInt32(pgSizeDrpDwn.SelectedValue);
            FillDataTable();
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
                        query = new StringBuilder();
                        query.Append("SELECT action, check_number, account_number, remarks, date_logged, username ");
                        query.Append("FROM Cheque_Log ");
                        query.Append("WHERE username = @name");
                        using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
                        {
                            cmd.Parameters.AddWithValue("@name", drpDwnUserSearch.SelectedItem.Text);
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
                        query = new StringBuilder();
                        query.Append("SELECT action, check_number, account_number, remarks, date_logged, username ");
                        query.Append("FROM Cheque_Log ");
                        query.Append("WHERE (date_logged BETWEEN @datefrom AND @dateto)");
                        using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
                        {
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
            FillDataTable();
        }
    }
}