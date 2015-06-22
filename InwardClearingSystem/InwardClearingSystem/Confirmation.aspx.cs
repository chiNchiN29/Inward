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

namespace InwardClearingSystem
{

    public partial class Confirmation : BasePage
    {
        CmsConnect CmsConnect = new CmsConnect();
        GridViewRow row;
        Int32 totalConfirmed = 0;
        RadioButton rb;
        SqlCommand cmd;
        String function = "Confirmation";
        String query;
        ISession session;
        CmsConnect cmsCon = new CmsConnect();

        protected void Page_Load(object sender, EventArgs e)
        {
            session = cmsCon.CreateSession("admin", "admin", "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom");
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }
            
            if (!IsPostBack)
            {
                ViewState["myDataTable"] = FillDataTable("FillConfirmationDataTable", activeConnectionOpen(), ConfirmView);
                ViewState["SelectRow"] = -1;
            }
            
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
                if (i == -1)
                {
                    Message("Please select a customer");
                }
                else if (String.IsNullOrWhiteSpace(confirmRemarks.Text))
                {
                    Message("Please input remarks");
                }
                else
                {
                    UpdateConfirmCheckData(i, "YES");
                    insertCheckLog(i, "Confirmation", "Successfully confirmed as: YES", ConfirmView);
                    FillDataTable("FillConfirmationDataTable", activeConnectionOpen(), ConfirmView);
                    NextRow(ConfirmView, i);
                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(ViewState["SelectRow"].ToString());
                if (i == -1)
                {
                    Message("Please select a customer");
                }
                else if (String.IsNullOrWhiteSpace(confirmRemarks.Text))
                {
                    Message("Please input remarks");
                }
                else
                {
                    UpdateConfirmCheckData(i, "NO");
                    insertCheckLog(i, "Confirmation", "Successfully confirmed as: NO", ConfirmView);
                    FillDataTable("FillConfirmationDataTable", activeConnectionOpen(), ConfirmView);
                    NextRow(ConfirmView, i);

                }
            }
            catch
            {
                Message("An error has occurred. Please try again");
            }
        }

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
            Response.ContentType = "application/text";

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

        protected void searchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "ConfirmationSearch";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnectionOpen();
                        cmd.Parameters.AddWithValue("@num", txtSearch.Text);
                        da = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        da.Fill(dt);
                        ConfirmView.DataSource = dt;
                        ConfirmView.DataBind();
                    }
                }
            }
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        protected void viewAllBtn_Click(object sender, EventArgs e)
        {
            FillDataTable("FillConfirmationDataTable", activeConnectionOpen(), ConfirmView);
        }

        protected void ConfirmView_Sorting(Object sender, GridViewSortEventArgs e)
        {
            dt = ViewState["myDataTable"] as DataTable;
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            ConfirmView.DataSource = dt;
            ConfirmView.DataBind();
        }

        protected void RowSelect_CheckedChanged(Object sender, EventArgs e)
        {
            try
            {
                int previousRow = Convert.ToInt32(ViewState["SelectRow"].ToString());
                if (previousRow != -1)
                {
                    row = ConfirmView.Rows[previousRow];
                    row.BackColor = Color.White;
                }

                rb = (RadioButton)sender;
                row = (GridViewRow)rb.NamingContainer;
                int i = row.RowIndex;
                ViewState["SelectRow"] = i;

                if (i != -1)
                {
                    row = ConfirmView.Rows[i];
                    row.BackColor = ColorTranslator.FromHtml("#FF7272");
                    row = ConfirmView.Rows[i];

                    string im = row.Cells[3].Text;
                    string age = row.Cells[1].Text;
                    string image = im + "_" + age;
                    CmsConnect.ShowChequeImage(session, image, checkImage);
                    ShowSigDTImage(row.RowIndex, cmd, ConfirmView, sigImage);
                }
            }
            catch
            {
                Message("An error has occurred. Please try again.");
            }
        }

        protected void ConfirmView_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            int total = ConfirmView.Rows.Count;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string confirmed = e.Row.Cells[11].Text;
                if (confirmed.Equals("YES"))
                {
                    e.Row.CssClass = "YesVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalConfirmed++;
                }
                if (confirmed.Equals("NO"))
                {
                    e.Row.CssClass = "NoVer";
                    rb = (RadioButton)e.Row.FindControl("RowSelect");
                    rb.Enabled = false;
                    totalConfirmed++;
                }
               
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[10].Text = "Confirmed: " + totalConfirmed.ToString();
                e.Row.Cells[11].Text = "Total: " + total.ToString();
                totalCon.Text = totalConfirmed.ToString();
                totalCount.Text = total.ToString();
                totalConHide.Value = totalConfirmed.ToString();
                totalCountHide.Value = total.ToString();
            }
        }

        private DataTable GetData()
        {
            query = "FilterForConfirmationGenerateList";
            activeConnectionOpen();
            da = new SqlDataAdapter(query, activeConnection);
            dt = new DataTable();
            da.Fill(dt);
            activeConnectionClose();
            return dt;
        }

        /// <summary>
        /// Updates check data with a new confirmation status and remarks.
        /// </summary>
        /// <param name="i">Row index indicating the check data to be updated.</param>
        /// <param name="confirm">Confirmation status the check data is updated with.</param>
        private void UpdateConfirmCheckData(int i, string confirm)
        {
            try
            {
                using (cmd = new SqlCommand())
                {
                    cmd.CommandText = "UpdateCheckDataConfirmationStatus";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = activeConnectionOpen();
                    cmd.Parameters.AddWithValue("@acctnumber", ConfirmView.Rows[i].Cells[3].Text);
                    cmd.Parameters.AddWithValue("@chknumber", ConfirmView.Rows[i].Cells[1].Text);
                    cmd.Parameters.AddWithValue("@fund", confirm);
                    cmd.Parameters.AddWithValue("@modby", Session["UserID"]);
                    cmd.Parameters.AddWithValue("@moddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@conremarks", confirmRemarks.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}