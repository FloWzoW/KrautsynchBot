using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Poll : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "poll";
            }
        }

        SortedList<string, clsUserPollCounter> usercounterlist;

        public clsPlugin_Poll(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.usercounterlist = new SortedList<string, clsUserPollCounter>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            if (this.pollhandler.checkIfPollIsRunning())
            {
                return;
            }
            string usernametolower = triggeredFromUsername.ToLower();
            if (!this.usercounterlist.ContainsKey(usernametolower))
            {
                this.usercounterlist.Add(usernametolower, new clsUserPollCounter());
                this.usercounterlist[usernametolower].pollsused = 1;
            }
            else
            {
                if (!this.usercounterlist[usernametolower].checkIfUserCanOpenPoll())
                {
                    SendChatMessage(Config.Plugin_Poll_NoMoreVotesMessage.Replace("[username]", triggeredFromUsername));
                    return;
                }
            }

            string argszusammengebaut = string.Empty;
            for (int i = 0; i < args.Count; i++)
            {
                argszusammengebaut = argszusammengebaut + args[i] + " ";
            }

            List<string> NewArgs = argszusammengebaut.Split(";"[0]).ToList();
            if (string.IsNullOrWhiteSpace(NewArgs.Last()))
            {
                NewArgs.RemoveAt(NewArgs.Count - 1);
            }
            if (NewArgs.Count() == 1 || NewArgs.Count() >= 7)
            {
                return;
            }

            clsPoll poll = new clsPoll("Userpoll " + triggeredFromUsername + " - " + NewArgs[0], Config.Plugin_Poll_30secondsMessage.Replace("[username]", triggeredFromUsername), Config.Plugin_Poll_10secondsMessage.Replace("[username]", triggeredFromUsername), 90);
            poll.options = new List<clsPollOption>();

            for (int i = 1; i < NewArgs.Count(); i++)
            {
                if (!string.IsNullOrWhiteSpace(NewArgs[i]))
                {
                    clsPollOption option = new clsPollOption(NewArgs[i].Replace("; ", ";"), "");
                    option.winningMessages = new List<string>();
                    option.winningMessages.Add("/me Option " + i.ToString() + " hat gewonnen:  *" + NewArgs[0] + " -> " + NewArgs[i] +  "* /applaus");
                    poll.options.Add(option);
                }
            }

            this.pollhandler.OpenPoll(poll);
        }

        public override string getHelp()
        {
            return "/me Jeder User kann 2 mal am Tag einen Poll eröffnen. Syntax: \"!poll [frage];[option 1];[option 2];...\" Passt auf das Semikolon richtig zu verwenden. Maximal nur 5 Optionen, bei mehr wird ignoriert. Dein Poll läuft 90 Sekunden.";
        }

        private class clsUserPollCounter
        {
            private DateTime datepoll;
            public int pollsused;

            public clsUserPollCounter()
            {
                this.datepoll = DateTime.Now;
                this.pollsused = 0;
            }

            public bool checkIfUserCanOpenPoll()
            {
                bool result = false;

                if (this.datepoll.Date == DateTime.Now.Date)
                {
                    if (this.pollsused >= Convert.ToInt32(Config.Plugin_Poll_PollsPerDay))
                    {
                        return result;
                    }
                    else
                    {
                        this.pollsused++;
                        result = true;
                    }
                }
                else
                {
                    this.datepoll = DateTime.Now;
                    this.pollsused = 1;
                    result = true;
                }


                return result;
            }
        }
    }
}
