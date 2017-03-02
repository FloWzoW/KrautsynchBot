using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Würfel : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "würfel";
            }
        }

        public clsPlugin_Würfel(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            Random rdm = new Random(Guid.NewGuid().GetHashCode());
            int würfel = rdm.Next(1, 7);
            string msg = "";
            if (würfel == 6)
            {
                msg = Config.Plugin_Würfel_Message_6;
            }
            else if (würfel == 1)
            {
                msg = Config.Plugin_Würfel_Message_1;
            }
            else
            {
                msg = Config.Plugin_Würfel_Message;
            }
            msg = msg.Replace("[zahl]", würfel.ToString());
            msg = msg.Replace("[username]", triggeredFromUsername);
            SendChatMessage(msg);
        }

        public override string getHelp()
        {
            return "/me Ein einfaches Würfelspiel damit ihr einen Grund zum saufen habt.";
        }
    }
}
