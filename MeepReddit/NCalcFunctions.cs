using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using NCalc;

namespace MeepReddit
{
    public static class NCalcFunctions
    {
        public static object URLSpam(FunctionArgs args)
        {
            string input = Convert.ToString(args.Parameters[0].Evaluate());

            if (!(input is null))
                return r_URLSpam.Match(input).Success;

            return false;
        }

        private static Regex r_URLSpam = new Regex(@"(https?:\/\/.*\/)(\n\s\1){2,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    }
}
