using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
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
    public partial class Verification : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (login == false)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                connection.Open();
                SqlCommand checker = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Membership.GetUser().UserName + "' AND END_USER.role_id = ROLE.role_id", connection);
                if (checker.ExecuteScalar().ToString() != "CLEARING DEPT")
                {
                    string script = "alert(\"You are not authorized to view this page!\");location ='/Default.aspx';";
                    ScriptManager.RegisterStartupScript(this, GetType(),
                                          "alertMessage", script, true);
                }
                else
                {
                    Page.MaintainScrollPositionOnPostBack = true;
                    SqlDataSource1.SelectCommand = "SELECT check_number AS 'Check Number', customer_name AS Name, CHEQUE.account_number AS 'Account Number', CONVERT(VARCHAR(10), check_date, 101) AS Date , amount AS Amount, balance as Balance, branch_name AS 'Branch Name', drawee_bank AS 'Drawee Bank', drawee_bank_branch AS 'Drawee Bank Branch', verification AS 'Verified?' FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND CHEQUE.amount >= minimum AND verification <> 'BTA' ORDER BY CHEQUE.account_number";
                    GridView1.DataSource = SqlDataSource1;
                    GridView1.DataBind();
                    CreatingSessionUsingAtomPub();
                    if (!Page.IsPostBack)
                    {
                        GridView1.DataBind();
                    }
                    int counter = 0;
                    if (!ReturnValue())
                    {
                        foreach (GridViewRow a in GridView1.Rows)
                        {
                            string karyuu = a.Cells[10].Text;
                            if (a.Cells[10].Text != "&nbsp;" && a.Cells[10].Text != null)
                            {
                                counter += 1;
                            }
                        }
                        //if (counter < GridView1.Rows.Count)
                        //{
                            ClientScriptManager CSM = Page.ClientScript;
                            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", "<script language=\"JavaScript\">window.onbeforeunload = confirmExit;function confirmExit(){return \"A total of " + counter + "/" + GridView1.Rows.Count + " has been verified.  Are you sure you want to exit this page?\";}</script>", false);
                        //}
                    }
                }
            }
        }

        private void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
            //parameters[DotCMIS.SessionParameter.Password] = "admin";
            //parameters[DotCMIS.SessionParameter.Password] = "H2scs2015";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
            //parameters[DotCMIS.SessionParameter.Password] = "admin";
            //parameters[DotCMIS.SessionParameter.Password] = "H2scs2015";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();
            string im = GridView1.SelectedRow.Cells[3].Text;
            string age = GridView1.SelectedRow.Cells[1].Text;
            string image = im + "_" + age;
            ShowChequeImage(session, image);
            ShowSigDTImage();     
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
            catch (Exception e)
            {
                Image1.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                Image1.Visible = true;
            }

        }

        //Signature image in Database
        private void ShowSigDTImage()
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                connection.Open();
                SqlCommand select = new SqlCommand("select signature_image from SIGNATURE WHERE account_number=@acctnumber", connection);
                select.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[3].Text);

                byte[] result = select.ExecuteScalar() as byte[];
                string base64string2 = Convert.ToBase64String(result, 0, result.Length);
                Image2.ImageUrl = "data:image/jpeg;base64," + base64string2;
                Image2.Visible = true;
                connection.Close();
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
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[3].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@verify", "YES");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if (GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }

        protected void rejectButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[3].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@verify", "NO");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if(GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }

        //insert signatures in database
        protected void insertSig_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand insert = new SqlCommand("insert into SIGNATURE(signature_image, account_number) values (@Sig, @ID)", connection);
            insert.Parameters.AddWithValue("@Sig", imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
            insert.Parameters.AddWithValue("@ID", TextBox1.Text);
            insert.ExecuteNonQuery();
            connection.Close();
        }

            // your code
        bool ReturnValue()
        {
            return false;
        }  

        //Generate List
        protected void Button1_Click(object sender, EventArgs e)
        {
            // Retrieves the schema of the table.
            DataTable dtSchema = new DataTable();
            dtSchema.Clear();
            dtSchema = GetData();

            // set the resulting file attachment name to the name of the report...
            string fileName = "test";

            //Response.Write(dtSchema.Rows.Count);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            foreach (DataRow datar in dtSchema.Rows)
            {
                for (int i = 0; i < dtSchema.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(datar[i]))
                    {
                        sb.Append(datar[i].ToString());
                    }
                    if (i < dtSchema.Columns.Count - 1)
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
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT check_number AS CheckNo, amount AS Amount, CONVERT(VARCHAR(10), check_date, 111) AS Date, branch_name AS 'Branch Name', drawee_bank AS 'Drawee Bank', drawee_bank_branch AS 'Drawee Bank Branch' , funded AS 'Funded?', verification AS 'Verified?', confirmed AS 'Confirmed?', CHEQUE.account_number AS AcctNo FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CUSTOMER.account_number AND verification = 'NO' ORDER BY CHEQUE.account_number"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
    }
}
