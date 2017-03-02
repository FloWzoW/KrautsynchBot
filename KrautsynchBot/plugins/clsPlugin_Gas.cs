using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Gas : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "gas";
            }
        }

        private SortedList<string, int> gaslist;

        public clsPlugin_Gas(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.gaslist = new SortedList<string, int>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            if (message.message.ToLower().Contains(Config.Plugin_Gas_Trigger.ToLower()))
            {
                if (checkIfArgsContainsAllAgrument(message.message.ToLower().Split(" "[0])))
                {
                    //gas geht an alle
                    int anzahlgas = System.Text.RegularExpressions.Regex.Matches(message.message.ToLower(), Config.Plugin_Gas_Trigger.ToLower()).Count;

                    List<string> userlist = userlistwrapper.getUsernameList();

                    foreach (string username in userlist)
                    {
                        string usernametolower = username.ToLower();
                        if (!this.gaslist.ContainsKey(usernametolower))
                        {
                            this.gaslist.Add(usernametolower, 0);
                        }
                        this.gaslist[usernametolower] = this.gaslist[usernametolower] + anzahlgas;
                    }
                }
                else
                {

                    //knuddelz geht an jemand bestimmtes
                    List<string> usernamesfound = new List<string>();
                    foreach (string arg in message.message.Split(" "[0]))
                    {
                        if (userlistwrapper.checkUsernameInChatlist(arg))
                        {
                            usernamesfound.Add(arg);
                        }
                    }

                    if (usernamesfound.Count >= 1)
                    {
                        int anzahlgas = System.Text.RegularExpressions.Regex.Matches(message.message.ToLower(), Config.Plugin_Gas_Trigger.ToLower()).Count;
                        foreach (string username in usernamesfound)
                        {
                            string usernametolower = username.ToLower();
                            if (!this.gaslist.ContainsKey(usernametolower))
                            {
                                this.gaslist.Add(usernametolower, 0);
                            }
                            this.gaslist[usernametolower] = this.gaslist[usernametolower] + anzahlgas;
                        }
                    }
                }
            }
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            if (args.Count == 0)
            {
                return;
            }
            if (!userlistwrapper.checkUsernameInChatlist(args[0]))
            {
                return;
            }
            string username = args[0].ToLower();
            string resultmessage = "";
            if (!this.gaslist.ContainsKey(username))
            {
                this.gaslist.Add(args[0].ToLower(), 0);
                resultmessage = Config.Plugin_Gas_0.Replace("[username]", args[0]);
            }
            else
            {
                if (this.gaslist[username] == 0)
                {
                    resultmessage = Config.Plugin_Gas_0.Replace("[username]", args[0]);
                }
                else if (this.gaslist[username] >= 1)
                {
                    resultmessage = Config.Plugin_Gas_ü1.Replace("[username]", args[0]);
                    resultmessage = resultmessage.Replace("[anzahl]", this.gaslist[args[0].ToLower()].ToString());
                }
            }
            SendChatMessage(resultmessage);
        }

        public override string getHelp()
        {
            return "/me Der Gas-Zähler nach einem Wunsch von unserer Quallenkanone.";
        }
    }
}
