using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMenus
{
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
    
}
