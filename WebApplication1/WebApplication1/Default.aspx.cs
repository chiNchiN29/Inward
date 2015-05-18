using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotCMIS;
using DotCMIS.Client.Impl;
using DotCMIS.Client;
using DotCMIS.Data.Impl;
using DotCMIS.Data.Extensions;
using System.IO;
using DotCMIS.Data;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class _Default : BasePage
    {    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
            }
        }

        protected void UploadImage(Object sender, EventArgs e)
        {
            HttpFileCollection hfc = Request.Files;
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFile hpf = hfc[i];
                if (hpf.ContentLength > 0)
                {
                    UploadADocument(session, imageToByteArray(System.Drawing.Image.FromStream(hpf.InputStream)), Path.GetFileNameWithoutExtension(hpf.FileName));
                }
            }
            Response.Write("<script langauge=\"javascript\">alert(\"Images successfully added\");</script>");
        }

        private void UploadADocument(ISession session, byte[] ImageFile, string fileName)
        {
            IFolder folder = (IFolder)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"));
            Dictionary<String, object> DocumentProperties = new Dictionary<string, object>();
            
            DocumentProperties[PropertyIds.Name] =  fileName;
            DocumentProperties[PropertyIds.ObjectTypeId] = "cmis:document";
            ContentStream contentStream = new ContentStream
            {
                MimeType = "image/jpeg",
                Length = ImageFile.Length,
                Stream = new MemoryStream(ImageFile)
            };

            folder.CreateDocument(DocumentProperties, contentStream, null);
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        protected void UploadCheckData(object sender, EventArgs e)
        {
            try
            {
                //string filepath = FileUpload2.PostedFile.FileName;
                StreamReader sr = new StreamReader(FileUpload2.PostedFile.InputStream);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] heart = line.Split(',');
                    SqlCommand validator = new SqlCommand("select check_number from CHEQUE where check_number = '" + heart[0] + "'", activeConnection);
                    if (validator.ExecuteScalar() == null)
                    {
                        SqlCommand insert = new SqlCommand("insert into CHEQUE(check_number, amount, check_date, branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, account_number) values (@checknum, @amount, @date, @branch, @draweebank, @draweebranch, @funded, @verified, @confirmed, @acctnum)", activeConnection);
                        SqlCommand checker = new SqlCommand("select minimum from THRESHOLD", activeConnection);
                        insert.Parameters.AddWithValue("@checknum", heart[0]);
                        insert.Parameters.AddWithValue("@amount", heart[1]);
                        insert.Parameters.AddWithValue("@date", heart[2]);
                        insert.Parameters.AddWithValue("@branch", heart[3]);
                        insert.Parameters.AddWithValue("@draweebank", heart[4]);
                        insert.Parameters.AddWithValue("@draweebranch", heart[5]);
                        insert.Parameters.AddWithValue("@funded", heart[6]);
                        if (decimal.Parse(heart[1]) < decimal.Parse(checker.ExecuteScalar().ToString()))
                        {
                            insert.Parameters.AddWithValue("@verified", "BTA");
                        }
                        else
                        {
                            insert.Parameters.AddWithValue("@verified", heart[7]);
                        }
                        insert.Parameters.AddWithValue("@confirmed", heart[8]);
                        insert.Parameters.AddWithValue("@acctnum", heart[9]);
                        insert.ExecuteNonQuery();
                    }
                    else
                    {
                        sr.Close();
                        Response.Write("Check Data Upload interrupted. A duplicate check number has been discovered.");
                    }
                }
                DataTable dt = FillDataTable();
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception b)
            {
                //Response.Write("Please re-check the format of the data for any missing fields.");
                Response.Write(b);
            }

      

             activeConnection.Close();
        }

        protected void clearCheck_Click1(object sender, EventArgs e)
        {
            SqlCommand delete = new SqlCommand("DELETE FROM CHEQUE", activeConnection);
            SqlCommand deleteBranches = new SqlCommand("DELETE FROM BRANCH DBCC CHECKIDENT('BRANCH', RESEED, 0)", activeConnection);
            delete.ExecuteNonQuery();
            deleteBranches.ExecuteNonQuery();
            
            GridView1.DataBind();
            activeConnection.Close();
            
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public DataTable FillDataTable()
        {
            string query = "SELECT check_number, customer_name, CHEQUE.account_number AS 'Account Number', CONVERT(VARCHAR(10), check_date, 101) AS Date, convert(varchar,cast(amount as money),1) AS amount, convert(varchar,cast(balance as money),1) AS balance, branch_name, drawee_bank, drawee_bank_branch, verification, funded FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id ORDER BY CHEQUE.check_number";
          
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, activeConnection);
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
    }
}
