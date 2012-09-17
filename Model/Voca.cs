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
        string _definition;
        DateTime _date;
        #endregion


        #region Properties
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                //RaisePropertyChanged("Word");
            }
        }

        public string Definition
        {
            get { return _definition; }
            set
            {
                _definition = value;
                //RaisePropertyChanged("Definition");
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                //RaisePropertyChanged("Date");
            }
        }
        #endregion
    }
}
