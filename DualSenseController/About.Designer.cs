namespace DualSenseController
{
	partial class About
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
			pictureBox1 = new PictureBox();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			panel1 = new Panel();
			label5 = new Label();
			lkl_luislasonbra = new LinkLabel();
			label4 = new Label();
			lkl_donar = new LinkLabel();
			label6 = new Label();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			panel1.SuspendLayout();
			SuspendLayout();
			// 
			// pictureBox1
			// 
			pictureBox1.BackColor = Color.Transparent;
			pictureBox1.Image = Properties.Resources.dualsenseController_logo;
			pictureBox1.Location = new Point(12, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(212, 212);
			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BackColor = Color.Transparent;
			label1.Font = new Font("Comic Sans MS", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
			label1.ForeColor = Color.FromArgb(59, 62, 67);
			label1.Location = new Point(251, 12);
			label1.Name = "label1";
			label1.Size = new Size(378, 51);
			label1.TabIndex = 1;
			label1.Text = "DualSenseController";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.BackColor = Color.Transparent;
			label2.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label2.ForeColor = Color.FromArgb(59, 62, 67);
			label2.Location = new Point(264, 70);
			label2.Name = "label2";
			label2.Size = new Size(59, 18);
			label2.TabIndex = 2;
			label2.Text = "v0.1.1-r5";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.BackColor = Color.Transparent;
			label3.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label3.ForeColor = Color.FromArgb(59, 62, 67);
			label3.Location = new Point(264, 118);
			label3.Name = "label3";
			label3.Size = new Size(224, 18);
			label3.TabIndex = 3;
			label3.Text = "Dualsense Controller is designed by";
			// 
			// panel1
			// 
			panel1.BackColor = SystemColors.ButtonFace;
			panel1.Controls.Add(label5);
			panel1.Dock = DockStyle.Bottom;
			panel1.ForeColor = SystemColors.ButtonFace;
			panel1.Location = new Point(0, 246);
			panel1.Name = "panel1";
			panel1.Size = new Size(629, 71);
			panel1.TabIndex = 4;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.BackColor = Color.Transparent;
			label5.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label5.ForeColor = Color.Silver;
			label5.Location = new Point(220, 26);
			label5.Name = "label5";
			label5.Size = new Size(156, 18);
			label5.TabIndex = 4;
			label5.Text = "@2023 - by luislasonbra";
			// 
			// lkl_luislasonbra
			// 
			lkl_luislasonbra.AutoSize = true;
			lkl_luislasonbra.BackColor = Color.Transparent;
			lkl_luislasonbra.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			lkl_luislasonbra.LinkColor = SystemColors.Highlight;
			lkl_luislasonbra.Location = new Point(484, 118);
			lkl_luislasonbra.Name = "lkl_luislasonbra";
			lkl_luislasonbra.Size = new Size(85, 18);
			lkl_luislasonbra.TabIndex = 5;
			lkl_luislasonbra.TabStop = true;
			lkl_luislasonbra.Text = "luislasonbra,";
			lkl_luislasonbra.LinkClicked += lkl_luislasonbra_LinkClicked;
			// 
			// label4
			// 
			label4.BackColor = Color.Transparent;
			label4.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label4.ForeColor = Color.FromArgb(59, 62, 67);
			label4.Location = new Point(264, 136);
			label4.Name = "label4";
			label4.Size = new Size(310, 107);
			label4.TabIndex = 6;
			label4.Text = "an independent developer who works to keep the Web open, public, and accessible to all.\r\n\r\nSounds interesting?";
			// 
			// lkl_donar
			// 
			lkl_donar.AutoSize = true;
			lkl_donar.BackColor = Color.Transparent;
			lkl_donar.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			lkl_donar.LinkColor = SystemColors.Highlight;
			lkl_donar.Location = new Point(392, 192);
			lkl_donar.Name = "lkl_donar";
			lkl_donar.Size = new Size(55, 18);
			lkl_donar.TabIndex = 7;
			lkl_donar.TabStop = true;
			lkl_donar.Text = "¡Donate!";
			lkl_donar.LinkClicked += lkl_donar_LinkClicked;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.BackColor = Color.Transparent;
			label6.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
			label6.ForeColor = Color.FromArgb(59, 62, 67);
			label6.Location = new Point(520, 70);
			label6.Name = "label6";
			label6.Size = new Size(84, 18);
			label6.TabIndex = 5;
			label6.Text = "2023/08/16";
			// 
			// About
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.White;
			ClientSize = new Size(629, 317);
			Controls.Add(label6);
			Controls.Add(lkl_donar);
			Controls.Add(label4);
			Controls.Add(lkl_luislasonbra);
			Controls.Add(panel1);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(pictureBox1);
			ForeColor = SystemColors.ControlText;
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "About";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "About DualSenseController";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox pictureBox1;
		private Label label1;
		private Label label2;
		private Label label3;
		private Panel panel1;
		private LinkLabel lkl_luislasonbra;
		private Label label4;
		private LinkLabel lkl_donar;
		private Label label6;
		private Label label5;
	}
}