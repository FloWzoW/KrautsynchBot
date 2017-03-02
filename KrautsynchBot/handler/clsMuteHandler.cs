using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;

namespace KrautsynchBot.handler
{
    class clsMuteHandler : plugins.pluginbase.clsPluginBase
    {
        List<string> mutedUser;

        public override string commandTag
        {
            get
            {
                return "mute";
            }
        }

        public clsMuteHandler(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.mutedUser = new List<string>();
        }

        public void addUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                this.mutedUser.Add(username);
            }
        }

        public void removeUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                string userfound = string.Empty;
                int indexfound = -1;
                for (int i = 0; i < mutedUser.Count; i++)
                {
                    if (this.mutedUser[i].ToLower() == username.ToLower())
                    {
                        indexfound = i;
                        i = mutedUser.Count;
                    }
                }
                if (indexfound >= 0)
                {
                    this.mutedUser.RemoveAt(indexfound);
                }
            }
        }

        public bool checkIfUserMuted(string username)
        {
            bool result = false;

            for (int i = 0; i < this.mutedUser.Count; i++)
            {
                if (this.mutedUser[i].ToLower() == username.ToLower())
                {
                    result = true;
                    i = this.mutedUser.Count;
                }
            }
            return result;
        }

        public string getMutedUser()
        {
            string result = "";

            foreach (string user in this.mutedUser)
            {
                result = result + ", " + user;
            }
            if (result.StartsWith(", "))
            {
                result = result.Remove(0, 2);
            }

            return result;
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            if (args.Count() == 0)
            {
                string useronlist = this.getMutedUser();
                if (string.IsNullOrEmpty(useronlist))
                {
                    useronlist = "niemanden";
                }
                string useronlistmsg = Config.CommandHandler_Mute_ListMessage;
                useronlistmsg = useronlistmsg.Replace("[usernames]", useronlist);
                SendChatMessage(useronlistmsg);
            }
            if (userlistwrapper.checkUsernameInChatlist(args[0]) && triggeredFromMod)
            {
                if (!checkIfUsernameIsMod(args[0]))
                {
                    if (this.checkIfUserMuted(args[0]))
                    {
                        this.removeUser(args[0]);
                        string removeusermsg = Config.CommandHandler_Mute_UnignoreMessage;
                        removeusermsg = removeusermsg.Replace("[username]", args[0]);
                        SendChatMessage(removeusermsg);
                    }
                    else
                    {
                        this.addUser(args[0]);
                        string removeusermsg = Config.CommandHandler_Mute_IgnoreMessage;
                        removeusermsg = removeusermsg.Replace("[username]", args[0]);
                        SendChatMessage(removeusermsg);
                    }
                }
                else
                {
                    SendChatMessage("/me Mötter können nicht gemuted werden");
                }

            }
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override string getHelp()
        {
            return "/me \"!mute\" zeigt alle derzeit ignorierten User, \"!mute [username]\" fügt einen User hinzu oder entfernet ihn wieder.";
        }
    }
}
