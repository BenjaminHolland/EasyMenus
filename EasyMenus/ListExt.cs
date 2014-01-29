using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMenus
{
    public static class ListExt
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
    
    
}
