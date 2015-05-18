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
using System.Web.Security;
using System.Text;

namespace WebApplication1
{
    public partial class Verification : System.Web.UI.Page
    {

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        SqlDataAdapter da;
        SqlCommand cmd;
        GridViewRow row;
        DataTable dt;

        int totalVerified = 0;
    
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
            //    SqlCommand checker = new SqlCommand("SELECT role_name FROM END_USER, ROLE WHERE username = '" + Membership.GetUser().UserName + "' AND END_USER.role_id = ROLE.role_id", connection);
            //    if(checker.ExecuteScalar().ToString() != "CLEARING DEPT" && checker.ExecuteScalar().ToString() != "OVERSEER")
            //    {
            //        string script = "alert(\"You are not authorized to view this page!\");location ='/Default.aspx';";
            //        ScriptManager.RegisterStartupScript(this, GetType(),
            //                              "alertMessage", script, true);
            //    }
            //    else
            //    {
            //string afterrow = ViewState["SelectRow"] as string;
            //if (afterrow != null)
            //{
            //    int wew = int.Parse(afterrow);
            //    GridViewRow row = GridView1.Rows[wew];
            //    RadioButton rb = (RadioButton)row.FindControl("RowSelect");
            //    rb.Checked = true;
            //}
                    Page.MaintainScrollPositionOnPostBack = true;
                    CreatingSessionUsingAtomPub();

                    if (!Page.IsPostBack)
                    {
                        ViewState["myDataTable"] = FillDataTable();
                        
                    }

                //}
                connection.Close();
            //}
        }

        private void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            //parameters[DotCMIS.SessionParameter.Password] = "092095";
            parameters[DotCMIS.SessionParameter.Password] = "admin";
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
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                SessionFactory factory = SessionFactory.NewInstance();
                ISession session;
                parameters[DotCMIS.SessionParameter.User] = "admin";
                //parameters[DotCMIS.SessionParameter.Password] = "092095";
                parameters[DotCMIS.SessionParameter.Password] = "admin";
                //parameters[DotCMIS.SessionParameter.Password] = "H2scs2015";
                parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
                parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
                //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
                session = factory.GetRepositories(parameters)[0].CreateSession();

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
                connection.Open();
                cmd = new SqlCommand("select signature_image from SIGNATURE WHERE account_number= @acctnumber", connection);
                cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[rowIndex].Cells[3].Text);


                byte[] result = cmd.ExecuteScalar() as byte[];
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
           int i = GetRowIndex();

           //if (i != -1)
           //{
           //    if (Image1.ImageUrl == "~/Resources/H2DefaultImage.jpg" || Image2.ImageUrl == "~/Resources/H2DefaultImage.jpg")
           //    {
           //       ErrorMessage("Cannot validate because there is no existing check or signature");
           //    }
           //    else
           //    {
                   connection.Open();
                   SqlCommand cmd = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
                   cmd.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[3].Text);
                   cmd.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                   cmd.Parameters.AddWithValue("@verify", "YES");
                   cmd.ExecuteNonQuery();
                   connection.Close();

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
                    connection.Open();
                    SqlCommand update = new SqlCommand("update CHEQUE SET verification = @verify WHERE account_number = @acctnumber AND check_number = @chknumber", connection);

          
                    update.Parameters.AddWithValue("@acctnumber", GridView1.Rows[i].Cells[3].Text);
                    update.Parameters.AddWithValue("@chknumber", GridView1.Rows[i].Cells[1].Text);
                    update.Parameters.AddWithValue("@verify", "NO");
                    update.ExecuteNonQuery();
                    connection.Close();

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

            connection.Open();
            SqlCommand cmd = new SqlCommand("insert into SIGNATURE(signature_image, account_number) values (@Sig, @ID)", connection);
            cmd.Parameters.AddWithValue("@Sig", imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
            cmd.Parameters.AddWithValue("@ID", TextBox1.Text);
            cmd.ExecuteNonQuery();
            connection.Close();

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

            using (connection)
            {
                using (cmd = new SqlCommand("SELECT check_number, amount, CONVERT(VARCHAR(10), check_date, 101), branch_name, drawee_bank, drawee_bank_branch, funded, verification, confirmed, CHEQUE.account_number FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.customer_id = CUSTOMER.customer_id AND verification = 'NO' ORDER BY CHEQUE.account_number"))
                {
                    using (da = new SqlDataAdapter())
                    {
                        cmd.Connection = connection;
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

        protected void GridView1_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        public DataTable FillDataTable()
        {

            string user = Membership.GetUser(User.Identity.Name).ToString();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT check_number, customer_name, CHEQUE.account_number, check_date, amount, balance, BRANCH.branch_name, drawee_bank, drawee_bank_branch, verification ");
            query.Append("FROM CHEQUE, CUSTOMER, ACCOUNT, THRESHOLD, BRANCH, END_USER ");
            query.Append("WHERE END_USER.username = @username AND END_USER.user_id = BRANCH.user_id AND BRANCH.branch_name = CHEQUE.branch_name AND CHEQUE.account_number = ACCOUNT.account_number ");
            query.Append("AND ACCOUNT.customer_id = CUSTOMER.customer_id AND CHEQUE.amount >= minimum AND verification <> 'BTA' ");
            query.Append("ORDER BY CHEQUE.account_number");
            cmd = new SqlCommand(query.ToString(), connection);
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
    }
}
