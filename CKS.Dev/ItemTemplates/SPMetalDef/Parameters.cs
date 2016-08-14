using System;
using Microsoft.SharePoint;

namespace $rootnamespace$.$subnamespace$
{
    public partial class $subnamespace$Context
    {
        public $subnamespace$Context()
            : this(SPContext.Current.Web.Url)
    	{
    	}

        partial void OnCreated()
        {
        }
    }
}
