using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLWD.Model;
using Google.GData.Spreadsheets;
using Google.GData.Documents;
using Google.GData.Client;
using CLWD.ViewModel;

namespace CLWD
{
    class SpreadSheetDB
    {
        #region Members
        GoogleOAuth2 _googleOAuth2;

        Random _random = new Random();
        string[] _word = { "Metallica", "Elvis Presley", "Madonna", "The Beatles", "The Rolling Stones", "Abba" };
        string[] _meaning = { "Islands in the Stream", "Imagine", "Living on a Prayer", "Enter Sandman", "A Little Less Conversation", "Wonderful World" };
        string doc_title = "CLWD Spreadsheet";

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

            // Update the local representation of the worksheet.
            if (!worksheet.Title.Text.Equals("My Word List") || !(worksheet.Cols == 3) || !(worksheet.Rows == 500))
            {

                worksheet.Title.Text = "My Word List";
                worksheet.Cols = 3;
                worksheet.Rows = 500;

                // Send the local representation of the worksheet to the API for
                // modification.
                worksheet.Update();
                // Fetch the cell feed of the worksheet.
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
                }


            }
        }

        public void Update(VocaViewModel vocaVM, long oldDate)
        {
            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];


            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
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


                    if (dateColumn.Value.Equals(oldDate.ToString()))
                    {
                        //update
                        wordColumn.Value = vocaVM.Word; // this will fire propertychanged event.
                        definitionColumn.Value = vocaVM.Definition;
                        dateColumn.Value = vocaVM.UnixTime.ToString();
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
                
                // Send the new row to the API for insertion.
                spreadsheetService.Insert(listFeed, row);
            }

        }

        public void Remove(VocaViewModel vocaVM)
        {
            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];


            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadsheetService.Query(listQuery);

          
            foreach (ListEntry row in listFeed.Entries)
            {
                try
                {
                    ListEntry.Custom dateColumn = row.Elements[0];
                    ListEntry.Custom wordColumn = row.Elements[1];
                    ListEntry.Custom definitionColumn = row.Elements[2];
                 
                    string dateString = vocaVM.UnixTime.ToString();

                    if (wordColumn.Value.Equals(vocaVM.Word) && dateColumn.Value.Equals(dateString))
                    {
                        row.Delete();
                    }

                }
                catch (System.Exception ex)
                {

                }
            }
        }

        public void Retrieve(BookViewModel bookVM)
        {

            WorksheetFeed wsFeed = spreadsheet.Worksheets;
            WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];


            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = spreadsheetService.Query(listQuery);


            // Iterate through each row, printing its cell values.
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

                        }
                    }
                    bookVM.Insert(vocaVM);
                }
                catch (System.Exception ex)
                {

                }



            }



        }
    }
}
