namespace Client.App
{
    partial class Messenger
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._send = new System.Windows.Forms.Button();
            this._messageContainer = new System.Windows.Forms.TextBox();
            this._users = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this._isToAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._messages = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _send
            // 
            this._send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._send.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._send.Location = new System.Drawing.Point(800, 12);
            this._send.Name = "_send";
            this._send.Size = new System.Drawing.Size(107, 28);
            this._send.TabIndex = 0;
            this._send.Text = "Отправить";
            this._send.UseVisualStyleBackColor = true;
            this._send.Click += new System.EventHandler(this.SendMessage);
            // 
            // _messageContainer
            // 
            this._messageContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._messageContainer.Location = new System.Drawing.Point(12, 12);
            this._messageContainer.Multiline = true;
            this._messageContainer.Name = "_messageContainer";
            this._messageContainer.Size = new System.Drawing.Size(782, 52);
            this._messageContainer.TabIndex = 1;
            // 
            // _users
            // 
            this._users.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._users.FormattingEnabled = true;
            this._users.ItemHeight = 16;
            this._users.Location = new System.Drawing.Point(698, 40);
            this._users.Name = "_users";
            this._users.Size = new System.Drawing.Size(209, 452);
            this._users.TabIndex = 3;
            this._users.SelectedValueChanged += new System.EventHandler(this.SelectedUserIndex);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._isToAll);
            this.panel1.Controls.Add(this._messageContainer);
            this.panel1.Controls.Add(this._send);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 497);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(919, 76);
            this.panel1.TabIndex = 4;
            // 
            // _isToAll
            // 
            this._isToAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._isToAll.AutoSize = true;
            this._isToAll.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold);
            this._isToAll.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this._isToAll.Location = new System.Drawing.Point(823, 41);
            this._isToAll.Name = "_isToAll";
            this._isToAll.Size = new System.Drawing.Size(63, 23);
            this._isToAll.TabIndex = 3;
            this._isToAll.Text = "Всем";
            this._isToAll.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(693, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Пользователи в сети";
            // 
            // _messages
            // 
            this._messages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._messages.BackColor = System.Drawing.Color.DarkSlateBlue;
            this._messages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._messages.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._messages.ForeColor = System.Drawing.Color.White;
            this._messages.Location = new System.Drawing.Point(12, 12);
            this._messages.Multiline = true;
            this._messages.Name = "_messages";
            this._messages.ReadOnly = true;
            this._messages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._messages.Size = new System.Drawing.Size(680, 481);
            this._messages.TabIndex = 6;
            // 
            // Messenger
            // 
            this.AcceptButton = this._send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(919, 573);
            this.Controls.Add(this._messages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._users);
            this.Name = "Messenger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messenger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseApp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _send;
        private System.Windows.Forms.TextBox _messageContainer;
        private System.Windows.Forms.ListBox _users;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox _isToAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _messages;
    }
}