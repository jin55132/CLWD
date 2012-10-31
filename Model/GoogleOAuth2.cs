using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;

namespace CLWD.Model
{
    public class GoogleOAuth2 : ObservableObject
    {
        #region Members
        private OAuth2Parameters _oauth2Parameter;
        #endregion

        public GoogleOAuth2()
        {
            _oauth2Parameter = new OAuth2Parameters();
            _oauth2Parameter.ClientId = "993296641183.apps.googleusercontent.com";
            _oauth2Parameter.ClientSecret = "u6ET18iH007SJ_jo6WdcNlA3";
            _oauth2Parameter.RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
            _oauth2Parameter.AccessToken = AccessToken;
            _oauth2Parameter.Scope = "https://docs.google.com/feeds/ https://spreadsheets.google.com/feeds/";
            //_oauth2Parameter.Scope = "https://docs.google.com/feeds/ https://docs.googleusercontent.com/ https://spreadsheets.google.com/feeds/ https://www.googleapis.com/auth/userinfo.profile";

        }


        #region Properties
        public string AccessToken
        {
            get { return _oauth2Parameter.AccessToken; }
            set
            {
                _oauth2Parameter.AccessToken = value;
                RaisePropertyChanged("AccessToken");
            }
        }

        
        public OAuth2Parameters Parameters
        {
            get {
                return _oauth2Parameter; 
            }
            set
            {
                _oauth2Parameter = value;
            }
        }


        #endregion
    }
}
