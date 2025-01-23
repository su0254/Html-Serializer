using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    public class HTMLHelper
    {
        public static HTMLHelper Instance => _instance;
        private readonly static HTMLHelper _instance = new HTMLHelper();
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HTMLHelper()
        {
            HtmlTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON Files/HtmlTags.json"));
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON Files/HtmlVoidTags.json"));
        }
    }
}
