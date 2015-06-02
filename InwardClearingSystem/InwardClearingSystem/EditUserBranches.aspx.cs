using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;

namespace InwardClearingSystem
{
    public partial class EditUserBranches : BasePage
    {
        SqlCommand cmd;
        List<string> newBranch = new List<string>();
        DataTable dt;
        StringBuilder query;
        SqlDataAdapter da;
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnectionOpen();
            cmd = new SqlCommand("SELECT role_desc FROM [User] u, Role r WHERE username = @username AND u.role_id = r.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "BANK BRANCH" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                Message("You are not authorized to view this page");
                Response.Redirect("Default.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["Branches"] = Branches;
                    
                    FillDropDown();
                }
            }
            activeConnectionClose();
        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UserMaintenance.aspx");
        }

        public DataTable FillDataTable()
        {
            activeConnectionOpen();
            cmd = new SqlCommand("SELECT branch_name, username FROM Branch b LEFT JOIN [User] u ON b.user_id = u.user_id ORDER BY " + ViewState["SortExpression"].ToString(), activeConnection);
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            branchView.DataSource = dt;
            branchView.DataBind();
            activeConnectionClose();
            return dt;
        }

        protected void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow row = (GridViewRow)cb.NamingContainer;
            if (cb.Checked == true)
            {
                Branches.Add(row.Cells[1].Text);
            }
            else
            {
                Branches.Remove(row.Cells[1].Text);
            }
            
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            using (activeConnectionOpen())
            {
                try
                {
                    List<string> mybranches = Branches;
                    if (UserDrpDwn.SelectedValue.Equals("None"))
                    {
                        Message("Please select a user");
                    }
                    else if (mybranches.Count == 0)
                    {
                        Message("Please select a branch");

                    }
                    else
                    {
                        foreach (string branchname in mybranches)
                        {
                            cmd = new SqlCommand("update Branch SET user_id = @userid WHERE branch_name = @branch", activeConnection);
                            cmd.Parameters.AddWithValue("@userid", UserDrpDwn.SelectedValue);
                            cmd.Parameters.AddWithValue("@branch", branchname);
                            cmd.ExecuteNonQuery();
                        }
                        dt = FillDataTable();
                        Branches.Clear();
                    }
                }
                catch (Exception b)
                {
                    Response.Write(b);
                }
            }
        }

        public List<string> Branches
        {
            get
            {
                if (this.ViewState["Branches"] == null)
                {
                    this.ViewState["Branches"] = new List<string>();
                }
                return (List<string>)(this.ViewState["Branches"]);
            }
        }

        protected void branchView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            branchView.PageIndex = e.NewPageIndex;
            Branches.Clear();
            FillDataTable();
        }

        public void FillDropDown()
        {
            using (activeConnectionOpen())
            {
                query = new StringBuilder();
                query.Append("SELECT user_id, username ");
                query.Append("FROM [User] u, Role r ");
                query.Append("WHERE u.role_id = r.role_id AND (r.role_desc = 'CLEARING DEPT' OR r.role_desc = 'OVERSEER')");
                dt = new DataTable();
                da = new SqlDataAdapter(query.ToString(), activeConnection);
                da.Fill(dt);
                UserDrpDwn.DataSource = dt;
                UserDrpDwn.DataTextField = "username";
                UserDrpDwn.DataValueField = "user_id";
                UserDrpDwn.DataBind();
                UserDrpDwn.Items.Insert(0, new ListItem("<Select User>", "None"));
            }
        }

        protected void BranchView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            branchView.DataSource = dt;
            branchView.DataBind();
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