using Microsoft.VisualStudio.TestTools.UnitTesting;
using PalindromeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PalindromeLibrary.Tests
{
    [TestClass()]
    public class PalindromeTests
    {
        [TestMethod()]
        public void IsPalindromeTest()
        {
            // arrange
            Palindrome p = new Palindrome();
            bool expectedValue = true;
            bool actualValue=false;
            //act
            actualValue=p.IsPalindrome("A nut for a jar of tuna");
            //assert
            Assert.AreEqual(expectedValue, actualValue);

        }

        [TestMethod()]
        public void IsPalindromeTest2()
        {
            // arrange
            Palindrome p = new Palindrome();
            bool expectedValue = true;
            bool actualValue = false;

            //act
            actualValue = p.IsPalindrome("Borrow or rob");
            //assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod()]
        public void IsPalindromeTest3()
        {
            // arrange
            Palindrome p = new Palindrome();
            bool expectedValue = true;
            bool actualValue = false;
            //act
            actualValue = p.IsPalindrome("343");
            //assert
            Assert.AreEqual(expectedValue, actualValue);
        }

    }
}