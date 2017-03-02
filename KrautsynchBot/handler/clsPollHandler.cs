using KrautsynchBot.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrautsynchBot.handler
{
    class clsPollHandler
    {

        public delegate void SendChatMessageHandler(string message);
        public event SendChatMessageHandler SendChatMessageEvent;

        Timer polltimer;

        private WebBrowser browser;
        private clsPoll activePoll;
        private bool pollIsRunning;

        public clsPollHandler(ref WebBrowser webBrowser)
        {
            this.pollIsRunning = false;
            this.browser = webBrowser;
        }

        public void CancelPoll()
        {
            if (pollIsRunning)
            {
                this.polltimer.Stop();
                this.polltimer = null;
                pollIsRunning = false;
                this.activePoll = null;
                SendChatMessageEvent("/me Poll abgebrochen. So funktioniert Demokratie /afd");
            }
        }

        public void OpenPoll(clsPoll poll)
        {
            if (this.checkIfPollIsRunning())
            {
                return;
            }
            this.activePoll = poll;
            this.polltimer = new Timer();

            if (poll.waitingForResultInSecounds == 0)
            {
                SendChatMessageEvent(poll.getPollString());
            }
            else if (poll.waitingForResultInSecounds > 30)
            {
                int intervall = poll.waitingForResultInSecounds - 30;
                polltimer.Interval = (intervall * 1000);  //umrechnen
                polltimer.Tick += Polltimer_Tick_30SecoundsLeft;
                SendChatMessageEvent(poll.getPollString());
                polltimer.Start();
            }
            else if (poll.waitingForResultInSecounds > 15)
            {
                int intervall = poll.waitingForResultInSecounds - 15;
                polltimer.Interval = (intervall * 1000); //umrechnen
                polltimer.Tick += Polltimer_Tick_10SecoundsLeft;
                SendChatMessageEvent(poll.getPollString());
                polltimer.Start();
            }
            else
            {
                polltimer.Interval = (poll.waitingForResultInSecounds * 1000); //umrechnen
                polltimer.Tick += Polltimer_Tick_10SecoundsLeft;
                SendChatMessageEvent(poll.getPollString());
                polltimer.Start();
            }

            pollIsRunning = true;
        }

        private void Polltimer_Tick_30SecoundsLeft(object sender, EventArgs e)
        {
            polltimer.Stop();
            polltimer = new Timer();
            if (!string.IsNullOrEmpty(this.activePoll.Message_30SecoundsLeft))
            {
                SendChatMessageEvent(this.activePoll.Message_30SecoundsLeft);
            }
            polltimer.Interval = 20000;
            polltimer.Tick += Polltimer_Tick_10SecoundsLeft;
            polltimer.Start();
        }

        private void Polltimer_Tick_10SecoundsLeft(object sender, EventArgs e)
        {
            polltimer.Stop();
            polltimer = new Timer();
            if (!string.IsNullOrEmpty(this.activePoll.Message_10SecoundsLeft))
            {
                SendChatMessageEvent(this.activePoll.Message_10SecoundsLeft);
            }
            polltimer.Interval = 10000;
            polltimer.Tick += Polltimer_Tick_Finish;
            polltimer.Start();
        }

        private void Polltimer_Tick_Finish(object sender, EventArgs e)
        {
            polltimer.Stop();

            this.ReadPollResult();

            clsPollOption optionwon = new clsPollOption();
            bool multiplewons = false;

            if (activePoll.WinRatio == 0)
            {
                foreach (clsPollOption option in this.activePoll.options)
                {
                    if (option.Votes == optionwon.Votes)
                    {
                        multiplewons = true;
                    }
                    if (option.Votes > optionwon.Votes)
                    {
                        optionwon = option;
                        multiplewons = false;
                    }
                }
            }
            else
            {
                int votesCount = 0;
                foreach (clsPollOption option in this.activePoll.options)
                {
                    votesCount = votesCount + option.Votes;
                }
                double votesneeded = Math.Ceiling(votesCount * activePoll.WinRatio);
                foreach (clsPollOption option in this.activePoll.options)
                {
                    if (option.Votes >= votesneeded)
                    {
                        optionwon = option;
                        multiplewons = false;
                    }
                }
            }

            if (optionwon != null && !multiplewons)
            {
                foreach (string winMessage in optionwon.winningMessages)
                {
                    SendChatMessageEvent(winMessage);
                }
                optionwon.fireWonEvent();
            }
            else
            {
                SendChatMessageEvent("/me Es scheint keine Option gewonnen zu haben /allesfalsch Poll beendet.");
                //keine option hat gewonnen
            }
            

            this.activePoll = null;
            this.pollIsRunning = false;
        }

        public bool checkIfPollIsRunning()
        {
            bool result = this.pollIsRunning;
            if (result)
            {
                SendChatMessageEvent("/me flüstert: Es läuft bereits ein Poll, versuch es später nochmal.");
            }
            return result;
        }


        private void ReadPollResult()
        {
            HtmlElement pollbox = this.browser.Document.GetElementById(Config.PollHandler_Pollbox_ID);

            //Abfangen wenn die Box nicht gefunden wird
            if (pollbox == null)
            {
                return;
            }

            List<clsPoll> polls = new List<clsPoll>();

            bool activepollfound = false;
            foreach (HtmlElement polldiv in pollbox.Children)
            {
                //jeden Poll durchgehen
                if (polldiv.GetAttribute("classname").ToLower().Contains(Config.PollHandler_Poll_Active_Class.ToLower()))
                {
                    activepollfound = true;
                    polls.Add(getPollFromDivBox(polldiv));
                }
            }

            if (!activepollfound)
            {
                SendChatMessageEvent(Config.PollHandler_Error_NoActivePollFound);
                return;
            }

            foreach (clsPoll poll in polls)
            {
                if (this.activePoll.question.ToLower().Contains(poll.question.ToLower()))
                {
                    this.activePoll.ClosePollButton = poll.ClosePollButton;
                    this.activePoll.RemovePollButton = poll.RemovePollButton;
                    foreach (clsPollOption foundoption in poll.options)
                    {
                        foreach (clsPollOption existingoption in this.activePoll.options)
                        {
                            if (existingoption.text.ToLower().Contains(foundoption.text.ToLower()))
                            {
                                existingoption.Votes = foundoption.Votes;
                            }
                        }
                    }
                }
            }

        }


        private clsPoll getPollFromDivBox(HtmlElement box)
        {
            clsPoll poll = new clsPoll();

            foreach (HtmlElement element in box.Children)
            {
                if (element.TagName.ToLower() == "button")
                {
                    if (element.InnerText.ToLower() == Config.PollHandler_ClosePoll_Button_Text.ToLower())
                    {
                        poll.ClosePollButton = element;
                    }
                    else if (element.InnerText.Contains("×"))
                    {
                        poll.RemovePollButton = element;
                    }
                }
                else if (element.TagName.ToLower() == "h3" )
                {
                    poll.question = element.InnerText;
                }
                else if (element.GetAttribute("classname").ToLower().Contains(Config.PollHandler_Option_Class.ToLower()))
                {
                    clsPollOption option = new clsPollOption();

                    string outerdivtext = element.OuterHtml;
                    outerdivtext = outerdivtext.Remove(0, outerdivtext.LastIndexOf("</button>"));
                    outerdivtext = outerdivtext.Remove(0, 9);
                    outerdivtext = outerdivtext.Remove(outerdivtext.LastIndexOf("</div>"), 6);
                    option.text = outerdivtext;
                    foreach (HtmlElement optiondivelement in element.Children)
                    {
                        if (optiondivelement.TagName.ToLower().Contains("button"))
                        {
                            option.Votes = Convert.ToInt32(optiondivelement.InnerText);
                        }
                    }
                    poll.options.Add(option);
                }
            }

            return poll;
        }
    }
}
