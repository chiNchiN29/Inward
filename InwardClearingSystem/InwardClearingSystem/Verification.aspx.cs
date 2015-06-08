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
using DotCMIS.Data;
using DotCMIS.Data.Extensions;
using DotCMIS.Data.Impl;

namespace InwardClearingSystem
{
    public partial class Verification : BasePage
    {
        SqlDataAdapter da;
        SqlCommand cmd;
        GridViewRow row;
        DataTable dt;
        int totalVerified = 0;
        RadioButton rb;
        StringBuilder query;
     
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnectionOpen();
            cmd = new SqlCommand("SELECT role_desc FROM [User] u, Role r WHERE username = @username AND u.role_id = r.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            string role = cmd.ExecuteScalar().ToString();
            activeConnectionClose();
            if (role != "CLEARING DEPT" && role != "OVERSEER")
            {
                Message("You are not authorized to view this page");
                Response.Redirect("~/Default.aspx");
                
            }                   
            else
            {
                if (!Page.IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["SelectRow"] = -1;
                }
            }
           
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

       
        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            int previousRow = Convert.ToInt32(ViewState["SelectRow"].ToString());
            if (previousRow != -1)
            {
                row = VerifyView.Rows[previousRow];
                row.BackColor = Color.White;
            }

            rb = (RadioButton)sender;
            row = (GridViewRow)rb.NamingContainer;
            int i = row.RowIndex;
            ViewState["SelectRow"] = i; 

            if (i != -1)
            {

                row = VerifyView.Rows[i];
                row.BackColor = Color.Aqua;
                row = VerifyView.Rows[i];
          
                string im = row.Cells[3].Text;
                string age = row.Cells[1].Text;
                string image = im + "_" + age;
                ShowChequeImage(session, image, checkImage);
                ShowSigDTImage(row.RowIndex, cmd, VerifyView, sigImage);
            }
        }

        protected void acceptButton_Click(object sender, EventArgs e)
        {
             int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

           if (i == -1)
           {
               Message("Please select a check");
           }
           else
           {
               //if (checkImage.ImageUrl == "~/Resources/H2DefaultImage.jpg" || sigImage.ImageUrl == "~/Resources/H2DefaultImage.jpg")
               //{
               //    Message("Cannot validate because there is no existing check or signature");
               //}
               //else
               //{
                       
                    UpdateCheckData(i, "YES", "");
                    insertCheckLog(i, "Verification", "Successfully verified yes", VerifyView);
                    FillDataTable();
                    NextRow(VerifyView, i);
                       
               //}
           }
        }

        

        private void UpdateCheckData(int i, string verify, string remarks )
        {
            query = new StringBuilder();
            query.Append("update Cheque SET verification = @verify, modified_by = @modby, modified_date = @moddate, verify_remarks = @veremarks ");
            query.Append("WHERE account_number = @acctnumber AND check_number = @chknumber");
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {
                
                cmd.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                cmd.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                cmd.Parameters.AddWithValue("@verify", verify);
                cmd.Parameters.AddWithValue("@modby", Session["UserID"]);
                cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
                cmd.Parameters.AddWithValue("@veremarks", remarks);
                cmd.ExecuteNonQuery();
            }
        }

        protected void rejectButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
            bool check = verifyChoice.SelectedIndex != -1;
            string remarks = "";
            if (i == -1)
            {
                Message("Please select a check");
            }
            else
            {
                //if (checkImage.ImageUrl == "~/Resources/H2DefaultImage.jpg" || sigImage.ImageUrl == "~/Resources/H2DefaultImage.jpg")
                //{
                //    Message("Cannot verify check because there is no existing image");
                //}
                if (String.IsNullOrWhiteSpace(verifyRemarks.Text) == true && check == false)
                {
                    Message("Please input technicality");
                }
                else
                {
                        List<string> selected = new List<string>();
                        foreach (ListItem item in verifyChoice.Items)
                        {
                            if (item.Selected)
                            {
                                if (remarks == "")
                                {
                                    remarks = item.Text;
                                }
                                else
                                {
                                    remarks += ". " + item.Text;
                                }
                            }
                        }

                        UpdateCheckData(i, "NO", remarks + ". " + verifyRemarks.Text);
                        insertCheckLog(i, "Verification", "Successfully verified no", VerifyView);
                        FillDataTable();
                        verifyChoice.Items.Add(verifyRemarks.Text);


                        NextRow(VerifyView, i);
                    }
                    verifyChoice.ClearSelection();
            }
        }

