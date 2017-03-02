using KrautsynchBot.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using System.Windows.Forms;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Kickroulette : pluginbase.clsPluginBase
    {
        private SortedList<string, DateTime> usernameTriggered;

        private List<string> gamerInGame;

        private bool isPlayerRegisterRunning = false;
        private bool isGameRunning = false;
        private int activePlayerIndex = -1;
        private int deadshot = -1;
        private int currentshot = -1;

        Timer roulettetimer;

        public override string commandTag
        {
            get
            {
                return "roulette";
            }
        }

        public clsPlugin_Kickroulette(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            this.usernameTriggered = new SortedList<string, DateTime>();
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            if (message.username.ToLower() != Config.Bot_Username.ToLower())
            {
                if (message.message.ToLower().Contains(Config.Plugin_Roulette_Register_Command.ToLower()) && this.isPlayerRegisterRunning && !checkIfUsernameIsPlaying(message.username))
                {
                    this.gamerInGame.Add(message.username);
                    this.SendChatMessage(Config.Plugin_Roulette_YoureIn_Message.Replace("[username]", message.username));
                }
                else if (message.message.ToLower().Contains("klick") && this.isGameRunning && message.username.ToLower() == this.gamerInGame[this.activePlayerIndex].ToLower())
                {
                    this.player_shoot(message.username);
                }
            }
        }

        private bool checkIfUsernameIsPlaying(string name)
        {
            foreach (string nameInList in this.gamerInGame)
            {
                if (name.ToLower() == nameInList.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string username = triggeredFromUsername.ToLower();
            if (this.usernameTriggered.ContainsKey(username) && !triggeredFromMod)
            {
                if (DateTime.Now.Subtract(this.usernameTriggered[username]).TotalMinutes <= Convert.ToInt32(Config.Plugin_Roulette_UserTimeout))
                {
                    SendChatMessage(Config.Plugin_Roulette_NotAgain_Message.Replace("[username]", triggeredFromUsername).Replace("[timeout]", Config.Plugin_Roulette_UserTimeout));
                    return;
                }
            }
            else if (triggeredFromMod)
            {
                this.usernameTriggered.Remove(username);
            }
            
            this.usernameTriggered.Add(username, DateTime.Now);
            SendChatMessage(Config.Plugin_Roulette_Begrüßung.Replace("[username]", triggeredFromUsername).Replace("[minplayercount]", Config.Plugin_Roulette_MinPlayerCount));
            this.gamerInGame = new List<string>();
            isPlayerRegisterRunning = true;
            this.usernameTriggered[username] = DateTime.Now;

            roulettetimer = new Timer();
            roulettetimer.Interval = (30 * 1000);
            roulettetimer.Tick += Roulettetimer_Tick;
            roulettetimer.Start();
        }

        private void ResetGame()
        {
            this.gamerInGame = new List<string>();
            this.isGameRunning = false;
            this.isPlayerRegisterRunning = false;
            this.activePlayerIndex = -1;
            this.deadshot = -1;
            this.currentshot = -1;
            this.roulettetimer = new Timer();
        }

        private void player_shoot(string player)
        {
            this.currentshot++;
            if (this.currentshot == this.deadshot)
            {
                SendChatMessage(Config.Plugin_Roulette_Hit_Message.Replace("[username]", player));
                SendChatMessage("BOOM! /ballan " + player + " ist tot, Spiel zuende!");
                ResetGame();
            } else
            {
                this.activePlayerIndex += 1;
                if (this.activePlayerIndex >= this.gamerInGame.Count)
                {
                    this.activePlayerIndex = 0;
                }
                SendChatMessage("*klick*, Pistole hat nicht ausgelöst. " + this.gamerInGame[this.activePlayerIndex] + " ist der nächste, Ballan mit \"klick\"");
            }
            
        }

        private void Roulettetimer_Tick(object sender, EventArgs e)
        {
            this.roulettetimer.Stop();
            if (this.gamerInGame.Count < Convert.ToInt32(Config.Plugin_Roulette_MinPlayerCount))
            {
                this.SendChatMessage("Nicht genug Spieler /allesfalsch Spiel beendet.");
                ResetGame();
                return;
            }



            this.isGameRunning = true;
            this.isPlayerRegisterRunning = false;
            this.activePlayerIndex = 0;
            Random rdm = new Random(Guid.NewGuid().GetHashCode());
            this.deadshot = rdm.Next(0, (this.gamerInGame.Count * 3));
            this.currentshot = -1;
            this.gamerInGame.Shuffle();
            this.SendChatMessage("Spiel beginnt!");
            this.SendChatMessage("Ballan mit \"klick\", " + this.gamerInGame[this.activePlayerIndex]);
        }

        public override string getHelp()
        {
            return "/me Kickroulette (ausm IRC geklaut lal). Mindestspieleranzahl: " + Config.Plugin_Roulette_MinPlayerCount.ToString() + " Spieler. Kann nur 1 mal pro Stunde per User gestartet werden, Mods können es immer starten.";
        }



        private static Random rng = new Random();

    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + System.Threading.Thread.CurrentThread.ManagedThreadId))); }
        }
    }
    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

