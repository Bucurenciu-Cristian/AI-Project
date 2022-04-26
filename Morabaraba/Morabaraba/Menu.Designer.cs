namespace Morabaraba
{
    partial class Menu
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonPlayer = new System.Windows.Forms.Button();
            this.buttonPC = new System.Windows.Forms.Button();
            this.buttonInstr = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Orange;
            this.pictureBox1.Image = global::Morabaraba.Properties.Resources.game_logo;
            this.pictureBox1.Location = new System.Drawing.Point(146, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(475, 75);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // buttonPlayer
            // 
            this.buttonPlayer.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.buttonPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPlayer.Location = new System.Drawing.Point(252, 169);
            this.buttonPlayer.Name = "buttonPlayer";
            this.buttonPlayer.Size = new System.Drawing.Size(259, 63);
            this.buttonPlayer.TabIndex = 1;
            this.buttonPlayer.Text = "Against Player";
            this.buttonPlayer.UseVisualStyleBackColor = false;
            this.buttonPlayer.Click += new System.EventHandler(this.buttonPlayer_Click);
            // 
            // buttonPC
            // 
            this.buttonPC.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.buttonPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPC.Location = new System.Drawing.Point(253, 249);
            this.buttonPC.Name = "buttonPC";
            this.buttonPC.Size = new System.Drawing.Size(258, 64);
            this.buttonPC.TabIndex = 2;
            this.buttonPC.Text = "Against PC";
            this.buttonPC.UseVisualStyleBackColor = false;
            this.buttonPC.Click += new System.EventHandler(this.buttonPC_Click);
            // 
            // buttonInstr
            // 
            this.buttonInstr.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.buttonInstr.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInstr.Location = new System.Drawing.Point(252, 332);
            this.buttonInstr.Name = "buttonInstr";
            this.buttonInstr.Size = new System.Drawing.Size(258, 59);
            this.buttonInstr.TabIndex = 3;
            this.buttonInstr.Text = "Instructions";
            this.buttonInstr.UseVisualStyleBackColor = false;
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(253, 413);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(257, 59);
            this.buttonExit.TabIndex = 4;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonInstr);
            this.Controls.Add(this.buttonPC);
            this.Controls.Add(this.buttonPlayer);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Menu";
            this.Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonPlayer;
        private System.Windows.Forms.Button buttonPC;
        private System.Windows.Forms.Button buttonInstr;
        private System.Windows.Forms.Button buttonExit;
    }
}