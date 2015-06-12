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
        SqlTransaction transaction;
        string function = "User Maintenance";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                Response.Redirect("~/NoAccess.aspx");
            }
            
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["SelectRow"] = -1;
                FillDropDown();
            }            
        }

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            row = (GridViewRow)rb.NamingContainer;
            int i = row.RowIndex;
            ViewState["SelectRow"] = i;
        }

        protected void assignBtn_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

            if (i == -1)
            {
                Message("Please select a user");
            }
            else
            {
                if (RoleDrpDwn.SelectedValue.Equals("None"))
                {
                    Message("Please select a role");
                }
                else
                {
                    row = UserView.Rows[i];
                    string user = row.Cells[2].Text;
                  
                    using (SqlCommand select = new SqlCommand())
                    {
                        select.CommandText = "PickRoleFromDropDown";
                        select.CommandType = CommandType.StoredProcedure;
                        select.Connection = activeConnectionOpen();
                        select.Parameters.AddWithValue("@role_desc", RoleDrpDwn.Text);
                        

                        cmd = new SqlCommand();
                        
                        cmd.CommandText = "UpdateUserRole";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnectionOpen();
                        cmd.Parameters.AddWithValue("@id", select.ExecuteScalar());
                        cmd.Parameters.AddWithValue("@username", user);
                        cmd.ExecuteNonQuery();

                        FillDataTable();
                        
                    }
                    
                }
            }
            ViewState["SelectRow"] = -1;
        }

        protected void branchBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditUserBranches.aspx");    
        }

        protected void deleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

                if (i == -1)
                {
                    Message("Please select a user");
                }
                else
                {
                    row = UserView.Rows[i];
                    string user_id = row.Cells[1].Text;

                    cmd = activeConnectionOpen().CreateCommand();
                  

                    transaction = activeConnection.BeginTransaction("UserTermination");

                    cmd.Connection = activeConnection;
                    cmd.Transaction = transaction;

                    cmd.CommandText = "CheckUserTiesWithBranches";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", user_id);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd.CommandText = "SeverUserTiesWithBranches";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "DeleteUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();


                    transaction.Commit();
                    Message("User successfully deleted.");
                    Server.Transfer("~/UserMaintenance.aspx");
                }
            }
            catch
            {
                transaction.Rollback();
                Message("User Delete has failed. Please try again");
            }
        }

        protected void addUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddUser.aspx");
        }

        protected void editUser_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

                if (i == -1)
                {
                    Message("Please select a user");
                }
                else
                {
                    row = UserView.Rows[i];
                    string user = row.Cells[2].Text;
                    using (activeConnectionOpen())
                    {

                        cmd = new SqlCommand("SELECT * FROM [User] u WHERE u.username = @username", activeConnection);
                        cmd.Parameters.AddWithValue("@username", user);
                        cmd.ExecuteNonQuery();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Session["TBEuserID"] = dr["user_id"];
                            Session["TBEusername"] = dr["username"];
                            Session["TBEfirstname"] = dr["f_name"];
                            Session["TBEmiddlename"] = dr["m_name"];
                            Session["TBElastname"] = dr["l_name"];
                            Session["TBEemail"] = dr["email"];
                            Session["TBEpassword"] = dr["password"];
                            Server.Transfer("~/EditUser.aspx");
                        }
                    }
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void searchUser_Click(object sender, EventArgs e)
        {
            try
            {
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "SearchUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnection;
                    cmd.Parameters.AddWithValue("@search", searchBar.Text);
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    UserView.DataSource = dt;
                    UserView.DataBind();
                }
            }
            catch
            {
                Message("An error has occrred. Please try again.");
            }
        }

        /// <summary>
        /// Fills the DropDown control with the roles in the database.
        /// </summary>
        public void FillDropDown()
        {
            using (da = new SqlDataAdapter("FillRoleDropDown", activeConnectionOpen()))
            {

                dt = new DataTable();

                da.Fill(dt);
                RoleDrpDwn.DataSource = dt;
                RoleDrpDwn.DataTextField = "role_desc";
                RoleDrpDwn.DataValueField = "role_desc";
                RoleDrpDwn.DataBind();
                RoleDrpDwn.Items.Insert(0, new ListItem("<Select Role>", "None"));
            }
        }

        /// <summary>
        /// Fills the DataTable control with the information of all registered users in the database.
        /// </summary>
        /// <returns>Filled DataTable</returns>
        public DataTable FillDataTable()
        {
            using (da = new SqlDataAdapter("FillUserDataTable", activeConnectionOpen()))
            {
                dt = new DataTable();

                da.Fill(dt);
                UserView.DataSource = dt;
                UserView.DataBind();
                return dt;
            }
        }
    }
}