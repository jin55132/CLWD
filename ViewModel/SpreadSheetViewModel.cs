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
using CLWD.Helpher;

namespace CLWD.ViewModel
{
    
    class SpreadSheetViewModel : BaseViewModel
    {
        private GoogleOAuth2LoginViewModel _loginViewModel;
        private SpreadSheetDB _database;
        private ObservableCollection<BookViewModel> _spreadsheet = new ObservableCollection<BookViewModel>();
        private bool _synchronizing;




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

        public bool Synchronizing
        {
            get { return _synchronizing; }
            set
            {
                _synchronizing = value;
                RaisePropertyChanged("Synchronizing");
            }
        }



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
            _loginViewModel = new GoogleOAuth2LoginViewModel();

            _loginViewModel.PropertyChanged += (obj, e) =>
            {
                if (e.PropertyName == "Authorized")
                    RaisePropertyChanged(e.PropertyName);
            };

            PropertyChanged += SpreadSheetViewModel_PropertyChanged;
        }

        void SpreadSheetViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Authorized")
            {
                if (Authorized)
                {


                    _database = new SpreadSheetDB(GoogleOAuth2LoginViewModel.OAuth2);


                    ThreadStart start = delegate()
                    {


                        ObservableCollection<BookViewModel> sheets = null;
                        try
                        {

                            Synchronizing = true;
                            _database.Init();


                            sheets = _database.RetrieveSpreadsheet();

                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }


                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                                                       new Action(() =>
                                                       {
                                                           if (sheets != null)
                                                           {
                                                               // deep copy of collection's items created from different thread 
                                                               foreach (var bookitem in sheets)
                                                               {
                                                                   BookViewModel book = new BookViewModel(_database, bookitem.BookTitle, bookitem.Entry);
                                                                   book.Book.CollectionChanged += book.VocaViewModel_PropertyChanged;

                                                                   foreach (var vocaitem in bookitem.Book)
                                                                   {
                                                                       book.Book.Add(vocaitem);
                                                                   }

                                                                   this.SpreadSheet.Add(book);
                                                               }

                                                               // move focus to the last
                                                               ICollectionView vs = CollectionViewSource.GetDefaultView(this.SpreadSheet);
                                                               vs.MoveCurrentToLast();
                                                           }

                                                           Synchronizing = false;
                                                       }));

                        }


                    };


                    new Thread(start).Start();




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
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive && !Synchronizing;
        }

        bool CanLogoutCommandExecute()
        {
            return Authorized && !GoogleOAuth2LoginViewModel.WindowAlive && !Synchronizing;
        }
        public ICommand LoginCommand { get { return new RelayCommand(LoginExecute, CanLoginCommandExecute); } }
        public ICommand LogoutCommand { get { return new RelayCommand(LoginExecute, CanLogoutCommandExecute); } }


        void AddNewSheetExecute()
        {


            ThreadStart start = delegate()
            {
                Synchronizing = true;

                BookViewModel book = _database.AddBook(this);

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
               new Action(() =>
               {
                   book.Book.CollectionChanged += book.VocaViewModel_PropertyChanged;
                   this.SpreadSheet.Add(book);

                   ICollectionView vs = CollectionViewSource.GetDefaultView(this.SpreadSheet);
                   vs.MoveCurrentTo(book);
               }));
                Synchronizing = false;

            };

            new Thread(start).Start();

        }


        public ICommand AddNewSheetCommand { get { return new RelayCommand(AddNewSheetExecute, CanLogoutCommandExecute); } }


        void DeleteCurrentSheetExecute()
        {

            ICollectionView vs = CollectionViewSource.GetDefaultView(SpreadSheet);


            BookViewModel current = vs.CurrentItem as BookViewModel;


            ThreadStart start = delegate()
            {
                Synchronizing = true;
                if (current != null)
                    _database.DeleteBook(this, current);


                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
               new Action(() =>
               {
                   this.SpreadSheet.Remove(current);
               }));
                Synchronizing = false;

            };

            new Thread(start).Start();

        }


        public ICommand DeleteCurrentSheetCommand { get { return new RelayCommand(DeleteCurrentSheetExecute, CanLogoutCommandExecute); } }


        public void Closing()
        {
            if (GoogleOAuth2LoginViewModel.WindowAlive)
                GoogleOAuth2LoginViewModel.OnRequestClose();
        }
    }
}
