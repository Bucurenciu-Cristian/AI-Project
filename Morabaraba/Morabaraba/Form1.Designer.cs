namespace Morabaraba
{
    partial class Form1
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
            this.modeOne = new System.Windows.Forms.Button();
            this.modeTwo = new System.Windows.Forms.Button();
            this.modeThree = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // modeOne
            // 
            this.modeOne.BackColor = System.Drawing.Color.Lime;
            this.modeOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeOne.Location = new System.Drawing.Point(21, 187);
            this.modeOne.Name = "modeOne";
            this.modeOne.Size = new System.Drawing.Size(145, 65);
            this.modeOne.TabIndex = 0;
            this.modeOne.Text = "PLay Offliine";
            this.modeOne.UseVisualStyleBackColor = false;
            // 
            // modeTwo
            // 
            this.modeTwo.BackColor = System.Drawing.Color.Cyan;
            this.modeTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeTwo.Location = new System.Drawing.Point(172, 149);
            this.modeTwo.Name = "modeTwo";
            this.modeTwo.Size = new System.Drawing.Size(145, 65);
            this.modeTwo.TabIndex = 1;
            this.modeTwo.Text = "Crete online game";
            this.modeTwo.UseVisualStyleBackColor = false;
            // 
            // modeThree
            // 
            this.modeThree.BackColor = System.Drawing.Color.Fuchsia;
            this.modeThree.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeThree.Location = new System.Drawing.Point(323, 187);
            this.modeThree.Name = "modeThree";
            this.modeThree.Size = new System.Drawing.Size(145, 65);
            this.modeThree.TabIndex = 2;
            this.modeThree.Text = "Play against PC";
            this.modeThree.UseVisualStyleBackColor = false;
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.Red;
            this.exitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.Location = new System.Drawing.Point(172, 302);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(145, 65);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Cyan;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(172, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 65);
            this.button1.TabIndex = 4;
            this.button1.Text = "Join online game";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Morabaraba.Properties.Resources.board;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.modeThree);
            this.Controls.Add(this.modeTwo);
            this.Controls.Add(this.modeOne);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Morabaraba";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button modeOne;
        private System.Windows.Forms.Button modeTwo;
        private System.Windows.Forms.Button modeThree;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button button1;
    }
}

