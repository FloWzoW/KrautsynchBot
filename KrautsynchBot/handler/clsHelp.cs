using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsHelp : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "help";
            }
        }

        private List<string> Befehle;

        public clsHelp(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler, List<string> befehle) : base(userlistwrapper, pollhandler)
        {
            this.Befehle = befehle;
        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {
            string befehle = string.Empty;
            foreach (string befehl in this.Befehle)
            {
                befehle += befehl + ", ";
            }
            if (befehle.Count() >= 2)
            {
                befehle = befehle.Remove(befehle.Count() - 2, 2);
            }
            

            List<string> befehlemessages = new List<string>();
            bool verkleinere = true;

            while (verkleinere)
            {
                if (befehle.Count() >= 250)
                {
                    for (int i = 231; i < 251; i++)
                    {
                        if (befehle.ElementAt(i) == ","[0])
                        {
                            befehlemessages.Add("/me " + befehle.Substring(0, i));
                            befehle = befehle.Remove(0, i);
                        }
                    }
                }
                else
                {
                    verkleinere = false;
                    befehlemessages.Add("/me " + befehle);
                }
            }
            SendChatMessage("/me Folgende Befehle sind verfügbar, jeder Befehl gibt mit \"![Befehl] Hilfe\" weitere Informationen:");
            foreach (string message in befehlemessages)
            {
                SendChatMessage(message);
            }
        }

        public override string getHelp()
        {
            return "Wie jetzt? Hilfe von der Hilfe? /finger";
        }
    }
}
