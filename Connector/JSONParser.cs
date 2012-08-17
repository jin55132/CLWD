using System.Net.Json;

namespace CLWD.Connector
{
    public class JSONParser
    {
        private JsonTextParser parser;
        private JsonObjectCollection col;
        public JSONParser(string strData)
        {
            parser = new JsonTextParser();
            col = (JsonObjectCollection)parser.Parse(strData);
        }
        /**
        * (확장)JsonObjectCollection.GetStringValue(string name)
        * 
        * param name: 찾을 엘리먼트의 이름
        * return: 엘리먼트의 값
        * 
        */
        public string GetStringValue(string name)
        {
            try
            {
                return (string)col[name].GetValue();
            }
            catch
            {
                return null;
            }
        }

        /**
         * (확장)JsonObjectCollection.GetStringArrayValue(string name)
         * 
         * param name: 찾을 엘리먼트의 이름
         * return: 엘리먼트의 배열값
         * 
         */
        public string[] GetStringArrayValue(string name)
        {
            try
            {
                JsonArrayCollection items = (JsonArrayCollection)col[name];
                string[] item = new string[items.Count];
                for (int count = 0; count < items.Count; count++)
                    item[count] = ((JsonStringValue)items[count]).Value;

                if (item.Length == 0)
                {
                    return null;
                }
                else
                {
                    return item;
                }
            }
            catch
            {
                return null;
            }
        }
        /**
         * (확장)JsonObjectCollection.Remove(string name)
         * 
         * param name: 삭제할 엘리먼트의 이름
         * 
         */
        public void Remove(string name)
        {
            try
            {
                JsonObject obj = col[name];
                col.Remove(obj);
            }
            catch
            {
            }
        }
    }



}
