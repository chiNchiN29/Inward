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
using System.Drawing;

namespace WebApplication1
{

    public partial class Confirmation : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

   
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter da;
        GridViewRow row;
        
        int totalConfirmed = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            //bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            //if (login == false)
            //{
            //    Response.Redirect("~/Account/Login.aspx");
            //}
            //else
            //{
            //    connection.Open();
 
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
                connection.Open();
                cmd = new SqlCommand("update CHEQUE SET confirmed = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
                cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[5].Text);
                cmd.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@fund", "YES");
                cmd.ExecuteNonQuery();
                connection.Close();


                dt = FillDataTable();
                
            }
            else
            {
                ErrorMessage("Please select a customer");
            }
        }

        private void ErrorMessage(string message)
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

                dt = FillDataTable();
                
            }
            else
            {
                ErrorMessage("Please select a customer");
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
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
            using (connection)
            {
                using (cmd = new SqlCommand("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, CHEQUE.account_number FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND confirmed = 'NO' ORDER BY CHEQUE.account_number"))
                {
                    using (da = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
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

        protected void GridView1_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public DataTable FillDataTable()
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT check_number, customer_name, customer_address, contact_number, CHEQUE.account_number, check_date, amount, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed ");
            query.Append("FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD ");
            query.Append("WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id ");
            query.Append("AND ((verification = 'YES' AND amount > maximum) OR verification = 'NO') ");
            query.Append("ORDER BY CHEQUE.account_number");

            connection.Open();
            dt = new DataTable();
            da = new SqlDataAdapter(query.ToString(), connection);
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
                row = GridView1.Rows[i];
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
            string previousRow = ViewState["SelectRow"] as string;
            if (previousRow != null)
            {
                int rows = int.Parse(previousRow);
                row = GridView1.Rows[rows];
                row.BackColor = Color.White;
            }

            int i = GetRowIndex();
            ViewState["SelectRow"] = i.ToString();  

            if (i != -1)
            {
                row = GridView1.Rows[i];
                row.BackColor = System.Drawing.Color.Aqua;
            
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
                    e.Row.CssClass = "YesVer";
                    totalConfirmed++;
                }
                if (confirmed.Equals("NO"))
                {
                    e.Row.CssClass = "NoVer";
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
    }
}