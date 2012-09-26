using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLWD.Model;
using Google.GData.Spreadsheets;
using Google.GData.Documents;
using Google.GData.Client;
using CLWD.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace CLWD
{
    class SpreadSheetDB
    {
        #region Members
        GoogleOAuth2 _googleOAuth2;

        Random _random = new Random();
        string[] _word = { "Metallica", "Elvis Presley", "Madonna", "The Beatles", "The Rolling Stones", "Abba" };
        string[] _meaning = { "Islands in the Stream", "Imagine", "Living on a Prayer", "Enter Sandman", "A Little Less Conversation", "Wonderful World" };
        string doc_title = "The Google Words Spreadsheet";

        GOAuth2RequestFactory spreadsheetRequestFactory;
        GOAuth2RequestFactory docRequestFactory;
        SpreadsheetsService spreadsheetService;
        DocumentsService docService;
        Google.GData.Spreadsheets.SpreadsheetQuery query;
        SpreadsheetFeed spreadsheetfeed;
        SpreadsheetEntry spreadsheet;
        #endregion


        public SpreadSheetDB(GoogleOAuth2 googleOAuth2)
        {
            _googleOAuth2 = googleOAuth2;

            spreadsheetRequestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", _googleOAuth2.Parameters);
            spreadsheetService = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            spreadsheetService.RequestFactory = spreadsheetRequestFactory;

            docRequestFactory = new GOAuth2RequestFactory(null, "MyDocumentsListIntegration-v1", _googleOAuth2.Parameters);
            docService = new DocumentsService("MyDocumentsListIntegration-v1");
            docService.RequestFactory = docRequestFactory;


            query = new Google.GData.Spreadsheets.SpreadsheetQuery();
            query.Title = doc_title;
        }


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


        private string genTitle(int index)
        {
            return string.Format("Book {0}", index);
        }

        public void Init()
        {
            spreadsheetfeed = spreadsheetService.Query(query);

            if (spreadsheetfeed.Entries.Count == 0)
            {
                DocumentEntry newentry = new DocumentEntry();
                newentry.Title.Text = doc_title;

                // Add the document category
                newentry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

                // Make a request to the API and create the document.
                DocumentEntry newEntry = docService.Insert(DocumentsListQuery.documentsBaseUri, newentry);
            }

            spreadsheetfeed = spreadsheetService.Query(query);
            spreadsheet = (SpreadsheetEntry)spreadsheetfeed.Entries[0];

            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];
                        
            
            InitWorksheet(worksheet, genTitle(1));


        }

        public void InitWorksheet(WorksheetEntry worksheet, string title)
        {

      
                worksheet.Title.Text = title;
                worksheet.Cols = 5;
                worksheet.Rows = 500;
                worksheet.Update();


                CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                cellQuery.ReturnEmpty = ReturnEmptyCells.yes;
                cellQuery.MaximumRow = 1;
                CellFeed cellFeed = spreadsheetService.Query(cellQuery);

                foreach (CellEntry cell in cellFeed.Entries)
                {
                    if (cell.Title.Text == "A1")
                    {
                        cell.InputValue = "date";
                        cell.Update();
                    }
                    else if (cell.Title.Text == "B1")
                    {
                        cell.InputValue = "word";
                        cell.Update();
                    }
                    else if (cell.Title.Text == "C1")
                    {
                        cell.InputValue = "definition";
                        cell.Update();
                    }
                    else if (cell.Title.Text == "D1")
                    {
                        cell.InputValue = "key";
                        cell.Update();
                    }
                    else if (cell.Title.Text == "E1")
                    {
                        cell.InputValue = "checked";
                        cell.Update();
                    }
                }
      
        }


        public void UpdateVoca(BookViewModel bookVM, VocaViewModel vocaVM, long oldDate, int oldKey)
        {

            AtomLink listFeedLink = bookVM.Entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadsheetService.Query(listQuery);

            // Iterate through each row, printing its cell values.


            bool bDuplicated = false;
            foreach (ListEntry row in listFeed.Entries)
            {
                try
                {
                    ListEntry.Custom dateColumn = row.Elements[0];
                    ListEntry.Custom wordColumn = row.Elements[1];
                    ListEntry.Custom definitionColumn = row.Elements[2];
                    ListEntry.Custom KeyColumn = row.Elements[3];
                    ListEntry.Custom checkedColumn = row.Elements[4];
                    if (dateColumn.Value.Equals(oldDate.ToString()) && KeyColumn.Value.Equals(oldKey.ToString()))
                    {
                        //update
                        wordColumn.Value = vocaVM.Word; 
                        definitionColumn.Value = vocaVM.Definition;
                        dateColumn.Value = vocaVM.UnixTime.ToString();
                        KeyColumn.Value = vocaVM.Key.ToString();
                        checkedColumn.Value = vocaVM.Checked.ToString();
                        bDuplicated = true;
                        spreadsheetService.Update(row);
                        break;
                    }

                }
                catch (System.Exception ex)
                {

                }
            }

            if (!bDuplicated)
            {
                ListEntry row = new ListEntry();
                row.Elements.Add(new ListEntry.Custom() { LocalName = "word", Value = vocaVM.Word });
                row.Elements.Add(new ListEntry.Custom() { LocalName = "definition", Value = vocaVM.Definition });
                row.Elements.Add(new ListEntry.Custom() { LocalName = "date", Value = vocaVM.UnixTime.ToString() });
                row.Elements.Add(new ListEntry.Custom() { LocalName = "key", Value = vocaVM.Key.ToString() });
                row.Elements.Add(new ListEntry.Custom() { LocalName = "checked", Value = vocaVM.Checked.ToString() });

                // Send the new row to the API for insertion.
                spreadsheetService.Insert(listFeed, row);
            }

        }


        public void RemoveVoca(BookViewModel bookVM, VocaViewModel vocaVM)

        {


            AtomLink listFeedLink = bookVM.Entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadsheetService.Query(listQuery);


            foreach (ListEntry row in listFeed.Entries)
            {
                try
                {
                    ListEntry.Custom dateColumn = row.Elements[0];
                    ListEntry.Custom wordColumn = row.Elements[1];
                    ListEntry.Custom definitionColumn = row.Elements[2];
                    ListEntry.Custom KeyColumn = row.Elements[3];
                    ListEntry.Custom checkedColumn = row.Elements[4];

                    //if (wordColumn.Value.Equals(vocaVM.Word) && dateColumn.Value.Equals(dateString))
                    //{
                    if (dateColumn.Value.Equals(vocaVM.UnixTime.ToString()) && KeyColumn.Value.Equals(vocaVM.Key.ToString()))
                    {
                        row.Delete();
                    }

                }
                catch (System.Exception ex)
                {

                }
            }
        }

        public void RetrieveSpreadsheet(SpreadSheetViewModel spreadsheetVM)
        {

           WorksheetFeed wsFeed = spreadsheet.Worksheets;

           AtomEntryCollection entries = wsFeed.Entries;

           foreach (WorksheetEntry worksheet in entries)
           {

               BookViewModel bookVM = new BookViewModel(this, worksheet.Title.Text, worksheet);
               bookVM.Book.CollectionChanged += bookVM.VocaViewModel_PropertyChanged;
               RetrieveBook(bookVM);
               spreadsheetVM.SpreadSheet.Add(bookVM);

           }


           ICollectionView vs = CollectionViewSource.GetDefaultView(spreadsheetVM.SpreadSheet);
           vs.MoveCurrentToLast();


        }

        public void AddBook(SpreadSheetViewModel spreadsheetVM)
        {
            WorksheetEntry worksheet = new WorksheetEntry();
            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            string title = genTitle(wsFeed.Entries.Count + 1);
            worksheet.Cols = 5;
            worksheet.Rows = 500;
            worksheet.Title.Text = title;
            spreadsheetService.Insert(wsFeed, worksheet);

           
            wsFeed = spreadsheet.Worksheets;
            worksheet = (WorksheetEntry)wsFeed.Entries[wsFeed.Entries.Count - 1];

            InitWorksheet(worksheet, title);


            BookViewModel bookVM = new BookViewModel(this, title, worksheet);
            bookVM.Book.CollectionChanged += bookVM.VocaViewModel_PropertyChanged;
            spreadsheetVM.SpreadSheet.Add(bookVM);


            ICollectionView vs = CollectionViewSource.GetDefaultView(spreadsheetVM.SpreadSheet);
            vs.MoveCurrentTo(bookVM);

        }


        public void DeleteBook(SpreadSheetViewModel spreadsheetVM, BookViewModel bookVM)

        {
             WorksheetFeed wsFeed = spreadsheet.Worksheets;


            foreach (WorksheetEntry worksheet in wsFeed.Entries)
            {
                if (bookVM.BookTitle.Equals(worksheet.Title.Text))
                {
                    worksheet.Delete();
                    spreadsheetVM.SpreadSheet.Remove(bookVM);
                    
                }

            }

        }


        public void RetrieveBook(BookViewModel bookVM)
        {


            AtomLink listFeedLink = bookVM.Entry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadsheetService.Query(listQuery);


            foreach (ListEntry row in listFeed.Entries)
            {

                VocaViewModel vocaVM = new VocaViewModel();

                try
                {
                    foreach (ListEntry.Custom element in row.Elements)
                    {

                        switch (element.LocalName)
                        {
                            case "word":
                                vocaVM.Word = element.Value;
                                break;

                            case "definition":
                                vocaVM.Definition = element.Value;
                                break;

                            case "date":
                                vocaVM.UnixTime = long.Parse(element.Value);
                                break;

                            case "key":
                                vocaVM.Key = int.Parse(element.Value);
                                break;

                            case "checked":
                                vocaVM.Checked = bool.Parse(element.Value);
                                break;

                        }
                    }
                    bookVM.Book.Add(vocaVM);
                }
                catch (System.Exception ex)
                {

                }

            }
            ICollectionView vs = CollectionViewSource.GetDefaultView(bookVM.Book);
            

            vs.SortDescriptions.Add(new SortDescription("Checked", ListSortDirection.Descending));
            vs.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            vs.MoveCurrentToLast();


        }
    }
}
