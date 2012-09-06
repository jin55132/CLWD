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
using System.Windows.Threading;
using System.Windows;

namespace CLWD.ViewModel
{
    class GoogleIDLoginViewModel : BaseViewModel
    {
        #region Members
        /// <summary>
        ///    
        /// </summary>
        private bool _authorized;
        private GoogleIDAccount _account;
        private bool _canLogin;
        private bool _saveAccount;
        #endregion

        #region Constructor
        public GoogleIDLoginViewModel()
        {
            _account = new GoogleIDAccount();
            _authorized = false;
            _canLogin = true;
            _saveAccount = false;

            LoadFromRegistry();

            if (SaveAccount)
            {
                LoginExecute();
            }
        }
        #endregion


        #region Properties
        public GoogleIDAccount Account
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

        public bool SaveAccount
        {
            get { return _saveAccount; }
            set
            {
                _saveAccount = value;
                RaisePropertyChanged("SaveAccount");
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


                    // OnRequestClose 는 UI Thread이므로 Dispatcher 사용해야함..

                    DispatcherOperation op = Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                         new Action(OnRequestClose));

                    //DispatcherOperationStatus status = op.Status;
                    //while (status != DispatcherOperationStatus.Completed)
                    //{
                    //    status = op.Wait(TimeSpan.FromMilliseconds(1000));
                    //    if (status == DispatcherOperationStatus.Aborted)
                    //    {
                    //        // Alert Someone
                    //    }
                    //}
              
                    
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

        public ICommand LoginAction
        {
            get { 
                return new RelayCommand(LoginExecute, CanLoginExecute); 
            }
        }
        #endregion



        public void LoadFromRegistry()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true).CreateSubKey("CLWD");


            ID = (string)key.GetValue("ID", "");
            Password = (string)key.GetValue("Password", "");

            if (!ID.Equals("") && !Password.Equals(""))
            {
                SaveAccount = true;
            }
            else
            {
                SaveAccount = false;
            }
        }

        public void SaveToRegistry()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("CLWD");

            if (SaveAccount == true)
            {
                key.SetValue("ID", ID);
                key.SetValue("Password", Password);
            }
            else
            {
                key.DeleteValue("ID");
                key.DeleteValue("Password");
            }

        }

    }
}
