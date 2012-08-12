using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLWD
{
    class VocaDB
    {
        #region Members
        Random _random = new Random();
        string[] _word = { "Metallica", "Elvis Presley", "Madonna", "The Beatles", "The Rolling Stones", "Abba" };
        string[] _meaning = { "Islands in the Stream", "Imagine", "Living on a Prayer", "Enter Sandman", "A Little Less Conversation", "Wonderful World" };
        #endregion

        #region Properties
        public string GetRandomWord
        {
            get { return _word[_random.Next(_word.Length)]; }
        }

        public string GetRandomMeaning
        {
            get { return _meaning[_random.Next(_meaning.Length)]; }
        }
        #endregion
    }
}
