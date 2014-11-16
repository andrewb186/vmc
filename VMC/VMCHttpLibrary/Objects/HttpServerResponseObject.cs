using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace VMCHttpLibrary.Objects
{
    class HttpServerResponseObject
    {
        private string _Filname;
        public string Filname
        {
            get { return _Filname; }
            set { _Filname = value; }
        }

        private string _FileExtension;
        public string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = value; }
        }

        private string _ContentType;
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        private Encoding _EncodingType;
        public Encoding EncodingType
        {
            get { return _EncodingType; }
            set { _EncodingType = value; }
        }

        private HttpStatusCode _StatusCode;
        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
            set { _StatusCode = value; }
        }

        private char[] _Data;
        public char[] Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        private long _ContentLength;
        public long ContentLength
        {
            get { return _ContentLength; }
            set { _ContentLength = value; }
        }

        public HttpServerResponseObject()
        {
            this._Filname = "";
            this._FileExtension = ".html";
            this._ContentType = "text/html";
            this._EncodingType = Encoding.UTF8;
            this._StatusCode = HttpStatusCode.BadRequest;
        }


    }
}
