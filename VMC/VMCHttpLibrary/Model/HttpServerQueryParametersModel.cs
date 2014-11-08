using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMCHttpLibrary.Model
{
    class HttpServerQueryParametersModel
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

        public HttpServerQueryParametersModel(string key, string value)
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
