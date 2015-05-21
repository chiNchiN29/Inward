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

namespace WebApplication1
{
    public partial class Verification : BasePage
    {
        SqlDataAdapter da;
        SqlCommand cmd;
        GridViewRow row;
        DataTable dt;
        int totalVerified = 0;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["SelectRow"] = -1;
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
                string juvy = ("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + fileName);
                IDocument doc = (IDocument)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + fileName);

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

            RadioButton rb = (RadioButton)sender;
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
                   SqlCommand cmd = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                   cmd.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                   cmd.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                   cmd.Parameters.AddWithValue("@verify", "YES");
                   cmd.ExecuteNonQuery();
                   activeConnection.Close();

                   dt = FillDataTable();

                   GridViewRow row = VerifyView.Rows[i];
                   RadioButton rb = (RadioButton)row.FindControl("RowSelect");
                   rb.InputAttributes["checked"] = "true";
                   row.BackColor = Color.Aqua;

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
                    SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);

                    update.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                    update.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                    update.Parameters.AddWithValue("@verify", "NO");
                    update.ExecuteNonQuery();
                    activeConnection.Close();

                    dt = FillDataTable();

                    GridViewRow row = VerifyView.Rows[i];
                    RadioButton rb = (RadioButton)row.FindControl("RowSelect");
                    rb.InputAttributes["checked"] = "true";
                    row.BackColor = Color.Aqua;
                }
            }
        }

        //insert signatures in database
        protected void insertSig_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert into SIGNATURE(signature_image, account_number) values (@Sig, @ID)", activeConnection);
            cmd.Parameters.AddWithValue("@Sig", imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
            cmd.Parameters.AddWithValue("@ID", TextBox1.Text);
            cmd.ExecuteNonQuery();
            activeConnection.Close();

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
            query.Append("SELECT check_number, customer_name, CHEQUE.account_number, check_date, amount, balance, BRANCH.branch_name, drawee_bank, drawee_bank_branch, verification ");
            query.Append("FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD, BRANCH, END_USER ");
            query.Append("WHERE END_USER.username = @username AND END_USER.user_id = BRANCH.user_id AND BRANCH.branch_name = CHEQUE.branch_name AND CHEQUE.account_number = ACCOUNT.account_number ");
            query.Append("AND ACCOUNT.customer_id = CUSTOMER.customer_id AND CHEQUE.amount >= minimum AND verification <> 'BTA' ");
            query.Append("ORDER BY CHEQUE.account_number");
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
                    totalVerified++;
                }
                if (verified.Equals("NO"))
                {
                    e.Row.CssClass = "NoVer";
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
