using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMenus
{
    public class MenuItem<T> : IMenuItem<T>
    {
        private List<IMenuItem<T>> _children;
        private string _itemText;
        private string _itemKey;

        protected MenuItem(int idx, string key, string text, T value, IEnumerable<IMenuItem<T>> children)
            : this(idx, key, text, value)
        {
            _children.AddRange(children);
        }
        public bool IsBack
        {
            get;
            private set;
        }
        public bool IsLeaf
        {
            get { return Count == 0; }
        }
        public T ItemValue
        {
            get;
            set;
        }
        public int ItemIdx
        {
            get;
            set;
        }
        public string ItemKey
        {
            get
            {
                return _itemKey;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("ItemKey");
                if (value.Trim().Length == 0)
                    throw new ArgumentException("ItemKey empty");
                _itemKey = value;
            }
        }
        public string ItemText
        {
            get
            {
                return _itemText;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("ItemText");
                _itemText = value;
            }
        }

        public MenuItem(int idx, string key, string text, T value, bool isBack = false)
        {
            _children = new List<IMenuItem<T>>();
            ItemIdx = idx;
            ItemKey = key;
            ItemText = text;
            ItemValue = value;
            IsBack = isBack;
        }

        public bool ContainsItemKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return _children.Exists(i => i.ItemKey == key);
        }
        public bool ContainsItemIndex(int idx)
        {
            return _children.Exists(i => i.ItemIdx == idx);
        }
        public IMenuItem<T> GetItemByKey(string key)
        {
            if (ContainsItemKey(key))
                return _children.First(i => i.ItemKey == key);
            return null;
        }
        public IMenuItem<T> GetItemByIndex(int index)
        {
            if (ContainsItemIndex(index))
                return _children.First(i => i.ItemIdx == index);
            return null;
        }
        public T Select(IMenuSelector<T> selector)
        {
            return selector.SelectFrom(this);
        }
        public void Add(IMenuItem<T> item)
        {
            if (ContainsItemKey(item.ItemKey))
                throw new InvalidOperationException("Duplicate ItemKey");
            if (ContainsItemIndex(item.ItemIdx))
                throw new InvalidOperationException("Duplicate ItemIdx");
            _children.Add(item);
        }
        public void Clear()
        {
            _children.Clear();
        }
        public bool Contains(IMenuItem<T> item)
        {
            return _children.Contains(item);
        }
        public void CopyTo(IMenuItem<T>[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return _children.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(IMenuItem<T> item)
        {
            return _children.Remove(item);
        }
        public IEnumerator<IMenuItem<T>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_children).GetEnumerator();
        }


        public virtual void OnSelectStarted()
        {

        }

        public virtual void OnSelectFailed()
        {

        }

        public virtual void OnSelectSucceded()
        {

        }
    }
    public class Menu<T> : MenuItem<T>
    {
        public static Menu<T> CreateRoot(string title = "ROOT", string prompt = "")
        {
            return new Menu<T>(title, prompt);
        }
        public static Menu<T> CreateRoot(IEnumerable<IMenuItem<T>> items, string title = "ROOT", string prompt = "")
        {
            return new Menu<T>(items, title, prompt);
        }
        public static Menu<T> CreateSubmenu(int idx, string key, string text, IEnumerable<IMenuItem<T>> items, string title = "SUBMENU", string prompt = ">")
        {
            return new Menu<T>(idx, key, text, items, title, prompt);
        }
        public static Menu<T> CreateSubmenu(int idx, string key, string text, string title = "SUBMENU", string prompt = ">")
        {
            return new Menu<T>(idx, key, text, title, prompt);
        }
        private string _menuTitle;
        public string MenuTitle
        {
            get
            {
                return _menuTitle;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("MenuTitle");
                _menuTitle = value;
            }
        }
        private string _menuPrompt;
        public string MenuPrompt
        {
            get
            {
                return _menuPrompt;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("MenuPrompt");
                _menuPrompt = value;
            }
        }
        protected Menu(IEnumerable<IMenuItem<T>> items, string title = "", string prompt = ">") : this(Int32.MinValue, "__root", "__root", items, title, prompt) { }
        protected Menu(string title = "", string prompt = ">")
            : this(Int32.MinValue, "__root", "__root", title, prompt)
        {

        }
        protected Menu(int idx, string key, string text, string title = "", string prompt = ">")
            : base(idx, key, text, default(T))
        {
            MenuTitle = title;
            MenuPrompt = prompt;
        }
        protected Menu(int idx, string key, string text, IEnumerable<IMenuItem<T>> items, string title = "", string prompt = ">")
            : base(idx, key, text, default(T), items)
        {
            MenuTitle = title;
            MenuPrompt = prompt;
        }
        public override void OnSelectFailed()
        {
            Console.WriteLine("Bad Selection");
        }
        public override void OnSelectStarted()
        {
            Console.WriteLine(MenuTitle);
            foreach (var item in this.OrderBy(i => i.ItemIdx))
            {
                Console.WriteLine("{0}. {1}", item.ItemKey, item.ItemText);
            }
            Console.Write(MenuPrompt);
        }
    }
}
