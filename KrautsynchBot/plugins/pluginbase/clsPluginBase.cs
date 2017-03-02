using KrautsynchBot.classes;
using KrautsynchBot.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrautsynchBot.plugins.pluginbase
{
    abstract class clsPluginBase
    {
        public clsUserListWrapper userlistwrapper;
        public clsPollHandler pollhandler;

        public delegate void SendChatMessageHandler(string message);
        public event SendChatMessageHandler SendChatMessageEvent;

        protected virtual void SendChatMessage(string message)
        {
            this.SendChatMessageEvent(message);
        }

        public delegate void SendConsoleMessageHandler(string message);
        public event SendConsoleMessageHandler SendConsoleMessageEvent;

        protected virtual void SendConsoleMessage(string message)
        {
            this.SendConsoleMessageEvent(message);
        }

        public abstract void checkMessageForAlgos(clsChatMessage message);
        public abstract void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod);
        public abstract string getHelp();

        public abstract string commandTag { get; }

        public clsPluginBase(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler)
        {
            this.userlistwrapper = userlistwrapper;
            this.pollhandler = pollhandler;
        }

        /// <summary>
        /// Auf Empty prüfen nicht vergessen!
        /// </summary>
        public List<string> searchUsernamesFromArgs(List<string> args)
        {
            List<string> usernamesfound = new List<string>();
            foreach (string word in args)
            {
                if (this.userlistwrapper.checkUsernameInChatlist(word))
                {
                    usernamesfound.Add(word);
                    break;
                }
            }
            return usernamesfound;
        }

        public bool checkIfArgsContainsAllAgrument(string[] args)
        {
            bool result = false;
            foreach (string allargument in Config.Plugin_Base_All_Argument.Split(";"[0]))
            {
                for (int i = 0; i < args.Count(); i++)
                {
                    if (args[i].ToLower() == allargument.ToLower())
                    {
                        return true;
                    }
                }
            }
            
            return result;
        }

        public bool checkIfUsernameIsMod(string username)
        {
            foreach (string mod in Config.CommandHandler_Moderatoren.Split(";"[0]))
            {
                if (mod.ToLower() == username.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //AN UND AUS EINBAUEN!

    }
}
