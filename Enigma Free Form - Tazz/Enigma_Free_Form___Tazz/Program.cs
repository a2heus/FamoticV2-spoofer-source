using System;
using System.Windows.Forms;

namespace Enigma_Free_Form___Tazz
{
	// Token: 0x02000005 RID: 5
	internal static class Program
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002095 File Offset: 0x00000295
		[STAThread]
		private static void Main()
		{
			ApplicationConfiguration.Initialize();
			Application.Run(new Form1());
		}
	}
}
