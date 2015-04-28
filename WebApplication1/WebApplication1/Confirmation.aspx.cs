using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotCMIS.Client.Impl;
using DotCMIS.Client;
using DotCMIS;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class Confirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDataSource1.SelectCommand = "SELECT check_number AS CheckNo, customer_name AS Name, customer_address AS Address, contact_number AS ContactNo, CHEQUE.account_number AS AcctNo, CONVERT(VARCHAR(10), check_date, 111) AS Date, amount, drawee_bank AS DraweeBank, drawee_bank_branch AS DraweeBankBranch, funded, verification AS Verified FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CUSTOMER.account_number ORDER BY CHEQUE.account_number";
            GridView1.DataSource = SqlDataSource1;
            GridView1.DataBind();
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET funded = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@fund", "YES");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if (GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET funded = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@fund", "NO");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if (GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }
    }


}