using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMenus;
namespace EasyMenuTestProgram
{
    class Program
    {
        
            static void Main(string[] args)
            {
                int[] ints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                int[] biggerInts = new int[] { 10, 20, 30, 40, 50 };
                int[] biggestInts = new int[] { 100, 200, 300, 400, 500 };

                Menu<int> rootMenu = ints.ToRootMenu(i => i.ToString(), "Integers");
                Menu<int> biggerMenu = biggerInts.ToSubMenu(10, "10", "Bigger Ints", i2 => i2.ToString(), "Bigger Integers");
                Menu<int> biggestMenu = biggestInts.ToSubMenu(6, "6", "Biggest Ints", i3 => i3.ToString(), "Biggest Integers");

                biggestMenu.Add(new MenuItem<int>(6, "b", "back", 0, true));
                biggerMenu.Add(biggestMenu);
                biggerMenu.Add(new MenuItem<int>(7, "b", "back", 0, true));
                rootMenu.Add(biggerMenu);
                int x = rootMenu.Select(MenuSelectorConsole<int>.Default);
                Console.WriteLine("You chose {0}", x);
                for (int c = 0; c < 100; c++)
                {
                    x = rootMenu.Select(MenuSelectorRandom<int>.Default);
                    Console.WriteLine("The computer chose {0}", x);
                }
                Console.ReadKey();
            }
        }
    
}
