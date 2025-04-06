using System;
using System.Collections.Generic;

namespace Tasks1_3

{
    class Program
    {
        public bool ValidatePassword(string password) 
        {
            bool result = false;
            bool hasNumber = false;
            bool isLower = false;
            bool isUpper = false;
            bool hasEight = false;

            hasNumber = password.Any(char.IsDigit);

            isLower = password.Any(char.IsLower);

            isUpper = password.Any(char.IsUpper);

            hasEight = password.Length >= 8;

            if (hasEight && hasNumber && isLower && isUpper)
            {
                result = true;
            }

            return result;
        }

        public int ConvertToArabic(string romanNumber)
        {
            Dictionary<char, int> romanDigits = new Dictionary<char, int>()
            {
            {'I', 1 },
            {'V', 5 },
            {'X', 10 },
            {'L', 50 },
            {'C', 100 },
            {'D', 500 },
            {'M', 1000 }
            };

            bool IsSubtractive(char ch1, char ch2)
            {
                return romanDigits[ch1] < romanDigits[ch2];
            }

            int result = 0;
            for (int i = 0; i < romanNumber.Length; i++)
            {
                if (i + 1 < romanNumber.Length && IsSubtractive(romanNumber[i], romanNumber[i + 1]))
                {
                    result -= romanDigits[romanNumber[i]];
                }
                else
                {
                    result += romanDigits[romanNumber[i]];
                }
            }
            return result;
        }

        public string SortString(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Sort(charArray);
            string resultStr = new string(charArray);
            return resultStr;

        }
        static void Main(string[] args)
        {
            
        }
    }
}
