using System;
using System.Linq;
using Sprache;

namespace string_calculator
{
    internal class ParserCombinatorCalculator : ICalculator
    {
        public int Add(string numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers == string.Empty) return 0;
            
            var newline = Parse.String("\n");
            var multiCharDelimiter =
                from delimiters in
                    (from delimiter in Parse.Contained(Parse.CharExcept(']').AtLeastOnce(), Parse.Char('['), Parse.Char(']'))                          
                    select string.Concat(delimiter)).AtLeastOnce()
                select delimiters.Select(Parse.String).Aggregate(Parse.Or);
            var customDelimiters = 
                from _ in Parse.String("//")
                from mc in multiCharDelimiter.Or(                                        
                    from c in Parse.AnyChar
                    select Parse.String(c.ToString()))
                from __ in newline
                select mc;
            
            var numberParser = 
                (from minus in Parse.Char('-')
                from num in Parse.Number
                select minus + num).Or(Parse.Number);
            var optionalDelimiterLineAndDelimitedNumbers =
                from cd in customDelimiters.Optional()                
                from nums in numberParser.DelimitedBy(cd.GetOrElse(Parse.String(",")).Or(newline))
                select nums.Select(int.Parse);

            var parsedNumbers = optionalDelimiterLineAndDelimitedNumbers.Parse(numbers);

            var negatives = parsedNumbers.Where(n => n < 0);

            if (negatives.Any())
                throw new ArgumentException("negatives not allowed: " + string.Join(", ", negatives));

            return parsedNumbers.Where(x => x <= 1000).Sum();
        }
    }
}
