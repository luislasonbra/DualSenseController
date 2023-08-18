namespace DualSenseController
{
	partial class Form2
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
			btn_connection_disconnection = new Button();
			groupBox2 = new GroupBox();
			btn_refresh_devices = new Button();
			cb_devices = new ComboBox();
			rb_player1 = new RadioButton();
			groupBox1 = new GroupBox();
			rb_player4 = new RadioButton();
			rb_player3 = new RadioButton();
			rb_player2 = new RadioButton();
			groupBox3 = new GroupBox();
			rb_player_brightness_high = new RadioButton();
			rb_player_brightness_medium = new RadioButton();
			rb_player_brightness_low = new RadioButton();
			statusStrip1 = new StatusStrip();
			toolStripStatusLabel1 = new ToolStripStatusLabel();
			tspb_battery_status = new ToolStripStatusLabel();
			toolStripStatusLabel2 = new ToolStripStatusLabel();
			tsbtn_help = new ToolStripButton();
			groupBox4 = new GroupBox();
			btn_lightbar_color = new Button();
			groupBox2.SuspendLayout();
			groupBox1.SuspendLayout();
			groupBox3.SuspendLayout();
			statusStrip1.SuspendLayout();
			groupBox4.SuspendLayout();
			SuspendLayout();
			// 
			// btn_connection_disconnection
			// 
			btn_connection_disconnection.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btn_connection_disconnection.Location = new Point(351, 190);
			btn_connection_disconnection.Name = "btn_connection_disconnection";
			btn_connection_disconnection.Size = new Size(75, 29);
			btn_connection_disconnection.TabIndex = 5;
			btn_connection_disconnection.Text = "Connect";
			btn_connection_disconnection.UseVisualStyleBackColor = true;
			btn_connection_disconnection.Click += btn_connection_disconnection_Click;
			// 
			// groupBox2
			// 
			groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			groupBox2.BackColor = Color.Transparent;
			groupBox2.Controls.Add(btn_refresh_devices);
			groupBox2.Controls.Add(cb_devices);
			groupBox2.Location = new Point(12, 12);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(414, 59);
			groupBox2.TabIndex = 4;
			groupBox2.TabStop = false;
			groupBox2.Text = "Devices";
			// 
			// btn_refresh_devices
			// 
			btn_refresh_devices.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btn_refresh_devices.Image = Properties.Resources.Refresh_16x;
			btn_refresh_devices.Location = new Point(384, 22);
			btn_refresh_devices.Name = "btn_refresh_devices";
			btn_refresh_devices.Size = new Size(24, 24);
			btn_refresh_devices.TabIndex = 4;
			btn_refresh_devices.UseVisualStyleBackColor = true;
			btn_refresh_devices.Click += btn_refresh_devices_Click;
			// 
			// cb_devices
			// 
			cb_devices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			cb_devices.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_devices.FormattingEnabled = true;
			cb_devices.Location = new Point(6, 22);
			cb_devices.Name = "cb_devices";
			cb_devices.Size = new Size(372, 23);
			cb_devices.TabIndex = 0;
			// 
			// rb_player1
			// 
			rb_player1.AutoSize = true;
			rb_player1.Location = new Point(6, 22);
			rb_player1.Name = "rb_player1";
			rb_player1.Size = new Size(66, 19);
			rb_player1.TabIndex = 6;
			rb_player1.TabStop = true;
			rb_player1.Text = "Player 1";
			rb_player1.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			groupBox1.BackColor = Color.Transparent;
			groupBox1.Controls.Add(rb_player4);
			groupBox1.Controls.Add(rb_player3);
			groupBox1.Controls.Add(rb_player2);
			groupBox1.Controls.Add(rb_player1);
			groupBox1.Location = new Point(12, 77);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(107, 142);
			groupBox1.TabIndex = 7;
			groupBox1.TabStop = false;
			groupBox1.Text = "Leds Player";
			// 
			// rb_player4
			// 
			rb_player4.AutoSize = true;
			rb_player4.Location = new Point(6, 97);
			rb_player4.Name = "rb_player4";
			rb_player4.Size = new Size(66, 19);
			rb_player4.TabIndex = 9;
			rb_player4.TabStop = true;
			rb_player4.Text = "Player 4";
			rb_player4.UseVisualStyleBackColor = true;
			// 
			// rb_player3
			// 
			rb_player3.AutoSize = true;
			rb_player3.Location = new Point(6, 72);
			rb_player3.Name = "rb_player3";
			rb_player3.Size = new Size(66, 19);
			rb_player3.TabIndex = 8;
			rb_player3.TabStop = true;
			rb_player3.Text = "Player 3";
			rb_player3.UseVisualStyleBackColor = true;
			// 
			// rb_player2
			// 
			rb_player2.AutoSize = true;
			rb_player2.Location = new Point(6, 47);
			rb_player2.Name = "rb_player2";
			rb_player2.Size = new Size(66, 19);
			rb_player2.TabIndex = 7;
			rb_player2.TabStop = true;
			rb_player2.Text = "Player 2";
			rb_player2.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			groupBox3.BackColor = Color.Transparent;
			groupBox3.Controls.Add(rb_player_brightness_high);
			groupBox3.Controls.Add(rb_player_brightness_medium);
			groupBox3.Controls.Add(rb_player_brightness_low);
			groupBox3.Location = new Point(125, 82);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new Size(107, 102);
			groupBox3.TabIndex = 10;
			groupBox3.TabStop = false;
			groupBox3.Text = "Leds Brightness";
			// 
			// rb_player_brightness_high
			// 
			rb_player_brightness_high.AutoSize = true;
			rb_player_brightness_high.Location = new Point(6, 72);
			rb_player_brightness_high.Name = "rb_player_brightness_high";
			rb_player_brightness_high.Size = new Size(51, 19);
			rb_player_brightness_high.TabIndex = 8;
			rb_player_brightness_high.TabStop = true;
			rb_player_brightness_high.Text = "High";
			rb_player_brightness_high.UseVisualStyleBackColor = true;
			// 
			// rb_player_brightness_medium
			// 
			rb_player_brightness_medium.AutoSize = true;
			rb_player_brightness_medium.Location = new Point(6, 47);
			rb_player_brightness_medium.Name = "rb_player_brightness_medium";
			rb_player_brightness_medium.Size = new Size(70, 19);
			rb_player_brightness_medium.TabIndex = 7;
			rb_player_brightness_medium.TabStop = true;
			rb_player_brightness_medium.Text = "Medium";
			rb_player_brightness_medium.UseVisualStyleBackColor = true;
			// 
			// rb_player_brightness_low
			// 
			rb_player_brightness_low.AutoSize = true;
			rb_player_brightness_low.Location = new Point(6, 22);
			rb_player_brightness_low.Name = "rb_player_brightness_low";
			rb_player_brightness_low.Size = new Size(47, 19);
			rb_player_brightness_low.TabIndex = 6;
			rb_player_brightness_low.TabStop = true;
			rb_player_brightness_low.Text = "Low";
			rb_player_brightness_low.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, tspb_battery_status, toolStripStatusLabel2, tsbtn_help });
			statusStrip1.Location = new Point(0, 222);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new Size(438, 22);
			statusStrip1.SizingGrip = false;
			statusStrip1.TabIndex = 11;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new Size(44, 17);
			toolStripStatusLabel1.Text = "Battery";
			// 
			// tspb_battery_status
			// 
			tspb_battery_status.Name = "tspb_battery_status";
			tspb_battery_status.Size = new Size(12, 17);
			tspb_battery_status.Text = "?";
			// 
			// toolStripStatusLabel2
			// 
			toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			toolStripStatusLabel2.RightToLeft = RightToLeft.No;
			toolStripStatusLabel2.Size = new Size(186, 17);
			toolStripStatusLabel2.Text = "          DualSense (PS5) to Xbox 360";
			// 
			// tsbtn_help
			// 
			tsbtn_help.DisplayStyle = ToolStripItemDisplayStyle.Image;
			tsbtn_help.Image = (Image)resources.GetObject("tsbtn_help.Image");
			tsbtn_help.ImageTransparentColor = Color.Magenta;
			tsbtn_help.Name = "tsbtn_help";
			tsbtn_help.Size = new Size(23, 20);
			tsbtn_help.Text = "He&lp";
			tsbtn_help.Click += tsbtn_help_Click;
			// 
			// groupBox4
			// 
			groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			groupBox4.BackColor = Color.Transparent;
			groupBox4.Controls.Add(btn_lightbar_color);
			groupBox4.Location = new Point(238, 82);
			groupBox4.Name = "groupBox4";
			groupBox4.Size = new Size(188, 102);
			groupBox4.TabIndex = 11;
			groupBox4.TabStop = false;
			groupBox4.Text = "Lightbar Color";
			// 
			// btn_lightbar_color
			// 
			btn_lightbar_color.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			btn_lightbar_color.BackColor = Color.Red;
			btn_lightbar_color.Location = new Point(6, 22);
			btn_lightbar_color.Name = "btn_lightbar_color";
			btn_lightbar_color.Size = new Size(176, 74);
			btn_lightbar_color.TabIndex = 0;
			btn_lightbar_color.UseVisualStyleBackColor = false;
			btn_lightbar_color.Click += btn_lightbar_color_Click;
			// 
			// Form2
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.Control;
			ClientSize = new Size(438, 244);
			Controls.Add(groupBox4);
			Controls.Add(statusStrip1);
			Controls.Add(groupBox3);
			Controls.Add(groupBox1);
			Controls.Add(btn_connection_disconnection);
			Controls.Add(groupBox2);
			DoubleBuffered = true;
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "Form2";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Form2";
			FormClosing += Form2_FormClosing;
			Load += Form2_Load;
			groupBox2.ResumeLayout(false);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox3.ResumeLayout(false);
			groupBox3.PerformLayout();
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			groupBox4.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btn_connection_disconnection;
		private GroupBox groupBox2;
		private Button btn_refresh_devices;
		private ComboBox cb_devices;
		private RadioButton rb_player1;
		private GroupBox groupBox1;
		private RadioButton rb_player4;
		private RadioButton rb_player3;
		private RadioButton rb_player2;
		private GroupBox groupBox3;
		private RadioButton rb_player_brightness_high;
		private RadioButton rb_player_brightness_medium;
		private RadioButton rb_player_brightness_low;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripStatusLabel tspb_battery_status;
		private GroupBox groupBox4;
		private Button btn_lightbar_color;
		private ToolStripButton tsbtn_help;
		private ToolStripStatusLabel toolStripStatusLabel2;
	}
}