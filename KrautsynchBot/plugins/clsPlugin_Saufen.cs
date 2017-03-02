using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Saufen : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "saufen";
            }
        }

        public clsPlugin_Saufen(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            List<string> usernames = new List<string>();
            string saufresult = "";
            string[] saufemotes = Config.Plugin_Saufen_Emotes.Split(';');
            Random rdmsaufen = new Random(Guid.NewGuid().GetHashCode());
            string saufemote = saufemotes[rdmsaufen.Next(0, saufemotes.Count())];
            bool ungültigernameenthalten = false;
            foreach (string arg in args)
            {
                if (arg.ToLower() == "alle" || arg.ToLower() == "jeder")
                {
                    usernames.Add(arg);
                }
                else if (userlistwrapper.checkUsernameInChatlist(arg))
                {
                    usernames.Add(arg);
                }
                else
                {
                    ungültigernameenthalten = true;
                }
            }
            if (!ungültigernameenthalten)
            {
                if (usernames.Count() == 1)
                {
                    if (usernames[0].ToLower() == "alle")
                    {
                        saufresult = Config.Plugin_Saufen_Message_alle;
                        string rdmuser = userlistwrapper.getRandomUsernameFromChatlist();
                        saufresult = saufresult.Replace("[username]", rdmuser);
                    }
                    else if (usernames[0].ToLower() == "jeder")
                    {
                        saufresult = Config.Plugin_Saufen_Message_jeder;
                    }
                    else
                    {
                        saufresult = Config.Plugin_Saufen_Message;
                        saufresult = saufresult.Replace("[username]", args[0]);
                        saufemote = "(allein /feelsbadman) " + saufemote;
                    }

                }
                if (usernames.Count() >= 2)
                {
                    rdmsaufen = new Random(Guid.NewGuid().GetHashCode());
                    saufresult = Config.Plugin_Saufen_Message;
                    saufresult = saufresult.Replace("[username]", args[rdmsaufen.Next(0, args.Count())]);

                }
                if (!string.IsNullOrEmpty(saufresult))
                {
                    saufresult = saufresult.Replace("[emote]", saufemote);
                    SendChatMessage(saufresult);
                }
            }
        }

        public override string getHelp()
        {
            return "/me Hiermit wird gesoffen! /stoss \"!saufen jeder\", \"!saufen alle\", \"!saufen [username]\", \"!saufen [username] [username] ...\"";
        }
    }
}
