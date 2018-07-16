using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 


namespace PalindromeLibrary
{
   

    public class Palindrome
    {
        public  bool IsPalindrome(string word)
        {
            word = word.Replace(" ", "");
            word = word.ToLower();
            char[] arr = word.ToCharArray();
            Array.Reverse(arr);
            string temp = new string(arr);
            return word.Equals(temp);
        }

    }
}
