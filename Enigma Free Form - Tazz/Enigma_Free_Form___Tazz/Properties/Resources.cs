using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Enigma_Free_Form___Tazz.Properties
{
	// Token: 0x02000008 RID: 8
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002CE6 File Offset: 0x00000EE6
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002CEE File Offset: 0x00000EEE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("Enigma_Free_Form___Tazz.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002D1A File Offset: 0x00000F1A
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002D21 File Offset: 0x00000F21
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000011 RID: 17
		private static ResourceManager resourceMan;

		// Token: 0x04000012 RID: 18
		private static CultureInfo resourceCulture;
	}
}
