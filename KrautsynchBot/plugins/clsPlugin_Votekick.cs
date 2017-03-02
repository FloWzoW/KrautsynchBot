using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;
using System.Windows.Forms;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Votekick : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "votekick";
            }
        }

        private SortedList<string, DateTime> usernameTriggered;

        public clsPlugin_Votekick(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
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
                if (DateTime.Now.Subtract(this.usernameTriggered[username]).TotalMinutes <= Convert.ToInt32(Config.Plugin_Votekick_UserVote_Timeout))
                {
                    SendChatMessage(Config.Plugin_Votekick_NotAgain_Message.Replace("[username]", triggeredFromUsername));
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

            string kickuser = string.Empty;
            foreach (string arg in args)
            {
                if (userlistwrapper.checkUsernameInChatlist(arg))
                {
                    kickuser = arg;
                    break;
                }
            }
            if (string.IsNullOrEmpty(kickuser))
            {
                return;
            }
            else
            {
                //check if user is mod
                if (!triggeredFromMod)
                {
                    if (this.checkIfUsernameIsMod(kickuser) || kickuser.ToLower() == Config.Bot_Username.ToLower())
                    {
                        SendChatMessage(Config.Plugin_Votekick_ModKickMessage);
                        return;
                    }
                }

            }

            clsPoll votekickpoll = new clsPoll(Config.Plugin_Votekick_QuestionMessage.Replace("[username]", kickuser), Config.Plugin_Votekick_10secondsMessage.Replace("[username]", kickuser), Config.Plugin_Votekick_30secondsMessage.Replace("[username]", kickuser), 60);

            clsPollOption yes = new clsPollOption(Config.Plugin_Votekick_KickOption_Yes.Replace("[username]", kickuser), "F1");

            string mutemessage = string.Empty;
            int mutedtime = Convert.ToInt32(Config.Plugin_Votekick_MuteTimeInMin);

            //if (kickuser.ToLower() == "antipazifist")
            //{
            //    mutedtime = 120;
            //}

            //Prüfen ob auch gemuted werden soll
            if (mutedtime >= 0)
            {
                mutemessage = " und für " + mutedtime.ToString() + " Minuten gemuted";
                yes.winningMessages.Add(Config.Plugin_Votekick_KickOption_Yes_muteCommand.Replace("[username]", kickuser));
                yes.value = kickuser;
            }
            yes.winningMessages.Add(Config.Plugin_Votekick_KickMessage_VoteSuccess.Replace("[username]", kickuser).Replace("[mutemessage]", mutemessage));
            yes.winningMessages.Add(Config.Plugin_Votekick_KickOption_Yes_KickCommand.Replace("[username]", kickuser));
            yes.OptionWonEvent += (value) => Yes_OptionWonEvent(value, mutedtime);

            clsPollOption no = new clsPollOption(Config.Plugin_Votekick_KickOption_No.Replace("[username]", kickuser));
            no.winningMessages.Add(Config.Plugin_Votekick_KickMessage_VoteFailed.Replace("[username]", kickuser));
            votekickpoll.options.Add(yes);
            votekickpoll.options.Add(no);

            this.pollhandler.OpenPoll(votekickpoll);
        }

        private void Yes_OptionWonEvent(string value, int time)
        {
            Timer unBanTimer = new Timer();
            unBanTimer.Interval = time * 60000;
            unBanTimer.Tick += (sender, e) => UnBanTimer_Tick(sender, e, value);
            unBanTimer.Start();
        }

        private void UnBanTimer_Tick(object sender, EventArgs e, string username)
        {
            SendChatMessage(Config.Plugin_Votekick_KickOption_Yes_unmuteCommand.Replace("[username]", username));
            SendChatMessage(Config.Plugin_Votekick_KickOption_Yes_unmuteMessage.Replace("[username]", username));
            Timer timer = (Timer)sender;
            timer.Stop();
            timer.Dispose();
        }

        public override string getHelp()
        {
            return "/me Startet den Votekick gegen einen User mit \"!votekick [username]\", funktioniert pro User nur einmal die Stunde. Der Poll läuft 60 Sekunden, Mods kann man nicht kicken /2mysides Mods können den Poll mit \"!" + this.commandTag + " weck\" abbrechen.";
        }
    }
}
