using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotCMIS.Client.Impl;
using DotCMIS.Client;
using DotCMIS;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace WebApplication1
{
    public partial class Confirmation : BasePage
    { 
        SqlCommand cmd;
        int totalConfirmed = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlCommand checker = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Membership.GetUser().UserName + "' AND END_USER.role_id = ROLE.role_id", activeConnection);
            if (checker.ExecuteScalar().ToString() != "BANK BRANCH" && checker.ExecuteScalar().ToString() != "OVERSEER")
            {
                string script = "alert(\"You are not authorized to view this page!\");location ='/Default.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(),
                                        "alertMessage", script, true);
            }
            else
            {
                if (!IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                }
            }
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            int i = GetRowIndex();
            if (i != -1)
            {
                SqlCommand update = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                update.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[5].Text);
                update.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                update.Parameters.AddWithValue("@fund", "YES");
                update.ExecuteNonQuery();
                activeConnection.Close();

                DataTable dt = FillDataTable();
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("alert('Please select a customer first');");
                sb.Append("<");
                sb.Append("/script>");

                if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            int i = GetRowIndex();
            if (i != -1)
            {
                cmd = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[5].Text);
                cmd.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@fund", "NO");
                cmd.ExecuteNonQuery();
                activeConnection.Close();

                DataTable dt = FillDataTable();
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("alert('Please select a customer first');");
                sb.Append("<");
                sb.Append("/script>");

                if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Retrieves the schema of the table.
            DataTable dtSchema = new DataTable();
            dtSchema.Clear();
            dtSchema = GetData();

            // set the resulting file attachment name to the name of the report...
            string fileName = "test";

            //Response.Write(dtSchema.Rows.Count);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (DataRow datar in dtSchema.Rows)
            {
                for (int i = 0; i < dtSchema.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(datar[i]))
                    {
                        sb.Append(datar[i].ToString());
                    }
                    if (i < dtSchema.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        private DataTable GetData()
        {
            using (cmd = new SqlCommand("SELECT check_number AS CheckNo, amount AS Amount, CONVERT(VARCHAR(10), check_date, 101) AS Date, branch_name AS 'Branch Name', drawee_bank AS 'Drawee Bank', drawee_bank_branch AS 'Drawee Bank Branch', funded AS 'Funded?', verification AS 'Verified?', confirmed AS 'Confirmed?', CHEQUE.account_number FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND confirmed = 'NO' ORDER BY CHEQUE.account_number"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = activeConnection;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        protected void GridView1_Sorting(Object sender, GridViewSortEventArgs e)
        {
            DataTable dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public DataTable FillDataTable()
        {
            string query = "SELECT check_number, customer_name, customer_address, contact_number, CHEQUE.account_number AS 'Account Number', CONVERT(VARCHAR(10), check_date, 101) AS Date, convert(varchar,cast(amount as money),1) AS amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND ((verification = 'YES' AND amount > maximum) OR verification = 'NO')  ORDER BY CHEQUE.account_number";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, activeConnection);
            da.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            activeConnection.Close();

            return dt;
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

        //get selected row index
        protected int GetRowIndex()
        {
            int x = -1;
            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                GridViewRow row = GridView1.Rows[i];
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
        
        
        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            int i = GetRowIndex();
            if (i != -1)
            {
                GridViewRow row = GridView1.Rows[i];
                
                row.BackColor = System.Drawing.Color.Aqua;
                row.Style.Add("class", "SelectedRowStyle");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("alert('Please select a chec');");
                sb.Append("<");
                sb.Append("/script>");

                if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
            }
        }

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = GridView1.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string confirmed = e.Row.Cells[13].Text;
                if (confirmed.Equals("YES"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Green;
                    totalConfirmed += 1;
                }
                if (confirmed.Equals("NO"))
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    totalConfirmed += 1;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[12].Text = "Confirmed: " + totalConfirmed.ToString();
                e.Row.Cells[13].Text = "Total: " + total.ToString();
                totalCon.Text = totalConfirmed.ToString();
                totalCount.Text = total.ToString();
                totalConHide.Value = totalConfirmed.ToString();
                totalCountHide.Value = total.ToString();
            }
        }
    }
}