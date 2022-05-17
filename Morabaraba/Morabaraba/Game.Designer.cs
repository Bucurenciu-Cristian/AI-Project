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
            this.labelName1 = new System.Windows.Forms.Label();
            this.labelName2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.textBoxGameLog.ReadOnly = true;
            this.textBoxGameLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGameLog.Size = new System.Drawing.Size(356, 183);
            this.textBoxGameLog.TabIndex = 1;
            // 
            // labelName1
            // 
            this.labelName1.AutoSize = true;
            this.labelName1.Location = new System.Drawing.Point(1094, 116);
            this.labelName1.Name = "labelName1";
            this.labelName1.Size = new System.Drawing.Size(39, 13);
            this.labelName1.TabIndex = 2;
            this.labelName1.Text = "nume1";
            // 
            // labelName2
            // 
            this.labelName2.AutoSize = true;
            this.labelName2.Location = new System.Drawing.Point(1094, 140);
            this.labelName2.Name = "labelName2";
            this.labelName2.Size = new System.Drawing.Size(39, 13);
            this.labelName2.TabIndex = 3;
            this.labelName2.Text = "nume2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1077, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "turn1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1127, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "turn2";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 1041);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelName2);
            this.Controls.Add(this.labelName1);
            this.Controls.Add(this.textBoxGameLog);
            this.Controls.Add(this.buttonSurrender);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Game";
            this.Text = "Game";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Game_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.Game_DragOver);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Game_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Game_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSurrender;
        private System.Windows.Forms.TextBox textBoxGameLog;
        private System.Windows.Forms.Label labelName1;
        private System.Windows.Forms.Label labelName2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}