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
using System.Windows.Data;
using System.Globalization;


namespace CLWD.ViewModel
{
    public class GoogleOAuth2LoginViewModel : BaseViewModel
    {
        #region Members
        /// <summary>
        ///    
        /// </summary>
        private bool _authorized;
        private GoogleOAuth2 _oauth2;
        private string _accessCode;
        private string _authorizationUrl;
        private bool _showButton;

        private OAuth2Parameters parameters;
        #endregion

        #region Constructor
        public GoogleOAuth2LoginViewModel()
        {
            _oauth2 = new GoogleOAuth2();
            _showButton = true;

            PropertyChanged += ViewModelPropertyChanged;




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

        public bool ShowButton
        {
            get { return _showButton; }
            set
            {
                _showButton = value;
                RaisePropertyChanged("ShowButton");
            }
        }



        #endregion

        public void initialize()
        {
            ShowButton = true;
            LoadFromRegistry();

            if (!Authorized)
            {
                //창생성후 로그인 준비
                ((App)App.Current).CreateLoginWindow(this);

                OnRequestShow();

            }
        }

        public void uninitialize()
        {
            AuthorizationURI = "about:blank";
            AccessToken = "";
            _accessCode = "";
            Authorized = false;
            DeleteFromRegistry();
        }

        public void Login()
        {

            BuildParameter();

            AuthorizationURI = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);

        }

        public void BuildParameter()
        {
            parameters = new OAuth2Parameters();
            parameters.ClientId = "993296641183.apps.googleusercontent.com";
            parameters.ClientSecret = "u6ET18iH007SJ_jo6WdcNlA3";
            parameters.RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
            parameters.AccessToken = AccessToken;
            parameters.Scope = "https://docs.google.com/feeds/ https://docs.googleusercontent.com/ https://spreadsheets.google.com/feeds/ https://www.googleapis.com/auth/userinfo.profile";
        }

        public void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AccessCode"))
            {
                if (parameters != null)
                {
                    parameters.AccessCode = AccessCode;
                    OAuthUtil.GetAccessToken(parameters);

                    AccessToken = parameters.AccessToken;
                    Authorized = true;
                    SaveToRegistry();

                }

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
                BuildParameter();
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

        }

        public void DeleteFromRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("CLWD");

            AccessToken = (string)key.GetValue("AccessToken", "");


            if (!AccessToken.Equals(""))
            {
                key.DeleteValue("AccessToken");
            }

        }


        #region Commands
        void SigninCommandExecute()
        {
            ShowButton = false;

            Login();


        }

        bool CanSigninCommandExecute()
        {
            return true;
        }

        public ICommand SigninCommand
        {
            get
            {
                return new RelayCommand(SigninCommandExecute, CanSigninCommandExecute);
            }
        }
        #endregion


    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            bool isAuthorized = (bool)value;

            if (isAuthorized)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }


        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
