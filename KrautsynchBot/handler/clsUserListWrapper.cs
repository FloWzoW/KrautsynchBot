using KrautsynchBot.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KrautsynchBot.handler
{
    class clsUserListWrapper
    {
        private Timer timer;
        WebBrowser browser;
        private string LastDocumentHash;

        private List<clsChatUser> userlist;

        public delegate void UserListChangedHandler(List<clsChatUser> userlist);
        public event UserListChangedHandler UserListChangedEvent;

        public delegate void SendChatMessageHandler(string message);
        public event SendChatMessageHandler SendChatMessageEvent;

        public clsUserListWrapper(ref WebBrowser webBrowser)
        {
            this.browser = webBrowser;
            this.userlist = new List<clsChatUser>();
            this.timer = new Timer();
            this.LastDocumentHash = string.Empty;
            this.timer.Interval = 500;
            this.timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            HtmlElement element = browser.Document.GetElementById("userlist");//Config.ElementID_Userlist
            if (element.OuterHtml != LastDocumentHash)
            {
                this.LastDocumentHash = element.OuterHtml;
                this.getUsernamesFromChatlist(element);
            }
        }

        public bool checkUsernameInChatlist(string Username)
        {
            for (int i = 0; i < userlist.Count; i++)
            {
                if (userlist[i].Username.ToLower() == Username.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public string getUsernameCaseSensetive(string usernameLowercase)
        {
            string result = string.Empty;
            for (int i = 0; i < userlist.Count; i++)
            {
                if (userlist[i].Username.ToLower() == usernameLowercase.ToLower())
                {
                    result = userlist[i].Username;
                }
            }
            return result;
        }

        public void setMettlevelerhöhung(string username, int erhöhung)
        {
            foreach (clsChatUser usr in this.userlist)
            {
                if (usr.Username == username)
                {
                    int neueslevel = (usr.MettLevel + erhöhung);
                    usr.MettLevel = neueslevel;
                    break;
                }
            }
        }
        public int getMettlevel(string username)
        {
            int result = 0;
            try
            {
                result = this.userlist.Find(x => x.Username.ToLower() == username.ToLower()).MettLevel;
            }
            catch (Exception)
            {
                //Username nicht da, kann passieren
            }
            return result;
        }
        public List<string> getUsernameList()
        {
            List<string> result = new List<string>();

            foreach (clsChatUser user in this.userlist)
            {
                result.Add(user.Username);
            }

            return result;
        }

        public string getRandomUsernameFromChatlist()
        {
            string result = string.Empty;
            bool search = true;
            int durchläufe = 0;
            while (search && durchläufe <= 20)
            {
                Random rdm = new Random(Guid.NewGuid().GetHashCode());
                int zufallszahl = rdm.Next(0, this.userlist.Count);
                result = this.userlist[zufallszahl].Username;
                if (!this.userlist[zufallszahl].isAfk && this.userlist[zufallszahl].Username.ToLower() != Config.Bot_Username.ToLower())
                {
                    search = false;
                }
                durchläufe = durchläufe + 1;
            }
            return result;
        }

        public string getAfkTimerString(string username)
        {
            string result = "";
            try
            {
                clsChatUser user = this.userlist.Find(x => x.Username.ToLower() == username.ToLower());
                if (user.isAfk)
                {
                    ///me [username] ist seit [zeit] weg von Schlüsselbrett
                    result = Config.Plugin_Afk_Message;
                    result = result.Replace("[username]", username);
                    DateTime jetzt = DateTime.Now;
                    DateTime vorhin = user.afkSince;
                    TimeSpan afkkseit = (jetzt - vorhin);

                    if (afkkseit.TotalMinutes >= 61)
                    {
                        result = result.Replace("[zeit]", afkkseit.Hours.ToString() + " Stunde/n und " + afkkseit.Minutes + " Minuten");
                    }
                    else if (afkkseit.TotalMinutes >= 1)
                    {
                        result = result.Replace("[zeit]", afkkseit.Minutes.ToString() + " Minuten");
                    }
                    else
                    {
                        result = result.Replace("[zeit]", afkkseit.Seconds.ToString() + " Sekunden");
                    }
                }
                else
                {
                    result = Config.Plugin_Afk_NotAfkMessage;
                    result = result.Replace("[username]", username);
                }
            }
            catch (Exception)
            {
                //Username nicht da, kann passieren
            }

            return result;
        }

        private void getUsernamesFromChatlist(HtmlElement UserlistBox)
        {
            List<clsChatUser> newuserlist = new List<clsChatUser>();
            foreach (HtmlElement item in UserlistBox.Children)
            {
                string userDivClass = item.GetAttribute("classname");

                if (userDivClass.Contains(Config.UserlistWrapper_Userlist_Item_Class))
                {
                    //Div-Element für einen user
                    clsChatUser user = new clsChatUser();

                    //Checken ob User afk ist
                    if (userDivClass.Contains(Config.UserlistWrapper_Userlist_AfkUser_Class))
                    {
                        user.isAfk = true;
                    }

                    if (item.Children.Count >= 2)
                    {
                        user.Username = item.Children[1].InnerText;
                        newuserlist.Add(user);
                    }
                }
            }


            bool userlistchanged = false;
            List<clsChatUser> addUserToList = new List<clsChatUser>();
            List<string> deleteUserFromList = new List<string>();

            foreach (clsChatUser existingUser in this.userlist)
            {
                bool userfound = false;
                foreach (clsChatUser newuser in newuserlist)
                {
                    if (newuser.Username == existingUser.Username)
                    {
                        userfound = true;
                        //user existiert schon in der liste, checken ob er nun afk ist
                        if (newuser.isAfk && !existingUser.isAfk)
                        {
                            userlistchanged = true;
                            existingUser.isAfk = true;
                            SendChatMessageEvent(existingUser.Username + " ist jetzt afk");
                        }
                        if (!newuser.isAfk && existingUser.isAfk)
                        {
                            existingUser.isAfk = false;
                            TimeSpan afkts = DateTime.Now - existingUser.afkSince;
                            existingUser.afkSince = DateTime.MinValue;
                            SendChatMessageEvent(existingUser.Username + " ist nicht mehr afk (War afk für " + afkts.ToString(@"hh\:mm\:ss") + ")");
                            userlistchanged = true;
                        }
                    }
                }
                if (!userfound)
                {
                    deleteUserFromList.Add(existingUser.Username);
                    SendChatMessageEvent(existingUser.Username + " ist jetzt offline");
                }
            }
            foreach (clsChatUser newuser in newuserlist)
            {
                bool userfound = false;
                foreach (clsChatUser existingUser in this.userlist)
                {
                    if (existingUser.Username == newuser.Username)
                    {
                        userfound = true;
                    }
                }
                if (!userfound)
                {
                    addUserToList.Add(newuser);
                    SendChatMessageEvent(newuser.Username + " ist jetzt online");
                }
            }

            if (addUserToList.Count >= 1 || deleteUserFromList.Count >= 1)
            {
                userlistchanged = true;
            }

            this.userlist.AddRange(addUserToList);
            foreach (string username in deleteUserFromList)
            {
                this.userlist.Remove(this.userlist.Where(user => user.Username == username).First());
            }

            //foreach (int i_item in deleteUserFromList)
            //{
            //    this.userlist.RemoveAt(i_item);
            //}

            if (userlistchanged)
            {
                UserListChangedEvent(this.userlist);
            }
        }

        private bool checkUsernameModerator(string username)
        {
            bool result = false;
            string[] mods = Config.CommandHandler_Moderatoren.Split(";"[0]);
            foreach (string mod in mods)
            {
                if (mod.ToLower() == username.ToLower())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
