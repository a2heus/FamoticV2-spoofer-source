using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using KeyAuth;

namespace Enigma_Free_Form___Tazz.Forms
{
	// Token: 0x02000009 RID: 9
	public partial class Spoofer : Form
	{
		// Token: 0x0600001E RID: 30
		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		// Token: 0x0600001F RID: 31 RVA: 0x00002D29 File Offset: 0x00000F29
		public Spoofer()
		{
			this.InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Region = Region.FromHrgn(Spoofer.CreateRoundRectRgn(0, 0, base.Width, base.Height, 20, 20));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D66 File Offset: 0x00000F66
		[NullableContext(1)]
		private void mouse_Down(object sender, MouseEventArgs e)
		{
			this.mouseLocation = new Point(-e.Y, -e.X);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D84 File Offset: 0x00000F84
		[NullableContext(1)]
		private void mouse_Move(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point mousePosition = Control.MousePosition;
				mousePosition.Offset(this.mouseLocation.Y, this.mouseLocation.X);
				base.Location = mousePosition;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000020E3 File Offset: 0x000002E3
		[NullableContext(1)]
		private void label1_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void panel3_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002DC8 File Offset: 0x00000FC8
		[NullableContext(1)]
		private void Spoofer_Load(object sender, EventArgs e)
		{
			Spoofer.KeyAuthApp.init();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002DD4 File Offset: 0x00000FD4
		[NullableContext(1)]
		private void guna2Button1_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Spoofing Please Wait... (Press Ok)");
			string address = "https://spookyy.cdn.zerocdn.com/mapper.exe";
			string address2 = "https://spookyy.cdn.zerocdn.com/randomize%20spook.sys";
			string fileName = "C:\\Windows\\IME\\mapper.exe";
			string text = "C:\\Windows\\IME\\randomize%20spook.sys";
			using (WebClient webClient = new WebClient())
			{
				try
				{
					webClient.DownloadFile(address, fileName);
					webClient.DownloadFile(address2, text);
					using (Process process = new Process())
					{
						process.StartInfo.FileName = fileName;
						process.StartInfo.Arguments = text;
						process.StartInfo.UseShellExecute = true;
						process.StartInfo.CreateNoWindow = true;
						process.StartInfo.Verb = "runas";
						process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						process.Start();
						Thread.Sleep(5000);
					}
				}
				catch (Exception)
				{
					MessageBox.Show("A Error Occured, Make sure all anti-virus disabled and run as admin.");
					Application.Exit();
				}
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void pictureBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void guna2Button2_Click(object sender, EventArgs e, WebClient client)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void pictureBox5_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void pictureBox6_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void panel1_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label5_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label6_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label7_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002F14 File Offset: 0x00001114
		[NullableContext(1)]
		private void guna2Button3_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Spoofing Please Wait... (Press Ok)");
			string address = "https://spookyy.cdn.zerocdn.com/hmapper.exe";
			string address2 = "https://spookyy.cdn.zerocdn.com/null.sys";
			string fileName = "C:\\Windows\\IME\\hmapper.exe";
			string text = "C:\\Windows\\IME\\null.sys";
			using (WebClient webClient = new WebClient())
			{
				try
				{
					webClient.DownloadFile(address, fileName);
					webClient.DownloadFile(address2, text);
					using (Process process = new Process())
					{
						process.StartInfo.FileName = fileName;
						process.StartInfo.Arguments = text;
						process.StartInfo.UseShellExecute = true;
						process.StartInfo.CreateNoWindow = true;
						process.StartInfo.Verb = "runas";
						process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						process.Start();
						Thread.Sleep(5000);
					}
				}
				catch (Exception)
				{
					MessageBox.Show("A Error Occured, Make sure all anti-virus disabled and run as admin.");
					Application.Exit();
				}
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
				Process.Start("cmd.exe", "/c taskkill /F /IM WmiPrvSE.exe /T");
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void guna2Panel3_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label15_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label21_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label16_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void Form2_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void pictureBox4_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003054 File Offset: 0x00001254
		[NullableContext(1)]
		private void pictureBox2_Click(object sender, EventArgs e)
		{
			Process.Start("https://discord.gg/azKsNc3WmF");
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void pictureBox2_Click_1(object sender, EventArgs e)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003054 File Offset: 0x00001254
		[NullableContext(1)]
		private void pictureBox2_Click_2(object sender, EventArgs e)
		{
			Process.Start("https://discord.gg/azKsNc3WmF");
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000021B9 File Offset: 0x000003B9
		[NullableContext(1)]
		private void label9_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000049A8 File Offset: 0x00002BA8
		private void guna2Button2_Click(object sender, EventArgs e)
		{
			string address = "http://chasenelson.cdn.zerocdn.com/arp.bat";
			string address2 = "http://chasenelson.cdn.zerocdn.com/2.bat";
			string fileName = "C:\\Windows\\IME\\arp.bat";
			string fileName2 = "C:\\Windows\\IME\\2.bat";
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadFile(address, fileName);
				webClient.DownloadFile(address2, fileName2);
				using (new Process())
				{
					new WebClient().DownloadFile(address, "C:\\Windows\\arp.bat");
					Process.Start("C:\\Windows\\arp.bat");
				}
				using (new Process())
				{
					new WebClient().DownloadFile(address, "C:\\Windows\\2.bat");
					Process.Start("C:\\Windows\\2.bat");
				}
				MessageBox.Show("Fortnite Clean Succesful!", "Made By Soda");
			}
		}

		// Token: 0x04000013 RID: 19
		[Nullable(1)]
		public static api KeyAuthApp = new api("Famotic Key Auth", "U99z1Uv0Ec", "f1affc4f2210201877df2f4dcd4c3ed5cdb394b77ce56e569bd0e8e6349390e9", "1.0", null);

		// Token: 0x04000014 RID: 20
		public Point mouseLocation;
	}
}
