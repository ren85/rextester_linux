using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Services
{
	public class YcmdEngine
	{
		public static List<string> GetCppCompletions(string code, int line, int column)
		{
			//var rn = (new Random ()).Next (Int32.MaxValue - 1).ToString ();
			string template = 
				@"{
				""file_data"":{
					""12345"":{
						""filetypes"":[
							""cpp""
						],
						__MAGIC__2
					}
				},
				""line_num"":__MAGIC__3,
				""column_num"":__MAGIC__4,
				""filepath"":""12345""
			}";

			/*var line_val = code.Split ("\n".ToCharArray ()) [line].Substring (0, column);
			string query = "";
			var ind = line_val.LastIndexOfAny (">:.".ToCharArray ());
			if (ind >= 0 && ind + 1 < line_val.Length) {
				query = line_val.Substring (ind + 1);
				line_val = line_val.Substring (0, ind + 1);
				column -= query.Length;
			}*/
			var c = JsonConvert.SerializeObject (new { contents =  code}).Trim("{}".ToCharArray());
			//var l = JsonConvert.SerializeObject (new { line_value = line_val }).Trim("{}".ToCharArray());
			//var q = JsonConvert.SerializeObject (new { query = query }).Trim("{}".ToCharArray());

			var data = template.Replace ("__MAGIC__2", c)
							   .Replace ("__MAGIC__3", (line + 1) + "")
							   .Replace ("__MAGIC__4", (column + 1) + "");
			using (WebClient client = new WebClient ()) 
			{
				client.Headers.Add (HttpResponseHeader.ContentType, "application/json");
				var val = client.UploadString ("http://localhost:51755/completions", data);			
				var res = JsonConvert.DeserializeObject<Res> (val)/*.Where(f => f.menu_text.Contains("mt19"))*/.completions;
				return res.Select(f => string.IsNullOrEmpty(f.menu_text) ? f.insertion_text : f.menu_text).ToList();
			}
		}
	}

	public class Res
	{
		public List<object> errors { get; set; }
		public List<Completion> completions { get; set; }
		public int completion_start_column { get; set; }
	}
	public class Completion
	{
		public string menu_text { get; set; }
		public string insertion_text { get; set; }
		public string detailed_info { get; set; }
		public string extra_menu_info { get; set; }
		public string kind { get; set; }
	}
}

