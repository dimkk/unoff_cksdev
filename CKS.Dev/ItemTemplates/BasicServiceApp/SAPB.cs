using System;
using Microsoft.SharePoint.Administration;
namespace $rootnamespace$
{
    [Serializable]
    public sealed class $subnamespace$ServiceApplicationPipeBind
    {
        Guid _id;

        public $subnamespace$ServiceApplicationPipeBind(SPServiceApplication pipebindObject)
        {
            if ((null != pipebindObject) && (pipebindObject is $subnamespace$ServiceApplication))
            {
                _id = pipebindObject.Id;
            }
        }

        public $subnamespace$ServiceApplicationPipeBind(Guid id)
        {
            _id = id;
        }

        public $subnamespace$ServiceApplicationPipeBind(string id)
        {
            _id = new Guid(id);
        }

        internal $subnamespace$ServiceApplication Read()
        {
            $subnamespace$ServiceApplication application = null;
            SPFarm local = SPFarm.Local;
            if (null != local)
            {
                application = local.GetObject(_id) as $subnamespace$ServiceApplication;
            }
            return application;
        }
    }
}
