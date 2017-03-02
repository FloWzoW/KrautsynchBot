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
    class clsPlugin_Promille : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "promille";
            }
        }

        private SortedList<string, clsPromillegehalt> Promilleliste;
        private Timer timer;

        public clsPlugin_Promille(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.Promilleliste = new SortedList<string, clsPromillegehalt>();
            this.timer = new Timer();
            timer.Interval = 3600000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, clsPromillegehalt> item in this.Promilleliste)
            {
                item.Value.setPromillegehaltVerringerung();
            }
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            string[] trigger = (Config.Plugin_Saufen_Emotes.Replace("/", "") + ";" + Config.Plugin_Promille_TriggerWords).ToLower().Split(";"[0]);
            foreach (string triggerword in trigger)
            {
                if (message.message.ToLower().Contains(triggerword))
                {
                    string username = message.username.ToLower();
                    if (!this.Promilleliste.ContainsKey(username))
                    {
                        this.Promilleliste.Add(username, new clsPromillegehalt());
                    }
                    this.Promilleliste[username].setPromillegehaltErhöhung();
                    break;
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
            if (!this.Promilleliste.ContainsKey(username))
            {
                this.Promilleliste.Add(username, new clsPromillegehalt());
            }
            string msg = string.Empty;
            if (this.Promilleliste[username].promille <= 0)
            {
                msg = Config.Plugin_Promille_Message_0;
            }
            else
            {
                msg = Config.Plugin_Promille_Message_ü01;
                msg = msg.Replace("[promille]", this.Promilleliste[username].promille.ToString());
                Random rdm = new Random(new Guid().GetHashCode());
                string[] emotes = Config.Plugin_Saufen_Emotes.Split(";"[0]);
                msg = msg.Replace("[saufemote]", emotes[rdm.Next(0, emotes.Count())]);
            }
            msg = msg.Replace("[username]", username);
            SendChatMessage(msg);
        }

        public override string getHelp()
        {
            return "/me Der Promille-Rechner zeigt den aktuellen Alkoholpegel eines Users mit \"!promille [username]\". Der Pegel kann sich alle " + Config.Plugin_Promille_Time + " Minuten erhöhren sobald ein User Sauf-Emotes pfostiert. Der Pegel verringert sich jede Stunde von arrein.";
        }


        private class clsPromillegehalt
        {
            private double _promille;
            public double promille
            {
                get
                {
                    return this._promille;
                }
            }
            private DateTime letzterSchluck;

            public clsPromillegehalt()
            {
                this._promille = 0;
                letzterSchluck = DateTime.MinValue;
            }

            public void setPromillegehaltErhöhung()
            {
                if (DateTime.Now.Subtract(letzterSchluck).TotalMinutes >= Convert.ToDouble(Config.Plugin_Promille_Time))
                {
                    letzterSchluck = DateTime.Now;
                    this._promille = this._promille + 0.1;
                }
            }
            public void setPromillegehaltVerringerung()
            {
                this._promille = this._promille - 0.1;
                if (this._promille <= 0)
                {
                    this._promille = 0;
                }
            }

        }   
    }

    
}
