using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CLWD.Model
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
                _book.Add(new VocaViewModel { Voca = new Voca { Word = _database.GetRandomWord, Meaning = _database.GetRandomMeaning } });
            }
        }
        #endregion

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
