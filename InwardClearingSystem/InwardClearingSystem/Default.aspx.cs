using DotCMIS;
using DotCMIS.Client;
using DotCMIS.Client.Impl;
using DotCMIS.Data;
using DotCMIS.Data.Extensions;
using DotCMIS.Data.Impl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InwardClearingSystem
{

    public partial class _Default : BasePage
    {
        DataTable dt;
        SqlDataAdapter da;
        SqlCommand cmd;
        StringBuilder query;
        
        int noImageUploaded = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnection.Close();
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
            }
        }

        private void UploadADocument(ISession session, byte[] ImageFile, string fileName)
        {
            IFolder folder = (IFolder)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"));
            Dictionary<String, object> DocumentProperties = new Dictionary<string, object>();

            DocumentProperties[PropertyIds.Name] = fileName;
            DocumentProperties[PropertyIds.ObjectTypeId] = "cmis:document";
            ContentStream contentStream = new ContentStream
            {
                MimeType = "image/jpeg",
                Length = ImageFile.Length,
                Stream = new MemoryStream(ImageFile)
            };

            folder.CreateDocument(DocumentProperties, contentStream, null);
        }

        protected void uploadImgBtn_Click(Object sender, EventArgs e)
        {
            try
            {
                ViewState["UploadImageClicked"] = "false";
                HttpFileCollection hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFile hpf = hfc[i];
                    if (hpf.ContentLength > 0)
                    {
                        UploadADocument(session, imageToByteArray(System.Drawing.Image.FromStream(hpf.InputStream)), Path.GetFileNameWithoutExtension(hpf.FileName));
                    }
                }
                Message("Images successfully added.");
                ViewState["ImageCount"] = hfc.Count - 1;
                ViewState["UploadImageClicked"] = "true";
                
            }
            catch
            {
                Message("An image already exists with the same name");
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

        protected void UploadCheckData(object sender, EventArgs e)
        {
            try
            {
                //string filepath = FileUpload2.PostedFile.FileName;
                StreamReader sr = new StreamReader(DataUpload.PostedFile.InputStream);
                int lineCount = 0;

                using (activeConnectionOpen())
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] heart = line.Split(',');
                        string userID = Session["UserID"].ToString();
                        query = new StringBuilder();
                        query.Append("insert into Cheque(check_number, amount, check_date, branch_name, drawee_bank, drawee_bank_branch, funded, ");
                        query.Append("verification, confirmed, bank_remarks, account_number, modified_by, modified_date) ");
                        query.Append("values (@checknum, @amount, @date, @branch, @draweebank, @draweebranch, ");
                        query.Append("@funded, @verified, @confirmed, @remarks, @acctnum, @modby, @moddate)");

                        SqlCommand validator = new SqlCommand("select check_number from Cheque where check_number = @checknum", activeConnection);
                        validator.Parameters.AddWithValue("@checknum", heart[0]);
                        if (validator.ExecuteScalar() == null)
                        {
                            SqlCommand insert = new SqlCommand(query.ToString(), activeConnection);
                            SqlCommand checker = new SqlCommand("select minimum from Threshold", activeConnection);
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
                            insert.Parameters.AddWithValue("@remarks", heart[9]);
                            insert.Parameters.AddWithValue("@acctnum", heart[10]);
                            insert.Parameters.AddWithValue("@modby", userID);
                            insert.Parameters.AddWithValue("@moddate", DateTime.Now);
                            insert.ExecuteNonQuery();
                            lineCount++;
                        }
                        //else
                        //{
                        //    Message("Check Data Upload interrupted. A duplicate check number has been discovered.");

                        //}

                    }
                    sr.Close();
                }
                DataTable dt = FillDataTable();
                    string wew = ViewState["UploadImageClicked"].ToString();
                    string wew2 = ViewState["ImageCount"].ToString();
                    string wew3 = lineCount.ToString();
                    bool uploadClicked = bool.Parse(ViewState["UploadImageClicked"].ToString());
                    if (uploadClicked)
                    {

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "script", "CompareData();", true);
                    }

                    
            }
            catch (Exception b)
            {
                Response.Write(b);
                //Response.Write("Please re-check the format of the data for any missing fields.")
            }

        }

        public int ImageCount
        {
            get { return Convert.ToInt32(ViewState["ImageCount"].ToString()); }
            set { ViewState["ImageCount"] = value; }
        }

        protected void clearCheck_Click1(object sender, EventArgs e)
        {
            using (activeConnectionOpen())
            {
                cmd = new SqlCommand("DELETE FROM Cheque", activeConnection);
                cmd.ExecuteNonQuery();
                ViewAllCheck.DataBind();
            }
        }

        protected void ViewAllCheck_Sorting(object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            ViewAllCheck.DataSource = dt;
            ViewAllCheck.DataBind();
        }

        public DataTable FillDataTable()
        {
            query = new StringBuilder();
            query.Append("SELECT check_number, (f_name + ' ' + m_name + ' ' + l_name) AS customer_name, ch.account_number, check_date, amount, balance, branch_name, ");
            query.Append("drawee_bank, drawee_bank_branch, verification, funded, bank_remarks, ch.modified_by, ch.modified_date ");
            query.Append("FROM Cheque ch, Customer c, Account a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id ");
            query.Append("ORDER BY ch.check_number");

            activeConnectionOpen();
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query.ToString(), activeConnection);
            da.Fill(dt);
            ViewAllCheck.DataSource = dt;
            ViewAllCheck.DataBind();
            activeConnectionClose();
            return dt;
        }

        //Generate List
        protected void genListBtn_Click(object sender, EventArgs e)
        {
            query = new StringBuilder();
            query.Append("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, ");
            query.Append("drawee_bank, drawee_bank_branch, funded, verification, verify_remarks, ch.account_number ");
            query.Append("FROM Cheque ch, Customer c, Account a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id ");
            query.Append("AND verification = 'NO' ORDER BY ch.account_number");

            // Retrieves the schema of the table.
            dt = new DataTable();
            dt.Clear();
            dt = GetData(query.ToString());

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

        private DataTable GetData(String query)
        {
            activeConnectionOpen();
            da = new SqlDataAdapter(query, activeConnection);
            dt = new DataTable();          
            da.Fill(dt);
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
    }
}
