using KrautsynchBot.classes;
using KrautsynchBot.plugins;
using KrautsynchBot.plugins.pluginbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrautsynchBot.handler
{
    class clsCommandHandler
    {
        public delegate void SendChatMessageHandler(string message);
        public event SendChatMessageHandler SendChatMessageEvent;

        public delegate void SendConsoleMessageHandler(string message);
        public event SendConsoleMessageHandler SendConsoleMessageEvent;


        private List<clsPluginBase> Plugins;
        private SortedList<string, int> PluginCommands;
        private SortedList<string, bool> PluginDisabled;
        private clsUserListWrapper userlistwrapper;

        private clsMuteHandler mutehandler
        {
            get
            {
                return (clsMuteHandler)this.Plugins[0];
            }
            set
            {
                this.Plugins[0] = value;
            }
        }

        private clsHelp helphandler
        {
            get
            {
                return (clsHelp)this.Plugins[1];
            }
            set
            {
                this.Plugins[1] = value;
            }
        }


        public clsCommandHandler(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler)
        {
            this.Plugins = new List<clsPluginBase>();
            this.PluginDisabled = new SortedList<string, bool>();
            this.userlistwrapper = userlistwrapper;

            this.Plugins.Add(new clsMuteHandler(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsHelp(userlistwrapper, pollhandler, new List<String>()));
            this.Plugins.Add(new clsPlugin_Afk(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Frage(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Heil(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Info(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Knuddelz(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Mett(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Saufen(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Soli(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Würfel(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Gas(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Votekick(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Mods(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Promille(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Lecken(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Poll(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Aufraeumen(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Armbernd(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Willkür(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Oder(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Yoo(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Yee(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Lauerstein(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Pizza(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Giphy(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_PopKey(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Molnar(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Twitch(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Kickroulette(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Tourette(userlistwrapper, pollhandler));
            this.Plugins.Add(new clsPlugin_Werbung(userlistwrapper, pollhandler));

            //add new Plugins here


            List<string> befehle = new List<string>();
            foreach (clsPluginBase plugin in this.Plugins)
            {
                befehle.Add(plugin.commandTag.ToLower());
            }
            this.helphandler = new clsHelp(userlistwrapper, pollhandler, befehle);

            this.PluginCommands = new SortedList<string, int>();
            this.PluginDisabled = new SortedList<string, bool>();
            for (int i = 0; i < this.Plugins.Count; i++)
            {
                this.PluginCommands.Add(this.Plugins[i].commandTag.ToLower(), i);
                this.PluginDisabled.Add(this.Plugins[i].commandTag.ToLower(), false);

                this.Plugins[i].SendChatMessageEvent += SendChatMessage;
                this.Plugins[i].SendConsoleMessageEvent += SendConsoleMessage;

            }
        }


        public void checkMessageCommand(clsChatMessage chatmessage)
        {
            //leerzeichen weg machen
            bool erasespaces = true;
            while (erasespaces)
            {
                if (chatmessage.message.StartsWith(" "))
                {
                    chatmessage.message = chatmessage.message.Remove(0, 1);
                }
                else
                {
                    erasespaces = false;
                }
            }

            //nur auf nachrichten reagieren die keine Nachrichten und nicht vom Bot selber sind
            if (!chatmessage.message.StartsWith(Config.CommandHandler_Message_StartTag) && chatmessage.username != Config.Bot_Username)
            {
                for (int i = 0; i < this.Plugins.Count; i++)
                {
                    this.Plugins[i].checkMessageForAlgos(chatmessage);
                }
            }
            else if (chatmessage.message.StartsWith(Config.CommandHandler_Message_StartTag) && chatmessage.username != Config.Bot_Username)
            {
                string message = chatmessage.message;
                if (!message.StartsWith(Config.CommandHandler_Message_StartTag))
                {
                    return;
                }
                if (mutehandler.checkIfUserMuted(chatmessage.username))
                {
                    return;
                }
                SendConsoleMessageEvent("Command triggered");

                //Args zusammen bauen
                List<string> args = message.Split(" "[0]).ToList();
                string command = args[0].ToLower().Replace("!", "");
                args.RemoveAt(0);

                //prüfen ob command vorhanden & angeschaltet
                if (!this.PluginCommands.ContainsKey(command))
                {
                    return; //Command nicht vorhanden
                }

                if (args.Count >= 1)
                {
                    //auf Hilfeaufruf prüfen
                    foreach (string helpargument in Config.CommandHandler_Help_Messages.Split(";"[0]))
                    {
                        if (args[0].ToLower() == helpargument.ToLower())
                        {
                            SendChatMessage(this.Plugins[this.PluginCommands[command]].getHelp());
                            return;
                        }
                    }

                    //prüfen ob an-command, geht nur bei mods
                    if (this.Plugins[0].checkIfUsernameIsMod(chatmessage.username))
                    {
                        foreach (string PluginOnArgument in Config.CommandHandler_Plugin_Enabled_Arguments.Split(";"[0]))
                        {
                            if (args[0].ToLower() == PluginOnArgument.ToLower())
                            {
                                if (this.PluginDisabled[command])
                                {
                                    this.PluginDisabled[command] = false;
                                    SendChatMessageEvent(Config.CommandHandler_Plugin_Enabled_Message.Replace("[command]", command));
                                    return;
                                }
                            }
                        }

                        //prüfen ob aus-command
                        foreach (string PluginOffArgument in Config.CommandHandler_Plugin_Disabled_Arguments.Split(";"[0]))
                        {
                            if (args[0].ToLower() == PluginOffArgument.ToLower())
                            {
                                if (!this.PluginDisabled[command])
                                {
                                    this.PluginDisabled[command] = true;
                                    SendChatMessageEvent(Config.CommandHandler_Plugin_Disabled_Message.Replace("[command]", command));
                                    return;
                                }
                            }
                        }
                    }

                }

                //prüfen ob plugin disabled
                if (this.PluginDisabled[command])
                {
                    return;
                }

                //So, nun Plugin triggern. Dazu erst schauen ob User ein Mod ist
                bool userisMod = false;
                foreach (string modname in Config.CommandHandler_Moderatoren.Split(";"[0]))
                {
                    if (modname.ToLower() == chatmessage.username.ToLower())
                    {
                        userisMod = true;
                        break;
                    }
                }
                this.Plugins[this.PluginCommands[command]].commandTriggered(args, chatmessage.username, userisMod);
            }
        }

        private void SendConsoleMessage(string message)
        {
            SendConsoleMessageEvent(message);
        }

        private void SendChatMessage(string message)
        {
            SendChatMessageEvent(message);
        }

        private void O1_OptionWonEvent(string value)
        {
            SendChatMessageEvent("event fired, option value: " + value);
        }


    }
}
