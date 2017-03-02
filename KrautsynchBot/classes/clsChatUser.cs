using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrautsynchBot.classes
{
    class clsChatUser
    {

        public string Username;
        private bool _isAfk;
        public bool isAfk
        {
            get
            {
                return this._isAfk;
            }
            set
            {
                this._isAfk = value;
                if (value)
                {
                    this.afkSince = DateTime.Now;
                }
            }
        }
        public bool isMod;
        public DateTime OnlineSince;
        public DateTime afkSince;

        public clsChatUser()
        {
            this.Username = string.Empty;
            this.isAfk = false;
            this.OnlineSince = DateTime.Now;
            this.afkSince = DateTime.MinValue;
            this.isMod = false;
        }

        public override string ToString()
        {
            string usernameToString = this.Username;
            if (this.isMod)
            {
                usernameToString = "[MOD] " + usernameToString;
            }
            if (this.isAfk)
            {
                usernameToString = usernameToString + " [AFK since " + this.afkSince.ToString("HH:mm:ss") + "]";
            }
            return usernameToString;
        }


        //Mett-Plugin
        private int _MettLevel;
        public int MettLevel
        {
            get
            {
                if (this._MettLevel <= 0)
                {
                    string[] mettstart = Config.Plugin_Mett_StartBetween.Split('-');
                    Random rdm = new Random(Guid.NewGuid().GetHashCode());
                    this._MettLevel = rdm.Next(Convert.ToInt32(mettstart[0]), Convert.ToInt32(mettstart[1]));
                }
                return _MettLevel;
            }
            set
            {
                this._MettLevel = value;
                
                if (this._MettLevel >= 101)
                {
                    this._MettLevel = 100;
                }
            }
        }
    }
}
