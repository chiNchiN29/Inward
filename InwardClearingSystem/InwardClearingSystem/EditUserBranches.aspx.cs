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
        SqlDataAdapter da;
        string function = "User Maintenance";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                Response.Redirect("~/NoAccess.aspx");
            }

            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["Branches"] = Branches;

                FillDropDown();
            }
           
        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UserMaintenance.aspx");
        }

        public DataTable FillDataTable()
        {

            using (cmd = new SqlCommand())
            {
                cmd.CommandText = "FillEditUserBranchesDataTable";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                    branchView.DataSource = dt;
                    branchView.DataBind();
                    activeConnectionClose();
                    return dt;
                }
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
                        FillDataTable();
                        Branches.Clear();
                    }
                }
                catch
                {
                    Message("An error has occurred. Please try again.");
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
            using (da = new SqlDataAdapter("FillUserDropDown", activeConnectionOpen()))
            {
                dt = new DataTable();
                
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