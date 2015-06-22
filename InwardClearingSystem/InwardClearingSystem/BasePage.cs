using DotCMIS;
using DotCMIS.Client;
using DotCMIS.Client.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DotCMIS.Data;

namespace InwardClearingSystem
{
    public class BasePage : System.Web.UI.Page
    {
        public DataTable dt;
        public SqlConnection activeConnection = new SqlConnection();
        public SqlDataAdapter da;
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (login == false)
            {
                Response.Redirect("~/Login.aspx");
            }

            Session["UserName"] = System.Web.HttpContext.Current.User.Identity.Name;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetCurrentUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@username", Session["UserName"].ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Session["UserID"] = Convert.ToInt32(dr["user_id"]);
                    Session["RoleID"] = Convert.ToInt32(dr["role_id"]);
                }
                dr.Close();
            }
        }

        /// <summary>
        /// Checks if the role of the current user has access to the current page.
        /// </summary>
        /// <param name="roleID">Role ID to be returned.</param>
        /// <param name="function">Name of the function predefined at each page.</param>
        /// <returns>The Role ID.</returns>
        public bool checkAccess(int roleID, string function)
        {
            SqlCommand cmd;
            try
            {
                //get function ID
                cmd = new SqlCommand();
                cmd.CommandText = "SearchFunctionID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@fname", function);
                string functionID = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand();
                cmd.CommandText = "CrossCheckAccess";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@functionID", functionID);
                if (cmd.ExecuteScalar() == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Used when adding new entries, this method compares attributes of the entity to be 
        /// added with the entities currently in the database.
        /// </summary>
        /// <param name="columnname">The attribute to be compared.</param>
        /// <param name="tablename">The table under which the attribute belongs.</param>
        /// <param name="compare">The new value to be compared</param>
        /// <returns>True or False.</returns>
        public bool checkForDuplicateData(string columnname, string tablename, string compare)
        {
            StringBuilder query;
            SqlCommand cmd;
            try
            {
                using (activeConnectionOpen())
                {
                    query = new StringBuilder();
                    query.Append("SELECT " + columnname + " ");
                    query.Append("FROM " + tablename + " ");
                    query.Append("WHERE " + columnname + " =  @comparison");
                    cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
                    cmd.Parameters.AddWithValue("@comparison", compare);
                    if (cmd.ExecuteScalar() == null)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fills up a DataTable with results from an SQL query.
        /// </summary>
        /// <param name="commandText">The SQL query made to the database.</param>
        /// <param name="sqlConnection">The required SqlConnection for the queries to work.</param>
        /// <param name="gridView">The affected GridView.</param>
        /// <returns>Filled DataTable</returns>
        public DataTable FillDataTable(String commandText, SqlConnection sqlConnection, GridView gridView)
        {
            try
            {
                using (da = new SqlDataAdapter(commandText, sqlConnection))
                {
                    dt = new DataTable();

                    da.Fill(dt);
                    gridView.DataSource = dt;
                    gridView.DataBind();
                    return dt;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks current state of the SqlConnection.
        /// </summary>
        /// <returns>The open SqlConnection.</returns>
        public SqlConnection activeConnectionOpen()
        {
            activeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                if (activeConnection.State == ConnectionState.Closed)
                {
                    activeConnection.Open();
                }
                return activeConnection;
            }
            catch
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/FailurePage.aspx");
                }
                return null;
            }
        }

        /// <summary>
        /// Checks the current state of the SqlConnection.
        /// </summary>
        /// <returns>The closed SqlConnection.</returns>
        public SqlConnection activeConnectionClose()
        {
            activeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                if (activeConnection.State == ConnectionState.Open)
                {
                    activeConnection.Close();
                }
                return activeConnection;
            }
            catch
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/FailurePage.aspx");
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the current sort direction being implemented in the GridView.
        /// </summary>
        /// <param name="column">Name of column.</param>
        /// <returns></returns>
        public string GetSortDirection(string column)
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

        /// <summary>
        /// Adds an entry into the Cheque Log when any changes have been made to a cheque.
        /// </summary>
        /// <param name="i">The row's index that contains the cheque and account numbers.</param>
        /// <param name="action">The particular change done to the cheque entry.</param>
        /// <param name="remarks">Remarks that come with the change. Usually only added with changes under Verification or Confirmation.</param>
        /// <param name="view">The GridView where this method is to occur.</param>
        public void insertCheckLog(int i, string action, string remarks, GridView view)
        {
            try
            {
                string user = Session["UserName"].ToString();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "InsertCheckLog";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@username", user);
                    cmd.Parameters.AddWithValue("@date_logged", DateTime.Now);
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@remarks", remarks);
                    cmd.Parameters.AddWithValue("@chknum", view.Rows[i].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@acctnum", view.Rows[i].Cells[3].Text);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Invokes a notification from the browser.
        /// </summary>
        /// <param name="message">
        /// Message to be printed out.
        /// </param>
        public void Message(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript'>");
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("');");
            sb.Append("</script>");

            if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
        }

        /// <summary>
        /// Selects the following row after completing an operation with one.
        /// </summary>
        /// <param name="view">The GridView this method will occur in.</param>
        /// <param name="i">The row index of the given GridView.</param>
        public void NextRow(GridView view, int i)
        {
            if (i < view.Rows.Count - 1)
            {
                int nextRow = i + 1;
                GridViewRow row = view.Rows[nextRow];
                RadioButton rb = (RadioButton)row.FindControl("RowSelect");
                rb.InputAttributes["checked"] = "true";
                row.BackColor = ColorTranslator.FromHtml("#FF7272");
            }
        }

        /// <summary>
        /// Retrieves the corresponding image of the account's registered signature.
        /// </summary>
        /// <param name="rowIndex">The index of the row that contains the specific account number used to retrieve the signature image.</param>
        /// <param name="cmd">The SqlCommand that looks through the database for the required images.</param>
        /// <param name="view">The GridView where this method is to occur.</param>
        /// <param name="image">The control where the retrieved image is to appear.</param>
        public void ShowSigDTImage(int rowIndex, SqlCommand cmd, GridView view, System.Web.UI.WebControls.Image image)
        {
            using (activeConnectionOpen())
            {
                try
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "FetchSignatureImage";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnection;
                        cmd.Parameters.AddWithValue("@acctnumber", view.Rows[rowIndex].Cells[3].Text);

                        byte[] result = cmd.ExecuteScalar() as byte[];
                        string base64string2 = Convert.ToBase64String(result, 0, result.Length);
                        image.ImageUrl = "data:image/jpeg;base64," + base64string2;
                        image.Visible = true;
                    }
                }
                catch
                {
                    image.ImageUrl = "~/Resources/No_image_available.jpg";
                    image.Visible = true;
                }
            }
        }
    }
}