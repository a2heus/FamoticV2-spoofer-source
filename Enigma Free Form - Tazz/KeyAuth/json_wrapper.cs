using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KeyAuth
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	[Nullable(0)]
	public class json_wrapper
	{
		// Token: 0x060000CD RID: 205 RVA: 0x000067A8 File Offset: 0x000049A8
		public static bool is_serializable(Type to_check)
		{
			return to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000067C8 File Offset: 0x000049C8
		public json_wrapper(object obj_to_work_with)
		{
			this.current_object = obj_to_work_with;
			Type type = this.current_object.GetType();
			this.serializer = new DataContractJsonSerializer(type);
			if (!json_wrapper.is_serializable(type))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("the object ");
				defaultInterpolatedStringHandler.AppendFormatted<object>(this.current_object);
				defaultInterpolatedStringHandler.AppendLiteral(" isn't a serializable");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006840 File Offset: 0x00004A40
		public object string_to_object(string json)
		{
			object result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(json)))
			{
				result = this.serializer.ReadObject(memoryStream);
			}
			return result;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006888 File Offset: 0x00004A88
		public T string_to_generic<[Nullable(2)] T>(string json)
		{
			return (T)((object)this.string_to_object(json));
		}

		// Token: 0x0400006E RID: 110
		private DataContractJsonSerializer serializer;

		// Token: 0x0400006F RID: 111
		private object current_object;
	}
}
