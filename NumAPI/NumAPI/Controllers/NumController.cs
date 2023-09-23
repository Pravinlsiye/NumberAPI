using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace NumAPI.Controllers
{

    [ApiController]
    [Route("api/number")]
    public class NumController : Controller
    {

        [HttpGet]
        public IActionResult GetNumber()
        {
            var resultNumber = GetRandomNumbers(10);
            var resultNumberInWords = ConvertNumberToWords(resultNumber);

            return new JsonResult(new
            {
                Number = resultNumber,
                NumberInWords = resultNumberInWords,
                Status = true
            });
        }

        [HttpGet("random")]
        public IActionResult GetRandomNumberInAnyDigit()
        {
            var NumDigit = GetRandomNumbers(2);
            var resultNumber = GetRandomNumbers(Int32.Parse(NumDigit));
            var resultNumberInWords = ConvertNumberToWords(resultNumber);

            return new JsonResult(new
            {
                Number = resultNumber,
                NumberInWords = resultNumberInWords,
                Status = true
            });

        }

        [HttpGet("{NumDigit}")]
        public IActionResult GetNumber(int NumDigit)
        {
            if (NumDigit > 100)
            {
                return new JsonResult(new
                {
                    Number = "This API Support Upto 100 Digits Only",
                    Status = false
                });
            }
            else if (NumDigit < 1)
            {
                return new JsonResult(new
                {
                    Number = "How the Digits Input will be In Negative or Zero",
                    Status = false
                });
            }

            var resultNumber = GetRandomNumbers(NumDigit);
            var resultNumberInWords = ConvertNumberToWords(resultNumber);
            return new JsonResult(new
            {
                Number = resultNumber,
                NumberInWords = resultNumberInWords,
                Status = true
            });
        }

        public static string GetRandomNumbers(int numDigits)
        {
            Random random = new Random();

            var randomNumber = "";
            for (int i = 0; i < numDigits; i++)
            {
                var temp = random.NextInt64(10);
                if (i == 0 && temp == 0)
                {
                    temp = random.NextInt64(9) + 1;
                }
                randomNumber += temp;
            }

            return randomNumber.ToString();
        }

        private string ConvertNumberToWords(string numberString)
        {
            BigInteger number = BigInteger.Parse(numberString);
            if (number == 0)
            {
                return "zero";
            }

            if (number < 0)
            {
                return "minus " + ConvertNumberToWords(BigInteger.Abs(number).ToString());
            }

            string[] units = {
            "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
        };

            string[] teens = {
            "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
            "seventeen", "eighteen", "nineteen"
        };

            string[] tens = {
            "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
        };

            string[] thousands = { "", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "quattuordecillion", "quindecillion", "sexdecillion", "septemdecillion", "octodecillion", "novemdecillion", "vigintillion", "unvigintillion", "duovigintillion", "trevigintillion", "quattuorvigintillion", "quinvigintillion", "sexvigintillion", "septvigintillion", "octovigintillion", "nonvigintillion", "trigintillion", "untrigintillion", "duotrigintillion" };

            var words = new List<string>();
            var groupCount = 0;

            while (number > 0)
            {
                var group = number % 1000;
                if (group > 0)
                {
                    var groupWords = new List<string>();
                    int hundreds = (int)(group / 100);
                    int remainder = (int)(group % 100);

                    if (hundreds > 0)
                    {
                        groupWords.Add(units[hundreds]);
                        groupWords.Add("hundred");
                    }

                    if (remainder > 0)
                    {
                        if (remainder < 10)
                        {
                            groupWords.Add(units[remainder]);
                        }
                        else if (remainder < 20)
                        {
                            groupWords.Add(teens[remainder - 10]);
                        }
                        else
                        {
                            groupWords.Add(tens[remainder / 10]);
                            if (remainder % 10 > 0)
                            {
                                groupWords.Add(units[remainder % 10]);
                            }
                        }
                    }

                    groupWords.Add(thousands[groupCount]);

                    words.InsertRange(0, groupWords);
                }

                number /= 1000;
                groupCount++;
            }

            return string.Join(" ", words);
        }
    }
}
