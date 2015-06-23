using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace InwardClearingSystem
{
    public partial class UpdateThreshold : BasePage
    {
        string function = "Update Threshold";
        SqlTransaction transact;
   
        protected void Page_Load(object sender, EventArgs e)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PH", false);
            if (checkAccess(Convert.ToInt32(Session["RoleID"]), function) == false)
            {
                if (this.Context != null)
                {
                    Response.Redirect("~/NoAccess.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                using (SqlCommand select = new SqlCommand())
                {
                    select.CommandText = "GetBypassValueStandard";
                    select.CommandType = CommandType.StoredProcedure;
                    select.Connection = activeConnection;
                    lblMin.Text = String.Format("{0:C}", select.ExecuteScalar());
                }
                using (SqlCommand select2 = new SqlCommand())
                {
                    select2.CommandText = "GetHighValueStandard";
                    select2.CommandType = CommandType.StoredProcedure;
                    select2.Connection = activeConnection;
                    lblMax.Text = String.Format("{0:C}", select2.ExecuteScalar());
                }
            }
     
        }

        protected void setThresholds_Click(object sender, EventArgs e)
        {
            try
            {
                transact = activeConnectionOpen().BeginTransaction("SetThreshold");
                if (txtBoxMin.Text != "" || txtBoxMax.Text != "")
                {
                    int num1;
                    if (txtBoxMin.Text != "")
                    {
                        bool inputIsNumber = int.TryParse(txtBoxMin.Text, out num1);
                        if (inputIsNumber == true && num1 >= 0)
                        {
                            using (SqlCommand updateMin = new SqlCommand())
                            {
                                updateMin.CommandText = "UpdateBypassValueStandard";
                                updateMin.CommandType = CommandType.StoredProcedure;
                                updateMin.Connection = activeConnection;
                                updateMin.Transaction = transact;
                                updateMin.Parameters.AddWithValue("@thresh", txtBoxMin.Text);
                                updateMin.ExecuteNonQuery();
                            }

                            string max = getMaxValueLbl();
                            
                            InsertThresholdHistory(txtBoxMin.Text, max, "Update Threshold", "Successful Threshold Update", "role_minimum (column updated)", activeConnection, transact);

                            using (SqlCommand select = new SqlCommand())
                            {
                                select.CommandText = "GetBypassValueStandard";
                                select.CommandType = CommandType.StoredProcedure;
                                select.Connection = activeConnection;
                                select.Transaction = transact;
                                lblMin.Text = String.Format("{0:C}", select.ExecuteScalar());
                            }
                        }
                        else
                        {
                            string min = getMinValueLbl();
                            string max = getMaxValueLbl();
                            InsertThresholdHistory(min, max, "Update Threshold", "Failed Threshold Update", "No change", activeConnection, transact);
                            Message("Minimum value input is invalid");
                        }
                    }

                    if (txtBoxMax.Text != "")
                    {
                        bool inputIsNumber = int.TryParse(txtBoxMax.Text, out num1);
                        if (inputIsNumber == true && num1 >= 0)
                        {
                            using (SqlCommand updateMax = new SqlCommand())
                            {
                                updateMax.CommandText = "UpdateHighValueStandard";
                                updateMax.CommandType = CommandType.StoredProcedure;
                                updateMax.Connection = activeConnection;
                                updateMax.Transaction = transact;
                                updateMax.Parameters.AddWithValue("@thresh", txtBoxMax.Text);
                                updateMax.ExecuteNonQuery();
                            }

                            string min = getMinValueLbl();
                            InsertThresholdHistory(min, txtBoxMax.Text, "Update Threshold", "Successful Threshold Update", "role_maximum (column updated)", activeConnection, transact);

                            using (SqlCommand select2 = new SqlCommand())
                            {
                                select2.CommandText = "GetHighValueStandard";
                                select2.CommandType = CommandType.StoredProcedure;
                                select2.Connection = activeConnection;
                                select2.Transaction = transact;
                                lblMax.Text = String.Format("{0:C}", select2.ExecuteScalar());
                            }
                        }
                        else
                        {
                            string min = getMinValueLbl();
                            string max = getMaxValueLbl();

                            InsertThresholdHistory(min, max, "Update Threshold", "Failed Threshold Update", "No change", activeConnection, transact);
                            Message("High Value input is invalid");
                        }
                        transact.Commit();
                    }
                }
            }
            catch
            {
                transact.Rollback();
                string min = getMinValueLbl();
                string max = getMaxValueLbl();
                InsertThresholdHistory(min, max, "Update Threshold", "Failed Threshold Update", "No change", activeConnection, transact);
                Message("An error has occurred. Please try again");
            }
            finally
            {
                activeConnectionClose();
            }
        }

        /// <summary>
        /// Gets Maximum Value Label
        /// </summary>
        /// <returns></returns>
        private string getMaxValueLbl()
        {
            try
            {
                string max = lblMax.Text;
                max = max.Replace("Php", String.Empty);
                max = max.Replace(",", String.Empty);
                return max;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets Minimum Value Label
        /// </summary>
        /// <returns></returns>
        private string getMinValueLbl()
        {
            try
            {
                string min = lblMin.Text;
                min = min.Replace("Php", String.Empty);
                min = min.Replace(",", String.Empty);
                return min;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts Threshold history
        /// </summary>
        /// <param name="min"> minimum value</param>
        /// <param name="max">maximum value</param>
        /// <param name="tag">history tag</param>
        /// <param name="message">history message</param>
        /// <param name="changes">changes made</param>
        /// <param name="con">sql connection</param>
        /// <param name="trans">sql transaction</param>
        private void InsertThresholdHistory(string min, string max, string tag, string message, string changes, SqlConnection con, SqlTransaction trans)
        {
            try
            {
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                StringBuilder query = new StringBuilder();
                query.Append("Insert into Threshold_History (minimum, maximum, modified_date, modified_by, ");
                query.Append("terminal, history_tag, changes, history_message) ");
                query.Append("values (@min, @max, @date, @id, @ip, @tag, @changes, @message)");
                SqlCommand cmd = new SqlCommand(query.ToString(), con, trans);
                cmd.Parameters.AddWithValue("@min", min);
                cmd.Parameters.AddWithValue("@max", max);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["UserID"]));
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@tag", tag);
                cmd.Parameters.AddWithValue("@changes", changes);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}