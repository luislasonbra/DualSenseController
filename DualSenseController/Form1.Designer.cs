namespace DualSenseController
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			cb_emulator_controller = new ComboBox();
			groupBox1 = new GroupBox();
			groupBox2 = new GroupBox();
			btn_refresh_devices = new Button();
			cb_devices = new ComboBox();
			button1 = new Button();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// cb_emulator_controller
			// 
			cb_emulator_controller.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_emulator_controller.FormattingEnabled = true;
			cb_emulator_controller.Location = new Point(6, 22);
			cb_emulator_controller.Name = "cb_emulator_controller";
			cb_emulator_controller.Size = new Size(256, 23);
			cb_emulator_controller.TabIndex = 0;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(cb_emulator_controller);
			groupBox1.Location = new Point(286, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(268, 59);
			groupBox1.TabIndex = 1;
			groupBox1.TabStop = false;
			groupBox1.Text = "Emulator Controller";
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(btn_refresh_devices);
			groupBox2.Controls.Add(cb_devices);
			groupBox2.Location = new Point(12, 12);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(268, 59);
			groupBox2.TabIndex = 2;
			groupBox2.TabStop = false;
			groupBox2.Text = "Devices";
			// 
			// btn_refresh_devices
			// 
			btn_refresh_devices.Image = Properties.Resources.Refresh_16x;
			btn_refresh_devices.Location = new Point(238, 20);
			btn_refresh_devices.Name = "btn_refresh_devices";
			btn_refresh_devices.Size = new Size(24, 24);
			btn_refresh_devices.TabIndex = 4;
			btn_refresh_devices.UseVisualStyleBackColor = true;
			btn_refresh_devices.Click += btn_refresh_devices_Click;
			// 
			// cb_devices
			// 
			cb_devices.DropDownStyle = ComboBoxStyle.DropDownList;
			cb_devices.FormattingEnabled = true;
			cb_devices.Location = new Point(6, 22);
			cb_devices.Name = "cb_devices";
			cb_devices.Size = new Size(226, 23);
			cb_devices.TabIndex = 0;
			// 
			// button1
			// 
			button1.Location = new Point(18, 77);
			button1.Name = "button1";
			button1.Size = new Size(75, 23);
			button1.TabIndex = 3;
			button1.Text = "Connect";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(button1);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "DualSenseController";
			FormClosed += Form1_FormClosed;
			Load += Form1_Load;
			groupBox1.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private ComboBox cb_emulator_controller;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private ComboBox cb_devices;
		private Button button1;
		private Button btn_refresh_devices;
	}
}