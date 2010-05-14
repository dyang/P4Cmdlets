using System;
using System.Collections.Generic;

namespace P4Cmdlets.Core
{
    public class Description
    {
        private List<string> _files = new List<string>();

        public Description(string[] output)
        {
            RemoveSpecialChars(output);
            Parse(output);
        }

        private void RemoveSpecialChars(string[] output)
        {
            for (int i = 0; i < output.Length; i++ )
            {
                output[i] = output[i].Replace("\n", "").Replace("\t", "");
            }
        }

        private void Parse(string[] output)
        {
            bool inAffectedBlock = false;

            for (int i = 0; i < output.Length; i ++ )
            {
                if (inAffectedBlock)
                {
                    if (!string.IsNullOrEmpty(output[i]))
                    {
                        _files.Add(output[i]);
                    }
                }
                if (output[i].StartsWith("Affected files ..."))
                {
                    inAffectedBlock = true;
                }
            }
        }

        public int NumberOfFiles
        {
            get { return _files.Count; }
        }
    }
}
