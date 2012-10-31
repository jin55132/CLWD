using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace CLWD.ViewModel
{
    class SpreadSheetViewModel : BaseViewModel
    {
        private GoogleOAuth2LoginViewModel _loginViewModel;
        private SpreadSheetDB _database;
        private ObservableCollection<BookViewModel> _spreadsheet = null;
        private bool _retrieving;

      


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
                RaisePropertyChanged("SpreadSheet");

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

        public bool Retrieving
        {
            get { return _retrieving; }
            set
            {
                _retrieving = value;
                RaisePropertyChanged("Retrieving");
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

                    Retrieving = true;
                    _database.Init();
                    ObservableCollection<BookViewModel> bookVM = null;
                    bookVM = _database.RetrieveSpreadsheet();

                    this.SpreadSheet = bookVM;
                    ICollectionView vs = CollectionViewSource.GetDefaultView(this.SpreadSheet);
                    vs.MoveCurrentToLast();
                    Retrieving = false;
                    

                    //ThreadStart start = delegate()
                    //{


                    //    
                    //    try
                    //    {



                    //    }
                    //    catch (System.Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                        
                    //  Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                    //                      new Action(() =>
                    //                      {
 
                    //                      }));

          
                    //};

                    
                    //new Thread(start).Start();

             
              

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
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive && !Retrieving;
        }

        bool CanLogoutCommandExecute()
        {
            return Authorized && !GoogleOAuth2LoginViewModel.WindowAlive && !Retrieving;
        }
        public ICommand LoginCommand { get { return new RelayCommand(LoginExecute, CanLoginCommandExecute); } }
        public ICommand LogoutCommand { get { return new RelayCommand(LoginExecute, CanLogoutCommandExecute); } }


        void AddNewSheetExecute()
        {
            _database.AddBook(this);

        }

     
        public ICommand AddNewSheetCommand { get { return new RelayCommand(AddNewSheetExecute, CanLogoutCommandExecute); } }


        void DeleteCurrentSheetExecute()
        {

            ICollectionView vs = CollectionViewSource.GetDefaultView(SpreadSheet);
            

            BookViewModel current = vs.CurrentItem as BookViewModel;
            
            if(current != null)
                _database.DeleteBook(this, current);

        }


        public ICommand DeleteCurrentSheetCommand { get { return new RelayCommand(DeleteCurrentSheetExecute, CanLogoutCommandExecute); } }


        public void Closing()
        {
            if (GoogleOAuth2LoginViewModel.WindowAlive)
                GoogleOAuth2LoginViewModel.OnRequestClose();
        }
    }
}
