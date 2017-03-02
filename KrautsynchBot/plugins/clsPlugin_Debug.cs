using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Debug : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "debug";
            }
        }

        public clsPlugin_Debug(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            clsPoll poll = new clsPoll();
            poll.question = "Testfrage! Testfrage?";
            poll.Message_10SecoundsLeft = "10 secounds left message";
            poll.Message_30SecoundsLeft = "30 secounds left message";
            poll.waitingForResultInSecounds = 40;
            clsPollOption o1 = new clsPollOption();
            clsPollOption o2 = new clsPollOption();
            o1.text = "option 1";
            o1.value = "option 1";
            o1.winningMessages.Add("option 1 won");
            o1.OptionWonEvent += O1_OptionWonEvent;
            o2.text = "option 2";
            o2.value = "option 2";
            o2.winningMessages.Add("option 2 won");
            o2.OptionWonEvent += O1_OptionWonEvent;
            poll.options.Add(o1);
            poll.options.Add(o2);
            pollhandler.OpenPoll(poll);
        }

        private void O1_OptionWonEvent(string value)
        {
            SendChatMessage("event fired, option value: " + value);
        }

        public override string getHelp()
        {
            return "/me /2mysides";
        }
    }
}
