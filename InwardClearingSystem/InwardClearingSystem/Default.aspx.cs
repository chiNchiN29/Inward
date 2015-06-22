using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
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

    public partial class _Default : BasePage
    {
        SqlCommand cmd;
        StringBuilder query;
        CmsConnect cmsCon = new CmsConnect();
        ISession session;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            activeConnection.Close();
            session = cmsCon.CreateSession("admin", "admin", "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom"); 
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable("DefaultCheckDataTable", activeConnectionOpen(), ViewAllCheck);
            }
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

        protected void produceReport_Click(object sender, EventArgs e)
        {
            query = new StringBuilder();
            query.Append("SELECT * ");
            query.Append("FROM Cheque ");
            query.Append("WHERE (verification = 'NO' ");
            query.Append("OR confirmed = 'NO') ");
            query.Append("OR (confirm_remarks <> '&nbsp;' ");
            query.Append("AND confirm_remarks <> NULL ");
            query.Append("AND confirm_remarks <> '') ");
            query.Append("OR (verify_remarks <> '&nbsp;' ");
            query.Append("AND verify_remarks <> NULL");
            query.Append("AND verify_remarks <> '') ");
            DataTable dt = GetData(query.ToString());
            string attachment = "attachment; filename=trial.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");

            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SearchCheckNumber";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnectionOpen();
                        cmd.Parameters.AddWithValue("@num", txtSearch.Text);
                        da = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        da.Fill(dt);
                        ViewAllCheck.DataSource = dt;
                        ViewAllCheck.DataBind();
                    }
                }
            }
            catch
            {
                Message("An error has occurred");
            }
        }

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            FillDataTable("DefaultCheckDataTable", activeConnectionOpen(), ViewAllCheck);
            txtSearch.Text = "";
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
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
            Response.Charset = "";

            Response.ContentType = "application/vnd.xls";

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

        /// <summary>
        /// upload check data to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uploadCheckData_Click(object sender, EventArgs e)
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

                        using (SqlCommand validator = new SqlCommand("select check_number from Cheque where check_number = @checknum", activeConnection))
                        {
                            validator.Parameters.AddWithValue("@checknum", heart[0]);
                            if (validator.ExecuteScalar() == null)
                            {
                                using (SqlCommand insert = new SqlCommand())
                                {
                                    insert.CommandText = "InsertCheckData";
                                    insert.CommandType = CommandType.StoredProcedure;
                                    insert.Connection = activeConnection;

                                    using (SqlCommand checker = new SqlCommand())
                                    {
                                        checker.CommandText = "FilterBTA";
                                        checker.CommandType = CommandType.StoredProcedure;
                                        checker.Connection = activeConnection;

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
                                }
                            }
                            //else
                            //{
                            //    Message("Check Data Upload interrupted. A duplicate check number has been discovered.");

                            //}
                        }

                    }
                    sr.Close();
                }
                    FillDataTable("DefaultCheckDataTable", activeConnectionOpen(), ViewAllCheck);
                    ImgCount.Value = ViewState["ImageCount"].ToString();
                    DataCount.Value = lineCount.ToString();
                    bool uploadClicked = bool.Parse(ViewState["UploadImageClicked"].ToString());
                    if (uploadClicked)
                    {

                        ClientScript.RegisterStartupScript(this.GetType(), "script", "CompareData();", true);
                    }

                    
            }
            catch (Exception b)
            {
                Response.Write(b);
                //Response.Write("Please re-check the format of the data for any missing fields.")
            }

        }

        /// <summary>
        /// Clears check data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void clearCheck_Click(object sender, EventArgs e)
        {
            try
            {
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "ClearCheckData";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.ExecuteNonQuery();
                    ViewAllCheck.DataBind();
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void ViewAllCheck_Sorting(object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            ViewAllCheck.DataSource = dt;
            ViewAllCheck.DataBind();
        }

        protected void ViewAllCheck_PageIndex(object sender, GridViewPageEventArgs e)
        {
            ViewAllCheck.PageIndex = e.NewPageIndex;
            if (ViewState["SortExpression"] == null)
            {
                FillDataTable("DefaultCheckDataTable", activeConnectionOpen(), ViewAllCheck);
            }
            else
            {
                dt = ViewState["myDataTable"] as DataTable;
                dt.DefaultView.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
                ViewAllCheck.DataSource = dt;
                ViewAllCheck.DataBind();
            }
        }

        protected void ViewAllCheck_PageSizeChange(object sender, EventArgs e)
        {
            ViewAllCheck.PageSize = Convert.ToInt32(pgSize.SelectedValue);
            FillDataTable("DefaultCheckDataTable", activeConnectionOpen(), ViewAllCheck);
        }

        /// <summary>
        /// Fills up a DataTable with the results from the SQL query made to the database.
        /// </summary>
        /// <param name="query">SQL query to be made to the database.</param>
        /// <returns>Filled DataTable</returns>
        private DataTable GetData(String query)
        {
            activeConnectionOpen();
            using (da = new SqlDataAdapter(query, activeConnection))
            {
                dt = new DataTable();
                da.Fill(dt);
                activeConnectionClose();
                return dt;
            }
        }

        /// <summary>
        /// convert image to byte
        /// </summary>
        /// <param name="imageIn">
        /// Image to be converted
        /// </param>
        /// <returns>
        /// The image in byte[] formate
        /// </returns>
        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// uploads check image to alfresco
        /// </summary>
        /// <param name="session">
        /// CMIS session
        /// </param>
        /// <param name="ImageFile">
        /// The Image File to be uploaded
        /// </param>
        /// <param name="fileName">
        /// The filename of the file
        /// </param>
        private void UploadADocument(ISession session, byte[] ImageFile, string fileName)
        {
            try
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
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// gets image count uploaded in alfresco
        /// </summary>
        public int ImageCount
        {
            get { return Convert.ToInt32(ViewState["ImageCount"].ToString()); }
            set { ViewState["ImageCount"] = value; }
        }

       
    }
}
