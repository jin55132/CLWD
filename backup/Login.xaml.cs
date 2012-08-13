using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using Google.GData.Documents;
namespace CLWD
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            //GoogleLogin google = new GoogleLogin();
            //this.DataContext = google;
            //OAuth2();
            //NormalAuth();
            //GetAccountInfo();
            //TimeStamp();
            //Update();
            //list();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            

        }

        //public void OAuth2()
        //{
        //    ////////////////////////////////////////////////////////////////////////////
        //    // STEP 1: Configure how to perform OAuth 2.0
        //    ////////////////////////////////////////////////////////////////////////////

        //    // TODO: Update the following information with that obtained from
        //    // https://code.google.com/apis/console. After registering
        //    // your application, these will be provided for you.

        //    string CLIENT_ID = "993296641183.apps.googleusercontent.com";

        //    // This is the OAuth 2.0 Client Secret retrieved
        //    // above.  Be sure to store this value securely.  Leaking this
        //    // value would enable others to act on behalf of your application!
        //    string CLIENT_SECRET = "u6ET18iH007SJ_jo6WdcNlA3";

        //    // Space separated list of scopes for which to request access.
        //    string SCOPE = "https://docs.google.com/feeds/ https://docs.googleusercontent.com/ https://spreadsheets.google.com/feeds/";

        //    // This is the Redirect URI for installed applications.
        //    // If you are building a web application, you have to set your
        //    // Redirect URI at https://code.google.com/apis/console.
        //    string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        //    ////////////////////////////////////////////////////////////////////////////
        //    // STEP 2: Set up the OAuth 2.0 object
        //    ////////////////////////////////////////////////////////////////////////////

        //    // OAuth2Parameters holds all the parameters related to OAuth 2.0.
        //    OAuth2Parameters parameters = new OAuth2Parameters();

        //    // Set your OAuth 2.0 Client Id (which you can register at
        //    // https://code.google.com/apis/console).
        //    parameters.ClientId = CLIENT_ID;

        //    // Set your OAuth 2.0 Client Secret, which can be obtained at
        //    // https://code.google.com/apis/console.
        //    parameters.ClientSecret = CLIENT_SECRET;

        //    // Set your Redirect URI, which can be registered at
        //    // https://code.google.com/apis/console.
        //    parameters.RedirectUri = REDIRECT_URI;

        //    ////////////////////////////////////////////////////////////////////////////
        //    // STEP 3: Get the Authorization URL
        //    ////////////////////////////////////////////////////////////////////////////

        //    // Set the scope for this particular service.
        //    parameters.Scope = SCOPE;

        //    // Get the authorization url.  The user of your application must visit
        //    // this url in order to authorize with Google.  If you are building a
        //    // browser-based application, you can redirect the user to the authorization
        //    // url.
        //    string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
        //    Console.WriteLine(authorizationUrl);
        //    Console.WriteLine("Please visit the URL above to authorize your OAuth "
        //      + "request token.  Once that is complete, type in your access code to "
        //      + "continue...");
        //    parameters.AccessCode = Console.ReadLine();

        //    ////////////////////////////////////////////////////////////////////////////
        //    // STEP 4: Get the Access Token
        //    ////////////////////////////////////////////////////////////////////////////

        //    // Once the user authorizes with Google, the request token can be exchanged
        //    // for a long-lived access token.  If you are building a browser-based
        //    // application, you should parse the incoming request token from the url and
        //    // set it in OAuthParameters before calling GetAccessToken().
        //    OAuthUtil.GetAccessToken(parameters);
        //    string accessToken = parameters.AccessToken;
        //    Console.WriteLine("OAuth Access Token: " + accessToken);

        //    ////////////////////////////////////////////////////////////////////////////
        //    // STEP 5: Make an OAuth authorized request to Google
        //    ////////////////////////////////////////////////////////////////////////////

        //    //// Initialize the variables needed to make the request
        //    //GOAuth2RequestFactory requestFactory =
        //    //    new GOAuth2RequestFactory(null, "MyDocumentsListIntegration-v1", parameters);
        //    //DocumentsService service = new DocumentsService("MyDocumentsListIntegration-v1");
        //    //service.RequestFactory = requestFactory;

        //    // Make the request to Google
        //    // See other portions of this guide for code to put here...
        //}

        //public void NormalAuth(string id, string pin)
        //{
        //    //DocumentsService service = new DocumentsService("MyDocumentsListIntegration-v1");
        //    //service.setUserCredentials(id, pin);





        //}

        //public void GetAccountInfo()
        //{


        //    SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");

        //    service.setUserCredentials("operator1732", "");
        //    // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
        //    Google.GData.Spreadsheets.SpreadsheetQuery query = new Google.GData.Spreadsheets.SpreadsheetQuery();
        //    query.Title = "operator1732@CLWD";

        //    // Make a request to the API and get all spreadsheets.
        //    SpreadsheetFeed feed = service.Query(query);

        //    if (feed.Entries.Count == 0)
        //    {
        //        DocumentsService docservice = new DocumentsService("MyDocumentsListIntegration-v1");
        //        docservice.setUserCredentials("operator1732", "Rnrnfld86");
        //        // TODO: Authorize the service object for a specific user (see Authorizing requests)

        //        // Instantiate a DocumentEntry object to be inserted.
        //        DocumentEntry newentry = new DocumentEntry();

        //        // Set the document title
        //        newentry.Title.Text = "operator1732@CLWD";

        //        // Add the document category
        //        newentry.Categories.Add(DocumentEntry.SPREADSHEET_CATEGORY);

        //        // Make a request to the API and create the document.
        //        DocumentEntry newEntry = docservice.Insert(
        //        DocumentsListQuery.documentsBaseUri, newentry);

        //        feed = service.Query(query);

        //    }


        //    if (feed.Entries.Count == 0)
        //    {
        //        Console.WriteLine("Unable to create a sheet");
        //    }
        //    else
        //    {
        //        SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
        //        WorksheetFeed wsFeed = spreadsheet.Worksheets;
        //        WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];

        //        // Update the local representation of the worksheet.
        //        if (!worksheet.Title.Text.Equals("My Word List") || !(worksheet.Cols == 3) || !(worksheet.Rows == 200))
        //        {

        //            worksheet.Title.Text = "My Word List";
        //            worksheet.Cols = 3;
        //            worksheet.Rows = 200;

        //            // Send the local representation of the worksheet to the API for
        //            // modification.
        //            worksheet.Update();
        //            // Fetch the cell feed of the worksheet.
        //            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
        //            cellQuery.ReturnEmpty = ReturnEmptyCells.yes;
        //            cellQuery.MaximumRow = 1;
        //            CellFeed cellFeed = service.Query(cellQuery);


        //            foreach (CellEntry cell in cellFeed.Entries)
        //            {
        //                if (cell.Title.Text == "A1")
        //                {
        //                    cell.InputValue = "word";
        //                    cell.Update();
        //                }
        //                else if (cell.Title.Text == "B1")
        //                {
        //                    cell.InputValue = "definition";
        //                    cell.Update();
        //                }
        //                else if (cell.Title.Text == "C1")
        //                {
        //                    cell.InputValue = "date";
        //                    cell.Update();
        //                }
        //            }

        //        }






        //        // Define the URL to request the list feed of the worksheet.
        //        AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

        //        // Fetch the list feed of the worksheet.
        //        ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
        //        ListFeed listFeed = service.Query(listQuery);

               
        //        ListEntry row = new ListEntry();
        //        // Create a local representation of the new row.
        //        row.Elements.Add(new ListEntry.Custom() { LocalName = "word", Value = "Joe" });
        //        row.Elements.Add(new ListEntry.Custom() { LocalName = "definition", Value = "Smith" });
        //        row.Elements.Add(new ListEntry.Custom() { LocalName = "date", Value = "26" });
        //       // Send the new row to the API for insertion.
        //        service.Insert(listFeed, row);

        //     }






        //}

        //private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");

        //    service.setUserCredentials("operator1732", "Rnrnfld86");
        //    // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
        //    Google.GData.Spreadsheets.SpreadsheetQuery query = new Google.GData.Spreadsheets.SpreadsheetQuery();
        //    query.Title = "operator1732@CLWD";

        //    // Make a request to the API and get all spreadsheets.
        //    SpreadsheetFeed feed = service.Query(query);

        //    SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
        //    WorksheetFeed wsFeed = spreadsheet.Worksheets;
        //    WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];
        //}
    }
}
