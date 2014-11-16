using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections;
using System.Web;
using log4net;
using System.Collections.Specialized;

namespace VMCHttpLibrary.Objects
{
    class HttpServerRequestObject
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        private string _RequestContentType;
        public string RequestContentType
        {
            get { return _RequestContentType; }
            set { _RequestContentType = value; }
        }


        private string _ResponseContentType;
        public string ResponseContentType
        {
            get { return _ResponseContentType; }
            set { _ResponseContentType = value; }
        }


        private List<HttpServerQueryParametersObject> _QueryParameters;
        public List<HttpServerQueryParametersObject> QueryParameters
        {
            get { return _QueryParameters; }
            set { _QueryParameters = value; }
        }


        public HttpServerRequestObject(HttpListenerRequest request)
        {
            this._Path = request.Url.AbsolutePath;
            log.Info("Requested Path: " + this._Path);

            this._RequestContentType = request.ContentType;
            log.Info("Request Content-Type: " + this._RequestContentType);

            this._ResponseContentType = request.AcceptTypes[0];
            log.Info("Response Content-Type: " + this._ResponseContentType);

            this._QueryParameters = this.GetQueryParameters(request.Url.Query);
        }


        private List<HttpServerQueryParametersObject> GetQueryParameters(string sQuery)
        {
            List<HttpServerQueryParametersObject> temp = new List<HttpServerQueryParametersObject>();

            try
            {
                //Parsing Query String
                NameValueCollection c = HttpUtility.ParseQueryString(sQuery);
                log.Info("Query string not null");

                //Populating List
                foreach (string key in c.AllKeys)
                {
                    log.Info("Initializing Key Value object");
                    log.DebugFormat("{0}={1}", key, c.Get(key));
                    temp.Add(new HttpServerQueryParametersObject(key, c.Get(key)));
                }
            }
            catch (ArgumentNullException e)
            {
                log.Error(e.Message, e);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
            }
                        
            return temp;
        }




    }
}
