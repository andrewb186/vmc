using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMCHttpLibrary.Model;
using log4net;
using System.IO;

namespace VMCHttpLibrary.Controller
{
    class HttpServerLoadFile
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Ecapsulate into an object
        //======
        private string fileName;
        private string responseContentType;
        private string fileExtension;
        //include field / property for error handling
        //======
        
        private string fullFilePath;

        public HttpServerLoadFile(HttpServerRequestModel requestObject)
        {
            if (requestObject.Path.Equals(@"/"))
            {
                this.fileName = @"\index";
                this.responseContentType = "text/html";
                this.fileExtension = ".html";
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

        private void setWithFileExtension(HttpServerRequestModel requestObject)
        {
            log.Info("Getting filename");
            this.fileName = Path.GetDirectoryName(requestObject.Path) + @"\"+ Path.GetFileNameWithoutExtension(requestObject.Path);
            
            log.Info("Getting file extension");
            this.fileExtension = Path.GetExtension(requestObject.Path);

            log.Info("Setting Content-Type");
            switch (fileExtension.ToLower())
            {
                case ".js":
                    this.responseContentType = "application/javascript";
                    log.Info("Content-type set to: application/javascript");
                    break;
                case ".css":
                    this.responseContentType = "text/css";
                    log.Info("Content-type set to: text/css");
                    break;
            }
        }

        private void setWithoutFileExtension(HttpServerRequestModel requestObject)
        {
            log.Info("Getting filename");
            this.fileName = @"\" + Path.GetFileName(requestObject.Path);

            log.Info("Getting Content-Type");
            this.responseContentType = requestObject.ResponseContentType;

            log.Info("Setting file extension");
            switch (this.responseContentType.ToLower())
            {
                case "text/html":
                    this.fileExtension = ".html";
                    log.Info("File extension set to: html");
                    break;
                case "application/json":
                    this.fileExtension = ".json";
                    log.Info("File extension set to: json");
                    break;
                case "application/xml":
                    this.fileExtension = ".xml";
                    log.Info("File extension set to: xml");
                    break;
                default:
                    log.Info("Invalid mime type. Setting to default type [text/html]");
                    fileExtension = ".html";
                    break;
            }     
        }

       

        public char [] loadFile()
        {
            char[] buffer = null;
            int count = 0;

            string _filePath =  AppDomain.CurrentDomain.BaseDirectory + @"html" + fileName + fileExtension;

            if (File.Exists(_filePath) == false)
            {
                _filePath = AppDomain.CurrentDomain.BaseDirectory + @"html\error.html";
                //set not found status code
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

            return buffer;            
        }
        

    }
}
