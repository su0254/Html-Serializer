using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HTMLSerializer
{
    internal class HTMLElement
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public Dictionary<string, string> Atributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HTMLElement Parent { get; set; }
        public List<HTMLElement> Childrens { get; set; } = new List<HTMLElement>();

        public IEnumerable<HTMLElement> Descendants()
        {
            Queue<HTMLElement> queue = new Queue<HTMLElement>();
            HTMLElement current = this;
            queue.Enqueue(current);
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                for (int i = 0; i < current.Childrens.Count; i++)
                {
                    queue.Enqueue(current.Childrens[i]);
                }
                yield return current;
            }
        }

        public IEnumerable<HTMLElement> Ancestors()
        {
            HTMLElement parent = this.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        private bool MatchElements(Selector selector)
        {
            if(selector==null) return false;
            if (selector.TagName != this.Name)  return false;
            for (int i = 0;i< this.Classes.Count&&i<selector.Classes.Count;i++)
                if (selector.Classes[i] != this.Classes[i]) return false;
            if (selector.Id != this.Id) return false;
            return true;
        }
        private void FilterElements(Selector selector, HTMLElement element, HashSet<HTMLElement> matches)
        {
            var children = element.Descendants().ToList();
            foreach (HTMLElement child in children)
            {
                if(child.MatchElements(selector))
                    if(selector.Child != null)
                       FilterElements(selector.Child, child, matches);
                    else
                        matches.Add(child);
            }
        }

        public HashSet<HTMLElement> FilterElements(string selector)
        {
            HashSet<HTMLElement> matches = new HashSet<HTMLElement>();

            FilterElements(Selector.ExtractSelector(selector), this, matches);
            return matches;
        }

        
    }
}
