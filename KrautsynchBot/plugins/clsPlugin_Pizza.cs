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
    class clsPlugin_Pizza : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "pizza";
            }
        }

        private SortedList<string, clsPizzaTrigger> triggerList;

        public override void checkMessageForAlgos(clsChatMessage message)
        {
            
        }

        public clsPlugin_Pizza(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {
            triggerList = new SortedList<string, clsPizzaTrigger>();
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            int zeit = 0;
            try
            {
                zeit = Convert.ToInt32(args[0]);
            }
            catch (Exception x)
            {
                // do nothing
            }

            if (zeit >= 1 && zeit <= 9001)
            {
                clsPizzaTrigger pizzatrigger = new clsPizzaTrigger(zeit, triggeredFromUsername);
                pizzatrigger.TimerTriggeredEvent += Pizzatrigger_TimerTriggeredEvent;
                string zusatz = "";

                if (this.triggerList.ContainsKey(triggeredFromUsername))
                {
                    this.triggerList[triggeredFromUsername].Stop();
                    this.triggerList.Remove(triggeredFromUsername);
                    zusatz = "Deine vorherige Erinnerung wurde entfernt.";
                }


                this.triggerList.Add(triggeredFromUsername, pizzatrigger);
                triggerList[triggeredFromUsername].Start();

                SendChatMessage("Ich erinnere dich " + triggeredFromUsername + " in " + zeit + " Minuten gerne an deine Pizza :3 " + zusatz);
            }
        }

        private void Pizzatrigger_TimerTriggeredEvent(string username)
        {
            string message = Config.Plugin_Pizza_Trigger_Message;
            message = message.Replace("[username]", username);
            this.SendChatMessage(message);
        }

        public override string getHelp()
        {
            return "/me erinnert dich an deine Pizza nach der angebenen Zeit. \"!Pizza 12\" erinnert dich z.B. in 12 Minuten. !Pizza war eine Idee von Jalapenis. Nur 1 Trigger pro Person gleichzeitig.";
        }

        private class clsPizzaTrigger
        {
            public string username;
            public Timer timer;
            private int timeInMinutes;

            public clsPizzaTrigger(int pTimeInMinutes, string username)
            {
                this.timeInMinutes = pTimeInMinutes;
                this.username = username;
            }

            public void Start()
            {
                this.timer = new Timer();
                timer.Interval = this.timeInMinutes * 60000;
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            public void Stop()
            {
                this.timer.Tick -= Timer_Tick;
                this.timer.Stop();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {
                if (this.TimerTriggeredEvent != null)
                {
                    this.TimerTriggeredEvent(this.username);
                }

                this.timer.Stop();
            }

            public delegate void TimerTriggeredEventHandler(string username);
            public event TimerTriggeredEventHandler TimerTriggeredEvent;
        }
    }
}
