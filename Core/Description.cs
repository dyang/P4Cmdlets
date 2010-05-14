using System;
using System.Collections.Generic;

namespace P4Cmdlets.Core
{
    public class Description
    {
        private List<string> _files = new List<string>();

        public Description(string output)
        {
            Parse(output);
        }

        private void Parse(string output)
        {
            var lines = output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            
            bool inAffectedBlock = false;
            for (int i = 0; i < lines.Length; i ++ )
            {
                if (inAffectedBlock)
                {
                    _files.Add(lines[i]);
                }
                if (lines[i].Equals("Affected files ..."))
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
