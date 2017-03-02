using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Lauerstein : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "lauerstein";
            }
        }

        public clsPlugin_Lauerstein(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string[] soliemotes = Config.Plugin_Lauerstein_Messages.Split(";"[0]);

            int solicommands = 1;

            string result = "";

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(this.commandTag.ToLower()))
                {
                    solicommands = solicommands + 1;
                }
            }

            for (int i = 0; i < solicommands; i++)
            {
                Random rdmsoliemote = new Random(Guid.NewGuid().GetHashCode());
                result = result + " " + soliemotes[rdmsoliemote.Next(0, soliemotes.Count())];
            }
            SendChatMessage(result);
        }

        public override string getHelp()
        {
            return "/me BUSFAHRER RAUS! /bus /2bus.";
        }
    }
}
