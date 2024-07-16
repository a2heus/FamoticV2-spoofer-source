using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KeyAuth
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class api
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public api(string name, string ownerid, string secret, string version, string path = null)
		{
			if (ownerid.Length != 10 || secret.Length != 64)
			{
				Process.Start("https://youtube.com/watch?v=RfDTdiBq4_o");
				Process.Start("https://keyauth.cc/app/");
				Thread.Sleep(2000);
				api.error("Application not setup correctly. Please watch the YouTube video for setup.");
				Environment.Exit(0);
			}
			this.name = name;
			this.ownerid = ownerid;
			this.secret = secret;
			this.version = version;
			this.path = path;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004B60 File Offset: 0x00002D60
		public void init()
		{
			string text = encryption.iv_key();
			api.enckey = text + "-" + this.secret;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "init";
			nameValueCollection["ver"] = this.version;
			nameValueCollection["hash"] = api.checksum(Process.GetCurrentProcess().MainModule.FileName);
			nameValueCollection["enckey"] = text;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			NameValueCollection nameValueCollection2 = nameValueCollection;
			if (!string.IsNullOrEmpty(this.path))
			{
				nameValueCollection2.Add("token", File.ReadAllText(this.path));
				nameValueCollection2.Add("thash", api.TokenHash(this.path));
			}
			string text2 = api.req(nameValueCollection2);
			if (text2 == "KeyAuth_Invalid")
			{
				api.error("Application not found");
				Environment.Exit(0);
			}
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(text2);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				if (response_structure.newSession)
				{
					Thread.Sleep(100);
				}
				api.sessionid = response_structure.sessionid;
				this.initialized = true;
				return;
			}
			if (response_structure.message == "invalidver")
			{
				this.app_data.downloadLink = response_structure.download;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004CBC File Offset: 0x00002EBC
		public static string TokenHash(string tokenPath)
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				using (FileStream fileStream = File.OpenRead(tokenPath))
				{
					result = BitConverter.ToString(sha.ComputeHash(fileStream)).Replace("-", string.Empty);
				}
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004D28 File Offset: 0x00002F28
		public void CheckInit()
		{
			if (!this.initialized)
			{
				api.error("You must run the function KeyAuthApp.init(); first");
				Environment.Exit(0);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004D44 File Offset: 0x00002F44
		public string expirydaysleft(string Type, int subscription)
		{
			this.CheckInit();
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
			d = d.AddSeconds((double)long.Parse(this.user_data.subscriptions[subscription].expiry)).ToLocalTime();
			TimeSpan timeSpan = d - DateTime.Now;
			string a = Type.ToLower();
			if (a == "months")
			{
				return Convert.ToString(timeSpan.Days / 30);
			}
			if (a == "days")
			{
				return Convert.ToString(timeSpan.Days);
			}
			if (!(a == "hours"))
			{
				return null;
			}
			return Convert.ToString(timeSpan.Hours);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004E00 File Offset: 0x00003000
		public void register(string username, string pass, string key, string email = "")
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "register";
			nameValueCollection["username"] = username;
			nameValueCollection["pass"] = pass;
			nameValueCollection["key"] = key;
			nameValueCollection["email"] = email;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004ED8 File Offset: 0x000030D8
		public void forgot(string username, string email)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "forgot";
			nameValueCollection["username"] = username;
			nameValueCollection["email"] = email;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004F64 File Offset: 0x00003164
		public void login(string username, string pass)
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "login";
			nameValueCollection["username"] = username;
			nameValueCollection["pass"] = pass;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00005020 File Offset: 0x00003220
		public void logout()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "logout";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00005094 File Offset: 0x00003294
		public void web_login()
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			HttpListener httpListener;
			HttpListenerRequest request;
			HttpListenerResponse httpListenerResponse;
			for (;;)
			{
				httpListener = new HttpListener();
				string text = "handshake";
				text = "http://localhost:1337/" + text + "/";
				httpListener.Prefixes.Add(text);
				httpListener.Start();
				HttpListenerContext context = httpListener.GetContext();
				request = context.Request;
				httpListenerResponse = context.Response;
				httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
				httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
				httpListenerResponse.AddHeader("Via", "hugzho's big brain");
				httpListenerResponse.AddHeader("Location", "your kernel ;)");
				httpListenerResponse.AddHeader("Retry-After", "never lmao");
				httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
				if (!(request.HttpMethod == "OPTIONS"))
				{
					break;
				}
				httpListenerResponse.StatusCode = 200;
				Thread.Sleep(1);
				httpListener.Stop();
			}
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			string text2 = request.RawUrl.Replace("/handshake?user=", "").Replace("&token=", " ");
			string value2 = text2.Split(Array.Empty<char>())[0];
			string value3 = text2.Split(' ', StringSplitOptions.None)[1];
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "login";
			nameValueCollection["username"] = value2;
			nameValueCollection["token"] = value3;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			bool flag = true;
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
				httpListenerResponse.StatusCode = 420;
				httpListenerResponse.StatusDescription = "SHEESH";
			}
			else
			{
				Console.WriteLine(response_structure.message);
				httpListenerResponse.StatusCode = 200;
				httpListenerResponse.StatusDescription = response_structure.message;
				flag = false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes("Whats up?");
			httpListenerResponse.ContentLength64 = (long)bytes.Length;
			httpListenerResponse.OutputStream.Write(bytes, 0, bytes.Length);
			Thread.Sleep(1);
			httpListener.Stop();
			if (!flag)
			{
				Environment.Exit(0);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00005318 File Offset: 0x00003518
		public void button(string button)
		{
			this.CheckInit();
			HttpListener httpListener = new HttpListener();
			string uriPrefix = "http://localhost:1337/" + button + "/";
			httpListener.Prefixes.Add(uriPrefix);
			httpListener.Start();
			HttpListenerContext context = httpListener.GetContext();
			HttpListenerRequest request = context.Request;
			HttpListenerResponse httpListenerResponse = context.Response;
			httpListenerResponse.AddHeader("Access-Control-Allow-Methods", "GET, POST");
			httpListenerResponse.AddHeader("Access-Control-Allow-Origin", "*");
			httpListenerResponse.AddHeader("Via", "hugzho's big brain");
			httpListenerResponse.AddHeader("Location", "your kernel ;)");
			httpListenerResponse.AddHeader("Retry-After", "never lmao");
			httpListenerResponse.Headers.Add("Server", "\r\n\r\n");
			httpListenerResponse.StatusCode = 420;
			httpListenerResponse.StatusDescription = "SHEESH";
			httpListener.AuthenticationSchemes = AuthenticationSchemes.Negotiate;
			httpListener.UnsafeConnectionNtlmAuthentication = true;
			httpListener.IgnoreWriteExceptions = true;
			httpListener.Stop();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000053FC File Offset: 0x000035FC
		public void upgrade(string username, string key)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "upgrade";
			nameValueCollection["username"] = username;
			nameValueCollection["key"] = key;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			response_structure.success = false;
			this.load_response_struct(response_structure);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00005490 File Offset: 0x00003690
		public void license(string key)
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "license";
			nameValueCollection["key"] = key;
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_user_data(response_structure.info);
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005540 File Offset: 0x00003740
		public void check()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "check";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000055B4 File Offset: 0x000037B4
		public void setvar(string var, string data)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "setvar";
			nameValueCollection["var"] = var;
			nameValueCollection["data"] = data;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data2 = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data2);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005640 File Offset: 0x00003840
		public string getvar(string var)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "getvar";
			nameValueCollection["var"] = var;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000056D0 File Offset: 0x000038D0
		public void ban(string reason = null)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "ban";
			nameValueCollection["reason"] = reason;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005750 File Offset: 0x00003950
		public string var(string varid)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "var";
			nameValueCollection["varid"] = varid;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.message;
			}
			return null;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000057E0 File Offset: 0x000039E0
		public List<api.users> fetchOnline()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "fetchOnline";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.users;
			}
			return null;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005864 File Offset: 0x00003A64
		public void fetchStats()
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "fetchStats";
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				this.load_app_data(response_structure.appinfo);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000058EC File Offset: 0x00003AEC
		public List<api.msg> chatget(string channelname)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "chatget";
			nameValueCollection["channel"] = channelname;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.messages;
			}
			return null;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000597C File Offset: 0x00003B7C
		public bool chatsend(string msg, string channelname)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "chatsend";
			nameValueCollection["message"] = msg;
			nameValueCollection["channel"] = channelname;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005A14 File Offset: 0x00003C14
		public bool checkblack()
		{
			this.CheckInit();
			string value = WindowsIdentity.GetCurrent().User.Value;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "checkblacklist";
			nameValueCollection["hwid"] = value;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			return response_structure.success;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public string webhook(string webid, string param, string body = "", string conttype = "")
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "webhook";
			nameValueCollection["webid"] = webid;
			nameValueCollection["params"] = param;
			nameValueCollection["body"] = body;
			nameValueCollection["conttype"] = conttype;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return response_structure.response;
			}
			return null;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005B68 File Offset: 0x00003D68
		public byte[] download(string fileid)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "file";
			nameValueCollection["fileid"] = fileid;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure response_structure = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(response_structure);
			if (response_structure.success)
			{
				return encryption.str_to_byte_arr(response_structure.contents);
			}
			return null;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005C00 File Offset: 0x00003E00
		public void log(string message)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "log";
			nameValueCollection["pcuser"] = Environment.UserName;
			nameValueCollection["message"] = message;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			api.req(nameValueCollection);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005C7C File Offset: 0x00003E7C
		public void changeUsername(string username)
		{
			this.CheckInit();
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection["type"] = "changeUsername";
			nameValueCollection["newUsername"] = username;
			nameValueCollection["sessionid"] = api.sessionid;
			nameValueCollection["name"] = this.name;
			nameValueCollection["ownerid"] = this.ownerid;
			string json = api.req(nameValueCollection);
			api.response_structure data = this.response_decoder.string_to_generic<api.response_structure>(json);
			this.load_response_struct(data);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005CFC File Offset: 0x00003EFC
		public static string checksum(string filename)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					result = BitConverter.ToString(md.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
				}
			}
			return result;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005D6C File Offset: 0x00003F6C
		public static void LogEvent(string content)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "KeyAuth", "debug", fileNameWithoutExtension);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "MMM_dd_yyyy");
			defaultInterpolatedStringHandler.AppendLiteral("_logs.txt");
			string path2 = defaultInterpolatedStringHandler.ToStringAndClear();
			string text = Path.Combine(path, path2);
			try
			{
				JObject jobject = JsonConvert.DeserializeObject<JObject>(content);
				api.RedactField(jobject, "sessionid");
				api.RedactField(jobject, "ownerid");
				api.RedactField(jobject, "app");
				api.RedactField(jobject, "secret");
				api.RedactField(jobject, "version");
				api.RedactField(jobject, "fileid");
				api.RedactField(jobject, "webhooks");
				api.RedactField(jobject, "nonce");
				string value = jobject.ToString(0, Array.Empty<JsonConverter>());
				using (StreamWriter streamWriter = File.AppendText(text))
				{
					TextWriter textWriter = streamWriter;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[");
					defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now);
					defaultInterpolatedStringHandler.AppendLiteral("] [");
					defaultInterpolatedStringHandler.AppendFormatted(AppDomain.CurrentDomain.FriendlyName);
					defaultInterpolatedStringHandler.AppendLiteral("] ");
					defaultInterpolatedStringHandler.AppendFormatted(value);
					textWriter.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error logging data: " + ex.Message);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005F04 File Offset: 0x00004104
		private static void RedactField(JObject jsonObject, string fieldName)
		{
			JToken jtoken;
			if (jsonObject.TryGetValue(fieldName, ref jtoken))
			{
				jsonObject[fieldName] = "REDACTED";
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005F30 File Offset: 0x00004130
		public static void error(string message)
		{
			string path = "Logs";
			string text = Path.Combine(path, "ErrorLogs.txt");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (!File.Exists(text))
			{
				using (File.Create(text))
				{
					File.AppendAllText(text, DateTime.Now.ToString() + " > This is the start of your error logs file");
				}
			}
			File.AppendAllText(text, DateTime.Now.ToString() + " > " + message + Environment.NewLine);
			Process.Start(new ProcessStartInfo("cmd.exe", "/c start cmd /C \"color b && title Error && echo " + message + " && timeout /t 5\"")
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			});
			Environment.Exit(0);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000600C File Offset: 0x0000420C
		private static string req(NameValueCollection post_data)
		{
			string result;
			try
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.Proxy = null;
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(api.assertSSL));
					Stopwatch stopwatch = new Stopwatch();
					stopwatch.Start();
					byte[] bytes = webClient.UploadValues("https://keyauth.win/api/1.2/", post_data);
					stopwatch.Stop();
					api.responseTime = stopwatch.ElapsedMilliseconds;
					ServicePointManager.ServerCertificateValidationCallback = (([Nullable(1)] object <p0>, X509Certificate <p1>, X509Chain <p2>, SslPolicyErrors <p3>) => true);
					api.sigCheck(Encoding.UTF8.GetString(bytes), webClient.ResponseHeaders["signature"], post_data.Get(0));
					api.LogEvent(Encoding.Default.GetString(bytes) + "\n");
					result = Encoding.Default.GetString(bytes);
				}
			}
			catch (WebException ex)
			{
				if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.TooManyRequests)
				{
					api.error("You're connecting too fast to loader, slow down.");
					api.LogEvent("You're connecting too fast to loader, slow down.");
					Environment.Exit(0);
					result = "";
				}
				else
				{
					api.error("Connection failure. Please try again, or contact us for help.");
					api.LogEvent("Connection failure. Please try again, or contact us for help.");
					Environment.Exit(0);
					result = "";
				}
			}
			return result;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00006164 File Offset: 0x00004364
		private static bool assertSSL(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if ((!certificate.Issuer.Contains("Google Trust Services") && !certificate.Issuer.Contains("Let's Encrypt")) || sslPolicyErrors != SslPolicyErrors.None)
			{
				api.error("SSL assertion fail, make sure you're not debugging Network. Disable internet firewall on router if possible. & echo: & echo If not, ask the developer of the program to use custom domains to fix this.");
				api.LogEvent("SSL assertion fail, make sure you're not debugging Network. Disable internet firewall on router if possible. If not, ask the developer of the program to use custom domains to fix this.");
				return false;
			}
			return true;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000061A4 File Offset: 0x000043A4
		private static void sigCheck(string resp, string signature, string type)
		{
			if (type == "log" || type == "file")
			{
				return;
			}
			try
			{
				if (!encryption.CheckStringsFixedTime(encryption.HashHMAC((type == "init") ? api.enckey.Substring(17, 64) : api.enckey, resp), signature))
				{
					api.error("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " + resp);
					api.LogEvent(resp + "\n");
					Environment.Exit(0);
				}
			}
			catch
			{
				api.error("Signature checksum failed. Request was tampered with or session ended most likely. & echo: & echo Response: " + resp);
				api.LogEvent(resp + "\n");
				Environment.Exit(0);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00006260 File Offset: 0x00004460
		private void load_app_data(api.app_data_structure data)
		{
			this.app_data.numUsers = data.numUsers;
			this.app_data.numOnlineUsers = data.numOnlineUsers;
			this.app_data.numKeys = data.numKeys;
			this.app_data.version = data.version;
			this.app_data.customerPanelLink = data.customerPanelLink;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000062C4 File Offset: 0x000044C4
		private void load_user_data(api.user_data_structure data)
		{
			this.user_data.username = data.username;
			this.user_data.ip = data.ip;
			this.user_data.hwid = data.hwid;
			this.user_data.createdate = data.createdate;
			this.user_data.lastlogin = data.lastlogin;
			this.user_data.subscriptions = data.subscriptions;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006337 File Offset: 0x00004537
		private void load_response_struct(api.response_structure data)
		{
			this.response.success = data.success;
			this.response.message = data.message;
		}

		// Token: 0x04000033 RID: 51
		public string name;

		// Token: 0x04000034 RID: 52
		public string ownerid;

		// Token: 0x04000035 RID: 53
		public string secret;

		// Token: 0x04000036 RID: 54
		public string version;

		// Token: 0x04000037 RID: 55
		public string path;

		// Token: 0x04000038 RID: 56
		public static long responseTime;

		// Token: 0x04000039 RID: 57
		private static string sessionid;

		// Token: 0x0400003A RID: 58
		private static string enckey;

		// Token: 0x0400003B RID: 59
		private bool initialized;

		// Token: 0x0400003C RID: 60
		public api.app_data_class app_data = new api.app_data_class();

		// Token: 0x0400003D RID: 61
		public api.user_data_class user_data = new api.user_data_class();

		// Token: 0x0400003E RID: 62
		public api.response_class response = new api.response_class();

		// Token: 0x0400003F RID: 63
		private json_wrapper response_decoder = new json_wrapper(new api.response_structure());

		// Token: 0x0200000B RID: 11
		[Nullable(0)]
		[DataContract]
		private class response_structure
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000064 RID: 100 RVA: 0x0000635B File Offset: 0x0000455B
			// (set) Token: 0x06000065 RID: 101 RVA: 0x00006363 File Offset: 0x00004563
			[DataMember]
			public bool success { get; set; }

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000066 RID: 102 RVA: 0x0000636C File Offset: 0x0000456C
			// (set) Token: 0x06000067 RID: 103 RVA: 0x00006374 File Offset: 0x00004574
			[DataMember]
			public bool newSession { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000068 RID: 104 RVA: 0x0000637D File Offset: 0x0000457D
			// (set) Token: 0x06000069 RID: 105 RVA: 0x00006385 File Offset: 0x00004585
			[DataMember]
			public string sessionid { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x0600006A RID: 106 RVA: 0x0000638E File Offset: 0x0000458E
			// (set) Token: 0x0600006B RID: 107 RVA: 0x00006396 File Offset: 0x00004596
			[DataMember]
			public string contents { get; set; }

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600006C RID: 108 RVA: 0x0000639F File Offset: 0x0000459F
			// (set) Token: 0x0600006D RID: 109 RVA: 0x000063A7 File Offset: 0x000045A7
			[DataMember]
			public string response { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600006E RID: 110 RVA: 0x000063B0 File Offset: 0x000045B0
			// (set) Token: 0x0600006F RID: 111 RVA: 0x000063B8 File Offset: 0x000045B8
			[DataMember]
			public string message { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000070 RID: 112 RVA: 0x000063C1 File Offset: 0x000045C1
			// (set) Token: 0x06000071 RID: 113 RVA: 0x000063C9 File Offset: 0x000045C9
			[DataMember]
			public string download { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000072 RID: 114 RVA: 0x000063D2 File Offset: 0x000045D2
			// (set) Token: 0x06000073 RID: 115 RVA: 0x000063DA File Offset: 0x000045DA
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.user_data_structure info { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000074 RID: 116 RVA: 0x000063E3 File Offset: 0x000045E3
			// (set) Token: 0x06000075 RID: 117 RVA: 0x000063EB File Offset: 0x000045EB
			[DataMember(IsRequired = false, EmitDefaultValue = false)]
			public api.app_data_structure appinfo { get; set; }

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000076 RID: 118 RVA: 0x000063F4 File Offset: 0x000045F4
			// (set) Token: 0x06000077 RID: 119 RVA: 0x000063FC File Offset: 0x000045FC
			[DataMember]
			public List<api.msg> messages { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00006405 File Offset: 0x00004605
			// (set) Token: 0x06000079 RID: 121 RVA: 0x0000640D File Offset: 0x0000460D
			[DataMember]
			public List<api.users> users { get; set; }
		}

		// Token: 0x0200000C RID: 12
		[Nullable(0)]
		public class msg
		{
			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600007B RID: 123 RVA: 0x00006416 File Offset: 0x00004616
			// (set) Token: 0x0600007C RID: 124 RVA: 0x0000641E File Offset: 0x0000461E
			public string message { get; set; }

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600007D RID: 125 RVA: 0x00006427 File Offset: 0x00004627
			// (set) Token: 0x0600007E RID: 126 RVA: 0x0000642F File Offset: 0x0000462F
			public string author { get; set; }

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600007F RID: 127 RVA: 0x00006438 File Offset: 0x00004638
			// (set) Token: 0x06000080 RID: 128 RVA: 0x00006440 File Offset: 0x00004640
			public string timestamp { get; set; }
		}

		// Token: 0x0200000D RID: 13
		[Nullable(0)]
		public class users
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x06000082 RID: 130 RVA: 0x00006449 File Offset: 0x00004649
			// (set) Token: 0x06000083 RID: 131 RVA: 0x00006451 File Offset: 0x00004651
			public string credential { get; set; }
		}

		// Token: 0x0200000E RID: 14
		[Nullable(0)]
		[DataContract]
		private class user_data_structure
		{
			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000085 RID: 133 RVA: 0x0000645A File Offset: 0x0000465A
			// (set) Token: 0x06000086 RID: 134 RVA: 0x00006462 File Offset: 0x00004662
			[DataMember]
			public string username { get; set; }

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000087 RID: 135 RVA: 0x0000646B File Offset: 0x0000466B
			// (set) Token: 0x06000088 RID: 136 RVA: 0x00006473 File Offset: 0x00004673
			[DataMember]
			public string ip { get; set; }

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000089 RID: 137 RVA: 0x0000647C File Offset: 0x0000467C
			// (set) Token: 0x0600008A RID: 138 RVA: 0x00006484 File Offset: 0x00004684
			[DataMember]
			public string hwid { get; set; }

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x0600008B RID: 139 RVA: 0x0000648D File Offset: 0x0000468D
			// (set) Token: 0x0600008C RID: 140 RVA: 0x00006495 File Offset: 0x00004695
			[DataMember]
			public string createdate { get; set; }

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x0600008D RID: 141 RVA: 0x0000649E File Offset: 0x0000469E
			// (set) Token: 0x0600008E RID: 142 RVA: 0x000064A6 File Offset: 0x000046A6
			[DataMember]
			public string lastlogin { get; set; }

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600008F RID: 143 RVA: 0x000064AF File Offset: 0x000046AF
			// (set) Token: 0x06000090 RID: 144 RVA: 0x000064B7 File Offset: 0x000046B7
			[DataMember]
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x0200000F RID: 15
		[Nullable(0)]
		[DataContract]
		private class app_data_structure
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x06000092 RID: 146 RVA: 0x000064C0 File Offset: 0x000046C0
			// (set) Token: 0x06000093 RID: 147 RVA: 0x000064C8 File Offset: 0x000046C8
			[DataMember]
			public string numUsers { get; set; }

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000094 RID: 148 RVA: 0x000064D1 File Offset: 0x000046D1
			// (set) Token: 0x06000095 RID: 149 RVA: 0x000064D9 File Offset: 0x000046D9
			[DataMember]
			public string numOnlineUsers { get; set; }

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x06000096 RID: 150 RVA: 0x000064E2 File Offset: 0x000046E2
			// (set) Token: 0x06000097 RID: 151 RVA: 0x000064EA File Offset: 0x000046EA
			[DataMember]
			public string numKeys { get; set; }

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x06000098 RID: 152 RVA: 0x000064F3 File Offset: 0x000046F3
			// (set) Token: 0x06000099 RID: 153 RVA: 0x000064FB File Offset: 0x000046FB
			[DataMember]
			public string version { get; set; }

			// Token: 0x1700001C RID: 28
			// (get) Token: 0x0600009A RID: 154 RVA: 0x00006504 File Offset: 0x00004704
			// (set) Token: 0x0600009B RID: 155 RVA: 0x0000650C File Offset: 0x0000470C
			[DataMember]
			public string customerPanelLink { get; set; }

			// Token: 0x1700001D RID: 29
			// (get) Token: 0x0600009C RID: 156 RVA: 0x00006515 File Offset: 0x00004715
			// (set) Token: 0x0600009D RID: 157 RVA: 0x0000651D File Offset: 0x0000471D
			[DataMember]
			public string downloadLink { get; set; }
		}

		// Token: 0x02000010 RID: 16
		[Nullable(0)]
		public class app_data_class
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x0600009F RID: 159 RVA: 0x00006526 File Offset: 0x00004726
			// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000652E File Offset: 0x0000472E
			public string numUsers { get; set; }

			// Token: 0x1700001F RID: 31
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x00006537 File Offset: 0x00004737
			// (set) Token: 0x060000A2 RID: 162 RVA: 0x0000653F File Offset: 0x0000473F
			public string numOnlineUsers { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000A3 RID: 163 RVA: 0x00006548 File Offset: 0x00004748
			// (set) Token: 0x060000A4 RID: 164 RVA: 0x00006550 File Offset: 0x00004750
			public string numKeys { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000A5 RID: 165 RVA: 0x00006559 File Offset: 0x00004759
			// (set) Token: 0x060000A6 RID: 166 RVA: 0x00006561 File Offset: 0x00004761
			public string version { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000656A File Offset: 0x0000476A
			// (set) Token: 0x060000A8 RID: 168 RVA: 0x00006572 File Offset: 0x00004772
			public string customerPanelLink { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000657B File Offset: 0x0000477B
			// (set) Token: 0x060000AA RID: 170 RVA: 0x00006583 File Offset: 0x00004783
			public string downloadLink { get; set; }
		}

		// Token: 0x02000011 RID: 17
		[Nullable(0)]
		public class user_data_class
		{
			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000AC RID: 172 RVA: 0x0000658C File Offset: 0x0000478C
			// (set) Token: 0x060000AD RID: 173 RVA: 0x00006594 File Offset: 0x00004794
			public string username { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000AE RID: 174 RVA: 0x0000659D File Offset: 0x0000479D
			// (set) Token: 0x060000AF RID: 175 RVA: 0x000065A5 File Offset: 0x000047A5
			public string ip { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000B0 RID: 176 RVA: 0x000065AE File Offset: 0x000047AE
			// (set) Token: 0x060000B1 RID: 177 RVA: 0x000065B6 File Offset: 0x000047B6
			public string hwid { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000B2 RID: 178 RVA: 0x000065BF File Offset: 0x000047BF
			// (set) Token: 0x060000B3 RID: 179 RVA: 0x000065C7 File Offset: 0x000047C7
			public string createdate { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x000065D0 File Offset: 0x000047D0
			// (set) Token: 0x060000B5 RID: 181 RVA: 0x000065D8 File Offset: 0x000047D8
			public string lastlogin { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x060000B6 RID: 182 RVA: 0x000065E1 File Offset: 0x000047E1
			// (set) Token: 0x060000B7 RID: 183 RVA: 0x000065E9 File Offset: 0x000047E9
			public List<api.Data> subscriptions { get; set; }
		}

		// Token: 0x02000012 RID: 18
		[Nullable(0)]
		public class Data
		{
			// Token: 0x1700002A RID: 42
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x000065F2 File Offset: 0x000047F2
			// (set) Token: 0x060000BA RID: 186 RVA: 0x000065FA File Offset: 0x000047FA
			public string subscription { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000BB RID: 187 RVA: 0x00006603 File Offset: 0x00004803
			// (set) Token: 0x060000BC RID: 188 RVA: 0x0000660B File Offset: 0x0000480B
			public string expiry { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000BD RID: 189 RVA: 0x00006614 File Offset: 0x00004814
			// (set) Token: 0x060000BE RID: 190 RVA: 0x0000661C File Offset: 0x0000481C
			public string timeleft { get; set; }
		}

		// Token: 0x02000013 RID: 19
		[Nullable(0)]
		public class response_class
		{
			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006625 File Offset: 0x00004825
			// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000662D File Offset: 0x0000482D
			public bool success { get; set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000C2 RID: 194 RVA: 0x00006636 File Offset: 0x00004836
			// (set) Token: 0x060000C3 RID: 195 RVA: 0x0000663E File Offset: 0x0000483E
			public string message { get; set; }
		}
	}
}
