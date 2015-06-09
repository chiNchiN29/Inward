using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotCMIS;
using DotCMIS.Client;
using DotCMIS.Client.Impl;
using DotCMIS.Data;
using System.IO;

namespace InwardClearingSystem
{
    public class CmsConnect : System.Web.UI.Page
    {
        public ISession session;

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
    }
}