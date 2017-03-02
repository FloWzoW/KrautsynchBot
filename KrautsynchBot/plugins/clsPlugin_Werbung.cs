using KrautsynchBot.classes;
using KrautsynchBot.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Werbung : pluginbase.clsPluginBase
    {
        static string UrlZusatz = ".botwerbung";

        public override string commandTag
        {
            get
            {
                return "werbung";
            }
        }

        public clsPlugin_Werbung(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string[] werbungimages = Config.Plugin_Werbung_Images.Split(";"[0]);

            int solicommands = 1;

            string result = "/lahm *WERBUNG* ";

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(this.commandTag.ToLower()))
                {
                    solicommands = solicommands + 1;
                }
            }

            for (int i = 0; i < solicommands; i++)
            {
                Random rdmwerbungimg = new Random(Guid.NewGuid().GetHashCode());
                result = result + " " + werbungimages[rdmwerbungimg.Next(0, werbungimages.Count())] + UrlZusatz;
            }
            result += " *WERBUNG*";
            SendChatMessage(result);
        }

        public override string getHelp()
        {
            return "/me Für die Werbepause Zwischendurch. Idee kam Berndboy2 beim fappen ohne Adblocker /fap";
        }
    }
}
