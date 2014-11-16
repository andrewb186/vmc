using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using VMCHttpLibrary.Model;
using log4net;
using System.IO;
using VMCHttpLibrary.Objects;

namespace VMCHttpLibrary.Controller
{
    class HttpServerLoadFile
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Ecapsulate into an object
        //======
        //private string fileName;
        //private string responseContentType;
        //private string fileExtension;
        //include field / property for error handling
        //Status Code
        //======

        private HttpServerResponseObject _responseObject = new HttpServerResponseObject();
        public HttpServerResponseObject ResponseObject
        {
            get { return _responseObject; }
            set { _responseObject = value; }
        }
        

        private string fullFilePath;


        public HttpServerLoadFile(HttpServerRequestObject requestObject)
        {
            if (requestObject.Path.Equals(@"/"))
            {
                this._responseObject.Filname = @"\index";
                this._responseObject.ContentType = "text/html";
                this._responseObject.FileExtension = ".html";
            }
            else
            {
                if (Path.HasExtension(requestObject.Path))
                {
                    //Path with file Extenstion
                    log.Info("File with file extension detected");
                    setWithFileExtension(requestObject);
                }
                else
                {
                    //Path without file Extension
                    log.Info("File without file extension detected");
                    setWithoutFileExtension(requestObject);
                }
            }
        }

        private void setWithFileExtension(HttpServerRequestObject requestObject)
        {
            log.Info("Getting filename");
            this.ResponseObject.Filname = Path.GetDirectoryName(requestObject.Path) + @"\" + Path.GetFileNameWithoutExtension(requestObject.Path);
            
            log.Info("Getting file extension");
            this.ResponseObject.FileExtension = Path.GetExtension(requestObject.Path);

            log.Info("Setting Content-Type");
            switch (this.ResponseObject.FileExtension.ToLower())
            {
                case ".js":
                    this.ResponseObject.ContentType = "application/javascript";
                    log.Info("Content-type set to: application/javascript");
                    break;
                case ".css":
                    this.ResponseObject.ContentType = "text/css";
                    log.Info("Content-type set to: text/css");
                    break;
            }
        }

        private void setWithoutFileExtension(HttpServerRequestObject requestObject)
        {
            log.Info("Getting filename");
            this.ResponseObject.Filname = @"\" + Path.GetFileName(requestObject.Path);

            log.Info("Getting Content-Type");
            this.ResponseObject.ContentType = requestObject.ResponseContentType;

            log.Info("Setting file extension");
            switch (this.ResponseObject.ContentType.ToLower())
            {
                case "text/html":
                    this.ResponseObject.FileExtension = ".html";
                    log.Info("File extension set to: html");
                    break;
                case "application/json":
                    this.ResponseObject.FileExtension = ".json";
                    log.Info("File extension set to: json");
                    break;
                case "application/xml":
                    this.ResponseObject.FileExtension = ".xml";
                    log.Info("File extension set to: xml");
                    break;
                default:
                    log.Info("Invalid mime type. Setting to default type [text/html]");
                    this.ResponseObject.FileExtension = ".html";
                    break;
            }     
        }

       
        public void loadFile()
        {
            char[] buffer = null;
            int count = 0;

            string _filePath =  AppDomain.CurrentDomain.BaseDirectory + @"html" + this.ResponseObject.Filname + this.ResponseObject.FileExtension;

            if (File.Exists(_filePath) == false)
            {
                _filePath = AppDomain.CurrentDomain.BaseDirectory + @"html\error.html";
                //set not found status code
                this.ResponseObject.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            else
            {
                this.ResponseObject.StatusCode = System.Net.HttpStatusCode.OK;
            }


            try
            {

                FileStream fStream = new FileStream(_filePath, FileMode.Open);
                StreamReader s = new StreamReader(fStream);

                buffer = new char[fStream.Length-1];
                count = Convert.ToInt32(fStream.Length - 1);

                s.Read(buffer, 0, count);
                s.Close();
                s.Dispose();
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }

            //return buffer;
            this.ResponseObject.Data = buffer;
            this.ResponseObject.ContentLength = Convert.ToInt64(Encoding.UTF8.GetByteCount(buffer));
        }
        

    }
}
