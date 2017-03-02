using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Lecken : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "lecken";
            }
        }

        private SortedList<string, clsLeckz> lecklist;

        public clsPlugin_Lecken(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.lecklist = new SortedList<string, clsLeckz>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            if (message.username.ToLower() == Config.Bot_Username.ToLower())
            {
                return;
            }
            bool triggerfound = false;
            foreach (string trigger in Config.Plugin_Lecken_Trigger.ToLower().Split(";"[0]))
            {
                if (message.message.ToLower().Contains(trigger))
                {
                    triggerfound = true;
                    break;
                }
            }
            if (triggerfound)
            {
                if (checkIfArgsContainsAllAgrument(message.message.ToLower().Split(" "[0])))
                {
                    //leckz geht an alle
                    List<string> userlist = userlistwrapper.getUsernameList();

                    foreach (string username in userlist)
                    {
                        string usernametolower = username.ToLower();
                        if (!this.lecklist.ContainsKey(usernametolower))
                        {
                            clsLeckz leckz = new clsLeckz();
                            leckz.Anzahl = 0;
                            leckz.ZuletztVon = "";
                            this.lecklist.Add(usernametolower, leckz);
                        }
                        this.lecklist[usernametolower].Anzahl = this.lecklist[usernametolower].Anzahl + 1;
                        this.lecklist[usernametolower].ZuletztVon = message.username;
                    }
                }
                else
                {

                    //leckz geht an jemand bestimmtes
                    List<string> usernamesfound = new List<string>();
                    List<string> argslist = message.message.Split(" "[0]).ToList();
                    if (argslist.Count >= 1)
                    {
                        argslist.RemoveAt(0);
                    }
                    foreach (string arg in argslist)
                    {
                        if (userlistwrapper.checkUsernameInChatlist(arg))
                        {
                            usernamesfound.Add(arg);
                        }
                    }

                    if (usernamesfound.Count >= 1)
                    {
                        int anzahlleckz = 1;
                        foreach (string username in usernamesfound)
                        {
                            string usernametolower = username.ToLower();
                            if (!this.lecklist.ContainsKey(usernametolower))
                            {
                                clsLeckz leckz = new clsLeckz();
                                leckz.Anzahl = 0;
                                leckz.ZuletztVon = "";
                                this.lecklist.Add(usernametolower, leckz);
                            }
                            this.lecklist[usernametolower].Anzahl = this.lecklist[usernametolower].Anzahl + anzahlleckz;
                            this.lecklist[usernametolower].ZuletztVon = message.username;
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
            if (!this.lecklist.ContainsKey(username))
            {
                clsLeckz leckz = new clsLeckz();
                leckz.Anzahl = 0;
                leckz.ZuletztVon = "";
                this.lecklist.Add(args[0].ToLower(), leckz);

                resultmessage = Config.Plugin_Lecken_Message_Count0.Replace("[username]", args[0]);
            }
            else
            {
                if (this.lecklist[username].Anzahl == 0)
                {
                    resultmessage = Config.Plugin_Lecken_Message_Count0.Replace("[username]", args[0]);
                }
                else if (this.lecklist[username].Anzahl >= 1)
                {
                    resultmessage = Config.Plugin_Lecken_Message.Replace("[username]", args[0]);
                    resultmessage = resultmessage.Replace("[anzahl]", this.lecklist[args[0].ToLower()].Anzahl.ToString());
                    resultmessage = resultmessage.Replace("[zuletztvon]", this.lecklist[args[0].ToLower()].ZuletztVon);
                }
            }
            SendChatMessage(resultmessage);
        }

        public override string getHelp()
        {
            return "/me Zeigt an wie oft ein User abgeleckt wurde. Beispiel: \"!lecken Soli\". Triggert mit folgenden Wörten und dazugehörigem Username oder dem Wort \"alle\": " + Config.Plugin_Lecken_Trigger.Replace(";", ", ");
        }




        private class clsLeckz
        {
            public int Anzahl;
            public string ZuletztVon;
        }
    }
}
