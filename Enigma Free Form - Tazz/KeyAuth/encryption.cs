using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace KeyAuth
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	public static class encryption
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00006658 File Offset: 0x00004858
		public static string HashHMAC(string enckey, string resp)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(enckey);
			byte[] bytes2 = Encoding.UTF8.GetBytes(resp);
			return encryption.byte_arr_to_str(new HMACSHA256(bytes).ComputeHash(bytes2));
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000668C File Offset: 0x0000488C
		public static string byte_arr_to_str(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			foreach (byte b in ba)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000066D0 File Offset: 0x000048D0
		public static byte[] str_to_byte_arr(string hex)
		{
			byte[] result;
			try
			{
				int length = hex.Length;
				byte[] array = new byte[length / 2];
				for (int i = 0; i < length; i += 2)
				{
					array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				}
				result = array;
			}
			catch
			{
				api.error("The session has ended, open program again.");
				Environment.Exit(0);
				result = null;
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006738 File Offset: 0x00004938
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static bool CheckStringsFixedTime(string str1, string str2)
		{
			if (str1.Length != str2.Length)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < str1.Length; i++)
			{
				num |= (int)(str1[i] ^ str2[i]);
			}
			return num == 0;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006780 File Offset: 0x00004980
		public static string iv_key()
		{
			return Guid.NewGuid().ToString().Substring(0, 16);
		}
	}
}
