using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebAssemblyMan.SVGRender
{
    public class SVG : IEnumerable<string>
    {
        private List<string> internalList = new List<string>();
        public IEnumerator<string> GetEnumerator() => internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => internalList.GetEnumerator();
        public List<string> GetAttributes()
        {
            return internalList;
        }

        public void Add(string name, string value) => internalList.Add($@"{name}={value}");

        private List<SVG> m_innerCollection = new List<SVG>();
        public List<SVG> GetChildren()
        {
            return m_innerCollection;
        }
        //public IEnumerator<SVG> GetItemsEnumerator() => m_innerCollection.GetEnumerator();
        //IEnumerator IEnumerable.GetEnumerator() => m_innerCollection.GetEnumerator();
        public void AddItems(params SVG[] items)
        {
            foreach (var item in items)
                m_innerCollection.Add(item);
        }
        public void Draw()
        {
            //if here it will end up depnednecy on buildrendertree
        }

        public string type = "svg";
    }
}
