using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;
namespace KrautsynchBot.plugins
{
    class clsPlugin_Tourette : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "tourette";
            }
        }

        private SortedList<string, trigger> usernameTriggered;


        public clsPlugin_Tourette(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.usernameTriggered = new SortedList<string, trigger>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {

            if (this.usernameTriggered.ContainsKey(triggeredFromUsername))
            {
                if (this.usernameTriggered[triggeredFromUsername].checkTrigger())
                {
                    this.usernameTriggered[triggeredFromUsername].doTrigger();
                } else
                {
                    return;
                }
            } else
            {
                trigger t = new trigger();
                t.doTrigger();
                this.usernameTriggered.Add(triggeredFromUsername, t);
            }


            string[] tourettemessages = Config.Plugin_Tourette_Messages.Split(";"[0]);

            int molnarcommands = 1;

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(this.commandTag.ToLower()))
                {
                    molnarcommands = molnarcommands + 1;
                }
            }
            if (molnarcommands >= 4)
            {
                molnarcommands = 3;
            }
            for (int i = 0; i < molnarcommands; i++)
            {
                Random rdmTouretteMessage = new Random(Guid.NewGuid().GetHashCode());
                SendChatMessage(tourettemessages[rdmTouretteMessage.Next(0, tourettemessages.Count())].ToUpper());
            }
        }

        public override string getHelp()
        {
            return "/me Der Tourette-Command nach einer Idee von nichtdieMAMA. Die Sprüche kommen aus der Community, siehe https://piratenpad.de/p/qrFXJTNDWqRP3 Dieser Befehl ist limitiert auf 3 Stk / Stunde / User";
        }

        private class trigger
        {
            public List<DateTime> triggertimes;

            public trigger()
            {
                this.triggertimes = new List<DateTime>();
            }

            public bool checkTrigger()
            {
                this.checktimes();
                if (this.triggertimes.Count >= 3)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public void doTrigger()
            {
                this.checktimes();
                this.triggertimes.Add(DateTime.Now);
            }

            private void checktimes()
            {
                for (int i = this.triggertimes.Count - 1; i >= 0; i--)
                {
                    double zeit = DateTime.Now.Subtract(this.triggertimes[i]).TotalMinutes;
                    if (zeit >= 60)
                    {
                        triggertimes.RemoveAt(i);
                    }
                }
            }

        }
    }
}
