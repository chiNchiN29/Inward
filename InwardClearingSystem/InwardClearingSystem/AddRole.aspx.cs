using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;

namespace InwardClearingSystem
{
    public partial class AddRole : BasePage
    {
        SqlCommand cmd;
        string roleID;
        StringBuilder query;
        SqlDataReader dr;
        SqlTransaction transact;

        protected void Page_Load(object sender, EventArgs e)
        {
            roleID = Request.QueryString["Role"];

            if (!Page.IsPostBack)
            {
                using (cmd = new SqlCommand("SELECT function_name FROM Functions", activeConnectionOpen())) 
                {
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        chkBoxFunctions.Items.Add(dr["function_name"].ToString());
                    }
                }

            }
            if (roleID == null)
            {
                lblHeader.Text = "ADD ROLE";
                saveBtn.Visible = false;
                delBtn.Visible = false;
            }
            else
            {
                lblHeader.Text = "EDIT ROLE";
                addBtn.Visible = false;

                if (!Page.IsPostBack)
                {
                    query = new StringBuilder();
                    query.Append("SELECT role_desc, role_type FROM Role WHERE role_id = @roleID");
                    using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
                    {
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            txtRoleName.Text = dr["role_desc"].ToString();
                            txtRoleType.Text = dr["role_type"].ToString();
                        }
                    }

                    query = new StringBuilder();
                    query.Append("SELECT function_name FROM Functions f, Role_Function rf ");
                    query.Append("WHERE f.function_id = rf.function_id AND role_id = @roleID");
                    using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
                    {
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            foreach (ListItem item in chkBoxFunctions.Items)
                            {
                                if (item.Text.Equals(dr["function_name"].ToString()))
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void backBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RoleMaintenance.aspx");
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int chkBoxItems = 0;

                foreach (ListItem item in chkBoxFunctions.Items)
                {
                    if (item.Selected)
                    {
                        chkBoxItems++;
                    }
                }
                if (chkBoxItems > 0)
                {
                    //update role name and role type
                    transact = activeConnectionOpen().BeginTransaction("RoleUpdate");

                    query = new StringBuilder();
                    query.Append("Update Role set role_desc = @rolename, role_type = @type ");
                    query.Append("WHERE role_id = @roleID");
                    using (cmd = new SqlCommand(query.ToString(), activeConnection, transact))
                    {
                        cmd.Parameters.AddWithValue("@rolename", txtRoleName.Text);
                        cmd.Parameters.AddWithValue("@type", txtRoleType.Text);
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        cmd.ExecuteNonQuery();
                    }

                    //delete from role function
                    query = new StringBuilder();
                    query.Append("DELETE FROM Role_Function WHERE role_id = @roleID");
                    using (cmd = new SqlCommand(query.ToString(), activeConnection, transact))
                    {
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        cmd.ExecuteNonQuery();
                    }

                    //insert new functions in role function
                    foreach (ListItem item in chkBoxFunctions.Items)
                    {
                        if (item.Selected)
                        {
                            insertFunctionRole(int.Parse(roleID), item.Text, activeConnection, transact);
                        }
                    }
                    transact.Commit();
                    Message("Successfully updated role");
                }
            }
            catch
            {
                transact.Rollback();
                lblError.Visible = true;
                lblError.Text = "Role Update has failed. Please try again.";
            }
            finally
            {
                activeConnectionClose();
            }
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            try
            {
                transact = activeConnectionOpen().BeginTransaction("RoleDelete");

                //delete from role function
                query = new StringBuilder();
                query.Append("DELETE FROM Role_Function WHERE role_id = @roleID");
                using (cmd = new SqlCommand(query.ToString(), activeConnection, transact))
                {
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.ExecuteNonQuery();
                }

                //delete role
                query = new StringBuilder();
                query.Append("DELETE FROM Role WHERE role_id = @roleID");
                using (cmd = new SqlCommand(query.ToString(), activeConnection, transact))
                {
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.ExecuteNonQuery();
                }

                transact.Commit();
                Message("Successfully delete Role");
                clearForm();
                saveBtn.Visible = false;
                delBtn.Visible = false;
            }
            catch
            {
                transact.Rollback();
                lblError.Visible = true;
                lblError.Text = "Role delete has failed. Please try again";
            }
            finally
            {
                activeConnectionClose();
            }
        }

        protected void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int chkBoxItems = 0;
                query = new StringBuilder();
                query.Append("SELECT role_desc FROM Role WHERE role_desc = @rolename");
                cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
                cmd.Parameters.AddWithValue("@rolename", txtRoleName.Text);
                if (cmd.ExecuteScalar() == null)
                {
                    foreach (ListItem item in chkBoxFunctions.Items)
                    {
                        if (item.Selected)
                        {
                            chkBoxItems++;
                        }
                    }
                    if (chkBoxItems > 0)
                    {
                        transact = activeConnectionOpen().BeginTransaction("RoleAdd");

                        //add role 
                        query = new StringBuilder();
                        query.Append("Insert into Role(role_desc, role_type, modified_by, modified_date) ");
                        query.Append("Values(@rolename, @roletype, @modby, @date)");
                        cmd = new SqlCommand(query.ToString(), activeConnection, transact);
                        cmd.Parameters.AddWithValue("@rolename", txtRoleName.Text);
                        cmd.Parameters.AddWithValue("@roletype", txtRoleType.Text);
                        cmd.Parameters.AddWithValue("@modby", Session["UserID"]);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.ExecuteNonQuery();

                        //get Role ID
                        string query2 = "SELECT role_id FROM Role WHERE role_desc = @rolename";
                        SqlCommand getID = new SqlCommand(query2, activeConnection, transact);
                        getID.Parameters.AddWithValue("@rolename", txtRoleName.Text);
                        int newroleID = Convert.ToInt32(getID.ExecuteScalar());

                        foreach (ListItem item in chkBoxFunctions.Items)
                        {
                            if (item.Selected)
                            {
                                string wew = item.Text;
                                insertFunctionRole(newroleID, item.Text, activeConnection, transact);
                            }
                        }
                        transact.Commit();
                        Message("Role successfully added");
                        clearForm();
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Please select a function";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Role name already exists";
                }
            }
            catch
            {
                transact.Rollback();
                lblError.Visible = true;
                lblError.Text = "Role add has failed. Please try again";
            }
            finally
            {
                activeConnectionClose();
            }
        }

        protected void insertFunctionRole(int inroleID, string fName, SqlConnection con, SqlTransaction trans)
        {
            try
            {
           
                //get function id of function name
                string query2 = "Select function_id FROM Functions WHERE function_name = @name";
                SqlCommand check = new SqlCommand(query2, con, trans);
                check.Parameters.AddWithValue("@name", fName);
                int functionID = Convert.ToInt32(check.ExecuteScalar());

                query = new StringBuilder();
                query.Append("Insert into Role_Function(role_id, function_id) ");
                query.Append("Values(@roleID, @functionID)");
                cmd = new SqlCommand(query.ToString(), con, trans);
                cmd.Parameters.AddWithValue("@roleID", inroleID);
                cmd.Parameters.AddWithValue("@functionID", functionID);
                cmd.ExecuteNonQuery();
                
            }
            catch
            {
                throw;
            }
        }

        private void clearForm()
        {
            txtRoleName.Text = "";
            txtRoleType.Text = "";
            foreach (ListItem item in chkBoxFunctions.Items)
            {
                item.Selected = false;
            }
        }
    }
}