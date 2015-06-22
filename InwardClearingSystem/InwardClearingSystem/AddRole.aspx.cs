using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace InwardClearingSystem
{
    public partial class AddRole : BasePage
    {
        SqlCommand cmd;
        SqlDataReader dr;
        SqlTransaction transact;
        String function = "User Maintenance";
        String roleID;
        StringBuilder query;

        protected void Page_Load(object sender, EventArgs e)
        {
            roleID = Request.QueryString["Role"];

            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

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

        protected void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int chkBoxItems = 0;
                if (checkForDuplicateData("role_desc", "Role", txtRoleName.Text) == false)
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

                        InsertRoleHistory(newroleID.ToString(), txtRoleName.Text, txtRoleType.Text, "Add", "Successful Role Add", "Role (added row)", activeConnection, transact);
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

        protected void backBtn_Click(object sender, EventArgs e)
        {
            if (this.Context != null)
            {
                Response.Redirect("~/RoleMaintenance.aspx");
            }
        }

        protected void delBtn_Click(object sender, EventArgs e)
        {
            try
            {
                transact = activeConnectionOpen().BeginTransaction("RoleDelete");

                //delete from role function
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "SeverRoleFunctionAssociation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnection;
                    cmd.Transaction = transact;
                    cmd.Parameters.AddWithValue("@roleID", roleID);
                    cmd.ExecuteNonQuery();
                }

                //delete role in user table
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "SeverUserRoleAssociation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnection;
                    cmd.Transaction = transact;
                    cmd.ExecuteNonQuery();
                }

                //delete role
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "DeleteRole";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnection;
                    cmd.Transaction = transact;
                    cmd.ExecuteNonQuery();
                }

                InsertRoleHistory(roleID.ToString(), txtRoleName.Text, txtRoleType.Text, "Delete", "Successful Role Delete", "Role (deleted row)", activeConnection, transact);
                transact.Commit();
                Message("Successfully deleted Role");
                clearForm();
                saveBtn.Visible = false;
                delBtn.Visible = false;
            }
            catch
            {
                transact.Rollback();
                InsertRoleHistory(roleID.ToString(), txtRoleName.Text, txtRoleType.Text, "Delete", "Failed Role Delete", "No change", activeConnection, transact);
                lblError.Visible = true;
                lblError.Text = "Role delete has failed. Please try again";
            }
            finally
            {
                activeConnectionClose();
            }
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
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "UpdateRole";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnection;
                        cmd.Transaction = transact;
                        cmd.Parameters.AddWithValue("@rolename", txtRoleName.Text);
                        cmd.Parameters.AddWithValue("@type", txtRoleType.Text);
                        cmd.Parameters.AddWithValue("@roleID", roleID);
                        cmd.ExecuteNonQuery();
                    }

                    //delete from role function
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SeverRoleFunctionAssociation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnection;
                        cmd.Transaction = transact;
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

                    InsertRoleHistory(roleID.ToString(), txtRoleName.Text, txtRoleType.Text, "Edit", "Successful Role Edit", "Role (row updated)", activeConnection, transact);
                    transact.Commit();
                    Message("Successfully updated Role");
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Please select a function";
                }
            }
            catch
            {
                transact.Rollback();
                lblError.Visible = true;
                InsertRoleHistory(roleID.ToString(), txtRoleName.Text, txtRoleType.Text, "Edit", "Failed Role Edit", "No change", activeConnection, transact);
                lblError.Text = "Role Update has failed. Please try again.";
            }
            finally
            {
                activeConnectionClose();
            }
        }

        /// <summary>
        /// Attaches selected functionalities to a chosen role. 
        /// </summary>
        /// <param name="inroleID">The chosen role's ID.</param>
        /// <param name="fName">The function name.</param>
        /// <param name="con">The required SqlConnection.</param>
        /// <param name="trans">KULANG PA DITO</param>
        protected void insertFunctionRole(int inroleID, string fName, SqlConnection con, SqlTransaction trans)
        {
            try
            {
                //get function id of function name
                string query2 = "Select function_id FROM Functions WHERE function_name = @name";
                SqlCommand check = new SqlCommand(query2, con, trans);
                check.Parameters.AddWithValue("@name", fName);
                int functionID = Convert.ToInt32(check.ExecuteScalar());

                //Adds a new entry under Role_Function.
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "InsertRoleFunction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@roleID", inroleID);
                    cmd.Parameters.AddWithValue("@functionID", functionID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Clears the form of all input.
        /// </summary>
        private void clearForm()
        {
            txtRoleName.Text = "";
            txtRoleType.Text = "";
            foreach (ListItem item in chkBoxFunctions.Items)
            {
                item.Selected = false;
            }
        }

        //problem with ip address
        private void InsertRoleHistory(string roleid, string desc, string type, string tag, string message, string changes, SqlConnection con, SqlTransaction trans)
        {
            try
            {
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                StringBuilder query = new StringBuilder();
                query.Append("Insert into Role_History (role_id, role_desc, role_type, modified_date, modified_by, ");
                query.Append("terminal, history_tag, changes, history_message) ");
                query.Append("values (@roleid, @desc, @type, @date, @id, @ip, @tag, @changes, @message)");
                cmd = new SqlCommand(query.ToString(), con, trans);
                cmd.Parameters.AddWithValue("@roleid", roleid);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["UserID"]));
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@tag", tag);
                cmd.Parameters.AddWithValue("@changes", changes);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}