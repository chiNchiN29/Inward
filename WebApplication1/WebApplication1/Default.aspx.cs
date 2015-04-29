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

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            connection.Open();
            CreatingSessionUsingAtomPub();
            SqlDataSource1.SelectCommand = "SELECT check_number AS CheckNo, customer_name AS Name, CHEQUE.account_number AS AcctNo, check_date AS Date, amount, balance, drawee_bank AS DraweeBank, drawee_bank_branch AS DraweeBankBranch, verification AS 'Verified?', funded AS 'Funded?', confirmed AS 'Confirmed?' FROM CHEQUE, CUSTOMER, ACCOUNT WHERE CHEQUE.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CUSTOMER.account_number ORDER BY CHEQUE.account_number";
            GridView1.DataSource = SqlDataSource1;
            GridView1.DataBind();
            connection.Close();
        }

        private void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
            ////parameters[DotCMIS.SessionParameter.Password] = "admin";
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
            /*if (!isFolderExist(session, "/Uploads/"))
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
            }*/
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

        private void UploadADocument(ISession session, byte[] ImageFile)
        {
            IFolder folder = (IFolder)session.GetObjectByPath("/Uploads/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd"));
            Dictionary<String, object> DocumentProperties = new Dictionary<string, object>();
            string id = PropertyIds.ObjectTypeId;
            DocumentProperties[PropertyIds.Name] = FileUpload1.FileName;
            DocumentProperties[PropertyIds.ObjectTypeId] = "cmis:document";
            ContentStream contentStream = new ContentStream
            {
                MimeType = "image/jpeg",
                Length = ImageFile.Length,
                Stream = new MemoryStream(ImageFile)
            };

            folder.CreateDocument(DocumentProperties, contentStream, null);
        }

        protected void uploadDoc_Click(Object sender, EventArgs e)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
            //parameters[DotCMIS.SessionParameter.Password] = "admin";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();
            UploadADocument(session, imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
        }

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        protected void uploadDoc0_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            //string filepath = FileUpload2.PostedFile.FileName;
            StreamReader sr = new StreamReader(FileUpload2.PostedFile.InputStream);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] heart = line.Split(',');
                //string wew = heart[0].ToString();
                //string wew2 = heart[1].ToString();
                //Response.Write(heart[0].ToString());
                
                //int x = 0;
                //string y = "";
                //while (x < heart.Length)
                //{
                //    y = y + heart[x];
                //    x++;
                //}
                //Label3.Text = y;
                
                SqlCommand insert = new SqlCommand("insert into CHEQUE(check_number, amount, check_date, drawee_bank, drawee_bank_branch, funded, verification, account_number, confirmed) values (@checknum, @amount, @date, @bank, @branch, @funded, @verified, @acctnum, @confirmed)", con);
                insert.Parameters.AddWithValue("@checknum", heart[0]);
                insert.Parameters.AddWithValue("@amount", heart[1]);
                insert.Parameters.AddWithValue("@date", heart[2]);
                insert.Parameters.AddWithValue("@bank", heart[3]);
                insert.Parameters.AddWithValue("@branch", heart[4]);
                insert.Parameters.AddWithValue("@funded", heart[5]);
                insert.Parameters.AddWithValue("@verified", heart[6]);
                insert.Parameters.AddWithValue("@acctnum", heart[7]);
                insert.Parameters.AddWithValue("@confirmed", heart[8]);
                insert.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}
