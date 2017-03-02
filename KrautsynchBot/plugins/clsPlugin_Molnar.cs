using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Molnar : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "molnar";
            }
        }

        public clsPlugin_Molnar(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string[] molnaremotes = Config.Plugin_Molnar_Emotes.Split(";"[0]);

            int molnarcommands = 1;

            string result = "";

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(this.commandTag.ToLower()))
                {
                    molnarcommands = molnarcommands + 1;
                }
            }

            for (int i = 0; i < molnarcommands; i++)
            {
                Random rdmsoliemote = new Random(Guid.NewGuid().GetHashCode());
                result = result + " " + molnaremotes[rdmsoliemote.Next(0, molnaremotes.Count())];
            }
            SendChatMessage(result);
        }

        public override string getHelp()
        {
            return "/me Der Molnar-Command erstellt für Kohlio (Notkascher ist Sohn von Hurre).";
        }
    }
}
