using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Enigma_Free_Form___Tazz.Forms;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using KeyAuth;

namespace Enigma_Free_Form___Tazz
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public partial class Form1 : Form
	{
		// Token: 0x06000007 RID: 7
		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		// Token: 0x06000008 RID: 8 RVA: 0x000020A6 File Offset: 0x000002A6
		public Form1()
		{
			this.InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Region = Region.FromHrgn(Form1.CreateRoundRectRgn(0, 0, base.Width, base.Height, 20, 20));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020E3 File Offset: 0x000002E3
		private void label1_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020EC File Offset: 0x000002EC
		private void mouse_Move(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Point mousePosition = Control.MousePosition;
				mousePosition.Offset(this.mouseLocation.Y, this.mouseLocation.X);
				base.Location = mousePosition;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002130 File Offset: 0x00000330
		private void mouse_Down(object sender, MouseEventArgs e)
		{
			this.mouseLocation = new Point(-e.Y, -e.X);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000214C File Offset: 0x0000034C
		private void guna2Button1_Click(object sender, EventArgs e)
		{
			Thread.Sleep(50);
			Form1.KeyAuthApp.license(this.key.Text);
			if (Form1.KeyAuthApp.response.success)
			{
				MessageBox.Show("Login Success.");
				new Spoofer().Show();
				base.Hide();
				return;
			}
			MessageBox.Show("Login Denied! Invalid Key!");
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021AD File Offset: 0x000003AD
		private void Form1_Load(object sender, EventArgs e)
		{
			Form1.KeyAuthApp.init();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021B9 File Offset: 0x000003B9
		private void key_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021B9 File Offset: 0x000003B9
		private void pictureBox1_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021B9 File Offset: 0x000003B9
		private void pictureBox2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021B9 File Offset: 0x000003B9
		private void pictureBox3_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021B9 File Offset: 0x000003B9
		private void label2_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021B9 File Offset: 0x000003B9
		private void pictureBox5_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000021B9 File Offset: 0x000003B9
		private void pictureBox4_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000021B9 File Offset: 0x000003B9
		private void key_TextChanged_1(object sender, EventArgs e)
		{
		}

		// Token: 0x04000003 RID: 3
		public Point mouseLocation;

		// Token: 0x04000004 RID: 4
		public static api KeyAuthApp = new api("Famotic Key Auth", "U99z1Uv0Ec", "f1affc4f2210201877df2f4dcd4c3ed5cdb394b77ce56e569bd0e8e6349390e9", "1.0", null);
	}
}
