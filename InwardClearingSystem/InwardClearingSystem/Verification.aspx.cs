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
        CmsConnect CmsConnect = new CmsConnect();
        DataTable dt;
        GridViewRow row;
        Int32 totalVerified = 0;
        RadioButton rb;
        SqlCommand cmd;
        SqlDataAdapter da;
        String function = "Signature Verification";
        String query;
        ISession session;

        protected void Page_Load(object sender, EventArgs e)
        {
            session = CmsConnect.CreateSession("admin", "092095", "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom");
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable();
                ViewState["SelectRow"] = -1;
            }
           
        }

        protected void RowSelect_CheckedChanged(object sender, EventArgs e)
        {
            try
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
                    row.BackColor = ColorTranslator.FromHtml("#FF7272");
                    row = VerifyView.Rows[i];

                    string im = row.Cells[3].Text;
                    string age = row.Cells[1].Text;
                    string image = im + "_" + age;
                    CmsConnect.ShowChequeImage(session, image, checkImage);
                    ShowSigDTImage(row.RowIndex, cmd, VerifyView, sigImage);
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void acceptButton_Click(object sender, EventArgs e)
        {
            try
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
            catch
            {
                Message("An error has occurred. Please try again.");
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

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserName"].ToString();
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "VerificationSearch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@username", user);
                    cmd.Parameters.AddWithValue("@num", txtSearch.Text);
                    dt = new DataTable();
                    da = new SqlDataAdapter(cmd);

                    da.Fill(dt);
                    VerifyView.DataSource = dt;
                    VerifyView.DataBind();
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void ViewAll_Click(object sender, EventArgs e)
        {
            FillDataTable();
            txtSearch.Text = "";
        }

        protected void VerifyView_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void VerifyView_Sorting(object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            VerifyView.DataSource = dt;
            VerifyView.DataBind();
        }

        /// <summary>
        /// Fills the DataTable with check data for Signature Verification. 
        /// This method is specific to Verification as the query requires a parameter.
        /// </summary>
        /// <returns>Filled DataTable</returns>
        public DataTable FillDataTable()
        {
            string user = Session["UserName"].ToString();
            using (cmd = new SqlCommand())
            {
                cmd.CommandText = "FillVerificationDataTable";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
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

        private DataTable GetData()
        {
            query = "FilterForVerificationGenerateList";
            activeConnectionOpen();
            da = new SqlDataAdapter(query.ToString(), activeConnection);
            dt = new DataTable();
            da.Fill(dt);
            activeConnectionClose();
            return dt;
        }

        /// <summary>
        /// Converts System.Drawing.Image to a byte array.
        /// </summary>
        /// <param name="imageIn">Image to be converted.</param>
        /// <returns>The image in the byte array type.</returns>
        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the check data with values with status regarding verification 
        /// with corresponding remarks.
        /// </summary>
        /// <param name="i">Row index that contains the check data to be updated.</param>
        /// <param name="verify">New verfication status of the check data.</param>
        /// <param name="remarks">Corresponding remarks to the change in verification status.</param>
        private void UpdateCheckData(int i, string verify, string remarks)
        {
            try
            {
                using (cmd = new SqlCommand(query, activeConnectionOpen()))
                {
                    cmd.CommandText = "UpdateCheckDataVerificationStatus";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@acctnumber", VerifyView.Rows[i].Cells[3].Text);
                    cmd.Parameters.AddWithValue("@chknumber", VerifyView.Rows[i].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@verify", verify);
                    cmd.Parameters.AddWithValue("@modby", Session["UserID"]);
                    cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@veremarks", remarks);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
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
