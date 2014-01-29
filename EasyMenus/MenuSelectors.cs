using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMenus
{
   public class MenuSelectorConsole<T>:IMenuSelector<T>
    {
        private static MenuSelectorConsole<T> _inst = new MenuSelectorConsole<T>();
        public static MenuSelectorConsole<T> Default
        {
            get
            {
                return _inst;
            }
        }
        private MenuSelectorConsole()
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
   public class MenuSelectorRandom<T> : IMenuSelector<T>
   {
       private static MenuSelectorRandom<T> _inst = new MenuSelectorRandom<T>();
       public static MenuSelectorRandom<T> Default
       {
           get
           {
               return _inst;
           }
       }
       Stack<IMenuItem<T>> _itemStack = new Stack<IMenuItem<T>>();

       private Random _idxRandom = new Random();
       public T SelectFrom(IMenuItem<T> target)
       {
           _itemStack.Push(target);
           do
           {
               IMenuItem<T> curTarget = _itemStack.Peek();
               //curTarget.OnSelectStarted();
               IMenuItem<T> item = target.ElementAt(_idxRandom.Next(0, target.Count));

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
       private MenuSelectorRandom()
       {

       }
   }
}
