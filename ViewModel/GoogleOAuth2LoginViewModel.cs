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
        //private string _accessCode;
        private string _authorizationUrl;
        private bool _showButton;

        
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
            get { return OAuth2.Parameters.AccessCode; }
            set
            {
                if (OAuth2.Parameters.AccessCode != value)
                {
                    OAuth2.Parameters.AccessCode = value;
                    RaisePropertyChanged("AccessCode");
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
            //AccessToken = "";
            //_accessCode = "";
            Authorized = false;
            DeleteFromRegistry();
        }

        public void Login()
        {
            AuthorizationURI = OAuthUtil.CreateOAuth2AuthorizationUrl(OAuth2.Parameters);
        }

        public void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AccessCode"))
            {
                if (OAuth2.Parameters != null)
                {
                    OAuth2.Parameters.AccessCode = AccessCode;
                    OAuthUtil.GetAccessToken(OAuth2.Parameters);
                    SaveToRegistry();
                    //AccessToken = OAuth2.Parameters.AccessToken;
                    Authorized = true;
                   

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


            OAuth2.Parameters.AccessToken = (string)key.GetValue("AccessToken", "");
            OAuth2.Parameters.RefreshToken = (string)key.GetValue("RefreshToken", "");

            if (!OAuth2.Parameters.AccessToken.Equals("") && !OAuth2.Parameters.RefreshToken.Equals(""))
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

            // always invoked when authrized..
            //if (Authorized == true)
            //{
                key.SetValue("AccessToken", OAuth2.Parameters.AccessToken);
                key.SetValue("RefreshToken", OAuth2.Parameters.RefreshToken);

            //}

        }

        public void DeleteFromRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("CLWD");

            OAuth2.Parameters.AccessToken = (string)key.GetValue("AccessToken", "");


            if (!OAuth2.Parameters.AccessToken.Equals(""))
            {
                key.DeleteValue("AccessToken");
            }

            if (!OAuth2.Parameters.RefreshToken.Equals(""))
            {
                key.DeleteValue("RefreshToken");
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
