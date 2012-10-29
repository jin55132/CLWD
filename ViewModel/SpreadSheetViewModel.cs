using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace CLWD.ViewModel
{
    class SpreadSheetViewModel : BaseViewModel
    {
        private GoogleOAuth2LoginViewModel _loginViewModel;
        private SpreadSheetDB _database;
        private ObservableCollection<BookViewModel> _spreadsheet = new ObservableCollection<BookViewModel>();

        #region properties
        public ObservableCollection<BookViewModel> SpreadSheet
        {
            get
            {
                return _spreadsheet;
            }
            set
            {
                _spreadsheet = value;

            }
        }
        public bool Authorized
        {
            get
            {
                return _loginViewModel.Authorized;
            }
            set
            {
                _loginViewModel.Authorized = value;

            }
        }

        //public bool InProgress
        //{
        //    get { return !Authorized; }
        //}

        public GoogleOAuth2LoginViewModel GoogleOAuth2LoginViewModel
        {
            get
            {
                return _loginViewModel;

            }
            set
            {
                if (_loginViewModel != value)
                    _loginViewModel = value;


            }
        }
        #endregion

        public SpreadSheetViewModel()
        {
            //_database = new SpreadSheetDB(_loginViewModel.OAuth2);
            _loginViewModel = new GoogleOAuth2LoginViewModel();

            _loginViewModel.PropertyChanged += (obj, e) =>
            {
                if (e.PropertyName == "Authorized")
                    RaisePropertyChanged(e.PropertyName);
            };

            PropertyChanged += SpreadSheetViewModel_PropertyChanged;
            //SpreadSheet.CollectionChanged += SpreadSheetCollectionChanged;
        }


        //public void SpreadSheetCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {


        //        foreach (BookViewModel item in e.NewItems)
        //        {



        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (BookViewModel item in e.OldItems)
        //        {
                   
        //        }
        //    }

        //}

         void SpreadSheetViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Authorized")
            {
                if (Authorized)
                {
                    _database = new SpreadSheetDB(GoogleOAuth2LoginViewModel.OAuth2);
                    _database.Init();

                    _database.RetrieveSpreadsheet(this);



                }
                else
                {

                }
            }

        }

        void LoginExecute()
        {
            SpreadSheet.Clear();
            GoogleOAuth2LoginViewModel.uninitialize();
            GoogleOAuth2LoginViewModel.initialize();
        }


        bool CanLoginCommandExecute()
        {
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive;
        }

        bool CanLogoutCommandExecute()
        {
            return Authorized && !GoogleOAuth2LoginViewModel.WindowAlive;
        }
        public ICommand LoginCommand { get { return new RelayCommand(LoginExecute, CanLoginCommandExecute); } }
        public ICommand LogoutCommand { get { return new RelayCommand(LoginExecute, CanLogoutCommandExecute); } }


        void AddNewSheetExecute()
        {

            _database.AddBook(this);



        }

        bool CanAddNewSheetExecute()
        {
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive;
        }
        public ICommand AddNewSheetCommand { get { return new RelayCommand(AddNewSheetExecute, CanAddNewSheetExecute); } }


        void DeleteCurrentSheetExecute()
        {

            ICollectionView vs = CollectionViewSource.GetDefaultView(SpreadSheet);
            

            BookViewModel current = vs.CurrentItem as BookViewModel;
            
            if(current != null)
                _database.DeleteBook(this, current);

        }


        bool CanDeleteCurrentSheetExecute()
        {
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive;
        }
        public ICommand DeleteCurrentSheetCommand { get { return new RelayCommand(DeleteCurrentSheetExecute, CanDeleteCurrentSheetExecute); } }




        public void Closing()
        {
            if (GoogleOAuth2LoginViewModel.WindowAlive)
                GoogleOAuth2LoginViewModel.OnRequestClose();
        }
    }
}
