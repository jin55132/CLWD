using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLWD.Model;
using System.Windows.Input;
using Microsoft.Win32;
using System.Diagnostics;
using Google.GData.Spreadsheets;
using System.Threading;
using Google.GData.Documents;

namespace CLWD.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        #region Members
        /// <summary>
        ///    
        /// </summary>
        private bool _authorized;
        private Account _account;
        private bool _canLogin;
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            _account = new Account();
            _authorized = false;
            _canLogin = true;
            LoadFromRegistry();
        }
        #endregion


        #region Properties
        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
            }
        }

        public string ID
        {
            get { return Account.ID; }
            set
            {
                if (Account.ID != value)
                {
                    Account.ID = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        public string Password
        {
            get { return Account.Password; }
            set
            {
                if (Account.Password != value)
                {
                    Account.Password = value;
                    RaisePropertyChanged("Password");
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

        public bool CanLogin
        {
            get { return _canLogin; }
            set
            {
                _canLogin = value;
                RaisePropertyChanged("CanLogin");
            }
        }

        #endregion




        #region Commands
        void LoginExecute()
        {
            CanLogin = false;

            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.setUserCredentials(ID, Password);
            string queryTitle = ID + "@CLWD";

            Google.GData.Spreadsheets.SpreadsheetQuery query = new Google.GData.Spreadsheets.SpreadsheetQuery();
            query.Title = queryTitle;
           

            ThreadStart start = delegate()
            {

                try
                {
                    SpreadsheetFeed feed = service.Query(query);

                    Authorized = true;
                    CanLogin = true;
                    SaveToRegistry();



                }
                catch
                {
                    ID = "";
                    Password = "";
                    Authorized = false;
                    CanLogin = true;
                }

            };
            new Thread(start).Start();

            
               


        }

        bool CanLoginExecute()
        {
            return CanLogin;
        }

        public ICommand LoginAction { get { return new RelayCommand(LoginExecute, CanLoginExecute); } }
        #endregion






        public void LoadFromRegistry()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true).CreateSubKey("CLWD");


            ID = (string)key.GetValue("ID", "");
            Password = (string)key.GetValue("Password", "");
        }

        public void SaveToRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("CLWD");

            key.SetValue("ID", ID);
            key.SetValue("Password", Password);
        }

    }
}
