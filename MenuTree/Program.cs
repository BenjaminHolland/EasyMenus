using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuTree
{
    public static class MenuExt
    {
        public static Menu<T> ToRootMenu<T>(this IList<IMenuItem<T>> items, string title = "", string prompt = ">")
        {
            return Menu<T>.CreateRoot(items, title, prompt);
        }
        public static Menu<T> ToRootMenu<T>(this IList<T> values, IList<int> indicies, IList<string> keys, IList<string> texts, string title = "", string prompt = ">")
        {
            if (indicies == null)
                throw new ArgumentNullException("indicies");
            if (keys == null)
                throw new ArgumentNullException("keys");
            if (texts == null)
                throw new ArgumentNullException("texts");
            if (values.Count != indicies.Count)
                throw new InvalidOperationException("Count Mismatch");
            if (values.Count != keys.Count)
                throw new InvalidOperationException("Count Mismatch");
            if (values.Count != texts.Count)
                throw new InvalidOperationException("Count Mismatch");

            var ret = Menu<T>.CreateRoot(title, prompt);
            for (int i = 0; i < values.Count; i++)
            {
                ret.Add(new MenuItem<T>(indicies[i], keys[i], texts[i], values[i]));
            }
            return ret;
        }
        public static Menu<T> ToRootMenu<T>(this IList<T> values, IList<string> texts, string title = "", string prompt = ">")
        {
            int[] autoIdx = new int[values.Count];
            string[] autoKey = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                autoIdx[i] = i;
                autoKey[i] = (i + 1).ToString();
            }
            return values.ToRootMenu(autoIdx, autoKey, texts, title, prompt);
        }
        public static Menu<T> ToRootMenu<T>(this IList<T> values, Func<T, int> toIdx, Func<T, string> toKey, Func<T, string> toText, string title = "", string prompt = ">")
        {
            var conIdxs = values.Select(toIdx).ToList();
            var conKeys = values.Select(toKey).ToList();
            var conTexts = values.Select(toText).ToList();
            return values.ToRootMenu(conIdxs, conKeys, conTexts, title, prompt);
        }
        public static Menu<T> ToRootMenu<T>(this IList<T> values, Func<T, string> toText, string title = "", string prompt = ">")
        {
            int[] autoIdx = new int[values.Count];
            string[] autoKey = new string[values.Count];
            string[] conTexts = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                autoIdx[i] = i;
                autoKey[i] = (i + 1).ToString();
                conTexts[i] = toText(values[i]);
            }
            return values.ToRootMenu(autoIdx, autoKey, conTexts, title, prompt);
        }
        public static Menu<T> ToSubMenu<T>(this IList<IMenuItem<T>> items, int idx, string key, string text, string title = "", string prompt = ">")
        {
            return Menu<T>.CreateSubmenu(idx, key, text, items, title, prompt);
        }
        public static Menu<T> ToSubMenu<T>(this IList<T> values, int idx, string key, string text, IList<int> indicies, IList<string> keys, IList<string> texts, string title = "", string prompt = ">")
        {
            if (indicies == null)
                throw new ArgumentNullException("indicies");
            if (keys == null)
                throw new ArgumentNullException("keys");
            if (texts == null)
                throw new ArgumentNullException("texts");
            if (values.Count != indicies.Count)
                throw new InvalidOperationException("Count Mismatch");
            if (values.Count != keys.Count)
                throw new InvalidOperationException("Count Mismatch");
            if (values.Count != texts.Count)
                throw new InvalidOperationException("Count Mismatch");

            var ret = Menu<T>.CreateSubmenu(idx, key, text, title, prompt);
            for (int i = 0; i < values.Count; i++)
            {
                ret.Add(new MenuItem<T>(indicies[i], keys[i], texts[i], values[i]));
            }
            return ret;
        }
        public static Menu<T> ToSubMenu<T>(this IList<T> values, int idx, string key, string text, IList<string> texts, string title = "", string prompt = ">")
        {
            int[] autoIdx = new int[values.Count];
            string[] autoKey = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                autoIdx[i] = i;
                autoKey[i] = (i + 1).ToString();
            }
            return values.ToSubMenu(idx, key, text, autoIdx, autoKey, texts, title, prompt);
        }
        public static Menu<T> ToSubMenu<T>(this IList<T> values, int idx, string key, string text, Func<T, int> toIdx, Func<T, string> toKey, Func<T, string> toText, string title = "", string prompt = ">")
        {
            var conIdxs = values.Select(toIdx).ToList();
            var conKeys = values.Select(toKey).ToList();
            var conTexts = values.Select(toText).ToList();
            return values.ToSubMenu(idx, key, text, conIdxs, conKeys, conTexts, title, prompt);
        }
        public static Menu<T> ToSubMenu<T>(this IList<T> values, int idx, string key, string text, Func<T, string> toText, string title = "", string prompt = ">")
        {
            int[] autoIdx = new int[values.Count];
            string[] autoKey = new string[values.Count];
            string[] conTexts = new string[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                autoIdx[i] = i;
                autoKey[i] = (i + 1).ToString();
                conTexts[i] = toText(values[i]);
            }
            return values.ToSubMenu(idx, key, text, autoIdx, autoKey, conTexts, title, prompt);
        }
    }
    
    public interface IMenuSelector<T>
    {
        T SelectFrom(IMenuItem<T> target);
    }
    public interface IMenu<T> : IMenuItem<T>
    {
        string MenuTitle
        {
            get;
        }
        string MenuPrompt
        {
            get;
        }
    }
    public interface IMenuItem<T> : ICollection<IMenuItem<T>>
    {
        bool IsBack
        {
            get;
        }
        bool IsLeaf
        {
            get;
        }
        T ItemValue
        {
            get;
        }
        int ItemIdx
        {
            get;
        }
        string ItemKey
        {
            get;
        }
        string ItemText
        {
            get;
        }
        IMenuItem<T> GetItemByIndex(int index);
        IMenuItem<T> GetItemByKey(string key);
        bool ContainsItemKey(string key);
        bool ContainsItemIndex(int idx);
        T Select(IMenuSelector<T> selector);
        void OnSelectStarted();
        void OnSelectFailed();
        void OnSelectSucceded();
    }
    
    public class MenuSelectorStd<T>:IMenuSelector<T>
    {
        private static MenuSelectorStd<T> _inst = new MenuSelectorStd<T>();
        public static MenuSelectorStd<T> Default
        {
            get
            {
                return _inst;
            }
        }
        private MenuSelectorStd()
        {

        }
        Stack<IMenuItem<T>> _itemStack = new Stack<IMenuItem<T>>();
        
        public T SelectFrom(IMenuItem<T> target)
        {
            _itemStack.Push(target);
            do
            {
                IMenuItem<T> curTarget = _itemStack.Peek();
                curTarget.OnSelectStarted();
                string key = Console.ReadLine().Trim();
                IMenuItem<T> item = curTarget.GetItemByKey(key);
                if (item != null)
                {
                    target.OnSelectSucceded();
                    if (item.IsBack)
                    {
                        _itemStack.Pop();
                      
                    }
                    else if (item.IsLeaf)
                    {
                        return item.ItemValue;
                    }
                    else
                    {
                        return SelectFrom(item);
                    }
                }
                else
                {
                    target.OnSelectFailed();
                }
            } while (_itemStack.Count>0);
            throw new InvalidOperationException("Selection Failed");
        }
    }
    public class MenuSelectorRnd<T> : IMenuSelector<T>
    {
        private static MenuSelectorRnd<T> _inst = new MenuSelectorRnd<T>();
        public static MenuSelectorRnd<T> Default
        {
            get
            {
                return _inst;
            }
        }
        Stack<IMenuItem<T>> _itemStack = new Stack<IMenuItem<T>>();
        
        private Random _idxRandom=new Random();
        public T SelectFrom(IMenuItem<T> target)
        {
            _itemStack.Push(target);
            do
            {
                IMenuItem<T> curTarget = _itemStack.Peek();
                //curTarget.OnSelectStarted();
                IMenuItem<T> item=target.ElementAt(_idxRandom.Next(0,target.Count));
                
                    //target.OnSelectSucceded();
                    if (item.IsBack)
                    {
                        _itemStack.Pop();

                    }
                    else if (item.IsLeaf)
                    {
                        return item.ItemValue;
                    }
                    else
                    {
                        return SelectFrom(item);
                    }
                
                
            } while (_itemStack.Count > 0);
            throw new InvalidOperationException("Selection Failed");
        }
        private MenuSelectorRnd()
        {
             
        }
    }
    public class MenuItem<T> : IMenuItem<T>
    {
        private List<IMenuItem<T>> _children;
        private string _itemText;
        private string _itemKey;
        
        protected MenuItem(int idx,string key,string text,T value,IEnumerable<IMenuItem<T>> children):this(idx,key,text,value)
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

        public MenuItem(int idx,string key,string text,T value,bool isBack=false)
        {
            _children=new List<IMenuItem<T>>();
            ItemIdx=idx;
            ItemKey=key;
            ItemText=text;
            ItemValue=value;
            IsBack = isBack;
        }
        
        public bool ContainsItemKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return _children.Exists(i=>i.ItemKey==key);
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
        protected Menu(IEnumerable<IMenuItem<T>> items,string title="",string prompt=">"):this(Int32.MinValue,"__root","__root",items,title,prompt){}
        protected Menu(string title="", string prompt=">"):this(Int32.MinValue,"__root","__root",title,prompt)
        {
            
        }
        protected Menu(int idx, string key, string text, string title = "", string prompt = ">")
            : base(idx, key, text, default(T))
        {
            MenuTitle = title;
            MenuPrompt = prompt;
        }
        protected Menu(int idx, string key, string text,IEnumerable<IMenuItem<T>> items, string title = "", string prompt = ">"):base(idx,key,text,default(T),items)
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
    class Program
    {
        static void Main(string[] args)
        {
            int[] ints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] biggerInts = new int[] { 10, 20, 30, 40, 50 };
            int[] biggestInts = new int[] { 100, 200, 300, 400, 500 };

            Menu<int> rootMenu = ints.ToRootMenu(i => i.ToString(), "Integers");
            Menu<int> biggerMenu=biggerInts.ToSubMenu(10, "10", "Bigger Ints", i2 => i2.ToString(), "Bigger Integers");
            Menu<int> biggestMenu = biggestInts.ToSubMenu(6, "6", "Biggest Ints", i3 => i3.ToString(), "Biggest Integers");
            
            biggestMenu.Add(new MenuItem<int>(6, "b", "back", 0, true));
            biggerMenu.Add(biggestMenu);
            biggerMenu.Add(new MenuItem<int>(7, "b", "back", 0, true));
            
            rootMenu.Add(biggerMenu);
            int x=rootMenu.Select(MenuSelectorStd<int>.Default);
            Console.WriteLine("You chose {0}", x);
            for (int c = 0; c < 100; c++)
            {
                x = rootMenu.Select(MenuSelectorRnd<int>.Default);
                Console.WriteLine("The computer chose {0}", x);
            }
            Console.ReadKey();
        }
    }
}
