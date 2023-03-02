using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Taf.Decoder.chunkdecoder
{
    public abstract class TafChunkDecoder : ITafChunkDecoder
    {
        /// <summary>
        /// Extract the corresponding chunk from the remaining taf.
        /// </summary>
        /// <param name="remainingTaf">matches array if any match (null if no match), + updated remaining taf</param>
        /// <param name="newRemainingTaf">new Remaining Taf</param>
        /// <returns></returns>
        public List<Group> Consume(string remainingTaf, out string newRemainingTaf)
        {
            var chunkRegex = new Regex(GetRegex(), RegexOptions.None, TimeSpan.FromMilliseconds(500));

            // try to match chunk's regexp on remaining taf
            var groups = chunkRegex.Match(remainingTaf).Groups.Cast<Group>().ToList();

            // consume what has been previously found with the same regexp
            newRemainingTaf = chunkRegex.Replace(remainingTaf, string.Empty);

            return groups;
        }

        /// <summary>
        /// Consume one chunk blindly, without looking for the specific pattern (only whitespace).
        /// </summary>
        /// <param name="remainingTaf"></param>
        /// <returns></returns>
        public static string ConsumeOneChunk(string remainingTaf)
        {
            var nextSpace = remainingTaf.IndexOf(" ");
            if (nextSpace > 0)
            {
                return remainingTaf.Substring(nextSpace + 1);
            }
            else
            {
                return remainingTaf;
            }
        }

        protected Dictionary<string, object> GetResults(string newRemainingTaf, Dictionary<string, object> result)
        {
            //return result + remaining taf
            return new Dictionary<string, object>()
            {
                { TafDecoder.ResultKey, result },
                { TafDecoder.RemainingTafKey, newRemainingTaf }
            };
        }

        public abstract string GetRegex();

        public abstract Dictionary<string, object> Parse(string remainingTaf, bool withCavok = false);
    }
}