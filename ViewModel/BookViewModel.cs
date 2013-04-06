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
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace CLWD.ViewModel
{
    [Serializable]
    class BookViewModel : BaseViewModel
    {
        #region Members
        private SpreadSheetDB _database;
        // private GoogleOAuth2LoginViewModel _loginViewModel;
        private ObservableCollection<VocaViewModel> _book = new ObservableCollection<VocaViewModel>();
       
        #endregion



        #region Properties

        public WorksheetEntry Entry { get; set; }


        public string BookTitle { get; set; }

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

        public BookViewModel(SpreadSheetDB db, string title, WorksheetEntry entry)
        {
            BookTitle = title;
            _database = db;
            Entry = entry;

        }

        #endregion




        public void VocaViewModel_PropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
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

                            _database.RemoveVoca(this, item);

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

                        _database.UpdateVoca(this, vocaVM, oldDate, oldKey);
        
                    }

                    catch
                    {

                    }

                };

                new Thread(start).Start();

            }

            else if (e.PropertyName.Equals("Checked"))
            {
                VocaViewModel vocaVM = ((VocaViewModel)sender);


                ThreadStart start = delegate()
                {

                    //WordReference 통해 단어의미 획득..json
                    long oldDate = vocaVM.UnixTime;
                    int oldKey = vocaVM.Key;

                    try
                    {

                        _database.UpdateVoca(this, vocaVM, oldDate, oldKey);

                    }
                    catch
                    {

                    }

                };

                new Thread(start).Start();
            }


        }

        public string extractDefinition(JsonObjectCollection collection)
        {



            int count = 0;
            string def = "";

            if (collection == null)
                return def;

            foreach (JsonObjectCollection cols in collection as JsonObjectCollection)
            {
                JsonObjectCollection OriginalTerm = (JsonObjectCollection)cols["OriginalTerm"];
                JsonObjectCollection FirstTranslation = (JsonObjectCollection)cols["FirstTranslation"];
                JsonObject term = FirstTranslation["term"];
                JsonObject pos = OriginalTerm["POS"];

                string strTerm = (string)term.GetValue();
                string strPos = (string)pos.GetValue();

                string voca;
                //if (collection.Count == count + 1)
                //{
                //    voca = string.Format("({1}) {2}", strPos, strTerm);
                //}
                //else
                {
                    voca = string.Format("({0}) {1}\n", strPos, strTerm);
                }

                def += voca;
            }

            return def;
        }

        public string jsonProcessor(string jsoninput)
        {
            // Thread.Sleep(1000);
            string meaning = "";

            JsonTextParser parser = new JsonTextParser();
            JsonObject obj = parser.Parse(jsoninput);

            JsonObjectCollection rootCol = obj as JsonObjectCollection;

            JsonObjectCollection principle = (JsonObjectCollection)((JsonObjectCollection)rootCol["term0"])["PrincipalTranslations"];
            JsonObjectCollection entries = (JsonObjectCollection)((JsonObjectCollection)rootCol["term0"])["Entries"];
            JsonObjectCollection additional = (JsonObjectCollection)((JsonObjectCollection)rootCol["term0"])["AdditionalTranslations"];



            meaning += extractDefinition(principle); ;
            meaning += extractDefinition(entries);
            meaning += extractDefinition(additional); ;

            if (meaning.Length > 0)
            {
                meaning = meaning.Remove(meaning.Length - 1);
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

    }
}
