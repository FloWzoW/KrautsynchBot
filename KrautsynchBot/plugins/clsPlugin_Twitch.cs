using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Twitch : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "twitch";
            }
        }

        public clsPlugin_Twitch(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {

        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string[] EmoteNo = Config.Plugin_Twitch_Emotes.Split(";"[0]);

            int EmoteCommandsCount = 1;

            string result = "/me ";

            foreach (string arg in args)
            {
                if (arg.ToLower().Contains(this.commandTag.ToLower()))
                {
                    EmoteCommandsCount = EmoteCommandsCount + 1;
                }
            }
            if (EmoteCommandsCount >= 5)
            {
                EmoteCommandsCount = 4;
            }

            for (int i = 0; i < EmoteCommandsCount; i++)
            {
                Random dmTwitchEmote = new Random(Guid.NewGuid().GetHashCode());
                result = result + " " + Config.Plugin_Twitch_Url.Replace("[emoteNo]", EmoteNo[dmTwitchEmote.Next(0, EmoteNo.Count())]) + ".pic";
            }
            SendChatMessage(result);
        }

        public override string getHelp()
        {
            return "/me Der twitch-Command erstellt für Notkascher und Zovcka. Zeigt nur max 4 Emotes, hat gerade " + Config.Plugin_Twitch_Emotes.Split(";"[0]).Count().ToString() + " Emotes in der Config";
        }
    }
}

