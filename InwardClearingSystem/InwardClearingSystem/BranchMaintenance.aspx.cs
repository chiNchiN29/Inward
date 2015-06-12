using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InwardClearingSystem
{
    public partial class BranchMaintenance : BasePage
    {
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt;
        StringBuilder query;
        RadioButton rb;
        GridViewRow row;
        string function = "Branch Maintenance";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                Response.Redirect("~/NoAccess.aspx");
            }

            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["SelectRow"] = -1;
            }
        }

        public DataTable FillDataTable()
        {
            try
            {
                activeConnectionOpen();
                cmd = new SqlCommand("SELECT branch_id, branch_name, branch_code, address FROM Branch", activeConnection);
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BranchView.DataSource = dt;
                BranchView.DataBind();
                activeConnectionClose();
                return dt;
            }
            catch
            {
                throw;
            }
        }

        protected void btnpopAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkForDuplicateData("branch_name", "Branch", txtBranchName.Text) == false)
                {
                    if(checkForDuplicateData("branch_code", "Branch", txtBranchCode.Text) == false)
                    {
                        using (cmd = new SqlCommand())
                        {
                            cmd.CommandText = "InsertBranch";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = activeConnectionOpen();
                            cmd.Parameters.AddWithValue("@name", txtBranchName.Text);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@code", txtBranchCode.Text);
                            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@modby", Convert.ToInt32(Session["UserID"]));
                            cmd.ExecuteNonQuery();
                            FillDataTable();
                        }
                        txtBranchName.Text = "";
                        txtBranchCode.Text = "";
                        txtAddress.Text = "";
                        Message("Successfully added Branch");
                    }
                    else
                    {
                        Message("Branch Code already exists");
                    }
                }
                else
                {
                    Message("Branch Name already exists");
                }
            }
            catch
            {
                Message("Adding Branch has failed. Please try again.");
            }
        }

        //this does something
        protected void btnpopCancel_Click(object sender, EventArgs e)
        {

        }

        protected void delBranch_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(BranchView.DataKeys[Convert.ToInt32(editRow.Value) - 1].Value);
                string query = "delete from Branch where branch_id = @id";
                using (cmd = new SqlCommand(query, activeConnectionOpen()))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                FillDataTable();
                Message("Successfully deleted Branch");
            }
            catch
            {
                Message("Branch delete has failed. Please try again.");
            }
        }

        protected void BranchView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BranchView.PageIndex = e.NewPageIndex;
                if (ViewState["SortExpression"] == null)
                {
                    FillDataTable();
                }
                else
                {
                    dt = ViewState["myDataTable"] as DataTable;
                    dt.DefaultView.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
                    BranchView.DataSource = dt;
                    BranchView.DataBind();
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void BranchView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            BranchView.DataSource = dt;
            BranchView.DataBind();
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

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            FillDataTable();
        }

        protected void BranchView_PageSize(object sender, EventArgs e)
        {
            BranchView.PageSize = Convert.ToInt32(pgSizeDrpDwn.SelectedValue);
            FillDataTable();
        }

        protected void btnpopEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    Message("Please input a branch name");
                }
                else
                {
                    query = new StringBuilder();
                    query.Append("Update Branch set branch_name = @name, branch_code = @code, address = @ad ");
                    query.Append("WHERE branch_id = @id");
                    using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
                    {
                        cmd.Parameters.AddWithValue("@name", txtBranchName.Text);
                        cmd.Parameters.AddWithValue("@code", txtBranchCode.Text);
                        cmd.Parameters.AddWithValue("@ad", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@id", editRow.Value);
                        cmd.ExecuteNonQuery();
                        FillDataTable();
                    }
                    Message("Successfully edited Branch");
                }
            }
            catch
            {
                Message("Branch edit has failed. Please try again.");
            }
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SearchBranch";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnectionOpen();
                        cmd.Parameters.AddWithValue("@name", txtSearch.Text);
                        if (cmd.ExecuteScalar() == null)
                        {
                            Message("No results found");
                        }
                        else
                        {
                            cmd.ExecuteNonQuery();
                            dt = new DataTable();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            BranchView.DataSource = dt;
                            BranchView.DataBind();
                        }
                    }
                }
            }
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        //di pa tapos
        private void InsertBranchHistory(string name, string userid,)
        {
            query = new StringBuilder();
            query.Append("Update Branch_History set branch_name = @name, modified_date = @date, modified_by = @id, ");
            query.Append("terminal = @ip, history_tag = @tag, changes = @changes, history_message = @message");
            cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
            cmd.Parameters.AddWithValue("@name", )
        }
    }
}