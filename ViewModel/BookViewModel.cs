using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CLWD.Model;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Net.Json;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using CLWD.Helpher;

namespace CLWD.ViewModel
{
    class BookViewModel : BaseViewModel
    {
        #region Members
        private SpreadSheetDB _database;
        private GoogleOAuth2LoginViewModel _loginViewModel;
        private ObservableCollection<VocaViewModel> _book = new ObservableCollection<VocaViewModel>();


        #endregion



        #region Properties
        public ObservableCollection<VocaViewModel> Book
        {
            get
            {
                return _book;
            }
            set
            {
                _book = value;
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



        #region Construction
        //  나중에 추상화 해야함.. OAuth2 login
        public BookViewModel()
        {
            _loginViewModel = new GoogleOAuth2LoginViewModel();

            _loginViewModel.PropertyChanged += (obj, e) =>
            {
                if (e.PropertyName == "Authorized")
                    RaisePropertyChanged(e.PropertyName);
            };


            _book.CollectionChanged += VocaViewModel_PropertyChanged;

            PropertyChanged += new PropertyChangedEventHandler(BookViewModel_PropertyChanged);

        }

        #endregion


        public void Insert(VocaViewModel vocaVM)
        {
            //VocaViewModel vocaVM = new VocaViewModel { Voca = voca };
            _book.Add(vocaVM);
        }

        void BookViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "Authorized")
            {
                if (Authorized)
                {
                    _database = new SpreadSheetDB(GoogleOAuth2LoginViewModel.OAuth2);
                    _database.Init();
                    _database.Retrieve(this);


                }
                else
                {

                }
            }

        }

        void VocaViewModel_PropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (VocaViewModel item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;

                    ThreadStart start = delegate()
                    {

                        try
                        {
                            _database.Remove(item);
                        }
                        catch
                        {

                        }

                    };

                    new Thread(start).Start();

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (VocaViewModel item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }

        }
        public int RandNumber(int Low, int High)
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }


        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("Word"))
            {
                VocaViewModel vocaVM = ((VocaViewModel)sender);


                ThreadStart start = delegate()
                {

                    //WordReference 통해 단어의미 획득..json
                    long oldDate = vocaVM.UnixTime;
                    int oldKey = vocaVM.Key;

                    try
                    {
                        string jsonUri = generateUrl(vocaVM.Word);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(jsonUri);
                        request.Method = WebRequestMethods.Http.Get;
                        request.Accept = "application/json";

                        var response = (HttpWebResponse)request.GetResponse();

                        string jstring;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var responseText = streamReader.ReadToEnd();
                            jstring = responseText;
                            
                        }

                        vocaVM.Key = RandNumber(1, 10000);
                        vocaVM.Definition = jsonProcessor(jstring);
                        vocaVM.Date = DateTime.Now;

                    }
                    catch
                    {
                        vocaVM.Definition = "No Match";
                    }

                    try
                    {
                        _database.Update(vocaVM, oldDate, oldKey);
                    }
                    catch
                    {

                    }

                };

                new Thread(start).Start();

            }


        }



        public string jsonProcessor(string jsoninput)
        {
            // Thread.Sleep(1000);
            string meaning = "";

            JsonTextParser parser = new JsonTextParser();
            JsonObject obj = parser.Parse(jsoninput);

            JsonObjectCollection rootCol = obj as JsonObjectCollection;
            JsonObjectCollection term0 = (JsonObjectCollection)((JsonObjectCollection)rootCol["term0"])["PrincipalTranslations"];


            int count = 0;
            foreach (JsonObjectCollection principalCol in term0 as JsonObjectCollection)
            {
                JsonObjectCollection OriginalTerm = (JsonObjectCollection)principalCol["OriginalTerm"];
                JsonObjectCollection FirstTranslation = (JsonObjectCollection)principalCol["FirstTranslation"];
                JsonObject term = FirstTranslation["term"];
                JsonObject pos = OriginalTerm["POS"];

                string strTerm = (string)term.GetValue();
                string strPos = (string)pos.GetValue();

                string voca;
                if (term0.Count == count + 1)
                {
                     voca = string.Format("{0}:({1}) {2}", count++, strPos, strTerm);
                }
                else
                {
                     voca = string.Format("{0}:({1}) {2}\n", count++, strPos, strTerm);
                }
               

                meaning += voca;
            }


            return meaning;
        }

        public string generateUrl(string word)
        {
            string uri = "http://api.wordreference.com/";
            string version = "0.8/";
            string key = "47a6b/";
            string method = "json/";
            string dictionary = "enko/";

            return uri + version + key + method + dictionary + word;
        }

        //public void LoginViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //if (e.PropertyName == "Authorized")
        //    {
        //        RaisePropertyChanged(e.PropertyName);
        //    }
        //}


        #region Commands

        void LoginExecute()
        {
            Book.Clear();
            GoogleOAuth2LoginViewModel.uninitialize();
            GoogleOAuth2LoginViewModel.initialize();
        }


        bool CanLoginCommandExecute()
        {
            return !Authorized && !GoogleOAuth2LoginViewModel.WindowAlive; ;
        }

        bool CanLogoutCommandExecute()
        {
            return Authorized && !GoogleOAuth2LoginViewModel.WindowAlive;
        }
        public ICommand LoginCommand { get { return new RelayCommand(LoginExecute, CanLoginCommandExecute); } }
        public ICommand LogoutCommand { get { return new RelayCommand(LoginExecute, CanLogoutCommandExecute); } }




        public void Closing()
        {
            if (GoogleOAuth2LoginViewModel.WindowAlive)
                GoogleOAuth2LoginViewModel.OnRequestClose();
        }
        //void AddAlbumArtistExecute()
        //{
        //    //    if (_songs == null)
        //    //        return;

        //    //    _songs.Add(new SongViewModel { Song = new Song { ArtistName = _database.GetRandomArtistName, SongTitle = _database.GetRandomSongTitle } });
        //    //
        //}

        //bool CanAddAlbumArtistExecute()
        //{
        //    return true;
        //}

        //public ICommand AddAlbumArtist { get { return new RelayCommand(AddAlbumArtistExecute, CanAddAlbumArtistExecute); } }

        //void UpdateSongTitlesExecute()
        //{
        //    //if (_songs == null)
        //    //    return;

        //    //++_count;
        //    //foreach (var song in _songs)
        //    //{
        //    //    song.SongTitle = _database.GetRandomSongTitle;
        //    //}
        //}

        //bool CanUpdateSongTitlesExecute()
        //{
        //    return true;
        //}

        //public ICommand UpdateSongTitles { get { return new RelayCommand(UpdateSongTitlesExecute, CanUpdateSongTitlesExecute); } }

        #endregion
    }
}
