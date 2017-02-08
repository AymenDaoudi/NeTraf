using System.Collections.Generic;
using System.IO;

namespace NeTraf
{
    public static class TupleExtensions
    {
        public static IEnumerable<string> ReadAllLines(this StreamReader streamReader)
        {
            string line;
            while ((line = streamReader.ReadLine()) !=  null)
            {
                yield return line;
            }
        }
    }
}