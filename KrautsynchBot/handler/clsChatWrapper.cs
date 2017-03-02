using KrautsynchBot.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace KrautsynchBot.handler
{
    class clsChatWrapper
    {

        #region "Private Objects"
        private Timer checkmessagestimer;
        private Timer sendmessagetimer;
        private WebBrowser browser;

        private int currentIndex;
        private DateTime lastsend;

        private SortedList<string, clsChatMessage> messages;


        private HtmlElement ChatBoxLine;
        #endregion

        #region "Events and Constructor"
        public delegate void ChatMessageReceivedHandler(clsChatMessage Message);
        public event ChatMessageReceivedHandler ChatMessageReceivedEvent;

        public delegate void SendConsoleMessageHandler(string Message);
        public event SendConsoleMessageHandler SendConsoleMessageEvent;

        public clsChatWrapper(ref WebBrowser webBrowser)
        {
            this.browser = webBrowser;
            this.lastsend = DateTime.Now;
            this.messages = new SortedList<string, clsChatMessage>();

            //timer to check chatbox for new messages
            this.checkmessagestimer = new Timer();
            this.checkmessagestimer.Interval = Convert.ToInt32(Config.ChatWrapper_ChatMessage_CheckInterval);
            this.checkmessagestimer.Tick += this.Timer_Tick;

            //timer to send new messages
            this.sendmessagetimer = new Timer();
            this.sendmessagetimer.Interval = Convert.ToInt32(Config.ChatWrapper_ChatMessage_SendInterval);
            this.sendmessagetimer.Tick += Sendmessagetimer_Tick;
        }
        #endregion

        #region "Chat-Listener"
        public void Start()
        {
            this.currentIndex = 0;
            this.ChatBoxLine = browser.Document.GetElementById(Config.TextboxID_Chatbox);
            this.checkmessagestimer.Start();


            this.sendmessagetimer.Start();
        }

        public void Stop()
        {
            this.checkmessagestimer.Stop();
            this.sendmessagetimer.Stop();
        }

        public void SetFokusOnChatLine()
        {
            this.ChatBoxLine.Focus();
        }

        

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                HtmlElement element = browser.Document.GetElementById(Config.ElementID_Messagebuffer);
                if (element != null)
                {
                    if (element.Children.Count >= (currentIndex + 1))
                    {
                        List<clsChatMessage> newmessages = new List<clsChatMessage>();

                        for (int i = currentIndex; i < element.Children.Count; i++)
                        {
                            clsChatMessage newmessage = GetChatMessageFromChatbox(element.Children[i]);
                            if (newmessage != null)
                            {
                                newmessages.Add(newmessage);
                            }
                            currentIndex = i;
                        }

                        foreach (clsChatMessage msg in newmessages)
                        {
                            string md5 = msg.GetMD5HashCode();
                            if (!this.messages.ContainsKey(md5))
                            {
                                this.messages.Add(md5, msg);
                                ChatMessageReceivedEvent(msg);
                            }
                        }
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
            }
            catch (Exception x)
            {
                SendConsoleMessageEvent("Exception in Timer_Tick von ChatWrapper: " + Environment.NewLine + x.Message + Environment.NewLine + x.StackTrace);
            }
        }

        private clsChatMessage GetChatMessageFromChatbox(HtmlElement divElement)
        {
            //List<clsChatMessage> messages = new List<clsChatMessage>();
            clsChatMessage result = null;

            //alle div durchgehen
            //HtmlElement divElement = chatbox.Children[chatbox.Children.Count - 1];
            if (divElement.TagName.ToLower() == "div")
            {

                clsChatMessage ChatMsg = new clsChatMessage();
                ChatMsg.rawHtml = divElement.InnerHtml;
                if (divElement.Children.Count == 0)
                {
                    //system-message wie eine poll-Ansage oderso
                    ChatMsg.username = "system";
                    ChatMsg.message = divElement.InnerText;
                    ChatMsg.timestamp = DateTime.Now;
                }
                else
                {
                    //chat-message, username aus klasse rausfiltern
                    string divClass = divElement.GetAttribute("classname");

                    if (divClass.ToLower().Contains(Config.ChatWrapper_ChatMessage_Contains_ClassName.ToLower()))
                    {
                        if (string.IsNullOrEmpty(ChatMsg.username))
                        {
                            string username = divClass.Replace(Config.ChatWrapper_ChatMessage_Contains_ClassName, "");
                            ChatMsg.username = username;
                        }
                        
                        //alle spans durchgehen
                        foreach (HtmlElement span in divElement.Children)
                        {
                            string spanClass = span.GetAttribute("classname");
                            if (spanClass.ToLower() == Config.ChatWrapper_ChatMessage_Timestamp_ClassName.ToLower())
                            {
                                string timestamp = span.InnerText;
                                timestamp = timestamp.Replace("[", "");
                                timestamp = timestamp.Replace("]", "");
                                ChatMsg.timestamp = DateTime.Parse(timestamp);
                            }
                            else
                            {
                                string test = span.GetAttribute("style");
                                //strong elemente überprüfen
                                if (span.Children.Count >= 1)
                                {
                                    if (span.Children[0].TagName.ToLower() == "strong")
                                    {
                                        //auf username prüfen
                                        string usernameClass = span.Children[0].GetAttribute("classname");
                                        if (usernameClass.ToLower() == Config.ChatWrapper_ChatMessage_Username_ClassName.ToLower())
                                        {
                                            string username = span.Children[0].InnerText;
                                            username = username.Replace(":", "");
                                            username = username.Replace(" ", "");
                                            ChatMsg.username = username;
                                        }
                                    }
                                    //else if () 
                                    //{
                                    //    //farbe raus killen
                                    //    MessageBox.Show("color detected");
                                    //}
                                    else
                                    {
                                        string messagetext = checkHtmlElement(span, span.InnerHtml);


                                        //Der veränderte Text steht direkt im Span
                                        ChatMsg.message = messagetext;
                                    }
                                }
                                else
                                {
                                    //Der Text steht direkt im Span
                                    ChatMsg.message = span.InnerHtml;
                                }

                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(ChatMsg.message))
                {
                    ChatMsg.message = "UNKNOWN MESSAGE, HTML: " + ChatMsg.rawHtml;
                }
                ChatMsg.message = ChatMsg.message.Replace("&gt;", ">");
                result = ChatMsg;
            }
            
            return result;
        }

        private string checkHtmlElement(HtmlElement element, string messagetext)
        {
            foreach (HtmlElement spanChild in element.Children)
            {
                if (spanChild.TagName.ToLower() == "img")
                {
                    string imgTitle = spanChild.GetAttribute("classname");
                    if (imgTitle.ToLower().Contains(Config.ChatWrapper_ChatMessage_Image_Embeded_ClassName.ToLower()))
                    {
                        imgTitle = spanChild.GetAttribute("href");
                        imgTitle = imgTitle + ".pic";
                        messagetext = messagetext.Replace(spanChild.OuterHtml, imgTitle);
                    }
                    else if (imgTitle.ToLower().Contains(Config.ChatWrapper_ChatMessage_Image_Embeded_Rotor_ClassName.ToLower()))
                    {
                        imgTitle = spanChild.GetAttribute("href");
                        imgTitle = imgTitle + ".pic.rötör";
                        messagetext = imgTitle;
                    }
                    else
                    {
                        imgTitle = spanChild.GetAttribute("title");
                        imgTitle = imgTitle.Replace("/", "");
                        imgTitle = "/" + imgTitle;
                        messagetext = messagetext.Replace(spanChild.OuterHtml, imgTitle);
                    }

                }
                else if (spanChild.TagName.ToLower() == "a" && spanChild.GetAttribute("classname") == "bild")
                {
                    messagetext = checkHtmlElement(spanChild, messagetext);
                    int startindex_starttag = messagetext.IndexOf("<a");
                    int endindex_starttag = messagetext.IndexOf(">", startindex_starttag);
                    messagetext = messagetext.Remove(startindex_starttag, endindex_starttag - startindex_starttag + 1);

                    int startindex_endtag = messagetext.IndexOf("</a");
                    int endindex_endtag = messagetext.IndexOf(">", startindex_endtag);
                    messagetext = messagetext.Remove(startindex_endtag, endindex_endtag - startindex_endtag + 1);

                }
                else if (spanChild.TagName.ToLower() == "span" && spanChild.OuterHtml.StartsWith("<span style=\"color:"))
                {
                    messagetext = checkHtmlElement(spanChild, messagetext);
                    int startindex_starttag = messagetext.IndexOf("<span style=\"color:");
                    int endindex_starttag = messagetext.IndexOf(">", startindex_starttag);
                    messagetext = messagetext.Remove(startindex_starttag, endindex_starttag - startindex_starttag + 1);

                    int startindex_endtag = messagetext.IndexOf("</span");
                    int endindex_endtag = messagetext.IndexOf(">", startindex_endtag);
                    messagetext = messagetext.Remove(startindex_endtag, endindex_endtag - startindex_endtag + 1);
                }
                else
                {
                    messagetext = checkHtmlElement(spanChild, messagetext);
                }
            }
            return messagetext;
        }
        #endregion

        #region "Chat-Writer"

        public void SendMessage(string message)
        {
            sendmessages.Enqueue(message);
        }

        private Queue<string> sendmessages = new Queue<string>();

        private void Sendmessagetimer_Tick(object sender, EventArgs e)
        {
            if (sendmessages.Count >= 1)
            {
                sendMessageToBrowser(sendmessages.Dequeue());
            }
        }

        private void sendMessageToBrowser(string message)
        {
            browser.Focus();
            ChatBoxLine.Focus();
            ChatBoxLine.SetAttribute("Value", message);
            SendKeys.Send("{ENTER}");
        }


        #endregion
    }
}
