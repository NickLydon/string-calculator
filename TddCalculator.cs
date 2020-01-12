using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace string_calculator
{
    internal class TddCalculator : ICalculator
    {
        public int Add(string numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers == string.Empty) return 0;      

            var delimiters = new[] { ",", '\n'.ToString() };

            if (numbers.StartsWith("//"))
            {
                numbers = numbers.Substring(2);
                if (numbers.StartsWith('['))
                {
                    delimiters = CustomDelimiters(numbers).Concat(new[] { '\n'.ToString() }).ToArray();
                    numbers = numbers.Substring(numbers.LastIndexOf(']'));
                }
                else 
                {
                    delimiters = new[] { numbers[0].ToString(), '\n'.ToString() };
                }
                numbers = numbers.Substring(1);
            }

            var parsed = numbers
                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse);

            var negatives = parsed.Where(n => n < 0);

            if (negatives.Any())
                throw new ArgumentException("negatives not allowed: " + string.Join(", ", negatives));

            return parsed.Where(n => n <= 1000).Sum();            
        }

        private static IEnumerable<string> CustomDelimiters(string nums)
        {
            if (nums == string.Empty || nums[0] == '\n') yield break;
            nums = nums.Substring(1);
            var endOfDelimiter = nums.IndexOf(']');
            var multiCharDelimiter = nums.Substring(0, endOfDelimiter);
            yield return multiCharDelimiter;
            nums = nums.Substring(endOfDelimiter + 1);
            foreach (var custom in CustomDelimiters(nums))
                yield return custom;
        }
    }
}
