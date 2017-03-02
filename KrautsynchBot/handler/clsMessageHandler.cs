using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrautsynchBot.classes;
using System.Windows.Forms;

namespace KrautsynchBot.handler
{
    class clsMessageHandler
    {
        private WebBrowser browser;


        public clsMessageHandler(ref WebBrowser webBrowser)
        {
            this.browser = webBrowser;

            HtmlElement userlist = webBrowser.Document.GetElementById(Config.ElementID_Userlist);

        }
    }
}
