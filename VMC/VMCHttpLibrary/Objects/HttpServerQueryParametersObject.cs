using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMCHttpLibrary.Objects
{
    class HttpServerQueryParametersObject
    {

        private string _Key;
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }


        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public HttpServerQueryParametersObject(string key, string value)
        {
            if ((key == null) || (value == null))
            {
                throw new Exception("Key or Value is NULL");
            }
            else
            {
                this._Key = key;
                this._Value = value;
            }
        }

    }
}
