using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Aufraeumen : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "aufräumen";
            }
        }

        private SortedList<string, DateTime> usernameTriggered;

        public clsPlugin_Aufraeumen(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.usernameTriggered = new SortedList<string, DateTime>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {


            string username = triggeredFromUsername.ToLower();
            if (triggeredFromMod)
            {
                foreach (string arg in args)
                {
                    if (arg.ToLower() == "weck")
                    {
                        this.pollhandler.CancelPoll();
                        return;
                    }
                }
            }

            if (this.pollhandler.checkIfPollIsRunning())
            {
                return;
            }

            if (this.usernameTriggered.ContainsKey(username))
            {
                if (DateTime.Now.Date == this.usernameTriggered[username].Date)
                {
                    SendChatMessage(Config.Plugin_Aufraeumen_NotAgain_Message.Replace("[username]", triggeredFromUsername));
                    return;
                }
                else
                {
                    this.usernameTriggered[username] = DateTime.Now;
                }
            }
            else
            {
                this.usernameTriggered.Add(username, DateTime.Now);
            }

            string deleteVideosFromUser = string.Empty;
            foreach (string arg in args)
            {
                if (userlistwrapper.checkUsernameInChatlist(arg))
                {
                    deleteVideosFromUser = arg;
                    break;
                }
            }
            if (string.IsNullOrEmpty(deleteVideosFromUser))
            {
                return;
            }
            else
            {
                //check if user is mod
                if (!triggeredFromMod)
                {
                    if (this.checkIfUsernameIsMod(deleteVideosFromUser) || deleteVideosFromUser.ToLower() == Config.Bot_Username.ToLower())
                    {
                        SendChatMessage(Config.Plugin_Aufraeumen_ModMessage);
                        return;
                    }
                }

            }

            clsPoll votekickpoll = new clsPoll(Config.Plugin_Aufraeumen_QuestionMessage.Replace("[username]", deleteVideosFromUser), Config.Plugin_Aufraeumen_10secondsMessage.Replace("[username]", deleteVideosFromUser), Config.Plugin_Aufraeumen_30secondsMessage.Replace("[username]", deleteVideosFromUser), 60);
            clsPollOption yes = new clsPollOption(Config.Plugin_Aufraeumen_Option_Yes.Replace("[username]", deleteVideosFromUser), "F1");
            yes.winningMessages.Add(Config.Plugin_Aufraeumen_VoteSuccess.Replace("[username]", deleteVideosFromUser));
            yes.winningMessages.Add(Config.Plugin_Aufraeumen_VoteSuccess_Command.Replace("[username]", userlistwrapper.getUsernameCaseSensetive(deleteVideosFromUser.ToLower())));
            clsPollOption no = new clsPollOption(Config.Plugin_Aufraeumen_Option_No.Replace("[username]", deleteVideosFromUser));
            no.winningMessages.Add(Config.Plugin_Aufraeumen_VoteFailed);
            votekickpoll.options.Add(yes);
            votekickpoll.options.Add(no);

            this.pollhandler.OpenPoll(votekickpoll);
        }

        public override string getHelp()
        {
            return "/me Löscht alle Videos des angegebenem Users bei erfolgreichem Poll mit 2/3 Mehrheit. Syntax: \"!aufräumen [username]\", funzt pro User nur einmal am Tag. Der Poll läuft 60 Sekunden. Mods können es mit \"!" + this.commandTag + " weck\" abbrechen.";
        }
    }
}
