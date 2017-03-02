using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using KrautsynchBot.handler;

namespace KrautsynchBot.plugins
{
    class clsPlugin_Oder : pluginbase.clsPluginBase
    {
        public override string commandTag
        {
            get
            {
                return "oder";
            }
        }

        public clsPlugin_Oder(clsUserListWrapper userlistwrapper, clsPollHandler pollhandler) : base(userlistwrapper, pollhandler)
        {

        }

        public override void checkMessageForAlgos(clsChatMessage message)
        {
        }

        public override void commandTriggered(List<string> args, string triggeredFromUsername, bool triggeredFromMod)
        {

            string argszusammengebaut = string.Empty;
            for (int i = 0; i < args.Count; i++)
            {
                argszusammengebaut = argszusammengebaut + args[i] + " ";
            }

            argszusammengebaut.Replace("?", "");
            argszusammengebaut.Replace(".", "");
            argszusammengebaut.Replace("!", "");

            string[] entscheidungen = argszusammengebaut.Split(';');

            if (entscheidungen.Count() <= 1)
            {
                return;
            }
            string[] aussagen = Config.Plugin_Frage_Aussage.Split(';');

            Random rdm = new Random(Guid.NewGuid().GetHashCode());
            SendChatMessage("/me " + aussagen[rdm.Next(0, aussagen.Count())] + " " + entscheidungen[rdm.Next(0, entscheidungen.Count())]);
        }

        public override string getHelp()
        {
            return "/me Der Bot sagt immer die Wahrheit. Wird so hier benutzt: \"!oder Ich lübbe Ref;ich lübbe Ref nicht\". Die Argumente also mit Semikolon trennen bitte. Dieser Befehl war eine Idee von armbernd";
        }
    }
}
