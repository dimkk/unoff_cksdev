using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;

namespace CKS.Dev.WCT.Mappers
{
    public class SiteDefinitionsMapper
    {
        
        public WCTContext WCTContext { get; set; }


        public SiteDefinitionsMapper(WCTContext context)
        {
            this.WCTContext = context;
        }

        public void Map()
        {
            VSItemMapper itemMapper = new VSItemMapper(this.WCTContext);

            foreach (var siteDef in this.WCTContext.Solution.SiteDefinitionManifests.AsSafeEnumable())
            {
                try
                {
                    itemMapper.CreateItem(siteDef.VSItem);
                }
                catch (Exception ex)
                {
                    Logger.LogError(String.Format(StringResources.Strings_Errors_MapSiteDefinition, siteDef.Location, ex.ToString()));
                }
            }
        }
    }
}
