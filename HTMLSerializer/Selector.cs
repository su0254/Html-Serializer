using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    internal class Selector
    {
        public string TagName { get; set; } = "";
        public string Id { get; set; } = "";
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; } 
        public Selector Child { get; set; }

        private static Selector SelectorChild(string query)
        {
            Selector current = new Selector();
            int j = 0;
            if (Char.IsLetter(query[0]))
            {
                while (Char.IsLetter(query[j++])) ;
                string name = query.Substring(0, j-1);
                if(HTMLHelper.Instance.HtmlTags.Contains(name))
                    current.TagName = name;
                var atributes = query.Substring(j).Split('.');
                for (int k = 0; k < atributes.Length; k++)
                {
                    if (!atributes[k].Contains('#'))
                        current.Classes.Add(atributes[k]);
                    else
                    {
                        var id = atributes[k].Split('#');
                        if (id.Length > 1)
                        {
                            current.Classes.Add(id[0]);
                            current.Id = id[1];
                        }
                        else
                            current.Id = id[0];
                    }
                }
            }
            return current;
        }
        public static Selector ExtractSelector(string query)
        {
            var queries = query.Split(' ');
            var root = SelectorChild(queries[0]);
            var current = root;
            for (int i = 1; i < queries.Length; i++)
            {
                current.Child = SelectorChild(queries[i]);
                current.Child.Parent = current;
                current = current.Child;
            }
            return root;
        }
    }
}