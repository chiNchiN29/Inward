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
        string sort = "branch_name";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["SelectRow"] = -1;
            }
        }

        public DataTable FillDataTable()
        {
            activeConnectionOpen();
            cmd = new SqlCommand("SELECT branch_id, branch_name FROM Branch", activeConnection);
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            BranchView.DataSource = dt;
            BranchView.DataBind();
            activeConnectionClose();
            return dt;
        }

        //add yung user id sa modified by and modified date
        protected void addBranch_Click(object sender, EventArgs e)
        {

            TextBox bn = ((TextBox)BranchView.FooterRow.FindControl("txtBranchName"));
            bn.CausesValidation = true;
            string name = bn.Text;
            using (activeConnectionOpen())
            {
                query = new StringBuilder();
                query.Append("insert into Branch(branch_name, modified_date) ");
                query.Append("values(@name, @date)");
                cmd = new SqlCommand(query.ToString(), activeConnection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();
                FillDataTable();
            }
            bn.CausesValidation = false;
        }

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            rb = (RadioButton)sender;
            row = (GridViewRow)rb.NamingContainer;
            int i = row.RowIndex;
            ViewState["SelectRow"] = i; 
        }

        protected void EditBranch(object sender, GridViewEditEventArgs e)
        {
            BranchView.EditIndex = e.NewEditIndex;
            FillDataTable();
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BranchView.EditIndex = -1;
            FillDataTable();
        }

        protected void UpdateBranch(object sender, GridViewUpdateEventArgs e)
        {
            TextBox bn = ((TextBox)BranchView.Rows[e.RowIndex].FindControl("txtedBranchName"));
            int id = Convert.ToInt32(BranchView.DataKeys[e.RowIndex].Value);
       
            string newname = bn.Text;
            
            using (activeConnectionOpen())
            {
                string query = "update Branch set branch_name = @name WHERE branch_id = @id";
                cmd = new SqlCommand(query, activeConnection);
                cmd.Parameters.AddWithValue("@name", newname);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            BranchView.EditIndex = -1;
            FillDataTable();
        }

        protected void delBranch_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

            if (i == -1)
            {
                Message("Please select a branch");
            }
            else
            {
                using (activeConnectionOpen())
                {
                    int id = Convert.ToInt32(BranchView.DataKeys[i].Value);
                    string query = "delete from Branch where branch_id = @id";
                    cmd = new SqlCommand(query, activeConnection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                FillDataTable();
            }
        }

        protected void BranchView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            BranchView.PageIndex = e.NewPageIndex;
            if (ViewState["SortExpression"].ToString() == null)
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
    }
}