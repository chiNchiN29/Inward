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
            //string afterrow = ViewState["SelectRow"] as string;
            //if (afterrow != null)
            //{
            //    int wew = int.Parse(afterrow);
            //    GridViewRow row = GridView1.Rows[wew];
            //    RadioButton rb = (RadioButton)row.FindControl("RowSelect");
            //    rb.Checked = true;
            //}
                    Page.MaintainScrollPositionOnPostBack = true;

                    if (!Page.IsPostBack)
                    {
                        ViewState["myDataTable"] = FillDataTable();
                        
                    }

                //}
                activeConnection.Close();
            //}
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

                Image1.ImageUrl = "data:image/jpeg;base64," + base64string;
                Image1.Visible = true;
            }
            catch
            {
                Image1.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                Image1.Visible = true;
            }

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
                row.BackColor = Color.Aqua;
                row = GridView1.Rows[i];
            
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
                cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[rowIndex].Cells[3].Text);


                byte[] result = cmd.ExecuteScalar() as byte[];
                string base64string2 = Convert.ToBase64String(result, 0, result.Length);
                Image2.ImageUrl = "data:image/jpeg;base64," + base64string2;
                Image2.Visible = true;
                activeConnection.Close();
            }
            catch
            {
                Image2.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                Image2.Visible = true;
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
            Image2.ImageUrl = "data:image/jpeg;base64," + base64string2;
            Image2.Visible = true;
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
           int i = GetRowIndex();

           //if (i != -1)
           //{
           //    if (Image1.ImageUrl == "~/Resources/H2DefaultImage.jpg" || Image2.ImageUrl == "~/Resources/H2DefaultImage.jpg")
           //    {
           //       ErrorMessage("Cannot validate because there is no existing check or signature");
           //    }
           //    else
           //    {
                   activeConnection.Open();
                   SqlCommand cmd = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);
                   cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[3].Text);
                   cmd.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                   cmd.Parameters.AddWithValue("@verify", "YES");
                   cmd.ExecuteNonQuery();
                   activeConnection.Close();

                  dt = FillDataTable();
                  
           //    }
           //}
           //else
           //{
                  //ErrorMessage("Please select a check");
           //}
        }

        protected void rejectButton_Click(object sender, EventArgs e)
        {
            int i = GetRowIndex();

            //if (i != -1)
            //{
            //    if (Image1.ImageUrl == "~/Resources/H2DefaultImage.jpg" || Image2.ImageUrl == "~/Resources/H2DefaultImage.jpg")
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append("<script language='javascript'>");
            //        sb.Append("alert('Cannot verify check because there is no existing image');");
            //        sb.Append("<");
            //        sb.Append("/script>");

            //        if (!ClientScript.IsClientScriptBlockRegistered("ErrorPopup"))
            //            ClientScript.RegisterClientScriptBlock(this.GetType(), "ErrorPopup", sb.ToString());
            //    }
            //    else
            //    {
                    SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", activeConnection);

          
                    update.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[3].Text);
                    update.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                    update.Parameters.AddWithValue("@verify", "NO");
                    update.ExecuteNonQuery();
                    activeConnection.Close();

                    dt = FillDataTable();
                    //GridView1.DataSource = dt;
                    
            //    }
            //}
            //else
            //{
                    //ErrorMessage("Please select a check");
            //}   
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

        //Generate List
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
            using (cmd = new SqlCommand("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, CHEQUE.account_number FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND verification = 'NO' ORDER BY CHEQUE.account_number"))
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

        protected void GridView1_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
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
            GridView1.DataSource = dt;
            GridView1.DataBind();
          
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

        protected void GridView1_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = GridView1.Rows.Count;

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
