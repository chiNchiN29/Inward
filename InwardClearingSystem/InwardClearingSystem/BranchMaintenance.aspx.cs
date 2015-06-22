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
        String function = "Branch Maintenance";
        SqlTransaction transact; 

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
                ViewState["myDataTable"] = FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
                ViewState["SelectRow"] = -1;
            }
        }

        protected void btnpopAdd_Click(object sender, EventArgs e)
        {
            try
            {
                transact = activeConnectionOpen().BeginTransaction("BranchAdd");
                if (checkForDuplicateData("branch_name", "Branch", txtBranchName.Text) == false)
                {
                    if(checkForDuplicateData("branch_code", "Branch", txtBranchCode.Text) == false)
                    { 
                        using (cmd = new SqlCommand())
                        {
                            cmd.CommandText = "InsertBranch";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = activeConnection;
                            cmd.Transaction = transact;
                            cmd.Parameters.AddWithValue("@name", txtBranchName.Text);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@code", txtBranchCode.Text);
                            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                            cmd.Parameters.AddWithValue("@modby", Convert.ToInt32(Session["UserID"]));
                            cmd.ExecuteNonQuery();
                            InsertBranchHistory(txtBranchName.Text, "Add", "Successful Branch Add", "Branch (added row)");
                        }
                        txtBranchName.Text = "";
                        txtBranchCode.Text = "";
                        txtAddress.Text = "";
                        FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
                        Message("Successfully added Branch");
                    }
                    else
                    {
                        InsertBranchHistory(txtBranchName.Text, "Add", "Branch Code already exists", "No change");
                        Message("Branch Code already exists");
                    }
                }
                else
                {
                    InsertBranchHistory(txtBranchName.Text, "Add", "Branch Name already exists", "No change");
                    Message("Branch Name already exists");
                }
            }
            catch
            {
                InsertBranchHistory(txtBranchName.Text, "Add", "Failed Branch Add", "No change");
            
                Message("Adding Branch has failed. Please try again.");
            }
        }

        //this does something
        protected void btnpopCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnpopEdit_Click(object sender, EventArgs e)
        {
            try
            {
                transact = activeConnectionOpen().BeginTransaction("BranchEdit");
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdateBranch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnection;
                    cmd.Transaction = transact;
                    cmd.Parameters.AddWithValue("@name", txtBranchName.Text);
                    cmd.Parameters.AddWithValue("@code", txtBranchCode.Text);
                    cmd.Parameters.AddWithValue("@ad", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@id", editRow.Value);
                    cmd.ExecuteNonQuery();
                }
                InsertBranchHistory(txtBranchName.Text, "Edit", "Successful Branch Edit", "Branch (updated row)");
                FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
                Message("Successfully edited Branch");         
            }
            catch
            {
                InsertBranchHistory(txtBranchName.Text, "Edit", "Failed Branch Edit", "No changes");
                transact.Rollback();
                Message("Branch edit has failed. Please try again.");
            }
        }

        protected void delBranch_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(BranchView.DataKeys[Convert.ToInt32(editRow.Value) - 1].Value);
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "DeleteBranch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                InsertBranchHistory(txtBranchName.Text, "Delete", "Successful Branch Delete", "Branch (deleted row)");
                FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
                Message("Successfully deleted Branch");
            }
            catch
            {
                InsertBranchHistory(txtBranchName.Text, "Delete", "Failed Branch Delete", "No changes");
                Message("Branch delete has failed. Please try again.");
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

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
        }

        protected void BranchView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BranchView.PageIndex = e.NewPageIndex;
                if (ViewState["SortExpression"] == null)
                {
                    FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
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

        protected void BranchView_PageSize(object sender, EventArgs e)
        {
            BranchView.PageSize = Convert.ToInt32(pgSizeDrpDwn.SelectedValue);
            FillDataTable("FillBranchMaintenanceDataTable", activeConnectionOpen(), BranchView);
        }

        protected void BranchView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            BranchView.DataSource = dt;
            BranchView.DataBind();
        }

        //problem with ip address
        private void InsertBranchHistory(string name, string tag, string message, string changes)
        {
            try
            {
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString(); 
                StringBuilder query = new StringBuilder();
                query.Append("Insert into Branch_History (branch_name, modified_date, modified_by, terminal, ");
                query.Append("history_tag, changes, history_message) ");
                query.Append("values (@name, @date, @id, @ip, @tag, @changes, @message)");
                cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["UserID"]));
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@tag", tag);
                cmd.Parameters.AddWithValue("@changes", changes);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}