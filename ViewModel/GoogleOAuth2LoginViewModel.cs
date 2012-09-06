using System;
using CLWD.Model;
using Google.GData.Spreadsheets;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using Google.Spreadsheets;
using Microsoft.Win32;
using Google.GData.Client;
using System.ComponentModel;


namespace CLWD.ViewModel
{
    class GoogleOAuth2LoginViewModel : BaseViewModel
    {
        #region Members
        /// <summary>
        ///    
        /// </summary>
        private bool _authorized;
        private GoogleOAuth2 _oauth2;
        private string _accessCode;
        private string _authorizationUrl;
        private OAuth2Parameters parameters = new OAuth2Parameters();
        #endregion

        #region Constructor
        public GoogleOAuth2LoginViewModel()
        {
            _oauth2 = new GoogleOAuth2();
            _authorized = false;
           
            LoadFromRegistry();
            
        }
        #endregion


        #region Properties
        public GoogleOAuth2 OAuth2
        {
            get
            {
                return _oauth2;
            }
            set
            {
                _oauth2 = value;
            }
        }


        public string AccessCode
        {
            get { return _accessCode; }
            set
            {
                if (_accessCode != value)
                {
                    _accessCode = value;
                    RaisePropertyChanged("AccessCode");
                }
            }
        }

        public string AccessToken
        {
            get { return OAuth2.AccessToken; }
            set
            {
                if (OAuth2.AccessToken != value)
                {
                    OAuth2.AccessToken = value;
                    RaisePropertyChanged("AccessToken");
                }
            }
        }

        public bool Authorized
        {
            get { return _authorized; }
            set
            {
                _authorized = value;
                RaisePropertyChanged("Authorized");
            }
        }

        public string AuthorizationURI
        {
            get { return _authorizationUrl; }
            set
            {
                _authorizationUrl = value;
                RaisePropertyChanged("AuthorizationURI");
            }

        }
   

        #endregion

        public void Login()
        {
            PropertyChanged += ViewModelPropertyChanged;

            PrepareLogin();

        }

        public void PrepareLogin()
        {
            parameters.ClientId = "993296641183.apps.googleusercontent.com";
            parameters.ClientSecret = "u6ET18iH007SJ_jo6WdcNlA3";
            parameters.RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
            parameters.Scope = "https://docs.google.com/feeds/ https://docs.googleusercontent.com/ https://spreadsheets.google.com/feeds/ https://www.googleapis.com/auth/userinfo.profile";


            if (Authorized)
            {
                parameters.AccessToken = AccessToken;
                OnRequestClose();

            }
            else
            {
                AuthorizationURI = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);

            }
        }
        public void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AccessCode"))
            {
                parameters.AccessCode = AccessCode;
                OAuthUtil.GetAccessToken(parameters);

                AccessToken = parameters.AccessToken;
                Authorized = true;
                SaveToRegistry();

                //GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
                //SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                //service.RequestFactory = requestFactory;


                //Google.GData.Spreadsheets.SpreadsheetQuery query = new Google.GData.Spreadsheets.SpreadsheetQuery();
                //query.Title = "operator1732@CLWD";
                //SpreadsheetFeed feed = service.Query(query);
            }

        }




        public void LoadFromRegistry()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true).CreateSubKey("CLWD");


            AccessToken = (string)key.GetValue("AccessToken", "");


            if (!AccessToken.Equals(""))
            {
                Authorized = true;
            }
            else
            {
                Authorized = false;
            }
        }

        public void SaveToRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("CLWD");

            if (Authorized == true)
            {
                key.SetValue("AccessToken", AccessToken);

            }
            else
            {
                key.DeleteValue("AccessToken");

            }

        }

    }
}
