using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Json;
namespace ConsoleApplication1
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    WebClient webclient = new WebClient();
        //    webclient.Encoding = System.Text.Encoding.UTF8;
        //    var json = webclient.DownloadString("http://api.wordreference.com/0.8/47a6b/json/enko/grin");


        //    JsonTextParser parser = new JsonTextParser();
        //    JsonObject obj = parser.Parse(json);
        //    JsonObjectCollection col = (JsonObjectCollection)obj;

        //   // String accno = (String)col["accno"].GetValue();
        //} 

        //   const string jsonText =
        //"{" +
        //" \"FirstValue\": 1.1," +
        //" \"SecondValue\": \"some text\"," +
        //" \"TrueValue\": true" +
        //"}";


        const string jsonText = "{\r\n\"term0\" : {\r\n\"PrincipalTranslations\" : {\r\n\t\"0\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"kill\", \"POS\" : \"vtr\", \"sense\" : \"cause death\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"죽이다,  살해하다\", \"POS\" : \"동 (타)\", \"sense\" : \"\"}, \"Note\" : \"\"},\r\n\t\"1\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"kill\", \"POS\" : \"vtr\", \"sense\" : \"put an end to\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"파괴하다, 지우다\", \"POS\" : \"동 (타)\", \"sense\" : \"\"}, \"Note\" : \"\"},\r\n\t\"2\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"kill\", \"POS\" : \"vtr\", \"sense\" : \"turn off\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"끄다\", \"POS\" : \"동 (타)\", \"sense\" : \"\"}, \"Note\" : \"\"}}},\r\n\"original\" : {\r\n\"Compounds\" : {\r\n\t\"0\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"dress to kill\", \"POS\" : \"\", \"sense\" : \"\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"아주 멋진 복장을 하다\", \"POS\" : \"\", \"sense\" : \"\"}, \"Note\" : \"\"},\r\n\t\"1\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"dressed to kill\", \"POS\" : \"\", \"sense\" : \"\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"아주 멋진 복장을 한\", \"POS\" : \"\", \"sense\" : \"\"}, \"Note\" : \"\"},\r\n\t\"2\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"kill time\", \"POS\" : \"\", \"sense\" : \"\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"하는 일 없이 시간을 보내다\", \"POS\" : \"\", \"sense\" : \"\"}, \"Note\" : \"\"},\r\n\t\"3\" :{\r\n\t\t\"OriginalTerm\" : { \"term\" : \"kill two birds with one stone\", \"POS\" : \"\", \"sense\" : \"\", \"usage\" : \"\"}, \r\n\t\t\"FirstTranslation\" : {\"term\" : \"일석이조\", \"POS\" : \"\", \"sense\" : \"\"}, \"Note\" : \"\"}}},\r\n\"Lines\" : \"End Reached\", \"END\" : true\r\n}";


        static void Main(string[] args)
        {
            // 1. parse sample

            Console.WriteLine();
            Console.WriteLine("Source data:");
            Console.WriteLine(jsonText);
            Console.WriteLine();

            JsonTextParser parser = new JsonTextParser();
            JsonObject obj = parser.Parse(jsonText);

            Console.WriteLine();
            Console.WriteLine("Parsed data with indentation in JSON data format:");
            Console.WriteLine(obj.ToString());
            Console.WriteLine();


            JsonUtility.GenerateIndentedJsonText = false;

            Console.WriteLine();
            Console.WriteLine("Parsed data without indentation in JSON data format:");
            Console.WriteLine(obj.ToString());
            Console.WriteLine();


            // enumerate values in json object
            Console.WriteLine();
            Console.WriteLine("Parsed object contains these nested fields:");

            JsonObjectCollection rootCol = obj as JsonObjectCollection;
            JsonObjectCollection term0 = (JsonObjectCollection)((JsonObjectCollection)rootCol["term0"])["PrincipalTranslations"];

            string meaning = "";
            int count = 0;
            foreach (JsonObjectCollection principalCol in term0 as JsonObjectCollection)
            {
                JsonObjectCollection OriginalTerm = (JsonObjectCollection)principalCol["OriginalTerm"];
                JsonObjectCollection FirstTranslation = (JsonObjectCollection)principalCol["FirstTranslation"];
                JsonObject term = FirstTranslation["term"];
                JsonObject pos = OriginalTerm["POS"];

                string strTerm = (string)term.GetValue();
                string strPos = (string)pos.GetValue();

                string voca = string.Format("{0}:({1}) {2}\n", count++, strPos, strTerm);





                meaning += voca;

                //string name = term.Name;
                //string value = string.Empty;

                //string type = term.GetValue().GetType().Name;

                //// try to get value.
                //switch (type)
                //{
                //    case "String":
                //        value = (string)term.GetValue();
                //        break;

                //    case "Double":
                //        value = field.GetValue().ToString();
                //        break;

                //    case "Boolean":
                //        value = field.GetValue().ToString();
                //        break;

                //    default:
                //        // in this sample we'll not parse nested arrays or objects.
                //        throw new NotSupportedException();
                //}

                //Console.WriteLine("{0} {1} {2}",
                //    name.PadLeft(15), type.PadLeft(10), value.PadLeft(15));
            }

            Console.Write(meaning);
            Console.WriteLine();


            // 2. generate sample
            Console.WriteLine();

            // root object
            JsonObjectCollection collection = new JsonObjectCollection();

            // nested values
            collection.Add(new JsonStringValue("FirstName", "Pavel"));
            collection.Add(new JsonStringValue("LastName", "Lazureykis"));
            collection.Add(new JsonNumericValue("Age", 23));
            collection.Add(new JsonStringValue("Email", "me@somewhere.com"));
            collection.Add(new JsonBooleanValue("HideEmail", true));

            Console.WriteLine("Generated object:");
            JsonUtility.GenerateIndentedJsonText = true;
            Console.WriteLine(collection);

            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
