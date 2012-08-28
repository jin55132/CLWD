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

namespace CLWD.ViewModel
{
    class BookViewModel
    {
        #region Members
        private VocaDB _database = new VocaDB();
        ObservableCollection<VocaViewModel> _book = new ObservableCollection<VocaViewModel>();

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
        #endregion

        #region Construction
        public BookViewModel()
        {
            for (int i = 0; i < 3; ++i)
            {
                VocaViewModel vocavm = new VocaViewModel { Voca = new Voca { Word = _database.GetRandomWord, Meaning = _database.GetRandomMeaning } };
                vocavm.PropertyChanged += EntityViewModelPropertyChanged;
                _book.Add(vocavm);
                //_book.Add(new VocaViewModel { Voca = new Voca { Word = _database.GetRandomWord, Meaning = _database.GetRandomMeaning} });

            }

            //_book.CollectionChanged += new NotifyCollectionChangedEventHandler(changed);
            _book.CollectionChanged += changed;



        }
        #endregion

        public void changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (VocaViewModel item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
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

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //This will get called when the property of an object inside the collection changes

            if (e.PropertyName.Equals("Word"))
            {
                VocaViewModel vocaVM = ((VocaViewModel)sender);

                //var request = WebRequest.Create(url);
                //string text;
                //var response = (HttpWebResponse)request.GetResponse();

                //using (var sr = new StreamReader(response.GetResponseStream()))
                //{
                //    text = sr.ReadToEnd();
                //}

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



                vocaVM.Meaning = jsonProcessor(jstring);



            }
        }

        public string jsonProcessor(string jsoninput)
        {
            string meaning = "";
            try
            {
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

                    string voca = string.Format("{0}:({1}) {2}\n", count++, strPos, strTerm);


                    meaning += voca;
                }
            }
            catch (System.Exception ex)
            {
                meaning = "No Match";
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

        #region Commands
        void UpdateAlbumArtistsExecute()
        {
            //if (_songs == null)
            //    return;

            //++_count;
            //foreach (var song in _songs)
            //{
            //    song.ArtistName = _database.GetRandomArtistName;
            //}
        }

        bool CanUpdateAlbumArtistsExecute()
        {
            return true;
        }

        public ICommand UpdateAlbumArtists { get { return new RelayCommand(UpdateAlbumArtistsExecute, CanUpdateAlbumArtistsExecute); } }


        void AddAlbumArtistExecute()
        {
            //    if (_songs == null)
            //        return;

            //    _songs.Add(new SongViewModel { Song = new Song { ArtistName = _database.GetRandomArtistName, SongTitle = _database.GetRandomSongTitle } });
            //
        }

        bool CanAddAlbumArtistExecute()
        {
            return true;
        }

        public ICommand AddAlbumArtist { get { return new RelayCommand(AddAlbumArtistExecute, CanAddAlbumArtistExecute); } }

        void UpdateSongTitlesExecute()
        {
            //if (_songs == null)
            //    return;

            //++_count;
            //foreach (var song in _songs)
            //{
            //    song.SongTitle = _database.GetRandomSongTitle;
            //}
        }

        bool CanUpdateSongTitlesExecute()
        {
            return true;
        }

        public ICommand UpdateSongTitles { get { return new RelayCommand(UpdateSongTitlesExecute, CanUpdateSongTitlesExecute); } }

        #endregion
    }
}
