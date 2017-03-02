using KrautsynchBot.classes;
using KrautsynchBot.handler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace KrautsynchBot
{
    public partial class Form1 : Form
    {

        //http://cytu.be/r/krautsynch
        //https://cytu.be/r/BerndboyBot_Testchan
        //http://instasync.com/r/krautsynch

        //BerndboyBot
        //4gpvbatx87
        //berndboybot @arcor.de

        //KrautsynchBot
        //reg1iurhi4
        //berndboy2@arcor.de

        clsChatWrapper chatwrapper;
        clsCommandHandler commander;
        clsUserListWrapper userlistwrapper;
        clsPollHandler pollhandler;

        private bool starting;

        public Form1()
        {
            InitializeComponent();

            this.starting = true;
            this.chatwrapper = new clsChatWrapper(ref webBrowser1);
            this.chatwrapper.ChatMessageReceivedEvent += ChatListener_ChatMessageReceivedEvent;
            this.chatwrapper.SendConsoleMessageEvent += AddListboxMessage;
            
            this.userlistwrapper = new clsUserListWrapper(ref webBrowser1);
            this.userlistwrapper.SendChatMessageEvent += Userlistwrapper_SendChatMessageEvent; 
            this.userlistwrapper.UserListChangedEvent += Userlistwrapper_UserListChangedEvent;

            this.pollhandler = new clsPollHandler(ref webBrowser1);
            pollhandler.SendChatMessageEvent += SendMessage;

            this.commander = new clsCommandHandler(this.userlistwrapper, this.pollhandler);
            this.commander.SendChatMessageEvent += SendMessage;
            //this.commander.SendChatMessageEvent += Userlistwrapper_SendChatMessageEvent;
            this.commander.SendConsoleMessageEvent += AddListboxMessage;

            button1.Enabled = false;

            this.webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;

        }

        private void Userlistwrapper_SendChatMessageEvent(string message)
        {
            this.lstBoxMessages.Items.Add(message);
        }

        private void Userlistwrapper_UserListChangedEvent(List<clsChatUser> userlist)
        {
            this.lstBoxUsernames.Items.Clear();
            foreach (clsChatUser user in userlist)
            {
                lstBoxUsernames.Items.Add(user.ToString());
            }
        }

        private void ChatListener_ChatMessageReceivedEvent(clsChatMessage Message)
        {
            AddListboxMessage(Message.ToString());
            if (!this.starting)
            {
                this.chatwrapper.SetFokusOnChatLine();
                commander.checkMessageCommand(Message);
            }
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void AddListboxMessage(string pMessage)
        {
            this.lstBoxMessages.Items.Add(pMessage);
            this.lstBoxMessages.SelectedIndex = this.lstBoxMessages.Items.Count - 1;
            this.lstBoxMessages.SelectedIndex = -1;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            chatwrapper.Start();
            userlistwrapper.Start();
        }

        private void SendMessage(string message)
        {
            this.chatwrapper.SendMessage(message);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.starting)
            {
                this.starting = false;
                this.button2.Text = "Stoppen!";
            }
            else
            {
                this.starting = true;
                this.button2.Text = "Starten!";
            }

        }

    }
}
