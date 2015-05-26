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

        protected void Page_Load(object sender, EventArgs e)
        {

            cmd = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = @username AND END_USER.role_id = ROLE.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "BANK BRANCH" && cmd.ExecuteScalar().ToString() != "OVERSEER")
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
                }
            }	
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (i == -1)
            {
                ErrorMessage("Please select a customer");
            }
            else
            {
                cmd = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                cmd.Parameters.AddWithValue("@acctnumber", ConfirmView.Rows[i].Cells[5].Text);
                cmd.Parameters.AddWithValue("@chknumber", ConfirmView.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@fund", "YES");
                cmd.ExecuteNonQuery();
                activeConnection.Close();
                dt = FillDataTable();

                NextRow(ConfirmView, i);
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (i == -1)
            {
                ErrorMessage("Please select a customer");
               
                
                
            }
            else
            {
                cmd = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                cmd.Parameters.AddWithValue("@acctnumber", ConfirmView.Rows[i].Cells[5].Text);
                cmd.Parameters.AddWithValue("@chknumber", ConfirmView.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@fund", "NO");
                cmd.ExecuteNonQuery();

                activeConnection.Close();
                dt = FillDataTable();

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
            using (activeConnection)
            {
                using (cmd = new SqlCommand("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, CHEQUE.account_number FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND confirmed = 'NO' ORDER BY CHEQUE.account_number"))
                {
                    using (da = new SqlDataAdapter())
                    {
                        cmd.Connection = activeConnection;
                        da.SelectCommand = cmd;
                        using (dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }

                    }
                }
            }
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

            StringBuilder query = new StringBuilder();
            query.Append("SELECT check_number, customer_name, customer_address, contact_number, CHEQUE.account_number, check_date, amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed ");
            query.Append("FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD ");
            query.Append("WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id ");
            query.Append("AND ((verification = 'YES' AND amount > maximum) OR verification = 'NO') ");
            query.Append("ORDER BY CHEQUE.account_number");
      
            
            dt = new DataTable();
            da = new SqlDataAdapter(query.ToString(), activeConnection);
            da.Fill(dt);
            ConfirmView.DataSource = dt;
            ConfirmView.DataBind();
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

            row = ConfirmView.Rows[i];
            row.BackColor = System.Drawing.Color.Aqua;
            
        
        }

        protected void ConfirmView_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = ConfirmView.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string confirmed = e.Row.Cells[13].Text;
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
                e.Row.Cells[12].Text = "Confirmed: " + totalConfirmed.ToString();
                e.Row.Cells[13].Text = "Total: " + total.ToString();
                totalCon.Text = totalConfirmed.ToString();
                totalCount.Text = total.ToString();
                totalConHide.Value = totalConfirmed.ToString();
                totalCountHide.Value = total.ToString();
            }
        }

        protected void UploadCheckData(object sender, EventArgs e)
        {
            
        }

        ////Function to bind gridview  
        //private void Bindgrid(DataTable csvdt)
        //{
        //    ConfirmView.DataSource = csvdt;
        //    ConfirmView.DataBind();
        //}  
    }
}