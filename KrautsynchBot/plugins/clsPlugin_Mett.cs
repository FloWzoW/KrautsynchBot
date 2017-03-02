using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{

    class clsPlugin_Mett : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "mett";
            }
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            string[] metttrigger = Config.Plugin_Mett_Trigger.Split(';');
            int mettgetriggert = 0;
            foreach (string trigger in metttrigger)
            {
                mettgetriggert = mettgetriggert + System.Text.RegularExpressions.Regex.Matches(message.message.ToLower(), trigger).Count;
            }
            if (mettgetriggert >= 1)
            {
                for (int i = 0; i < mettgetriggert; i++)
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    int mettcount = rdm.Next(2, 6);
                    userlistwrapper.setMettlevelerhöhung(message.username, mettcount);
                }
            }
            else
            {
                int level = userlistwrapper.getMettlevel(message.username);

                if (level >= 90)
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    int mettcount = rdm.Next(-9, -5);
                    userlistwrapper.setMettlevelerhöhung(message.username, mettcount);
                }
                else if (level >= 80)
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    int mettcount = rdm.Next(-7, -2);
                    userlistwrapper.setMettlevelerhöhung(message.username, mettcount);
                }
                else if (level >= 50 && message.message.Contains("/"[0]))
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    int mettcount = rdm.Next(-6, -1);
                    userlistwrapper.setMettlevelerhöhung(message.username, mettcount);
                }
                else if (level >= 50)
                {
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    int mettcount = rdm.Next(-5, -2);
                    userlistwrapper.setMettlevelerhöhung(message.username, mettcount);
                }
                else if (level >= 30)
                {
                    userlistwrapper.setMettlevelerhöhung(message.username, -2);
                }
                else
                {
                    userlistwrapper.setMettlevelerhöhung(message.username, -1);
                }
            }
        }

        public clsPlugin_Mett(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            List<string> usernamesfound = this.searchUsernamesFromArgs(args);
            foreach (string usernamefound in usernamesfound)
            {
                if (!string.IsNullOrEmpty(usernamefound))
                {


                    int mettcount = userlistwrapper.getMettlevel(usernamefound);
                    //if (usernamefound.ToLower() == "antipazifist")
                    //{
                    //    mettcount = 100;
                    //}


                    if (mettcount == 100)
                    {
                        string mettmessage = Config.Plugin_Mett_100;
                        mettmessage = mettmessage.Replace("[username]", usernamefound);
                        mettmessage = mettmessage.Replace("[mettlevel]", mettcount.ToString());
                        SendChatMessage(mettmessage);
                    }
                    else if (mettcount >= 90)
                    {
                        string mettmessage = Config.Plugin_Mett_über90;
                        mettmessage = mettmessage.Replace("[username]", usernamefound);
                        mettmessage = mettmessage.Replace("[mettlevel]", mettcount.ToString());
                        SendChatMessage(mettmessage);
                    }
                    else if (mettcount >= 80)
                    {
                        string mettmessage = Config.Plugin_Mett_über80;
                        mettmessage = mettmessage.Replace("[username]", usernamefound);
                        mettmessage = mettmessage.Replace("[mettlevel]", mettcount.ToString());
                        SendChatMessage(mettmessage);
                    }
                    else
                    {
                        string mettmessage = Config.Plugin_Mett_unter80;
                        mettmessage = mettmessage.Replace("[username]", usernamefound);
                        mettmessage = mettmessage.Replace("[mettlevel]", mettcount.ToString());
                        SendChatMessage(mettmessage);
                    }
                }
                else
                {
                    SendConsoleMessage("Nickname nicht gefunden.");
                }
            }
        }


        public override string getHelp()
        {
            return "/me das alseits beliebte Mett-O-Meter. \"!mett [username]\"";
        }
    }
}
