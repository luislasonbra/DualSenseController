using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DualSenseController
{
	public partial class About : Form
	{
		public static string _aboutVersion = "v0.1.1-r6";

		public About()
		{
			InitializeComponent();
		}

		private void About_Load(object sender, EventArgs e)
		{
			label2.Text = _aboutVersion;
		}

		private void lkl_luislasonbra_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = "https://github.com/luislasonbra",
				UseShellExecute = true
			});
		}

		private void lkl_donar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = "https://www.paypal.com/donate/?hosted_button_id=QEMXHPY5LG4AQ",
				UseShellExecute = true
			});
		}
	}
}
