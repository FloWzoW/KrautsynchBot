using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Willkür : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "willkür";
            }
        }

        public clsPlugin_Willkür(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            SendChatMessage("Willkür ist noch nicht Fertig. ");
        }

        public override string getHelp()
        {
            return "/me Willkür setzt das vom User zuletzt addierte Video an Stelle 2 in der Playlist. Kann nur einmal am Tag pro User verwendet werden. Willkür war eine Idee von Alkoholiker.";
        }
    }
}
