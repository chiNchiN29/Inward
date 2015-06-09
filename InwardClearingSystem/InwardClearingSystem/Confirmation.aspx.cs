using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotCMIS;
using DotCMIS.Client;
using DotCMIS.Client.Impl;

namespace InwardClearingSystem
{

    public partial class Confirmation : BasePage
    {
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        GridViewRow row;
        int totalConfirmed = 0;
        RadioButton rb;
        StringBuilder query;

        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnectionOpen();
            cmd = new SqlCommand("SELECT role_desc FROM [User] u, Role r WHERE username = @username AND u.role_id = r.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            string role = cmd.ExecuteScalar().ToString();
            activeConnectionClose();
            if (role != "BANK BRANCH" && role != "OVERSEER")
            {
                Message("You are not authorized to view this page");
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["SelectRow"] = -1;
                }
            }
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (i == -1)
            {
                Message("Please select a customer");
            }
            else if (String.IsNullOrWhiteSpace(confirmRemarks.Text))
            {
                Message("Please input remarks");
            }
            else
            {    
                UpdateConfirmCheckData(i, "YES");
                insertCheckLog(i, "Confirmation", "Successfully confirmed yes", ConfirmView);
                FillDataTable();
                NextRow(ConfirmView, i);
   
            }
        }

        private void UpdateConfirmCheckData(int i, string confirm)
        {
            query = new StringBuilder();
            query.Append("Update Cheque SET confirmed = @fund, modified_by = @modby, modified_date = @moddate, confirm_remarks = @conremarks ");
            query.Append("WHERE account_number = @acctnumber AND check_number = @chknumber");
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {
                cmd.Parameters.AddWithValue("@acctnumber", ConfirmView.Rows[i].Cells[3].Text);
                cmd.Parameters.AddWithValue("@chknumber", ConfirmView.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@fund", confirm);
                cmd.Parameters.AddWithValue("@modby", Session["UserID"]);
                cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
                cmd.Parameters.AddWithValue("@conremarks", confirmRemarks.Text);
                cmd.ExecuteNonQuery();
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (i == -1)
            {
                Message("Please select a customer");   
            }
            else if (String.IsNullOrWhiteSpace(confirmRemarks.Text))
            {
                Message("Please input remarks");
            }
            else
            {
                UpdateConfirmCheckData(i, "NO");
                insertCheckLog(i, "Confirmation", "Successfully confirmed no", ConfirmView);
                FillDataTable();
                NextRow(ConfirmView, i);

            }
        }

        protected void genListBtn_Click(object sender, EventArgs e)
        {
            // Retrieves the schema of the table.
            dt = new DataTable();
            dt.Clear();
            dt = GetData();

            // set the resulting file attachment name to the name of the report...
            string fileName = "test";

            //Response.Write(dtSchema.Rows.Count);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (DataRow datar in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(datar[i]))
                    {
                        sb.Append(datar[i].ToString());
                    }
                    if (i < dt.Columns.Count - 1)
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
            query = new StringBuilder();
            query.Append("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, ");
            query.Append("drawee_bank_branch, funded, verification, confirmed, ch.account_number ");
            query.Append("FROM Cheque ch, Customer c, Account a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id AND confirmed = 'NO' ");
            query.Append("ORDER BY ch.account_number");
            activeConnectionOpen();
            da = new SqlDataAdapter(query.ToString(), activeConnection);
            dt = new DataTable();                        
            da.Fill(dt);
            activeConnectionClose();
            return dt;      
        }

        protected void ConfirmView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            ConfirmView.DataSource = dt;
            ConfirmView.DataBind();
        }

        public DataTable FillDataTable()
        {
            query = new StringBuilder();
            query.Append("SELECT check_number, (f_name + ' ' + m_name + ' ' + l_name) AS customer_name, address, contact_number, ch.account_number, ");
            query.Append("check_date, amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, confirm_remarks ");
            query.Append("FROM Cheque ch, Customer c, Account a, Threshold t ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id AND verification = 'NO' ");
            da = new SqlDataAdapter(query.ToString(), activeConnectionOpen());

            dt = new DataTable();
            
            da.Fill(dt);
            ConfirmView.DataSource = dt;
            ConfirmView.DataBind();
            activeConnectionClose();

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

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            int previousRow = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (previousRow != -1)
            {
                row = ConfirmView.Rows[previousRow];
                row.BackColor = Color.White;
            }

            rb = (RadioButton)sender;
            row = (GridViewRow)rb.NamingContainer;
            int i = row.RowIndex;
            ViewState["SelectRow"] = i;

            if (i != -1)
            {

                row = ConfirmView.Rows[i];
                row.BackColor = Color.Aqua;
                row = ConfirmView.Rows[i];

                string im = row.Cells[3].Text;
                string age = row.Cells[1].Text;
                string image = im + "_" + age;
                ShowChequeImage(session, image, checkImage);
                ShowSigDTImage(row.RowIndex, cmd, ConfirmView, sigImage);
            }

        }

        protected void ConfirmView_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = ConfirmView.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string confirmed = e.Row.Cells[11].Text;
                if (confirmed.Equals("YES"))
                {
                    e.Row.CssClass = "YesVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalConfirmed++;
                }
                if (confirmed.Equals("NO"))
                {
                    e.Row.CssClass = "NoVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalConfirmed++;
                }
               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[10].Text = "Confirmed: " + totalConfirmed.ToString();
                e.Row.Cells[11].Text = "Total: " + total.ToString();
                totalCon.Text = totalConfirmed.ToString();
                totalCount.Text = total.ToString();
                totalConHide.Value = totalConfirmed.ToString();
                totalCountHide.Value = total.ToString();
            }
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            query = new StringBuilder();
            query.Append("SELECT check_number, (f_name + ' ' + m_name + ' ' + l_name) AS customer_name, address, contact_number, ch.account_number, ");
            query.Append("check_date, amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, confirm_remarks ");
            query.Append("FROM Cheque ch, Customer c, Account a, Threshold t ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id AND verification = 'NO' ");
            query.Append("AND ch.check_number LIKE @num + '%' ");
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {
                cmd.Parameters.AddWithValue("@num", txtSearch.Text);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                ConfirmView.DataSource = dt;
                ConfirmView.DataBind();
            }
        }

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            FillDataTable();
        }
    }
}