using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace InwardClearingSystem
{
    public partial class EditUserBranches : BasePage
    {
        SqlCommand cmd;
        List<string> newBranch = new List<string>();
        DataTable dt;
        
        protected void Page_Load(object sender, EventArgs e)
        {   
            cmd = new SqlCommand("SELECT role_desc FROM [USER], ROLE WHERE username = @username AND [USER].role_id = ROLE.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "BANK BRANCH" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                ErrorMessage("You are not authorized to view this page");
                Response.Redirect("Default.aspx");
            }
            else
            {
                int userID = int.Parse(Request.QueryString["UserID"].ToString());
                if (!Page.IsPostBack)
                {
                    cmd = new SqlCommand("SELECT username FROM [USER] WHERE user_id = @userid", activeConnection);
                    cmd.Parameters.AddWithValue("@userid", userID);
                    userLbl.Text = cmd.ExecuteScalar() as string;
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["Branches"] = Branches;
                }
            }
            activeConnection.Close();
            
        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserMaintenance.aspx");
        }

        public DataTable FillDataTable()
        {  
            cmd = new SqlCommand("SELECT branch_name, username FROM BRANCH LEFT JOIN [USER] ON BRANCH.user_id = [USER].user_id", activeConnection);
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            branchView.DataSource = dt;
            branchView.DataBind();
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
            using (activeConnection)
            {
                activeConnection.Open();
                try
                {
                    int userID = int.Parse(Request.QueryString["UserID"].ToString());
                    List<string> mybranches = Branches;
                    if (mybranches.Count != 0)
                    {
                        foreach (string branchname in mybranches)
                        {
                            cmd = new SqlCommand("update BRANCH SET user_id = @userid WHERE branch_name = @branch", activeConnection);
                            cmd.Parameters.AddWithValue("@userid", userID);
                            cmd.Parameters.AddWithValue("@branch", branchname);
                            cmd.ExecuteNonQuery();
                        }
                        activeConnection.Close();
                        dt = FillDataTable();
                        Branches.Clear();
                    }
                    else
                    {
                        ErrorMessage("Please select a branch");
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
    }
}