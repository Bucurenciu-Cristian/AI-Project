namespace Morabaraba
{
    partial class Game
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
            this.buttonSurrender = new System.Windows.Forms.Button();
            this.textBoxGameLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonSurrender
            // 
            this.buttonSurrender.BackColor = System.Drawing.Color.Red;
            this.buttonSurrender.Location = new System.Drawing.Point(1083, 522);
            this.buttonSurrender.Name = "buttonSurrender";
            this.buttonSurrender.Size = new System.Drawing.Size(242, 56);
            this.buttonSurrender.TabIndex = 0;
            this.buttonSurrender.Text = "Surrender";
            this.buttonSurrender.UseVisualStyleBackColor = false;
            this.buttonSurrender.Click += new System.EventHandler(this.buttonSurrender_Click);
            // 
            // textBoxGameLog
            // 
            this.textBoxGameLog.Location = new System.Drawing.Point(1014, 208);
            this.textBoxGameLog.Multiline = true;
            this.textBoxGameLog.Name = "textBoxGameLog";
            this.textBoxGameLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGameLog.Size = new System.Drawing.Size(356, 183);
            this.textBoxGameLog.TabIndex = 1;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 1041);
            this.Controls.Add(this.textBoxGameLog);
            this.Controls.Add(this.buttonSurrender);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Game";
            this.Text = "Game";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Game_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSurrender;
        private System.Windows.Forms.TextBox textBoxGameLog;
    }
}