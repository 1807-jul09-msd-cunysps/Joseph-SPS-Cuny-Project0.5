using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalindromeLibrary;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Palindrome p = new Palindrome();
            bool answer=p.IsPalindrome("racecar");
            Console.WriteLine(answer);
            Console.ReadKey();

        }
    }
}
