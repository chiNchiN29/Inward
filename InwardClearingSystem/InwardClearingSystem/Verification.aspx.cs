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

            cmd = new SqlCommand("SELECT role_desc FROM [USER] u, ROLE r WHERE username = @username AND u.role_id = r.role_id", activeConnection);
            cmd.Parameters.AddWithValue("@username", Session["UserName"]);
            if (cmd.ExecuteScalar().ToString() != "CLEARING DEPT" && cmd.ExecuteScalar().ToString() != "OVERSEER")
            {
                ErrorMessage("You are not authorized to view this page");
                Response.Redirect("Default.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    ViewState["myDataTable"] = FillDataTable();
                    ViewState["SelectRow"] = -1;
                }
            }

            activeConnection.Close();
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void ShowChequeImage(ISession session, string fileName)
        {
            try
            {
        
                IDocument doc = (IDocument)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" +  fileName);
      
                IContentStream contentStream = doc.GetContentStream();
                byte[] result;
                using (var streamReader = new MemoryStream())
                {
                    contentStream.Stream.CopyTo(streamReader);
                    result = streamReader.ToArray();
                }
                string base64string = Convert.ToBase64String(result, 0, result.Length);

                checkImage.ImageUrl = "data:image/jpeg;base64," + base64string;
                checkImage.Visible = true;
            }
            catch
            {
                checkImage.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                checkImage.Visible = true;
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
                ShowChequeImage(session, image);
                ShowSigDTImage(row.RowIndex);
            }
        }

        //Signature image in Database
        private void ShowSigDTImage(int rowIndex)
        {
            try
            {
                activeConnection.Open();
                cmd = new SqlCommand("select signature_image from SIGNATURE WHERE account_number= @acctnumber", activeConnection);
                cmd.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[rowIndex].Cells[3].Text);

                byte[] result = cmd.ExecuteScalar() as byte[];
                string base64string2 = Convert.ToBase64String(result, 0, result.Length);
                sigImage.ImageUrl = "data:image/jpeg;base64," + base64string2;
                sigImage.Visible = true;
                activeConnection.Close();
            }
            catch
            {
                sigImage.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                sigImage.Visible = true;
            }

        }

        //Signature image in Alfresco
        private void ShowSigImage(ISession session, string fileName)
        {
            IDocument doc2 = (IDocument)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + fileName + ".jpg");

            IContentStream contentStream2 = doc2.GetContentStream();
            byte[] result2;
            using (var streamReader = new MemoryStream())
            {
                contentStream2.Stream.CopyTo(streamReader);
                result2 = streamReader.ToArray();
            }
            string base64string2 = Convert.ToBase64String(result2, 0, result2.Length);
            sigImage.ImageUrl = "data:image/jpeg;base64," + base64string2;
            sigImage.Visible = true;
        }

        //for controls in not master page
        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
            return Root;
 
            foreach (Control Ctl in Root.Controls)
             {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                 if (FoundCtl != null)
                 return FoundCtl;
              }
            return null;
        }

        protected void acceptButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

           if (i == -1)
           {
               ErrorMessage("Please select a check");
           }
           else
           {
               if (checkImage.ImageUrl == "~/Resources/H2DefaultImage.jpg" || sigImage.ImageUrl == "~/Resources/H2DefaultImage.jpg")
               {
                   ErrorMessage("Cannot validate because there is no existing check or signature");
               }
               else
               {
                   activeConnection.Open();
                   using (SqlCommand cmd = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection))
                   {
                       cmd.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                       cmd.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                       cmd.Parameters.AddWithValue("@verify", "YES");
                       cmd.ExecuteNonQuery();
                   }
                   activeConnection.Close();

                   dt = FillDataTable();

                   NextRow(VerifyView, i);

               }
           }
        }

        protected void rejectButton_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(ViewState["SelectRow"].ToString());

            if (i == -1)
            {
                ErrorMessage("Please select a check");
            }
            else
            {
                if (checkImage.ImageUrl == "~/Resources/H2DefaultImage.jpg" || sigImage.ImageUrl == "~/Resources/H2DefaultImage.jpg")
                {
                    ErrorMessage("Cannot verify check because there is no existing image");
                }
                else
                {
                    activeConnection.Open();
                    using (SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection))
                    {
                        update.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                        update.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                        update.Parameters.AddWithValue("@verify", "NO");
                        update.ExecuteNonQuery();
                    }
                    activeConnection.Close();

                    dt = FillDataTable();

                    NextRow(VerifyView, i);
                }
            }
        }

        //insert signatures in database
        protected void insertSig_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            cmd = new SqlCommand("insert into SIGNATURE(signature_image, account_number, modified_by, modified_date) values (@Sig, @ID, @modby, @moddate)", activeConnection);
            cmd.Parameters.AddWithValue("@Sig", imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
            cmd.Parameters.AddWithValue("@ID", TextBox1.Text);
            cmd.Parameters.AddWithValue("@modby", userID);
            cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
            cmd.ExecuteNonQuery();
            activeConnection.Close();

        }

        //Generate List
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
    
            Response.ContentType = "text/csv";

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
            query.Append("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, ch.account_number ");
            query.Append("FROM CHEQUE ch, CUSTOMER c, ACCOUNT a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id AND verification = 'NO' ORDER BY ch.account_number");

            using (activeConnection)
            {
                using (cmd = new SqlCommand(query.ToString()))
                {
                    using (da = new SqlDataAdapter())
                    {
                        cmd.Connection = activeConnection;
                        da.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }

                    }
                }
            }
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
            StringBuilder query = new StringBuilder();
            query.Append("SELECT check_number, (c.f_name + ' ' + c.m_name + ' ' + c.l_name) AS name, ch.account_number, check_date, amount, ");
            query.Append("balance, b.branch_name, drawee_bank, drawee_bank_branch, verification ");
            query.Append("FROM CHEQUE ch, CUSTOMER c, ACCOUNT a, THRESHOLD t, BRANCH b, [USER] u ");
            query.Append("WHERE u.username = @username AND u.user_id = b.user_id AND b.branch_name = ch.branch_name AND ch.account_number = a.account_number ");
            query.Append("AND a.customer_id = c.customer_id AND ch.amount >= minimum AND verification <> 'BTA' AND bank_remarks = NULL ");
            query.Append("ORDER BY ch.account_number");
            cmd = new SqlCommand(query.ToString(), activeConnection);
            cmd.Parameters.AddWithValue("@username", user); 
            dt = new DataTable();
            da = new SqlDataAdapter(cmd);

            da.Fill(dt);
            VerifyView.DataSource = dt;
            VerifyView.DataBind();
          
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
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[9].Text = "Verified: " + totalVerified.ToString();
                e.Row.Cells[10].Text = "Total: " + total.ToString();
                totalVer.Text = totalVerified.ToString();
                totalCount.Text = total.ToString();
                totalVerHide.Value = totalVerified.ToString();
                totalCountHide.Value = total.ToString();
            }
        }
    }
}
