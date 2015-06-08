using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InwardClearingSystem
{
    public partial class AddRole : System.Web.UI.Page
    {
        string roleID;

        protected void Page_Load(object sender, EventArgs e)
        {
            roleID = Request.QueryString["Role"];

            if (roleID == null)
            {
                lblHeader.Text = "ADD ROLE";
                saveBtn.Visible = false;
                delBtn.Visible = false;
            }
            else
            {
                lblHeader.Text = "EDIT ROLE";
                addBtn.Visible = false;
            }
        }

        protected void cancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RoleMaintenance.aspx");
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {

        }

        protected void delBtn_Click(object sender, EventArgs e)
        {

        }

        protected void addBtn_Click(object sender, EventArgs e)
        {

        }
    }
}