using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.plugins.pluginbase;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Info : pluginbase.clsPluginBase
    {
        public clsPlugin_Info(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            //some code
        }


        public override string commandTag
        {
            get
            {
                return "info";
            }
        }


        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string mötter = Config.CommandHandler_Moderatoren;
            mötter = mötter.Replace(";", ", ");
            this.SendChatMessage("/me ist der Krautsynch-Bot von BerndBoy2 <3 Version: 0.5");
            this.SendChatMessage("/me Letztes Update: 02.03.17, NEU: Jetzt mit !Werbung für die Werbepause zwischendurch");
        }

        public override string getHelp()
        {
            return "/me Zeigt laufende Bot Version und das letzte Update. Mötter sind jetzt unter !mods einzusehen.";
        }
    }
}
