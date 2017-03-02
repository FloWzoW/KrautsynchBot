using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Mods : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "mods";
            }
        }
        public clsPlugin_Mods(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            SendChatMessage("Derzeit als Mod hinterlegt: " + Config.CommandHandler_Moderatoren.Replace(";", ", "));
        }

        public override string getHelp()
        {
            return "/me zeigt halt die mods an /facepalm";
        }
    }
}
