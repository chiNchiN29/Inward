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
        List<string> newBranch = new List<string>();
        SqlCommand cmd;
        String function = "User Maintenance";
        
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
                ViewState["myDataTable"] = FillDataTable("FillEditUserBranchesDataTable", activeConnectionOpen(), branchView);
                ViewState["Branches"] = Branches;
            }
           
        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            if (this.Context != null)
            {
                Response.Redirect("~/UserMaintenance.aspx");
            }
        }

        protected void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch
            {
                Message("An error has occurred. Please try again");
            }
            
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            using (activeConnectionOpen())
            {
                try
                {
                    List<string> mybranches = Branches;
                    if (UserDrpDwn.SelectedValue.Equals(0))
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
                            using (cmd = new SqlCommand())
                            {
                                cmd.CommandText = "UpdateBranchHandlers";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = activeConnection;
                                cmd.Parameters.AddWithValue("@userid", UserDrpDwn.SelectedValue);
                                cmd.Parameters.AddWithValue("@branch", branchname);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        FillDataTable("FillEditUserBranchesDataTable", activeConnectionOpen(), branchView);
                        Branches.Clear();
                    }
                }
                catch
                {
                    Message("An error has occurred. Please try again.");
                }
            }
        }

        protected void branchView_PageIndex(object sender, GridViewPageEventArgs e)
        {
            branchView.PageIndex = e.NewPageIndex;
            Branches.Clear();
            FillDataTable("FillEditUserBranchesDataTable", activeConnectionOpen(), branchView);
        }

        protected void BranchView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            branchView.DataSource = dt;
            branchView.DataBind();
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
    }
}