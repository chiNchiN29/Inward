using DotCMIS;
using DotCMIS.Client;
using DotCMIS.Client.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DotCMIS.Data;

namespace InwardClearingSystem
{
    public class BasePage : System.Web.UI.Page
    {
        public SqlConnection activeConnection = new SqlConnection();
        public ISession session;
      
  
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
       
            bool login = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (login == false)
                Response.Redirect("~/Account/Login.aspx");
            Session["UserName"] = System.Web.HttpContext.Current.User.Identity.Name;
            using (SqlCommand cmd = new SqlCommand("select user_id, role_id from [User] where username = @username", activeConnectionOpen()))
            {
                cmd.CommandText = "GetCurrentUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@username", Session["UserName"].ToString());
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Session["UserID"] = Convert.ToInt32(dr["user_id"]);
                    Session["RoleID"] = Convert.ToInt32(dr["role_id"]);
                }
            }
             
            session = CreateSession("admin", "admin", "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom");
        }

        public void Message(string message)
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

        public ISession CreateSession(string username, string password, string url)
        {
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters[DotCMIS.SessionParameter.User] = username;
                parameters[DotCMIS.SessionParameter.Password] = password;
                parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
                parameters[DotCMIS.SessionParameter.AtomPubUrl] = url;
                //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
                SessionFactory factory = SessionFactory.NewInstance();

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
            catch
            {
                Response.Redirect("FailurePage.aspx");
            }
          
             return session;
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

        public void NextRow(GridView view, int i)
        {
            if (i < view.Rows.Count - 1)
            {
                int nextRow = i + 1;
                GridViewRow row = view.Rows[nextRow];
                RadioButton rb = (RadioButton)row.FindControl("RowSelect");
                rb.InputAttributes["checked"] = "true";
                row.BackColor = ColorTranslator.FromHtml("#FF7272");
            }
        }

        public SqlConnection activeConnectionOpen()
        {
            activeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                if (activeConnection.State == ConnectionState.Closed)
                {
                    activeConnection.Open();
                }
                return activeConnection;
            }
            catch
            {
                Response.Redirect("~/FailurePage.aspx");
                return null;
            }
        }

        public SqlConnection activeConnectionClose()
        {
            activeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            try
            {
                if (activeConnection.State == ConnectionState.Open)
                {
                    activeConnection.Close();
                }
                return activeConnection;
            }
            catch
            {
                Response.Redirect("~/FailurePage.aspx");
                return null;
            }
        }

        public void ShowChequeImage(ISession session, string fileName, System.Web.UI.WebControls.Image image)
        {
            try
            {
                IDocument doc = (IDocument)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/" + fileName);

                IContentStream contentStream = doc.GetContentStream();
                byte[] result;
                using (var streamReader = new MemoryStream())
                {
                    contentStream.Stream.CopyTo(streamReader);
                    result = streamReader.ToArray();
                }
                string base64string = Convert.ToBase64String(result, 0, result.Length);

                image.ImageUrl = "data:image/jpeg;base64," + base64string;
                image.Visible = true;
            }
            catch
            {
                image.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                image.Visible = true;
            }

        }

        //Signature image in Alfresco
        public void ShowSigImage(ISession session, string fileName, System.Web.UI.WebControls.Image image)
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
            image.ImageUrl = "data:image/jpeg;base64," + base64string2;
            image.Visible = true;
        }

        //Signature image in Database
        public void ShowSigDTImage(int rowIndex, SqlCommand cmd, GridView view, System.Web.UI.WebControls.Image image)
        {
            using (activeConnectionOpen())
            {
                try
                {
                    using (cmd = new SqlCommand())
                    {
                        cmd.CommandText = "FetchSignatureImage";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = activeConnection;
                        cmd.Parameters.AddWithValue("@acctnumber", view.Rows[rowIndex].Cells[3].Text);

                        byte[] result = cmd.ExecuteScalar() as byte[];
                        string base64string2 = Convert.ToBase64String(result, 0, result.Length);
                        image.ImageUrl = "data:image/jpeg;base64," + base64string2;
                        image.Visible = true;
                    }
                }
                catch
                {
                    image.ImageUrl = "~/Resources/H2DefaultImage.jpg";
                    image.Visible = true;
                }
            }
        }

        public void insertCheckLog(int i, string action, string remarks, GridView view)
        {
            try
            {
                string user = Session["UserName"].ToString();
				using (SqlCommand cmd = new SqlCommand())
				{
					cmd.CommandText = "InsertCheckLog";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Connection = activeConnectionOpen();
					cmd.Parameters.AddWithValue("@username", user);
					cmd.Parameters.AddWithValue("@date_logged", DateTime.Now);
					cmd.Parameters.AddWithValue("@action", action);
					cmd.Parameters.AddWithValue("@remarks", remarks);
					cmd.Parameters.AddWithValue("@chknum", view.Rows[i].Cells[1].Text);
					cmd.Parameters.AddWithValue("@acctnum", view.Rows[i].Cells[3].Text);
					cmd.ExecuteNonQuery();
				}
            }
            catch
            {
                throw;
            }
        }

        public int checkAccess(int roleID, string function)
        {
            SqlCommand cmd;
            try
            {
                //get function ID
                cmd = new SqlCommand();
                cmd.CommandText = "SearchFunctionID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@fname", function);
                string functionID = cmd.ExecuteScalar().ToString();

                cmd = new SqlCommand();
                cmd.CommandText = "CrossCheckAccess";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = activeConnectionOpen();
                cmd.Parameters.AddWithValue("@roleID", roleID);
                cmd.Parameters.AddWithValue("@functionID", functionID);
                if (cmd.ExecuteScalar() == null)
                {
                    return 1;
                }
                return 0;
            }
            catch
            {
                throw;
            }
        }

        public bool checkForDuplicateData(string columnname, string tablename, string compare)
        {
            StringBuilder query;
            SqlCommand cmd;
            try
            {
                query = new StringBuilder();
                query.Append("SELECT " + columnname + " ");
                query.Append("FROM " + tablename + " ");
                query.Append("WHERE " + columnname + " =  @comparison");
                cmd = new SqlCommand(query.ToString(), activeConnectionOpen());
                cmd.Parameters.AddWithValue("@comparison", compare);
                if (cmd.ExecuteScalar() == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}