using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CLWD.Model;
using System.Collections.Specialized;
using System.ComponentModel;

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

        public void changed (object sender, NotifyCollectionChangedEventArgs e)
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
            Console.Write("d");
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
