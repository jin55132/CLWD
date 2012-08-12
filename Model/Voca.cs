using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLWD.Model
{
    class Voca : ObservableObject
    {
        #region Members
        string _word;
        string _meaning;
        #endregion


        #region Properties
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                RaisePropertyChanged("Word");
            }
        }

        public string Meaning
        {
            get { return _meaning; }
            set
            {
                _meaning = value;
                RaisePropertyChanged("Meaning");
            }
        }
        #endregion
    }
}
