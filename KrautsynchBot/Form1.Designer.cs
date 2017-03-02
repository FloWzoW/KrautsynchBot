namespace KrautsynchBot
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.button1 = new System.Windows.Forms.Button();
            this.lstBoxMessages = new System.Windows.Forms.ListBox();
            this.lstBoxUsernames = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(752, 517);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("http://cytu.be/r/krautsynch", System.UriKind.Absolute);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(758, 442);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Laden...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstBoxMessages
            // 
            this.lstBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBoxMessages.FormattingEnabled = true;
            this.lstBoxMessages.Location = new System.Drawing.Point(758, 13);
            this.lstBoxMessages.Name = "lstBoxMessages";
            this.lstBoxMessages.Size = new System.Drawing.Size(505, 342);
            this.lstBoxMessages.TabIndex = 3;
            // 
            // lstBoxUsernames
            // 
            this.lstBoxUsernames.FormattingEnabled = true;
            this.lstBoxUsernames.Location = new System.Drawing.Point(893, 361);
            this.lstBoxUsernames.Name = "lstBoxUsernames";
            this.lstBoxUsernames.Size = new System.Drawing.Size(409, 134);
            this.lstBoxUsernames.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(759, 472);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Starten!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 517);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lstBoxUsernames);
            this.Controls.Add(this.lstBoxMessages);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.webBrowser1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstBoxMessages;
        private System.Windows.Forms.ListBox lstBoxUsernames;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

