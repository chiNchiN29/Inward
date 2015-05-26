using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication1
{
    public partial class EditUserBranches : BasePage
    {
        SqlCommand cmd;
        List<string> newBranch = new List<string>();
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = int.Parse(Request.QueryString["UserID"].ToString());
            //bool login = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            //if (login == false)
            //    Response.Redirect("~/Account/LogIn.aspx");

            if (!Page.IsPostBack)
            {
                cmd = new SqlCommand("SELECT username FROM END_USER WHERE user_id = @userid", activeConnection);
                cmd.Parameters.AddWithValue("@userid", userID);
                userLbl.Text = cmd.ExecuteScalar() as string;
                ViewState["myDataTable"] = FillDataTable();
                ViewState["Branches"] = newBranch;

            }
            activeConnection.Close();

        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserMaintenance.aspx");
        }

        public DataTable FillDataTable()
        {
            using (cmd = new SqlCommand("SELECT branch_name, username FROM BRANCH LEFT JOIN END_USER ON BRANCH.user_id = END_USER.user_id", activeConnection))
            {
                using (DataTable dt = new DataTable())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        branchView.DataSource = dt;
                        branchView.DataBind();


                        return dt;
                    }
                }
            }
        }

        protected void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            newBranch = (List<string>)ViewState["Branches"];
            CheckBox cb = (CheckBox)sender;
            GridViewRow row = (GridViewRow)cb.NamingContainer;
            if (cb.Checked == true)
            {
                newBranch.Add(row.Cells[1].Text);
            }
            else
            {
                newBranch.Remove(row.Cells[1].Text);
            }
            ViewState["Branches"] = newBranch;

        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {

            int userID = int.Parse(Request.QueryString["UserID"].ToString());
            activeConnection.Open();
            List<string> mybranches = (List<string>)ViewState["Branches"];
            foreach (string branchname in mybranches)
            {

                cmd = new SqlCommand("update BRANCH SET user_id = @userid WHERE branch_name = @branch", activeConnection);
                cmd.Parameters.AddWithValue("@userid", userID);
                cmd.Parameters.AddWithValue("@branch", branchname);
                cmd.ExecuteNonQuery();

            }
            activeConnection.Close();

            dt = FillDataTable();
            branchView.DataSource = dt;
            branchView.DataBind();
        }

    }
}