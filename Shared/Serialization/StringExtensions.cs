using System.Collections.Generic;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitToJsons(this string source)
        {
            var openBrackets = new Queue<char>();
            var prevJsonPosition = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '{')
                {
                    openBrackets.Enqueue(source[i]);
                }
                else if (source[i] == '}')
                {
                    if (openBrackets.Count == 1)
                    {
                        if (i == source.Length)
                        {
                            yield return source.Substring(prevJsonPosition, i - prevJsonPosition);
                        }
                        else
                        {
                            yield return source.Substring(prevJsonPosition, i - prevJsonPosition + 1);
                            prevJsonPosition = i + 1;
                            openBrackets.Dequeue();
                        }
                    }
                    else if (openBrackets.Count > 1)
                    {
                        _ = openBrackets.Dequeue();
                    }
                }
            }
        }
    }
}
