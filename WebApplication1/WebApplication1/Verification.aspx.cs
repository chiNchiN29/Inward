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
    public partial class Verification : System.Web.UI.Page
    {
      

        int x = 1;
        int i = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            CreatingSessionUsingAtomPub();
            if (!Page.IsPostBack)
            {
                Session["X"] = x;
                Session["Y"] = i;

                SqlDataSource1.SelectCommand = "SELECT customer_id, customer_name, CUSTOMER.account_number, balance, funded, verification FROM CUSTOMER, ACCOUNT, CHEQUE WHERE CUSTOMER.account_number = ACCOUNT.account_number AND ACCOUNT.account_number = CHEQUE.account_number "; 
                GridView1.DataSource = SqlDataSource1;
                GridView1.DataBind();
            }
        }

        private void CreatingSessionUsingAtomPub()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
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

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
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
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();
                UploadADocument(session, imageToByteArray(System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream)));
        }

        private void LoadDocument()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            SessionFactory factory = SessionFactory.NewInstance();
            ISession session;
            parameters[DotCMIS.SessionParameter.User] = "admin";
            parameters[DotCMIS.SessionParameter.Password] = "092095";
            parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            //parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://192.168.0.133:8080/alfresco/api/-default-/public/cmis/versions/1.0/atom";
            session = factory.GetRepositories(parameters)[0].CreateSession();

            ShowChequeImage(session, "conceal.jpg");
            
            /*
            int valueFromSession = Convert.ToInt32(Session["X"]);
            int sigval = Convert.ToInt32(Session["Y"]);

            var label = (Label)FindControlRecursive(this.Master, "Label" + valueFromSession);
            //var label = (Label)Page.FindControl("Label" + valueFromSession);
            //Response.Write(valueFromSession);
            //Response.Write(sigval.ToString());

            if (valueFromSession < 11)
            {
                if (label.Text[0].ToString().Equals(sigval.ToString()))
                {
                    ShowChequeImage(session, label.Text);
                    ShowSigImage(session, sigval.ToString());
                    valueFromSession++;
                    Session["X"] = valueFromSession;
                }
                else
                {
                    sigval++;
                    Session["Y"] = sigval;
                    LoadDocument();
                }
            }
            */
        }

        private void ShowChequeImage(ISession session, string fileName)
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

            Image1.ImageUrl = "data:image/jpeg;base64," + base64string;
            Image1.Visible = true;
        }

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

        protected void loadDoc_Click(Object sender, EventArgs e)
        {
            LoadDocument();
            int valueFromSession = Convert.ToInt32(Session["X"]);

            Session["X"] = valueFromSession;
            //string str = "Kill_me_now";
            //char[] split = new char[] { '_' };
            //string first = str.Split(split)[0];
            //Response.Write(first);

            //Response.Write(valueFromSession);
            //int sigval = Convert.ToInt32(Session["Y"]);
            //Response.Write(sigval);

        }

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable tblcsv = new DataTable();
            //creating columns  
            tblcsv.Columns.Add("Check #");
            tblcsv.Columns.Add("Account #");
            tblcsv.Columns.Add("Amount");
            tblcsv.Columns.Add("Check Date");
            tblcsv.Columns.Add("Drawee Bank");
            tblcsv.Columns.Add("Drawee Bank Branch");
            tblcsv.Columns.Add("Funded?");
            tblcsv.Columns.Add("Verified?");

            //getting full file path of Uploaded file  
            string CSVFilePath = Path.GetFullPath(FileUpload2.PostedFile.FileName);
            //Reading All text  
            string ReadCSV = File.ReadAllText(CSVFilePath);
            //spliting row after new line  
            foreach (string csvRow in ReadCSV.Split('\n'))
            {
                if (!string.IsNullOrEmpty(csvRow))
                {
                    //Adding each row into datatable  
                    tblcsv.Rows.Add();
                    int count = 0;
                    foreach (string FileRec in csvRow.Split(','))
                    {
                        tblcsv.Rows[tblcsv.Rows.Count - 1][count] = FileRec;
                        count++;
                    }
                }


            }
            //Calling insert Functions  
            InsertCSVRecords(tblcsv); 

            /*try
            {
                //FileStream fs = new FileStream("hello.txt", FileMode.OpenOrCreate);
                FileStream fs = new FileStream(FileUpload2.PostedFile.FileName, FileMode.OpenOrCreate);
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    Console.WriteLine(sr.ReadLine());
                }
                sr.Close();
                fs.Close();
                Console.ReadLine();
            }
            catch (Exception b)
            {
                Console.WriteLine(b.Message);
            }*/
        }



        private void InsertCSVRecords(DataTable csvdt)
        {
            string sqlconn = ConfigurationManager.ConnectionStrings["SqlCom"].ConnectionString;
            SqlConnection con = new SqlConnection(sqlconn);
            //creating object of SqlBulkCopy    
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name    
            objbulk.DestinationTableName = "Employee";
            //Mapping Table column    
            objbulk.ColumnMappings.Add("Name", "Name");
            objbulk.ColumnMappings.Add("City", "City");
            objbulk.ColumnMappings.Add("Address", "Address");
            objbulk.ColumnMappings.Add("Designation", "Designation");
            //inserting Datatable Records to DataBase    
            con.Open();
            objbulk.WriteToServer(csvdt);
            con.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream("hello.txt", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                
                sw.WriteLine("Hello!");
                sw.WriteLine("World!");
                sw.Close();
                fs.Close();
            }
            catch (Exception b)
            {
                Console.WriteLine(b.Message);
            }
        }

        public void GetCSV(object sender, EventArgs e)
        {
            DataView dv = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            var dt = dv.ToTable();

            var csv = dt.ToCSV();

            WriteToOutput(csv, "export.csv", "text/csv");
        }

        private void WriteToOutput(String csv, String fileName, String mimeType)
        {
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", fileName));
            Response.Write(csv);
            Response.End();
        }

        /* private void LoadDocument()
         {
              Dictionary<string, string> parameters = new Dictionary<string, string>();
              parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
              parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/cmis/versions/1.0/atom";
              parameters[DotCMIS.SessionParameter.User] = "admin";
              parameters[DotCMIS.SessionParameter.Password] = "092095";
             SessionFactory factory = SessionFactory.NewInstance();
             ISession session = factory.GetRepositories(parameters)[0].CreateSession();
             IObjectId id = session.CreateObjectId("12345678");
             IDocument doc = session.GetObject(id) as IDocument;
             Console.WriteLine(doc.Name);
             Console.WriteLine(doc.GetPropertyValue("my:property"));
             IProperty myProperty = doc["my:property"];
             Console.WriteLine("Id:    " + myProperty.Id);
             Console.WriteLine("Value: " + myProperty.Value);
             Console.WriteLine("Type:  " + myProperty.PropertyType);

             // content
             IContentStream contentStream = doc.GetContentStream();
             Console.WriteLine("Filename:   " + contentStream.FileName);
             Console.WriteLine("MIME type:  " + contentStream.MimeType);
             Console.WriteLine("Has stream: " + (contentStream.Stream != null));
         }

         protected void loadDocument_Click(Object sender, EventArgs e)
         {
             LoadDocument3();
         }
       
         private void LoadImage2()
         {
             Dictionary<string, string> parameters = new Dictionary<string, string>();
             parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
             parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/cmis/versions/1.0/atom";
             parameters[DotCMIS.SessionParameter.User] = "admin";
             parameters[DotCMIS.SessionParameter.Password] = "092095";
             SessionFactory factory = SessionFactory.NewInstance();
             ISession session = factory.GetRepositories(parameters)[0].CreateSession();
             //IDocument doc = session.GetObjectByPath
             //string i = doc.Name;
             //Response.Write(i);
         }

         private void LoadDocument3()
         {
             Dictionary<string, string> parameters = new Dictionary<string, string>();
             parameters[DotCMIS.SessionParameter.BindingType] = BindingType.AtomPub;
             parameters[DotCMIS.SessionParameter.AtomPubUrl] = "http://localhost:8080/alfresco/api/-default-/cmis/versions/1.0/atom";
             parameters[DotCMIS.SessionParameter.User] = "admin";
             parameters[DotCMIS.SessionParameter.Password] = "092095";
             SessionFactory factory = SessionFactory.NewInstance();
             ISession session = factory.GetRepositories(parameters)[0].CreateSession();
             IObjectId id = session.CreateObjectId("4bd21f6d-909b-487b-bdfa-a71abd493978");
             //IObjectId id = session.CreateObjectId("9b638969-2a36-4c55-ab36-ebfe48e7d680");
             IDocument doc = session.GetObject(id) as IDocument;
             FileUpload wew = doc as FileUpload;
             //IContentStream stream = doc.GetContentStream();
             //IContentStream content = doc.GetContentStream();
             //IDocument checkout = doc.CheckOut() as IDocument;
             //checkout.geGetContentStream()
             //Stream stream = checkout.GetContentStream().Stream;
             //MemoryStream ms = new MemoryStream(FileUpload1.FileBytes);
             //Response.Write(doc.GetContentStream().Stream);
             //System.Drawing.Image mage = System.Drawing.Image.FromStream(doc.GetContentStream());  
             //int uu = (int) doc.GetContentStream().Length;

             MemoryStream ms = new MemoryStream();
             ms.ReadByte();
             string base64string = Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);
             Image1.ImageUrl = "data:image/jpeg;base64," + base64string;

             //string base64string = Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);
             Image1.ImageUrl = "data:image/jpeg;base64," + base64string;

             get image from local folder
             MemoryStream ms = new MemoryStream(FileUpload1.FileBytes);
             string base64string = Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);
             Image1.ImageUrl = "data:image/jpeg;base64," + base64string;
         }*/
    }
}
