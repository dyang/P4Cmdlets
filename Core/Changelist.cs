using P4API;

namespace P4Cmdlets.Core
{
    public class Changelist
    {
        private readonly P4PendingChangelist _changelist;

        public Changelist(P4PendingChangelist changelist)
        {
            _changelist = changelist;
        }


        public string Description
        {
            get { return _changelist.Description; }
        }

        public long Id
        {
            get { return _changelist.Number; }
        }
    }
}