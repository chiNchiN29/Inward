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
        
        protected void Page_Load(object sender, EventArgs e)
        {
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
            catch
            {
                Response.Write("<script langauge=\"javascript\">alert(\"An image already exists with the same name\");</script>");
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
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] heart = line.Split(',');
                    string userID = Session["UserID"].ToString();
                    query = new StringBuilder();
                    query.Append("insert into CHEQUE(check_number, amount, check_date, branch_name, drawee_bank, drawee_bank_branch, funded, ");
                    query.Append("verification, confirmed, bank_remarks, account_number, modified_by, modified_date) ");
                    query.Append("values (@checknum, @amount, @date, @branch, @draweebank, @draweebranch, ");
                    query.Append("@funded, @verified, @confirmed, @remarks, @acctnum, @modby, @moddate)");

                    using (SqlCommand validator = new SqlCommand("select check_number from CHEQUE where check_number = @checknum", activeConnection))
                    {
                        validator.Parameters.AddWithValue("@checknum", heart[0]);
                        if (validator.ExecuteScalar() == null)
                        {
                            using (SqlCommand insert = new SqlCommand(query.ToString(), activeConnection))
                            {
                                using (SqlCommand checker = new SqlCommand("select minimum from THRESHOLD", activeConnection))
                                {
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
                                }
                            }
                        }
                        else
                        {


                            Response.Write("Check Data Upload interrupted. A duplicate check number has been discovered.");
                        }
                    }
                } 
                sr.Close();
                DataTable dt = FillDataTable();
                ViewAllCheck.DataSource = dt;
                ViewAllCheck.DataBind();
            }
            catch (Exception b)
            {
                Response.Write(b);
                //Response.Write("Please re-check the format of the data for any missing fields.");
              
            }

             activeConnection.Close();

        }

        protected void clearCheck_Click1(object sender, EventArgs e)
        {
             using (SqlCommand delete = new SqlCommand("DELETE FROM [CHEQUE]", activeConnection))
            {
                delete.ExecuteNonQuery();

                ViewAllCheck.DataBind();
                activeConnection.Close();
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
            query.Append("FROM CHEQUE ch, CUSTOMER c, ACCOUNT a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id ");
            query.Append("ORDER BY ch.check_number");
            
            dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query.ToString(), activeConnection);
            da.Fill(dt);
            ViewAllCheck.DataSource = dt;
            ViewAllCheck.DataBind();
     
           
            return dt;
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
            query.Append("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, ");
            query.Append("drawee_bank, drawee_bank_branch, funded, verification, confirmed, ch.account_number ");
            query.Append("FROM CHEQUE ch, CUSTOMER c, ACCOUNT a ");
            query.Append("WHERE ch.account_number = a.account_number AND a.customer_id = c.customer_id ");
            query.Append("AND verification = 'NO' ORDER BY ch.account_number");
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
