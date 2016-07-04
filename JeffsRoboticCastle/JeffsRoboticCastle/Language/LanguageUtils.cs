using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.Language
{
    class LanguageUtils
    {
        // Joins a bunch of strings with commas using normal English rules
        // For example, {"a","b","c"} returns "a, b, and c"
        public static String Enumerate(List<String> clauses)
        {
            if (clauses.Count == 0)
                return "";
            if (clauses.Count == 1)
                return clauses[0];
            if (clauses.Count == 2)
                return clauses[0] + " and " + clauses[1];
            // more than 2 clauses - join the first n-1 of them with ", "
            String result = clauses[0];
            for (int i = 1; i < clauses.Count - 1; i++)
            {
                result = result + ", " + clauses[i];
            }
            result = result + ", and " + clauses[clauses.Count - 1]; // sure, let's include the Oxford Comma
            return result;
                     

        }

        // Formats a quantity, including maybe adding an 's'
        public static String FormatQuantity(double quantity, String unit)
        {
            String result = quantity.ToString() + " " + unit;
            if (quantity != 1)
                result += "s";
            return result;
        }
    }
}
