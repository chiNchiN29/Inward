using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication1
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
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                FillDropDown();
            }
        }

        //get selected row index
        protected int GetRowIndex()
        {
            int x = -1;
            for (int i = 0; i <= UserView.Rows.Count - 1; i++)
            {
                row = UserView.Rows[i];
                RadioButton rb = (RadioButton)row.FindControl("RowSelect");
                if (rb != null)
                {
                    if (rb.Checked == true)
                    {
                        return row.RowIndex;
                    }
                }
            }
            return x;
        }

        public void FillDropDown()
        {
            query = "SELECT role_name FROM ROLE";
            dt = new DataTable();
            da = new SqlDataAdapter(query, activeConnection);
            da.Fill(dt);
            RoleDrpDwn.DataSource = dt;
            RoleDrpDwn.DataTextField = "role_name";
            RoleDrpDwn.DataValueField = "role_name";
            RoleDrpDwn.DataBind();
            RoleDrpDwn.Items.Insert(0, new ListItem("<Select Role>", "0"));
            activeConnection.Close();
        }

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            int i = GetRowIndex();
            if (i != -1)
            {
                deleteBtn.Visible = true;
                row = UserView.Rows[i];
                string role = row.Cells[4].Text;
                if (role.Equals("CLEARING DEPT"))
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
            query = "SELECT user_id, username, email, ROLE.role_name FROM END_USER, ROLE WHERE END_USER.role_id = ROLE.role_id";

            dt = new DataTable();
            da = new SqlDataAdapter(query, activeConnection);
            da.Fill(dt);
            UserView.DataSource = dt;
            UserView.DataBind();


            return dt;
        }

        protected void assignBtn_Click(object sender, EventArgs e)
        {
            int i = GetRowIndex();

            if (i != -1)
            {
                if (int.Parse(RoleDrpDwn.Text) == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script language='javascript'>");
                    sb.Append("alert('Please select a role');");
                    sb.Append("<");
                    sb.Append("/script>");

                    if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
                }
                else
                {
                    row = UserView.Rows[i];
                    string user = row.Cells[2].Text;
                    using (SqlCommand select = new SqlCommand("SELECT ROLE.role_id FROM ROLE, END_USER WHERE @rolename = ROLE.role_name", activeConnection))
                    {
                        select.Parameters.AddWithValue("@rolename", RoleDrpDwn.Text);

                    cmd = new SqlCommand("update END_USER SET role_id = @id WHERE END_USER.username = @name", activeConnection);
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
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("alert('Please select a user');");
                sb.Append("<");
                sb.Append("/script>");

                if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
            }
        }

        protected void branchBtn_Click(object sender, EventArgs e)
        {
            int i = GetRowIndex();
            if (i != -1)
            {
                string userId = UserView.DataKeys[i].Value.ToString();
                Response.Redirect("EditUserBranches.aspx?UserID=" + userId);
            }
        }

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("DELETE FROM END_USER WHERE username = @name", activeConnection);
            int i = GetRowIndex();

            if (i != -1)
            {
                row = UserView.Rows[i];
                string user = row.Cells[2].Text;
                cmd.Parameters.AddWithValue("@name", user);
                using (SqlCommand nullifier = new SqlCommand("UPDATE BRANCH SET user_id = NULL FROM BRANCH, END_USER WHERE END_USER. user_id = BRANCH.user_id AND END_USER.username = @name", activeConnection))
                {
                    nullifier.Parameters.AddWithValue("@name", user);
                    nullifier.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("~/UserMaintenance.aspx");
        }
    }
}