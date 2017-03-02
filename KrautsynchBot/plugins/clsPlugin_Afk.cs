using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Afk : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "afk";
            }
        }

        public clsPlugin_Afk(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            List<string> afkusernamesfound = this.searchUsernamesFromArgs(args);
            foreach (string username in afkusernamesfound)
            {
                SendChatMessage(this.userlistwrapper.getAfkTimerString(username));
            }
        }

        public override string getHelp()
        {
            return "/me Zeigt an wie lang ein User schon afk ist.";
        }
    }
}
