using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using P4Cmdlets.Core.Cmdlets;

namespace P4Cmdlets.Core
{
    [RunInstaller(true)]
    public class P4CmdletsSnapIn : CustomPSSnapIn
    {
        private Collection<CmdletConfigurationEntry> _cmdlets;

        /// <summary>
        /// Gets description of powershell snap-in.
        /// </summary>
        public override string Description
        {
            get { return "A Description of MyCmdlet"; }
        }

        /// <summary>
        /// Gets name of power shell snap-in
        /// </summary>
        public override string Name
        {
            get { return "P4-Cmdlets"; }
        }

        /// <summary>
        /// Gets name of the vendor
        /// </summary>
        public override string Vendor
        {
            get { return "Derek Yang (yanghada@gmail.com)"; }
        }

        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get
            {
                if (null == _cmdlets)
                {
                    _cmdlets = new Collection<CmdletConfigurationEntry>();
                    _cmdlets.Add(new CmdletConfigurationEntry
                      ("Connect-Repository", typeof(ConnectRepository), "P4Cmdlets.Core.dll-Help.xml"));
                }
                return _cmdlets;
            }
        }

    }
}
