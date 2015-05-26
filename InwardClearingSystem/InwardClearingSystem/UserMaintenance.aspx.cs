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
    public partial class UserMaintenance : BasePage
    {
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        GridViewRow row;
        string query;

        protected void Page_Load(object sender, EventArgs e)
        {
            cmd = new SqlCommand("SELECT role_desc FROM [USER], ROLE WHERE username = @username AND [USER].role_id = ROLE.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "ADMIN" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                ErrorMessage("You are not authorized to view this page");
                Response.Redirect("Default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["SelectRow"] = -1;
                    FillDropDown();
                }
            }
        }
    
        public void FillDropDown()
        {
            query = "SELECT role_desc FROM ROLE";
            dt = new DataTable();
            da = new SqlDataAdapter(query, activeConnection);
            da.Fill(dt);
            RoleDrpDwn.DataSource = dt;
            RoleDrpDwn.DataTextField = "role_desc";
            RoleDrpDwn.DataValueField = "role_desc";
            RoleDrpDwn.DataBind();
            RoleDrpDwn.Items.Insert(0, new ListItem("<Select Role>", "None"));
            activeConnection.Close();
        }

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            row = (GridViewRow)rb.NamingContainer;
            int i = row.RowIndex;
            ViewState["SelectRow"] = i;

            if (i != -1)
            {
                row = UserView.Rows[i];
                string role = row.Cells[4].Text;
                if (role.Equals("CLEARING DEPT") || role.Equals("OVERSEER"))
                {
                    branchBtn.Visible = true;
                }
                else
                {
                    branchBtn.Visible = false;
                }
            }
        }

        public DataTable FillDataTable()
        {
            query = "SELECT user_id, username, email, ROLE.role_desc FROM [USER], ROLE WHERE [USER].role_id = ROLE.role_id";
          
            dt = new DataTable();
            da = new SqlDataAdapter(query, activeConnection);
            da.Fill(dt);
            UserView.DataSource = dt;
            UserView.DataBind();
       

            return dt;
        }

        protected void assignBtn_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

            if (i == -1)
            {
                ErrorMessage("Please select a user");
            }
            else
            {
                if (RoleDrpDwn.SelectedValue.Equals("None"))
                {
                    ErrorMessage("Please select a role");
                }
                else
                {
                    row = UserView.Rows[i];
                    string user = row.Cells[2].Text;
					using (SqlCommand select = new SqlCommand("SELECT ROLE.role_id FROM ROLE, [USER] WHERE @rolename = ROLE.role_desc", activeConnection))
                    {
                        select.Parameters.AddWithValue("@rolename", RoleDrpDwn.Text);

                    cmd = new SqlCommand("update [USER] SET role_id = @id WHERE [USER].username = @name", activeConnection);
                    cmd.Parameters.AddWithValue("@id", select.ExecuteScalar());
                    cmd.Parameters.AddWithValue("@name", user);
                    cmd.ExecuteNonQuery();

                    dt = FillDataTable();
                    UserView.DataSource = dt;
                    UserView.DataBind();
                    activeConnection.Close();
                        }
                }
            }
            ViewState["SelectRow"] = -1;
        }

        protected void branchBtn_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (i != -1)
            {
                string userId = UserView.DataKeys[i].Value.ToString();
                Response.Redirect("EditUserBranches.aspx?UserID=" + userId);     
            }
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

            if (i == -1)
            {
                ErrorMessage("Please select a user");
            }
            else
            {
                row = UserView.Rows[i];
                string user = row.Cells[2].Text;
                cmd.Parameters.AddWithValue("@name", user);
                using (SqlCommand nullifier = new SqlCommand("UPDATE BRANCH SET user_id = NULL FROM BRANCH, [USER] WHERE [USER]. user_id = BRANCH.user_id AND [USER].username = @name", activeConnection))
                {
                    nullifier.Parameters.AddWithValue("@name", user);
                    nullifier.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();

                }
            }
        }
    }
}