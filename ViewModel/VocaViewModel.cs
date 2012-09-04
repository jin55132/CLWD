using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CLWD.Model;

namespace CLWD.ViewModel
{
    class VocaViewModel : BaseViewModel
    {
        #region Construction
        /// <summary>
        ///
        /// </summary>
        public VocaViewModel()
        {
            _voca = new Voca { Word = "Insert a word", Meaning = "" };
        }
        #endregion

        #region Members
        Voca _voca;
        #endregion

        #region Properties
        public Voca Voca
        {
            get
            {
                return _voca;
            }
            set
            {
                _voca = value;
            }
        }


        
        public string Word
        {
            get { return Voca.Word; }
            set 
            {
                if (Voca.Word != value)
                {
                    Voca.Word = value;
                    RaisePropertyChanged("Word");
                }
            }
        }

        public string Meaning
        {
            get { return Voca.Meaning;}
            set
            {
                if (Voca.Meaning != value)
                {
                    Voca.Meaning = value;
                    RaisePropertyChanged("Meaning");
                }
            }
        } 
        #endregion

        #region Commands
        void UpdateArtistNameExecute()
        {
          //  ++_count;
           // ArtistName = string.Format("Elvis ({0})", _count);
        }

        bool CanUpdateArtistNameExecute()
        {
            return true;
        }

        public ICommand UpdateArtistName { get { return new RelayCommand(UpdateArtistNameExecute, CanUpdateArtistNameExecute); } }
        #endregion
    
    }
}
