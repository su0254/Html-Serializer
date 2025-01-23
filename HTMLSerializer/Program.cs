
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HTMLSerializer;

static HTMLElement ExtractHtmlElement(string line)
{
    HTMLElement element = new HTMLElement();
    var firstWord = line.TrimStart().Split(' ')[0];
    element.Name = firstWord;
    foreach (Match atrr in new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line))
    {
        if (atrr.Groups.Count == 3)
        {
            string name = atrr.Groups[1].Value;
            string value = atrr.Groups[2].Value;
            if (name == "id")
                element.Id = value;
            else if (name == "class")
                element.Classes = value.Split(' ').ToList();
            else
                element.Atributes[name] = value;
        }
    }
    return element;
}
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

var html = await Load("https://www.malkabruk.co.il/login?app=Chat");
//var html = File.ReadAllText("JSON files/TextFile1.txt");
var cleanHtml = new Regex("\\s").Replace(html, " ");
var tagMatches = Regex.Matches(cleanHtml, @"<\/?([a-zA-Z][a-zA-Z0-9]*)\b[^>]*>|([^<]+)").Where(l => !String.IsNullOrWhiteSpace(l.Value));

var htmlLines = new List<string>();
foreach (Match item in tagMatches)
{
    string tag = item.Value.Trim();
    if (tag.StartsWith('<'))
        tag = tag.Trim('<', '>');
    htmlLines.Add(tag);
}
HTMLElement root = ExtractHtmlElement(htmlLines[1]);
HTMLElement current = root;

for (int i = 2; i < htmlLines.Count(); i++)
{
    var firstWord = htmlLines[i].TrimStart().Split(' ')[0];
    
    if (HTMLHelper.Instance.HtmlTags.Contains(firstWord) || HTMLHelper.Instance.HtmlVoidTags.Contains(firstWord))
    {
        HTMLElement element = ExtractHtmlElement(htmlLines[i]);
        element.Parent = current;
        current.Childrens.Add(element);
        if (!HTMLHelper.Instance.HtmlVoidTags.Contains(firstWord))
            current = element;
    }
    else if (firstWord.StartsWith('/')&& current != null)
        current = current.Parent;
    else if (current != null)
        current.InnerHtml += htmlLines[i];
}

foreach (var item in root.FilterElements(".modal-content div"))
{
    Console.WriteLine(item.Name + " " + item.Id);
    foreach (var classes in item.Classes)
        Console.WriteLine(classes);
}
