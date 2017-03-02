using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Knuddelz : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "knuddelz";
            }
        }

        private SortedList<string, clsKnuddelz> knuddelzlist;

        public clsPlugin_Knuddelz(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.knuddelzlist = new SortedList<string, clsKnuddelz>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            if (message.message.ToLower().Contains(Config.Plugin_Knuddelz_Trigger.ToLower()))
            {
                if (checkIfArgsContainsAllAgrument(message.message.ToLower().Split(" "[0])))
                {
                    //knuddelz geht an alle
                    int anzahlknuddelz = System.Text.RegularExpressions.Regex.Matches(message.message.ToLower(), Config.Plugin_Knuddelz_Trigger.ToLower()).Count;

                    List<string> userlist = userlistwrapper.getUsernameList();

                    foreach (string username in userlist)
                    {
                        string usernametolower = username.ToLower();
                        if (!this.knuddelzlist.ContainsKey(usernametolower))
                        {
                            clsKnuddelz knuddelz = new clsKnuddelz();
                            knuddelz.Anzahl = 0;
                            knuddelz.ZuletztVon = "";
                            this.knuddelzlist.Add(usernametolower, knuddelz);
                        }
                        this.knuddelzlist[usernametolower].Anzahl = this.knuddelzlist[usernametolower].Anzahl + anzahlknuddelz;
                        this.knuddelzlist[usernametolower].ZuletztVon = message.username;
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
                        int anzahlknuddelz = System.Text.RegularExpressions.Regex.Matches(message.message.ToLower(), Config.Plugin_Knuddelz_Trigger.ToLower()).Count;
                        foreach (string username in usernamesfound)
                        {
                            string usernametolower = username.ToLower();
                            if (!this.knuddelzlist.ContainsKey(usernametolower))
                            {
                                clsKnuddelz knuddelz = new clsKnuddelz();
                                knuddelz.Anzahl = 0;
                                knuddelz.ZuletztVon = "";
                                this.knuddelzlist.Add(usernametolower, knuddelz);
                            }
                            this.knuddelzlist[usernametolower].Anzahl = this.knuddelzlist[usernametolower].Anzahl + anzahlknuddelz;
                            this.knuddelzlist[usernametolower].ZuletztVon = message.username;
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
            if (!this.knuddelzlist.ContainsKey(username))
            {
                clsKnuddelz knuddelz = new clsKnuddelz();
                knuddelz.Anzahl = 0;
                knuddelz.ZuletztVon = "";
                this.knuddelzlist.Add(args[0].ToLower(), knuddelz);

                resultmessage = Config.Plugin_Knuddelz_0.Replace("[username]", args[0]);
            }
            else
            {
                if (this.knuddelzlist[username].Anzahl == 0)
                {
                    resultmessage = Config.Plugin_Knuddelz_0.Replace("[username]", args[0]);
                }
                else if (this.knuddelzlist[username].Anzahl >= 1)
                {
                    resultmessage = Config.Plugin_Knuddelz_ü1.Replace("[username]", args[0]);
                    resultmessage = resultmessage.Replace("[knuddelzlevel]", this.knuddelzlist[args[0].ToLower()].Anzahl.ToString());
                    resultmessage = resultmessage.Replace("[knuddelzvon]", this.knuddelzlist[args[0].ToLower()].ZuletztVon);
                }
            }
            SendChatMessage(resultmessage);
        }

        public override string getHelp()
        {
            return "/me Der Knuddel-Zähler, gewidmet für Franke den Profiknuddler /knuddeln";
        }




        private class clsKnuddelz
        {
            public int Anzahl;
            public string ZuletztVon;
        }
    }
}
