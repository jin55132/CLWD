using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLWD.Model
{
    class GoogleOAuth2 : ObservableObject
    {
        #region Members
        string _accessToken;

        #endregion

        public GoogleOAuth2()
        {
            _accessToken = "";
        }


        #region Properties
        public string AccessToken
        {
            get { return _accessToken; }
            set
            {
                _accessToken = value;
                RaisePropertyChanged("AccessToken");
            }
        }



        #endregion
    }
}
