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

namespace OCS.DMS
{
    public partial class Archiving : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreatingSessionUsingAtomPub();
            //CreatingSessionUsingWebService();
        }

        private static void CreatingSessionUsingWebService()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.WebServices;
            parameters[DotCMIS.SessionParameter.WebServicesRepositoryService] = "https://cmis.alfresco.com:80/cmisws/RepositoryService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesAclService] = "https://cmis.alfresco.com:80/cmisws/ACLService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesDiscoveryService] = "https://cmis.alfresco.com:80/cmisws/DiscoveryService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesMultifilingService] = "https://cmis.alfresco.com:80/cmisws/MultiFilingService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesNavigationService] = "https://cmis.alfresco.com:80/cmisws/NavigationService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesObjectService] = "https://cmis.alfresco.com:80/cmisws/ObjectService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesPolicyService] = "https://cmis.alfresco.com:80/cmisws/PolicyService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesRelationshipService] = "https://cmis.alfresco.com:80/cmisws/RelationshipService?wsdl";
            parameters[DotCMIS.SessionParameter.WebServicesVersioningService] = "https://cmis.alfresco.com:80/cmisws/VersioningService?wsdl";
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "H2scs2014";

            SessionFactory factory = SessionFactory.NewInstance();
            ISession session = factory.GetRepositories(parameters)[0].CreateSession();
        }

        private static void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "H2scs2014";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
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

            #region Uploading of Image
            UploadADocument(session, imageToByteArray(System.Drawing.Image.FromFile(@"C:\Users\UserPC\Documents\Visual Studio 2010\Projects\OCS_BranchV2\OCS_BranchV2\bin\Debug\Check Images\03-02-2015\Check#03_GRAYSCALE200(Front).jpeg")));
            #endregion


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
        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private static void UploadADocument(ISession session, byte[] ImageFile)
        {
            
            IFolder folder = (IFolder)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"));

            Dictionary<String, object> DocumentProperties = new Dictionary<string, object>();
            DocumentProperties[PropertyIds.Name] = "Mytest.jpg";
            DocumentProperties[PropertyIds.ObjectTypeId] = "cmis:document";
            ContentStream contentStream = new ContentStream
            {
                MimeType = "image/jpeg",
                Length = ImageFile.Length,
                Stream = new MemoryStream(ImageFile)
            };

            folder.CreateDocument(DocumentProperties, contentStream, null);
   
        }


    }
}