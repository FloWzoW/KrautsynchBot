using KrautsynchBot.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Yoo : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "yoo";
            }
        }

        public clsPlugin_Yoo(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            if (triggeredFromMod)
            {
                this.SendChatMessage("yoo?");
            }
        }

        public override string getHelp()
        {
            return "/me triggert das Yoo Script. Können nur Mods wegen der Gründen. Wer mod ist gibt es mit !mods";
        }
    }
}
