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
            _voca = new Voca { Word = "Insert a word", Definition = "" , Date = DateTime.Now};
        
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

        public bool Checked
        {
            get { return Voca.Checked; }
            set
            {
                if (Voca.Checked != value)
                {
                    Voca.Checked = value;
                    RaisePropertyChanged("Checked");
                }
            }
        }

        public string Definition
        {
            get { return Voca.Definition; }
            set
            {
                if (Voca.Definition != value)
                {
                    Voca.Definition = value;
                    RaisePropertyChanged("Definition");
                }
            }
        }

        public DateTime Date
        {
            get { return Voca.Date; }
            set
            {
                if (Voca.Date != value)
                {
                    Voca.Date = value;
                    RaisePropertyChanged("Date");
                }
            }
        }

        public long UnixTime
        {
            get { return ToUnixTime(Voca.Date); }
            set
            {
                if (Voca.Date != FromUnixTime(value))
                {
                    Voca.Date = FromUnixTime(value);
                    //RaisePropertyChanged("Date");
                }
            }
        }

        public int Key
        {
            get { return Voca.Key; }
            set
            {
                if (Voca.Key != value)
                {
                    Voca.Key = value;
                    //RaisePropertyChanged("Date");
                }
            }
        }
        #endregion

   

        public DateTime FromUnixTime( long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public long ToUnixTime( DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

    
    }
}
