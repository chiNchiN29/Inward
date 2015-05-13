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

namespace WebApplication1
{
    public partial class Confirmation : System.Web.UI.Page
    {
    
            
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool login = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (login == false)
                    Response.Redirect("~/Account/LogIn.aspx");
            if (!IsPostBack)
            {
                Session["myDataSet"] = FillDataSet();

            }
               
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@fund", "YES");
            update.ExecuteNonQuery();
            connection.Close();
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            connection.Open();
            cmd = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            cmd.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            cmd.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            cmd.Parameters.AddWithValue("@fund", "NO");
            cmd.ExecuteNonQuery();
            connection.Close();
            
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
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (cmd = new SqlCommand("SELECT check_number AS CheckNo, amount AS Amount, CONVERT(VARCHAR(10), check_date, 111) AS Date, branch_name AS 'Branch Name' drawee_bank AS 'Drawee Bank', drawee_bank_branch AS 'Drawee Bank Branch', funded AS 'Funded?', verification AS 'Verified?', CHEQUE.account_number AS AcctNo, confirmed AS 'Confirmed?' FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CUSTOMER.account_number AND confirmed = 'NO' ORDER BY CHEQUE.account_number"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        protected void GridView1_Sorting(Object sender, GridViewSortEventArgs e)
        {
            DataTable dt = ((DataSet)Session["myDataSet"]).Tables[0];
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public DataSet FillDataSet()
        {
            string query = "SELECT check_number, customer_name, customer_address, contact_number, CHEQUE.account_number AS 'Account Number', CONVERT(VARCHAR(10), check_date, 101) AS Date, amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND ((verification = 'YES' AND amount > maximum) OR verification = 'NO')  ORDER BY CHEQUE.account_number";
            connection.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(query, connection);
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();
            connection.Close();

            return ds;
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
    }


}