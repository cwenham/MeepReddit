using System;

using NCalc;

namespace MeepReddit
{
    /// <summary>
    /// Data type conversions
    /// </summary>
    public static class Converters
    {
        public const string Alphabet_hex_uppercase = "0123456789ABCDEF";

        public const string Alphabet_hex_lowercase = "0123456789abcdef";

        /// <summary>
        /// Lowercase Base36 alphabet
        /// </summary>
        public const string Alphabet_base36_lowercase = "0123456789abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Uppercase Crockford (base32) alphabet
        /// </summary>
        /// <remarks>Often used for randomly generated passwords and coupon codes because it eliminates
        /// I,L,O, and U to avoid confusing with 0 and 1 and accidental naughty words.</remarks>
        public const string Alphabet_crockford_uppercase = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        /// <summary>
        /// Convert a value in an arbitrary base to a long
        /// </summary>
        /// <param name="value">String with the base n value, eg: "FF00FF"</param>
        /// <param name="alphabet">All digits in the base, E.G.: hex would be "0123456789ABCDEF"</param>
        /// <returns>value converted to a long</returns>
        /// <remarks>Deduces the base from the alphabet, so if you pass a 17-char alphabet it will assume base 17.
        ///
        /// <para>If you're converting from a classic base like Hex or Base64 it's probably better to use dotNet's
        /// native converters, since they'll be faster.</para></remarks>
        public static long BaseNToLong(this ReadOnlySpan<char> value, string alphabet)
        {
            long result = 0;

            int pos = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                int idx = alphabet.IndexOf(value[i]);
                if (idx >= 0)
                    result += idx * (long)Math.Pow(alphabet.Length, pos);
                else
                    throw new InvalidOperationException(String.Format("Invalid character {0}", value[pos]));
                pos++;
            }

            return result;
        }

        public static object BaseNToLong(FunctionArgs args)
        {
            string input = Convert.ToString(args.Parameters[0].Evaluate());
            string alphabet = Convert.ToString(args.Parameters[1].Evaluate());

            if (string.IsNullOrWhiteSpace(alphabet))
                return 0;

            return input.AsSpan().BaseNToLong(alphabet);
        }
    }
}
