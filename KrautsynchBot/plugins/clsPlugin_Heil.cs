using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{

    class clsPlugin_Heil : pluginbase.clsPluginBase
    {
        public clsPlugin_Heil(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            //some code
        }
        public override string commandTag
        {
            get
            {
                return "heil";
            }
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            SendChatMessage("Ja Heil! /heilchen");
        }

        public override string getHelp()
        {
            return "Heil elo! Heil armbernd! /heilchen";
        }
    }
}
