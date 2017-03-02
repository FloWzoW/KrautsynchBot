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
    class clsPlugin_Giphy : pluginbase.clsPluginBase
    {
        clsGiphyClient gclient;
        private static string APIKEY = "dc6zaTOxFJmzC";

        public override string commandTag
        {
            get
            {
                return "g";
            }
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            
        }

        public clsPlugin_Giphy(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            gclient = new clsGiphyClient();
            gclient.SendChatMessageEvent += Gclient_SendChatMessageEvent;
        }

        private void Gclient_SendChatMessageEvent(string message)
        {
            this.SendConsoleMessage(message);
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {

            string argszusammengebaut = string.Empty;
            string arg = "";
            for (int i = 0; i < args.Count; i++)
            {
                if (!args[i].StartsWith("-"))
                {
                    argszusammengebaut = argszusammengebaut + args[i] + " ";

                }
                else
                {
                    if (triggeredFromMod)
                    {
                        arg = args[i];
                    }
                }
                
            }

            if (argszusammengebaut.Count() >= 1)
            {

                
                string url = gclient.getSearchQueryUrl(argszusammengebaut, APIKEY, arg);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    SendChatMessage("/me " + url + ".pic");
                }
                else
                {
                    SendChatMessage("/me gibbets ned.");
                }
                
            }
            
        }

        public override string getHelp()
        {
            return "Verwendet die Giphy-API, siehe https://github.com/Giphy/GiphyAPI. Gibt z.B. mit \"!g lol\" ein gif von giphy.com zum Stichwort \"lol\" zurück. Many thanks to Giphy.com ;)";
        }


        private class clsGiphyClient
        {
            private string Url = "http://api.giphy.com/v1/gifs/search?";

            public string getSearchQueryUrl(string search, string api_key, string arg)
            {
                string responseurl = "";
                string newUrl = this.Url + "q=" + Uri.EscapeUriString(search);
                newUrl += "&api_key=" + api_key;

                string searchUrl = "https://api.giphy.com/v1/gifs/search?q=" + search + "&api_key=dc6zaTOxFJmzC";
                this.SendChatMessageEvent(searchUrl);
                var rawResponse = new WebClient().DownloadString(searchUrl);
                GiphySearchResult response = JsonConvert.DeserializeObject<GiphySearchResult>(rawResponse);

                if (response.data.Count >= 1)
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    GiphySearchResult.Images imagelist = response.data[rdm.Next(0, response.data.Count)].images;
                    if (arg.ToLower() == "original")
                    {
                        responseurl = imagelist.original.url;
                    }
                    else
                    {
                        responseurl = imagelist.fixed_height_small.url;
                    }

                    this.SendChatMessageEvent(responseurl);
                    
                }

                return responseurl;
            }


            public delegate void SendChatMessageHandler(string message);
            public event SendChatMessageHandler SendChatMessageEvent;



            public class GiphySearchResult
            {
                public List<Datum> data { get; set; }
                public Meta meta { get; set; }
                public Pagination pagination { get; set; }

                public class FixedHeight
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string mp4 { get; set; }
                    public string mp4_size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedHeightStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class FixedHeightDownsampled
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedWidth
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string mp4 { get; set; }
                    public string mp4_size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedWidthStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class FixedWidthDownsampled
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedHeightSmall
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string mp4 { get; set; }
                    public string mp4_size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedHeightSmallStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class FixedWidthSmall
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string mp4 { get; set; }
                    public string mp4_size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class FixedWidthSmallStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class Downsized
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                }

                public class DownsizedStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class DownsizedLarge
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                }

                public class DownsizedMedium
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                }

                public class Original
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                    public string size { get; set; }
                    public string frames { get; set; }
                    public string mp4 { get; set; }
                    public string mp4_size { get; set; }
                    public string webp { get; set; }
                    public string webp_size { get; set; }
                }

                public class OriginalStill
                {
                    public string url { get; set; }
                    public string width { get; set; }
                    public string height { get; set; }
                }

                public class Looping
                {
                    public string mp4 { get; set; }
                }

                public class Images
                {
                    public FixedHeight fixed_height { get; set; }
                    public FixedHeightStill fixed_height_still { get; set; }
                    public FixedHeightDownsampled fixed_height_downsampled { get; set; }
                    public FixedWidth fixed_width { get; set; }
                    public FixedWidthStill fixed_width_still { get; set; }
                    public FixedWidthDownsampled fixed_width_downsampled { get; set; }
                    public FixedHeightSmall fixed_height_small { get; set; }
                    public FixedHeightSmallStill fixed_height_small_still { get; set; }
                    public FixedWidthSmall fixed_width_small { get; set; }
                    public FixedWidthSmallStill fixed_width_small_still { get; set; }
                    public Downsized downsized { get; set; }
                    public DownsizedStill downsized_still { get; set; }
                    public DownsizedLarge downsized_large { get; set; }
                    public DownsizedMedium downsized_medium { get; set; }
                    public Original original { get; set; }
                    public OriginalStill original_still { get; set; }
                    public Looping looping { get; set; }
                }

                public class Datum
                {
                    public string type { get; set; }
                    public string id { get; set; }
                    public string slug { get; set; }
                    public string url { get; set; }
                    public string bitly_gif_url { get; set; }
                    public string bitly_url { get; set; }
                    public string embed_url { get; set; }
                    public string username { get; set; }
                    public string source { get; set; }
                    public string rating { get; set; }
                    public string content_url { get; set; }
                    public string source_tld { get; set; }
                    public string source_post_url { get; set; }
                    public int is_indexable { get; set; }
                    public string import_datetime { get; set; }
                    public string trending_datetime { get; set; }
                    public Images images { get; set; }
                }

                public class Meta
                {
                    public int status { get; set; }
                    public string msg { get; set; }
                    public string response_id { get; set; }
                }

                public class Pagination
                {
                    public int total_count { get; set; }
                    public int count { get; set; }
                    public int offset { get; set; }
                }

            }

        }


    }


}
