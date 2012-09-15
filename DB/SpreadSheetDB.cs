using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLWD.Model;
using Google.GData.Spreadsheets;
using Google.GData.Documents;
using Google.GData.Client;

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
            // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            Google.GData.Spreadsheets.SpreadsheetQuery query = new Google.GData.Spreadsheets.SpreadsheetQuery();
            query.Title = doc_title;

            // Make a request to the API and get all spreadsheets.
            SpreadsheetFeed spreadsheetfeed = spreadsheetService.Query(query);

            if (spreadsheetfeed.Entries.Count == 0)
            {
                DocumentEntry newentry = new DocumentEntry();
                newentry.Title.Text = doc_title;

                // Add the document category
                newentry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

                // Make a request to the API and create the document.
                DocumentEntry newEntry = docService.Insert(
                DocumentsListQuery.documentsBaseUri, newentry);

                spreadsheetfeed = spreadsheetService.Query(query);

            }


            if (spreadsheetfeed.Entries.Count == 0)
            {
                Console.WriteLine("Unable to create a sheet");
            }
            else
            {
                SpreadsheetEntry spreadsheet = (SpreadsheetEntry)spreadsheetfeed.Entries[0];
                WorksheetFeed wsFeed = spreadsheet.Worksheets;
                WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];

                // Update the local representation of the worksheet.
                if (!worksheet.Title.Text.Equals("My Word List") || !(worksheet.Cols == 3) || !(worksheet.Rows == 200))
                {

                    worksheet.Title.Text = "My Word List";
                    worksheet.Cols = 3;
                    worksheet.Rows = 200;

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
                            cell.InputValue = "word";
                            cell.Update();
                        }
                        else if (cell.Title.Text == "B1")
                        {
                            cell.InputValue = "definition";
                            cell.Update();
                        }
                        else if (cell.Title.Text == "C1")
                        {
                            cell.InputValue = "date";
                            cell.Update();
                        }
                    }

                }
            }
        }
    }
}
