using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using P4Cmdlets.Core;

namespace P4Cmdlets.Test
{
    [TestFixture]
    public class DescriptionTest
    {
        [Test]
        public void ShouldParseZeroAffectedFiles()
        {
            string[] output = new[]
                                  {
                                      "Change 1 by Administrator@P4CmdletsClient on 2010/05/14 17:00:44 *pending*\n\n\tDescription\n\n",
                                      "Affected files ...\n\n",
                                      "\n"
                                  };
            var desc = new Description(output);
            Assert.AreEqual(0, desc.NumberOfFiles);
        }

        [Test]
        public void ShouldParseManyAffectedFiles()
        {
            string[] output = new[]
                                  {
                                      "Change 1 by Administrator@P4CmdletsClient on 2010/05/14 17:00:44 *pending*\n\n\tDescription\n\n",
                                      "\tMessage\n\n",
                                      "Affected files ...\n\n",
                                      "... //depot/file1.txt#1 add\n",
                                      "... //depot/file2.txt#1 add\n",
                                      "\n"
                                  };
            var desc = new Description(output);
            Assert.AreEqual(2, desc.NumberOfFiles);
        }
    }
}