        //insert signatures in database
        protected void insertSig_Click(object sender, EventArgs e)
        {
            activeConnectionOpen();
            int userID = Convert.ToInt32(Session["UserID"]);
            cmd = new SqlCommand("insert into Signature(signature_image, account_number, modified_by, modified_date) values (@Sig, @ID, @modby, @moddate)", activeConnection);
            cmd.Parameters.AddWithValue("@Sig", imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
            cmd.Parameters.AddWithValue("@ID", TextBox1.Text);
            cmd.Parameters.AddWithValue("@modby", userID);
            cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
            cmd.ExecuteNonQuery();
            activeConnectionClose();

        }

        private DataTable GetData()
        {
            query = new StringBuilder();
            query.Append("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, ch.account_number ");
            query.Append("FROM Cheque ch, Customer c, Account a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id AND verification = 'NO' "); 
            query.Append("ORDER BY ch.account_number");

            activeConnectionOpen();
            da = new SqlDataAdapter(query.ToString(), activeConnection);
            dt = new DataTable();
            da.Fill(dt);
            activeConnectionClose();
            return dt; 
        }
            
        protected void VerifyView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            VerifyView.DataSource = dt;
            VerifyView.DataBind();
        }

        public DataTable FillDataTable()
        {
            string user = Session["UserName"].ToString();
            query = new StringBuilder();
            query.Append("SELECT check_number, (c.f_name + ' ' + c.m_name + ' ' + c.l_name) AS name, ch.account_number, check_date, amount, ");
            query.Append("balance, b.branch_name, drawee_bank, drawee_bank_branch, verification, bank_remarks, verify_remarks ");
            query.Append("FROM Threshold AS t INNER JOIN Cheque AS ch INNER JOIN [User] AS u INNER JOIN  Branch AS b ON u.user_id = b.user_id ");
            query.Append("ON ch.branch_name = b.branch_name INNER JOIN Account AS a ON ch.account_number = a.account_number INNER JOIN ");
            query.Append("Customer AS c ON a.customer_id = c.customer_id ON t.minimum <= ch.amount ");
            query.Append("WHERE (u.username = @username) AND (ch.verification <> 'BTA') AND (ch.bank_remarks = NULL OR ch.bank_remarks = ' ') ");   
            query.Append("ORDER BY ch.account_number");
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {
                
                cmd.Parameters.AddWithValue("@username", user); 
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                VerifyView.DataSource = dt;
                VerifyView.DataBind();
                activeConnectionClose();
                return dt;
            }

     
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

        protected void VerifyView_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = VerifyView.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string verified = e.Row.Cells[10].Text;
                if (verified.Equals("YES"))
                {
                    e.Row.CssClass = "YesVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalVerified++;
                }
                if (verified.Equals("NO"))
                {
                    e.Row.CssClass = "NoVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalVerified++;
                }
            }
            totalVer.Text = totalVerified.ToString();
            totalCount.Text = total.ToString();
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            string user = Session["UserName"].ToString();
            query = new StringBuilder();
            query.Append("SELECT check_number, (c.f_name + ' ' + c.m_name + ' ' + c.l_name) AS name, ch.account_number, check_date, amount, ");
            query.Append("balance, b.branch_name, drawee_bank, drawee_bank_branch, verification, bank_remarks, verify_remarks ");
            query.Append("FROM Threshold AS t INNER JOIN Cheque AS ch INNER JOIN [User] AS u INNER JOIN  Branch AS b ON u.user_id = b.user_id ");
            query.Append("ON ch.branch_name = b.branch_name INNER JOIN Account AS a ON ch.account_number = a.account_number INNER JOIN ");
            query.Append("Customer AS c ON a.customer_id = c.customer_id ON t.minimum <= ch.amount ");
            query.Append("WHERE (check_number LIKE @num + '%') AND (u.username = @username) AND (ch.verification <> 'BTA') AND (ch.bank_remarks = NULL OR ch.bank_remarks = ' ') ");
            query.Append("ORDER BY ch.account_number");
            using (cmd = new SqlCommand(query.ToString(), activeConnectionOpen()))
            {

                cmd.Parameters.AddWithValue("@username", user);
                cmd.Parameters.AddWithValue("@num", txtSearch.Text);
                dt = new DataTable();
                da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                VerifyView.DataSource = dt;
                VerifyView.DataBind();
            }
        }

        protected void ViewAll_Click(object sender, EventArgs e)
        {
            FillDataTable();
            txtSearch.Text = "";
        }


        //protected void VerifyView_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if (e.Row.RowIndex == 0)   //you can change any condition here
        //        {
        //            RadioButton rb = (RadioButton)e.Row.FindControl("RowSelect");
        //            rb.InputAttributes["checked"] = "true";
        //            e.Row.BackColor = Color.Aqua;
        //        }
        //    }
        //}
    }
}
