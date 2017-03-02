using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Frage : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "frage";
            }
        }

        public clsPlugin_Frage(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            //evtl für später: "ich habe diese Frage schon beantwortet"

            string[] nachrichten = Config.Plugin_Frage_Messages.Split(";"[0]);
            Random rdm = new Random(Guid.NewGuid().GetHashCode());
            string msg = "/me ";
            string[] aussagen = Config.Plugin_Frage_Aussage.Split(';');
            msg = msg + aussagen[rdm.Next(0, aussagen.Count())] + " ";
            msg = msg + nachrichten[rdm.Next(0, nachrichten.Count())];
            msg = msg.Replace("[username]", userlistwrapper.getRandomUsernameFromChatlist());
            SendChatMessage(msg);
        }

        public override string getHelp()
        {
            return "/me Stelle mir eine Frage mit \"!frage [frage hier einfügen]\" Ich sage immer die Wahrheit!";
        }
    }
}
