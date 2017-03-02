using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;
using System.Net;
using Newtonsoft.Json;

namespace KrautsynchBot.plugins
{
    class clsPlugin_PopKey : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "p";
            }
        }

        private PopkeyApiClient popkeyClient; 

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            
        }

        public clsPlugin_PopKey(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            popkeyClient = new PopkeyApiClient();
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string APIKEY = "ZGVtbzplYTdiNjZmYjVlNjZjNjJkNmNmYTQ5ZmJlMGYyN2UwMDJjMjUxNGVlZDljNzVlYTlmNjVlOWQ3NTk4Y2I5YTkw";

            string test = popkeyClient.getSearchQueryUrl(args[0], APIKEY, "");

            if (string.IsNullOrEmpty(test))
            {
                SendChatMessage("/me gibts nich");
            }
            else
            {
                SendChatMessage(test + ".pic");
            }
            
        }

        public override string getHelp()
        {
            return "/me Popkey ist eine weitere Gif-Suchmaschine. Benutzen mit \"!p Cat\" um nach Kadsen zu suchen.";
        }

        private class PopkeyApiClient {

            //https://api.popkey.co/v2/media/search?q=
            private string ApiUrl = "https://api.popkey.co/v2/media/search?q=";

            public string getSearchQueryUrl(string search, string api_key, string arg)
            {

                string searchUrl = ApiUrl + search;

                WebClient webclient = new WebClient();

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(api_key));
                webclient.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", api_key);


                var rawResponse = webclient.DownloadString(searchUrl);
                PopKeySearchResultItem[] response = JsonConvert.DeserializeObject<PopKeySearchResultItem[]>(rawResponse);

                Random rdm = new Random(Guid.NewGuid().GetHashCode());

                string imageurl = response[rdm.Next(0, response.Count())].images.small.gif;

                return imageurl;
            }

            public class Source
            {
                public string origin { get; set; }
                public string url { get; set; }
                public string type { get; set; }
                public int width { get; set; }
                public int height { get; set; }
            }

            public class Original
            {
                public string gif { get; set; }
                public string mp4 { get; set; }
                public string jpg { get; set; }
                public string webp { get; set; }
            }

            public class MediumCropped
            {
                public string gif { get; set; }
                public string mp4 { get; set; }
                public string jpg { get; set; }
                public string webp { get; set; }
            }

            public class Small
            {
                public string gif { get; set; }
                public string mp4 { get; set; }
                public string jpg { get; set; }
                public string webp { get; set; }
            }

            public class SmallCropped
            {
                public string gif { get; set; }
                public string mp4 { get; set; }
                public string jpg { get; set; }
                public string webp { get; set; }
            }

            public class Images
            {
                public Original original { get; set; }
                public MediumCropped medium_cropped { get; set; }
                public Small small { get; set; }
                public SmallCropped small_cropped { get; set; }
            }

            public class Avatar
            {
                public string small { get; set; }
                public string medium { get; set; }
                public string large { get; set; }
            }

            public class User
            {
                public string id { get; set; }
                public string user_name { get; set; }
                public string type { get; set; }
                public string profile_url { get; set; }
                public Avatar avatar { get; set; }
                public string date_created { get; set; }
                public string date_modified { get; set; }
            }

            public class PopKeySearchResultItem
            {
                public string id { get; set; }
                public string slug { get; set; }
                public List<string> tags { get; set; }
                public string url { get; set; }
                public Source source { get; set; }
                public string rating { get; set; }
                public bool @public { get; set; }
                public bool disabled { get; set; }
                public Images images { get; set; }
                public User user { get; set; }
                public int view_count { get; set; }
                public string date_created { get; set; }
                public object date_modified { get; set; }
                public object date_popular { get; set; }
                public object date_curated { get; set; }
            }
           
        }
    }
}
