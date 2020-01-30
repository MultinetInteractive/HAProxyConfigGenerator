using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HAProxyConfigGenerator
{
	public class HAProxyConfig
	{
		public static HAProxyConfig Parse(string file)
		{
			return JsonConvert.DeserializeObject<HAProxyConfig>(File.ReadAllText(file));
		}

		public List<UserList> UserLists { get; set; } = new List<UserList>();
		public GlobalSection Global { get; set; } = new GlobalSection();
		public DefaultsSection Defaults { get; set; } = new DefaultsSection();
		public List<Frontend> FrontEnd { get; set; } = new List<Frontend>();
		public List<Backend> BackEnd { get; set; } = new List<Backend>();
		[JsonProperty("cache")]
		public List<Cache> Caches { get; set; } = new List<Cache>();

		#region Subclasses

		public class Compression
		{
			public List<string> Algorithms { get; set; } = new List<string>();
			public List<string> Types { get; set; } = new List<string>();
			public bool Offload { get; set; }
		}

		public class Frontend
		{
			public string Name { get; set; }
			public long? MaxConn { get; set; }
			public string Mode { get; set; }
			[JsonProperty("default_backend")]
			public string DefaultBackend { get; set; }

			public Compression Compression { get; set; }

			public CaptureItem Capture { get; set; }

			public List<AclRow> ACL { get; set; } = new List<AclRow>();
			public List<BindItem> Bind { get; set; } = new List<BindItem>();
			public Dictionary<string, object> Option { get; set; } = new Dictionary<string, object>();
			public Dictionary<string, object> Stats { get; set; } = new Dictionary<string, object>();
			public Dictionary<string, string> ErrorFile { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string> Timeout { get; set; } = new Dictionary<string, string>();
			public List<RequestCaseInsensitiveReplace> ReqIRep { get; set; } = new List<RequestCaseInsensitiveReplace>();
			public List<string> RspIDel { get; set; } = new List<string>();
			[JsonProperty("http-response")]
			public HttpResponse HttpResponse { get; set; }
			[JsonProperty("http-request")]
			public HttpRequest HttpRequest { get; set; }
			[JsonProperty("redirect")]
			public Redirect Redirect { get; set; }
			[JsonProperty("use_backend")]
			public List<UseBackend> BackEnds { get; set; } = new List<UseBackend>();

			public class CaptureItem
			{
				public List<object> Request { get; set; } = new List<object>();
				public List<object> Response { get; set; } = new List<object>();
			}

			public class UseBackend
			{
				public string Backend { get; set; }
				public object Conditions { get; set; }
			}

			public class BindItem
			{
				public string Host { get; set; }
				public object Options { get; set; }
			}
		}

		public class Backend
		{
			public string Name { get; set; }
			public string Balance { get; set; }
			public long? FullConn { get; set; }
			public string Mode { get; set; }

			public Compression Compression { get; set; }

			public List<AclRow> ACL { get; set; } = new List<AclRow>();
			public Dictionary<string, object> Option { get; set; } = new Dictionary<string, object>();
			public Dictionary<string, object> Stats { get; set; } = new Dictionary<string, object>();
			public Dictionary<string, string> ErrorFile { get; set; } = new Dictionary<string, string>();
			public List<CookieRow> Cookie { get; set; } = new List<CookieRow>();
			public Dictionary<string, string> Timeout { get; set; } = new Dictionary<string, string>();
			public List<Server> Servers { get; set; } = new List<Server>();
			public List<RequestCaseInsensitiveReplace> ReqIRep { get; set; } = new List<RequestCaseInsensitiveReplace>();

			[JsonProperty("http-check")]
			public HttpCheck HttpCheck { get; set; }
			[JsonProperty("http-response")]
			public HttpResponse HttpResponse { get; set; }
			[JsonProperty("http-request")]
			public HttpRequest HttpRequest { get; set; }
			[JsonProperty("redirect")]
			public Redirect Redirect { get; set; }

			public class Server
			{
				public string Name { get; set; }
				public string Host { get; set; }
				public int Port { get; set; }
				public List<string> Options { get; set; } = new List<string>();
			}

			public class CookieRow
			{
				public string Name { get; set; }
				public string Method { get; set; }
			}
		}

		public class Cache
		{
			[JsonProperty("cacheName")]
			public string CacheName { get; set; }
			[JsonProperty("totalMaxSize")]
			public int TotalMaxSize { get; set; }
			[JsonProperty("maxAge")]
			public int MaxAge { get; set; }
		}

		public class HttpCheck
		{
			public List<string> Expect { get; set; } = new List<string>();
		}

		public class HttpResponse
		{
			[JsonProperty("set-header")]
			public List<SetHeader> SetHeader { get; set; } = new List<SetHeader>();
			[JsonProperty("replace-header")]
			public List<ReplaceHeader> ReplaceHeader { get; set; } = new List<ReplaceHeader>();
			[JsonProperty("cache-store")]
			public List<string> CacheStore { get; set; } = new List<string>();
		}

		public class HttpRequest
		{
			[JsonProperty("set-header")]
			public List<SetHeader> SetHeader { get; set; } = new List<SetHeader>();
			[JsonProperty("replace-header")]
			public List<ReplaceHeader> ReplaceHeader { get; set; } = new List<ReplaceHeader>();
			[JsonProperty("redirect")]
			public List<RedirectHeader> Redirect { get; set; } = new List<RedirectHeader>();

			[JsonProperty("deny")]
			public List<string> Deny { get; set; } = new List<string>();
			[JsonProperty("silent-drop")]
			public List<string> SilentDrop { get; set; } = new List<string>();
			[JsonProperty("cache-use")]
			public List<string> CacheUse { get; set; } = new List<string>();
		}

		public class Redirect
		{
			public List<RedirectScheme> Scheme { get; set; } = new List<RedirectScheme>();
			public List<RedirectPrefix> Prefix { get; set; } = new List<RedirectPrefix>();
			public List<RedirectLocation> Location { get; set; } = new List<RedirectLocation>();
		}

		public class RedirectScheme
		{
			public string Protocol { get; set; }
			public int? Code { get; set; }
			public List<string> Option { get; set; } = new List<string>();
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class RedirectPrefix
		{
		}

		public class RedirectLocation
		{
		}

		public class RedirectHeader
		{
			public int? Code { get; set; }
			public string Url { get; set; }
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class SetHeader
		{
			public string Header { get; set; }
			public string Value { get; set; }
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class ReplaceHeader
		{
			public string Header { get; set; }
			public string Match { get; set; }
			public string Replace { get; set; }
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class RequestCaseInsensitiveReplace
		{
			public string Match { get; set; }
			public string Replace { get; set; }
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class AclRow
		{
			public string Name { get; set; }
			public List<string> Conditions { get; set; } = new List<string>();
		}

		public class DefaultsSection
		{
			public string Log { get; set; }
			public string Mode { get; set; }
			public string Balance { get; set; }
			[JsonProperty("load-server-state-from-file")]
			public string LoadServerStateFromFile { get; set; }

			public Compression Compression { get; set; }

			public long? MaxConn { get; set; }
			public int? Retries { get; set; }
			public Dictionary<string, string> ErrorFile { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, string> Timeout { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, object> Option { get; set; } = new Dictionary<string, object>();
		}

		public class GlobalSection
		{
			[JsonProperty("ca-base")]
			public string CA_Base { get; set; }
			[JsonProperty("crt-base")]
			public string CRT_Base { get; set; }
			[JsonProperty("chroot")]
			public string CHRoot { get; set; }
			[JsonProperty("hard-stop-after")]
			public string HardStopAfter { get; set; }
			public bool? Daemon { get; set; }
			[JsonProperty("Log")]
			public List<Log> LogItems { get; set; } = new List<Log>();
			public string User { get; set; }
			public string Group { get; set; }
			public long? MaxConn { get; set; }
			[JsonProperty("ssl-server-verify")]
			public string SSLServerVerify { get; set; }
			[JsonProperty("ssl-default-bind-ciphers")]
			public string SSLDefaultBindCiphers { get; set; }
			[JsonProperty("ssl-default-bind-options")]
			public string SSLDefaultBindOptions { get; set; }
			[JsonProperty("spread-checks")]
			public int? SpreadChecks { get; set; }
			[JsonProperty("server-state-file")]
			public string ServerStateFile { get; set; }
			[JsonProperty("ssl-engine")]
			public string SSLEngine { get; set; }
			[JsonProperty("ssl-mode-async")]
			public bool SSLModeAsync { get; set; }
			public int? nbproc { get; set; }
			public int? nbthread { get; set; }
			[JsonProperty("cpu-map")]
			public string CpuMap { get; set; }

			public Dictionary<string, string> Stats { get; set; } = new Dictionary<string, string>();
			public Dictionary<string, object> Tune { get; set; } = new Dictionary<string, object>();

			public class Log
			{
				public string Path { get; set; }
				public string Facility { get; set; }
				public string MaxLevel { get; set; }
				public string MinLevel { get; set; }
			}
		}

		public class UserList
		{
			public string Name { get; set; }
			public List<Group> Groups { get; set; } = new List<Group>();
			public List<User> Users { get; set; }

			public class Group
			{
				public string Name { get; set; }
				public List<string> Users { get; set; } = new List<string>();
			}

			public class User
			{
				public string Name { get; set; }
				[JsonProperty("password-type")]
				public string PasswordType { get; set; }
				public string Password { get; set; }
				public List<string> Groups { get; set; } = new List<string>();
			}
		}
		#endregion

		public string ToHAProxyConfigString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("# Generated by MultiNet Interactive AB (https://www.multinet.com) HAProxy-JSON->config generator (https://github.com/MultinetInteractive/HAProxyConfigGenerator)");
			sb.AppendLine("# Generated @ " + DateTime.UtcNow.ToString() + " UTC");
			sb.AppendLine();

			RenderUserLists(sb);
			RenderGlobalSection(sb);
			RenderDefaultsSection(sb);
			RenderFrontends(sb);
			RenderBackends(sb);
			RenderCaches(sb);

			var endConfig = sb.ToString();
			while (endConfig.Contains(Environment.NewLine + Environment.NewLine + Environment.NewLine))
			{
				endConfig = endConfig.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine);
			}
			return endConfig.Trim();
		}

		private void RenderCaches(StringBuilder sb)
		{
			foreach (var c in Caches.OrderBy(f => f.CacheName))
			{
				RenderCache(sb, c);
			}
		}

		private void RenderCache(StringBuilder sb, Cache c)
		{
			sb.AppendLine(string.Format("cache {0}", c.CacheName));
			if (c.TotalMaxSize > 0)
			{
				sb.AppendLine(string.Format("    total-max-size\t{0}", c.TotalMaxSize));
			}
			if (c.MaxAge > 0)
			{
				sb.AppendLine(string.Format("    max-age\t{0}", c.MaxAge));
			}
		}

		public void RenderFrontends(StringBuilder sb)
		{
			foreach (var f in FrontEnd.OrderBy(f => f.Name))
			{
				RenderFrontEnd(sb, f);
			}
		}

		private void RenderFrontEnd(StringBuilder sb, Frontend f)
		{
			sb.AppendLine(string.Format("frontend\t{0}", f.Name).TrimEnd());

			foreach (var bind in f.Bind)
			{
				sb.AppendLine(string.Format("    bind\t{0}\t{1}", bind.Host, (bind.Options is bool ? "" : string.Join(" ", ((JArray)bind.Options).ToList()))));
			}
			sb.AppendLine();

			if (!string.IsNullOrWhiteSpace(f.Mode))
			{
				sb.AppendLine(string.Format("    mode\t{0}", f.Mode).TrimEnd());
			}

			if (f.MaxConn.HasValue)
			{
				sb.AppendLine(string.Format("    maxconn\t{0}", f.MaxConn).TrimEnd());
			}

			if (f.Compression != null)
			{
				if (f.Compression.Algorithms?.Count > 0)
				{
					sb.AppendLine("    compression algo " + string.Join(" ", f.Compression.Algorithms));
				}
				if (f.Compression.Types?.Count > 0)
				{
					sb.AppendLine("    compression type " + string.Join(" ", f.Compression.Types));
				}
				if (f.Compression.Offload)
				{
					sb.AppendLine("    compression offload");
				}
			}
			sb.AppendLine();

			foreach (var a in f.ACL)
			{
				sb.AppendLine(string.Format("    acl\t{0}\t{1}", a.Name, string.Join(" ", a.Conditions)).TrimEnd());
			}
			sb.AppendLine();

			if (f.Capture != null)
			{
				foreach (JArray cr in f.Capture.Request)
				{
					sb.AppendLine(string.Format("    capture\trequest\theader\t{0}\tlen\t{1}", cr.First, cr.Last));
				}
				sb.AppendLine();

				foreach (JArray cr in f.Capture.Response)
				{
					sb.AppendLine(string.Format("    capture\tresponse\theader\t{0}\tlen\t{1}", cr.First, cr.Last));
				}
			}
			sb.AppendLine();

			foreach (var kv in f.Option)
			{
				var vd = kv.Value as JObject;
				if (!(kv.Value is bool))
				{
					foreach (var vk in vd)
					{
						sb.AppendLine(string.Format("    option\t{0}.{1}\t{2}", kv.Key, vk.Key, vk.Value).TrimEnd());
					}
				}
				else
				{
					sb.AppendLine(string.Format("    option\t{0}", kv.Key).TrimEnd());
				}
			}
			sb.AppendLine();

			if (f.HttpRequest != null)
			{
				foreach (var sh in f.HttpRequest.SetHeader)
				{
					sb.AppendLine(string.Format("    http-request\tset-header\t{0}\t{1}\t{2}", sh.Header, sh.Value, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in f.HttpRequest.ReplaceHeader)
				{
					sb.AppendLine(string.Format("    http-request\treplace-header\t{0}\t{1}\t{2}\t{3}", sh.Header, sh.Match, sh.Replace, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in f.HttpRequest.Redirect)
				{
					sb.AppendLine(string.Format("    http-request\tredirect\t{0}\t{1}\t{2}", (sh.Code.HasValue ? "code " + sh.Code + "\tlocation" : "location"), sh.Url, string.Join(" ", sh.Conditions)));
				}

				sb.AppendLine();

				if (f.HttpRequest.Deny.Count > 0)
				{
					sb.AppendLine(string.Format("    http-request\tdeny\tif\t{0}", string.Join(" ", f.HttpRequest.Deny)));
				}

				sb.AppendLine();

				if (f.HttpRequest.SilentDrop.Count > 0)
				{
					sb.AppendLine(string.Format("    http-request\tsilent-drop\tif\t{0}", string.Join(" ", f.HttpRequest.SilentDrop)));
				}

				sb.AppendLine();
			}

			if (f.HttpResponse != null)
			{
				foreach (var sh in f.HttpResponse.SetHeader)
				{
					sb.AppendLine(string.Format("    http-response\tset-header\t{0}\t{1}\t{2}", sh.Header, sh.Value, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in f.HttpResponse.ReplaceHeader)
				{
					sb.AppendLine(string.Format("    http-response\treplace-header\t{0}\t{1}\t{2}\t{3}", sh.Header, sh.Match, sh.Replace, string.Join(" ", sh.Conditions)));
				}

				sb.AppendLine();
			}

			foreach (var rs in f.RspIDel)
			{
				sb.AppendLine(string.Format("    rspidel\t{0}", rs));
			}
			sb.AppendLine();

			foreach (var reqirep in f.ReqIRep)
			{
				sb.AppendLine(string.Format("    reqirep\t{0}\t{1}\t{2}", reqirep.Match, reqirep.Replace, string.Join(" ", reqirep.Conditions)));
			}
			sb.AppendLine();

			if (f.Redirect != null)
			{
				foreach (var sch in f.Redirect.Scheme)
				{
					sb.AppendLine(string.Format("    redirect\tscheme\t{0}\t{1}\t{2}\t{3}", sch.Protocol, (sch.Code.HasValue ? "code " + sch.Code : ""), string.Join(" ", sch.Option), string.Join(" ", sch.Conditions)));
				}
				sb.AppendLine();
			}

			foreach (var ub in f.BackEnds)
			{
				sb.AppendLine(string.Format("    use_backend\t{0}\t{1}", ub.Backend, (ub.Conditions is bool ? "" : string.Join(" ", ((JArray)ub.Conditions).ToList()))));
			}

			sb.AppendLine();

			if (!string.IsNullOrWhiteSpace(f.DefaultBackend))
			{
				sb.AppendLine(string.Format("    default_backend\t{0}", f.DefaultBackend).TrimEnd());
			}

			sb.AppendLine();
			sb.AppendLine();
		}

		public void RenderBackends(StringBuilder sb)
		{
			foreach (var b in BackEnd.OrderBy(b2 => b2.Name))
			{
				RenderBackEnd(sb, b);
			}
		}

		private void RenderBackEnd(StringBuilder sb, Backend b)
		{
			sb.AppendLine(string.Format("backend\t{0}", b.Name).TrimEnd());

			if (!string.IsNullOrWhiteSpace(b.Mode))
			{
				sb.AppendLine(string.Format("    mode\t{0}", b.Mode).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(b.Balance))
			{
				sb.AppendLine(string.Format("    balance\t{0}", b.Balance).TrimEnd());
			}

			if (b.FullConn.HasValue)
			{
				sb.AppendLine(string.Format("    fullconn\t{0}", b.FullConn).TrimEnd());
			}
			sb.AppendLine();

			if (b.Compression != null)
			{
				if (b.Compression.Algorithms?.Count > 0)
				{
					sb.AppendLine("    compression algo " + string.Join(" ", b.Compression.Algorithms));
				}
				if (b.Compression.Types?.Count > 0)
				{
					sb.AppendLine("    compression type " + string.Join(" ", b.Compression.Types));
				}
				if (b.Compression.Offload)
				{
					sb.AppendLine("    compression offload");
				}
			}
			sb.AppendLine();

			foreach (var a in b.ACL)
			{
				sb.AppendLine(string.Format("    acl\t{0}\t{1}", a.Name, string.Join(" ", a.Conditions)).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in b.Timeout)
			{
				sb.AppendLine(string.Format("    timeout\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
			}
			sb.AppendLine();

			foreach (var s in b.Cookie)
			{
				sb.AppendLine(string.Format("    cookie\t{0}\t{1}", s.Name, s.Method).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in b.ErrorFile)
			{
				sb.AppendLine(string.Format("    errorfile\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in b.Stats)
			{
				if (kv.Value == null) continue;
				if (kv.Value is bool)
				{
					sb.AppendLine(string.Format("    stats\t{0}", kv.Key).TrimEnd());
				}
				else if (kv.Value is JArray)
				{
					sb.AppendLine(string.Format("    stats\t{0}\t{1}", kv.Key, string.Join(" ", (JArray)kv.Value)).TrimEnd());
				}
				else
				{
					sb.AppendLine(string.Format("    stats\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
				}
			}
			sb.AppendLine();

			foreach (var kv in b.Option)
			{
				if (kv.Key == "httpchk")
				{
					var chk = kv.Value as JObject;
					sb.AppendLine(string.Format("    option\thttpchk\t{0}\t{1}\t{2}", chk.Property("method").Value, chk.Property("uri").Value, chk.Property("version").Value).TrimEnd());
				}
				else
				{
					sb.AppendLine(string.Format("    option\t{0}", kv.Key).TrimEnd());
				}
			}
			sb.AppendLine();

			if (b.HttpRequest != null)
			{
				foreach (var sh in b.HttpRequest.SetHeader)
				{
					sb.AppendLine(string.Format("    http-request\tset-header\t{0}\t{1}\t{2}", sh.Header, sh.Value, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in b.HttpRequest.ReplaceHeader)
				{
					sb.AppendLine(string.Format("    http-request\treplace-header\t{0}\t{1}\t{2}\t{3}", sh.Header, sh.Match, sh.Replace, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in b.HttpRequest.Redirect)
				{
					sb.AppendLine(string.Format("    http-request\tredirect\t{0}\t{1}\t{2}", (sh.Code.HasValue ? "code\t" + sh.Code + "\tlocation" : "location"), sh.Url, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				if (b.HttpRequest.CacheUse.Count > 0)
				{
					sb.AppendLine(string.Format("    http-request\tcache-use\t{0}", string.Join(" ", b.HttpRequest.CacheUse)));
				}
				sb.AppendLine();
			}

			if (b.HttpResponse != null)
			{
				foreach (var sh in b.HttpResponse.SetHeader)
				{
					sb.AppendLine(string.Format("    http-response\tset-header\t{0}\t{1}\t{2}", sh.Header, sh.Value, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				foreach (var sh in b.HttpResponse.ReplaceHeader)
				{
					sb.AppendLine(string.Format("    http-response\treplace-header\t{0}\t{1}\t{2}\t{3}", sh.Header, sh.Match, sh.Replace, string.Join(" ", sh.Conditions)));
				}
				sb.AppendLine();

				if (b.HttpResponse.CacheStore.Count > 0)
				{
					sb.AppendLine(string.Format("    http-response\tcache-store\t{0}", string.Join(" ", b.HttpResponse.CacheStore)));
				}
				sb.AppendLine();
			}

			foreach (var reqirep in b.ReqIRep)
			{
				sb.AppendLine(string.Format("    reqirep\t{0}\t{1}\t{2}", reqirep.Match, reqirep.Replace, string.Join(" ", reqirep.Conditions)));
			}
			sb.AppendLine();

			if (b.Redirect != null)
			{
				foreach (var sch in b.Redirect.Scheme)
				{
					sb.AppendLine(string.Format("    redirect\tscheme\t{0}\t{1}\t{2}\t{3}", sch.Protocol, (sch.Code.HasValue ? "code\t" + sch.Code : ""), string.Join(" ", sch.Option), string.Join(" ", sch.Conditions)));
				}
				sb.AppendLine();
			}

			if (b.HttpCheck?.Expect.Count > 0)
			{
				sb.AppendLine(string.Format("    http-check\texpect\t{0}", string.Join(" ", b.HttpCheck.Expect)).TrimEnd());
			}

			sb.AppendLine();

			foreach (var s in b.Servers)
			{
				sb.AppendLine(string.Format("    server\t{0}\t{1}:{2}\t{3}", s.Name, s.Host, s.Port, string.Join(" ", s.Options)));
			}
			sb.AppendLine();
			sb.AppendLine();
		}

		private void RenderDefaultsSection(StringBuilder sb)
		{
			sb.AppendLine("defaults");
			if (!string.IsNullOrWhiteSpace(Defaults.Log))
			{
				sb.AppendLine(string.Format("    log\t{0}", Defaults.Log).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Defaults.Mode))
			{
				sb.AppendLine(string.Format("    mode\t{0}", Defaults.Mode).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Defaults.Balance))
			{
				sb.AppendLine(string.Format("    balance\t{0}", Defaults.Balance).TrimEnd());
			}

			if (Defaults.MaxConn.HasValue)
			{
				sb.AppendLine(string.Format("    maxconn\t{0}", Defaults.MaxConn).TrimEnd());
			}

			if (Defaults.Retries.HasValue)
			{
				sb.AppendLine(string.Format("    retries\t{0}", Defaults.Retries).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Defaults.LoadServerStateFromFile))
			{
				sb.AppendLine(string.Format("    load-server-state-from-file\t{0}", Defaults.LoadServerStateFromFile).TrimEnd());
			}

			sb.AppendLine();

			if (Defaults.Compression != null)
			{
				if (Defaults.Compression.Algorithms?.Count > 0)
				{
					sb.AppendLine("    compression algo " + string.Join(" ", Defaults.Compression.Algorithms));
				}
				if (Defaults.Compression.Types?.Count > 0)
				{
					sb.AppendLine("    compression type " + string.Join(" ", Defaults.Compression.Types));
				}
				if (Defaults.Compression.Offload)
				{
					sb.AppendLine("    compression offload");
				}
			}
			sb.AppendLine();

			foreach (var kv in Defaults.ErrorFile)
			{
				sb.AppendLine(string.Format("    errorfile\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in Defaults.Timeout)
			{
				sb.AppendLine(string.Format("    timeout\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in Defaults.Option)
			{
				var vd = kv.Value as JObject;
				if (!(kv.Value is bool))
				{
					foreach (var vk in vd)
					{
						sb.AppendLine(string.Format("    option\t{0}.{1}\t{2}", kv.Key, vk.Key, vk.Value).TrimEnd());
					}
				}
				else
				{
					sb.AppendLine(string.Format("    option\t{0}", kv.Key).TrimEnd());
				}
			}
			sb.AppendLine();
			sb.AppendLine();
		}

		private void RenderGlobalSection(StringBuilder sb)
		{
			sb.AppendLine("global");
			foreach (var l in Global.LogItems)
			{
				sb.AppendLine(string.Format("    log\t{0}\t{1}\t{2}\t{3}", l.Path, l.Facility, l.MaxLevel, l.MinLevel).TrimEnd());
			}
			sb.AppendLine();

			if (!string.IsNullOrWhiteSpace(Global.HardStopAfter))
			{
				sb.AppendLine(string.Format("    hard-stop-after\t{0}", Global.HardStopAfter).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.CA_Base))
			{
				sb.AppendLine(string.Format("    ca-base\t{0}", Global.CA_Base).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.CRT_Base))
			{
				sb.AppendLine(string.Format("    crt-base\t{0}", Global.CRT_Base).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.CHRoot))
			{
				sb.AppendLine(string.Format("    chroot\t{0}", Global.CHRoot).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.ServerStateFile))
			{
				sb.AppendLine(string.Format("    server-state-file\t{0}", Global.ServerStateFile).TrimEnd());
			}

			if (Global.Daemon.HasValue && Global.Daemon.Value)
			{
				sb.AppendLine("    daemon");
			}

			if (!string.IsNullOrWhiteSpace(Global.User))
			{
				sb.AppendLine(string.Format("    user\t{0}", Global.User).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.Group))
			{
				sb.AppendLine(string.Format("    group\t{0}", Global.Group).TrimEnd());
			}

			if (Global.MaxConn.HasValue)
			{
				sb.AppendLine(string.Format("    maxconn\t{0}", Global.MaxConn).TrimEnd());
			}

			if (Global.SpreadChecks.HasValue)
			{
				sb.AppendLine(string.Format("    spread-checks\t{0}", Global.SpreadChecks).TrimEnd());
			}

			sb.AppendLine();

			if (Global.nbproc.HasValue)
			{
				sb.AppendLine(string.Format("    nbproc\t{0}", Global.nbproc).TrimEnd());
			}

			if (Global.nbthread.HasValue)
			{
				sb.AppendLine(string.Format("    nbthread\t{0}", Global.nbthread).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.CpuMap))
			{
				sb.AppendLine(string.Format("    cpu-map\t{0}", Global.CpuMap).TrimEnd());
			}

			sb.AppendLine();

			if (!string.IsNullOrWhiteSpace(Global.SSLServerVerify))
			{
				sb.AppendLine(string.Format("    ssl-server-verify\t{0}", Global.SSLServerVerify).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.SSLDefaultBindCiphers))
			{
				sb.AppendLine(string.Format("    ssl-default-bind-ciphers\t{0}", Global.SSLDefaultBindCiphers).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.SSLDefaultBindOptions))
			{
				sb.AppendLine(string.Format("    ssl-default-bind-options\t{0}", Global.SSLDefaultBindOptions).TrimEnd());
			}

			if (!string.IsNullOrWhiteSpace(Global.SSLEngine))
			{
				sb.AppendLine(string.Format("    ssl-engine\t{0}", Global.SSLEngine).TrimEnd());
			}

			if (Global.SSLModeAsync)
			{
				sb.AppendLine("    ssl-mode-async");
			}

			sb.AppendLine();

			foreach (var kv in Global.Stats)
			{
				sb.AppendLine(string.Format("    stats\t{0}\t{1}", kv.Key, kv.Value).TrimEnd());
			}
			sb.AppendLine();

			foreach (var kv in Global.Tune)
			{
				var vd = kv.Value as JObject;
				if (vd != null)
				{
					foreach (var vk in vd)
					{
						sb.AppendLine(string.Format("    tune.{0}.{1}\t{2}", kv.Key, vk.Key, vk.Value).TrimEnd());
					}
				}
				else
				{
					sb.AppendLine(string.Format("    tune.{0}\t{1}", kv.Key, kv.Value).TrimEnd());
				}
			}

			sb.AppendLine();
			sb.AppendLine();
		}

		private void RenderUserLists(StringBuilder sb)
		{
			foreach (var ul in UserLists)
			{
				sb.AppendLine(string.Format("userlist\t{0}", ul.Name).Trim());
				foreach (var g in ul.Groups)
				{
					var line = (g.Users.Count > 0 ? "    group\t{0}\tusers\t{1}" : "    group\t{0}");
					sb.AppendLine(string.Format(line, g.Name, string.Join(",", g.Users)).TrimEnd());
				}
				foreach (var u in ul.Users)
				{
					var line = (u.Groups.Count > 0 ? "    user\t{0}\t{1}\t{2}\tgroups\t{3}" : "    user\t{0}\t{1}\t{2}");
					sb.AppendLine(string.Format(line, u.Name, u.PasswordType, u.Password, string.Join(",", u.Groups)).TrimEnd());
				}
				sb.AppendLine();
			}

			sb.AppendLine();
			sb.AppendLine();
		}
	}
}
