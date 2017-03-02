using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrautsynchBot.classes
{
    class clsPoll
    {
        public string question { get; set; }
        public List<clsPollOption> options;
        public int waitingForResultInSecounds = -1;
        /// <summary>
        /// Winning-Ratio, z.B. 0.6 (für 2/3 Mehrheit) oder 0.75 (für 3/4 Mehrheit). Muss höher als 0.5 sein damit auch eine Option gewinnen kann. Bei 0 reicht eine einfache Mehrheit
        /// </summary>
        public double WinRatio = 0;

        public string Message_10SecoundsLeft { get; set; }
        public string Message_30SecoundsLeft { get; set; }
        public HtmlElement ClosePollButton;
        public HtmlElement RemovePollButton;

        public clsPoll(string question = "", string message10SecoundsLeft = "", string message30SecoundsLeft = "", int waitingForResultInSecounds = 40)
        {
            this.options = new List<clsPollOption>();
            this.question = question;
            this.Message_10SecoundsLeft = message10SecoundsLeft;
            this.Message_30SecoundsLeft = message30SecoundsLeft;
            this.waitingForResultInSecounds = waitingForResultInSecounds;
        }

        public string getPollString()
        {
            string result = "";

            if (this.waitingForResultInSecounds == 0)
            {
                return "/poll Ansage:" + this.question.Replace(",","") + ",ok";
            }

            //auf Vollständigkeit prüfen
            if (string.IsNullOrEmpty(this.question) || this.options.Count <= 0)
            {
                return result;
            }

            result = "/poll ";
            result += question.Replace(",", " ");
            result += ",";
            foreach (clsPollOption option in this.options)
            {
                result += option.text.Replace(",", " ");
                result += ","; 
            }
            //komma am ende entfernen
            result = result.Remove(result.Count() - 1, 1);

            return result;
        }
    }

    class clsPollOption
    {
        /// <summary>
        /// max 255 Chars!
        /// </summary>
        public string text { get; set; }

        public clsPollOption(string text = "", string value = "")
        {
            this.winningMessages = new List<string>();
            this.Votes = -1;
            this.text = text;
            this.value = value;
        }


        /// <summary>
        /// werden pfostiert wenn die option gewinnt
        /// </summary>
        public List<string> winningMessages { get; set; }

        public int Votes = 0;

        public string value;

        

        public delegate void OptionWonEventHandler(string value);
        public event OptionWonEventHandler OptionWonEvent;
        public void fireWonEvent()
        {
            if (this.OptionWonEvent != null)
            {
                this.OptionWonEvent(this.value);
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.text))
            {
                return this.text;
            }
            else
            {
                return "";
            }
        }

    }

}
