using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotCMIS.Client.Impl;
using DotCMIS.Client;
using DotCMIS;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class Confirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreatingSessionUsingAtomPub();   
            SqlDataSource1.SelectCommand = "SELECT check_number AS CheckNo, customer_name AS Name, customer_address AS Adress, contact_number AS ContactNo, CHEQUE.account_number AS AcctNo, check_date AS Date, amount, drawee_bank AS DraweeBank, drawee_bank_branch AS DraweeBankBranch, funded, verification AS Verified FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CUSTOMER.account_number ORDER BY CHEQUE.account_number";
            GridView1.DataSource = SqlDataSource1;
            GridView1.DataBind();
        }

        private void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            //parameters[DotCMIS.SessionParameter.Password] = "092095";
            parameters[DotCMIS.SessionParameter.Password] = "admin";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();

            #region Getting the Root Folder and it Children Folders
            ///// get the root folder
            //IFolder rootFolder = session.GetRootFolder();

            //String FolderName = string.Empty;
            //// list all children
            //foreach (ICmisObject cmisObject in rootFolder.GetChildren())
            //{
            //    FolderName = cmisObject.Name.ToString();
            //    //Console.WriteLine(cmisObject.Name);
            //} 
            #endregion

            #region Checking and Creation of Target Folders
            //Checking and Creating of Main Folder from the Root Folder
            if (!isFolderExist(session, "/Uploads/"))
            {
                CreateAFolder(session, "Uploads");
            }

            //Creating a SubFolder for current Year
            if (!isFolderExist(session, "/Uploads/" + DateTime.Now.Year.ToString()))
            {
                CreateASubFolder(session, DateTime.Now.Year.ToString(), "/Uploads/");
            }

            //Creating a SubFolder for current Month
            if (!isFolderExist(session, "/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM")))
            {
                CreateASubFolder(session, DateTime.Now.ToString("MM"), "/Uploads/" + DateTime.Now.Year.ToString());
            }

            //Creating a SubFolder for current Day
            if (!isFolderExist(session, "/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd")))
            {
                CreateASubFolder(session, DateTime.Now.ToString("dd"), "/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM"));
            }
            #endregion

            //#region Uploading of Image
            //UploadADocument(session, imageToByteArray(System.Drawing.Image.FromFile(@"C:\Users\Kathrine Chua\Pictures\sad\2.jpeg")));
            //#endregion

            #region GET A PAGE
            //// get a page
            //IItemEnumerable<ICmisObject> children = rootFolder.GetChildren();
            //IItemEnumerable<ICmisObject> page = children.SkipTo(20).GetPage(10); // children 20 to 30

            //foreach (ICmisObject cmisObject in page)
            //{
            //    Console.WriteLine(cmisObject.Name);
            //} 
            #endregion

            #region Diff. AtomPUbUrl according to Version
            //try
            //{
            //    parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/service/api/cmis"; //THROWS "NOT FOUND"
            //    
            //    //session = factory.CreateSession(parameters);//THROWS :Repository Id is not set!"
            //}
            //catch
            //{
            //    parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/cmisatom"; //THROWS "Unauthorized" 
            //    session = factory.GetRepositories(parameters)[0].CreateSession();
            //    //session = factory.CreateSession(parameters);//THROWS: Repository Id is not set!"
            //}
            //finally
            //{
            //    try
            //    {
            //        parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom"; //THROWS "Unauthorized"
            //        session = factory.GetRepositories(parameters)[0].CreateSession();
            //        //session = factory.CreateSession(parameters);
            //    }
            //    catch
            //    {
            //        
            //        session = factory.GetRepositories(parameters)[0].CreateSession();
            //        //session = factory.CreateSession(parameters);
            //    }
            //} 
            #endregion
        }

        private static Boolean isFolderExist(ISession session, String FolderPath)
        {
            IFolder folder;
            try
            {
                folder = (IFolder)session.GetObjectByPath(FolderPath);
                return true;
            }
            catch (DotCMIS.Exceptions.CmisObjectNotFoundException)
            {
                return false;
            }
        }

        private static void CreateAFolder(ISession session, String FolderName)
        {
            Dictionary<String, object> FolderProperties = new Dictionary<string, object>();
            FolderProperties[DotCMIS.PropertyIds.Name] = FolderName;
            FolderProperties[DotCMIS.PropertyIds.ObjectTypeId] = "cmis:folder";
            session.GetRootFolder().CreateFolder(FolderProperties);
        }

        private static void CreateASubFolder(ISession session, String FolderName, String ParentFolderPath)
        {
            Dictionary<String, object> FolderProperties = new Dictionary<string, object>();
            FolderProperties[DotCMIS.PropertyIds.Name] = FolderName;
            FolderProperties[DotCMIS.PropertyIds.ObjectTypeId] = "cmis:folder";
            IFolder newFolder = (IFolder)session.GetObjectByPath(ParentFolderPath);
            newFolder.CreateFolder(FolderProperties);
        }

        protected void fundButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET funded = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@fund", "YES");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if (GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }

        protected void unfundButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("update CHEQUE SET funded = @fund WHERE account_number = @acctnumber AND check_number = @chknumber", connection);
            update.Parameters.AddWithValue("@acctnumber", GridView1.SelectedRow.Cells[5].Text);
            update.Parameters.AddWithValue("@chknumber", GridView1.SelectedRow.Cells[1].Text);
            update.Parameters.AddWithValue("@fund", "NO");
            update.ExecuteNonQuery();
            connection.Close();
            GridView1.DataBind();
            if (GridView1.SelectedRow.RowIndex < GridView1.Rows.Count - 1)
            {
                GridView1.SelectRow(GridView1.SelectedRow.RowIndex + 1);
            }
        }
    }


}