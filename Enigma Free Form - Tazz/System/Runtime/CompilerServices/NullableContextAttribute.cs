﻿using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000004 RID: 4
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableContextAttribute : Attribute
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002086 File Offset: 0x00000286
		public NullableContextAttribute(byte A_1)
		{
			this.Flag = A_1;
		}

		// Token: 0x04000002 RID: 2
		public readonly byte Flag;
	}
}
