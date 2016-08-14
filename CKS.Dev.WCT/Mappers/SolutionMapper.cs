using System.Collections.Generic;
using System.IO;
using CKS.Dev.WCT.Common;
using CKS.Dev.WCT.Resources;
using CKS.Dev.WCT.SolutionModel;

namespace CKS.Dev.WCT.Mappers
{

    public class SolutionMapper
    {


        public WCTContext WCTContext { get; set; }


        public SolutionMapper(WCTContext context)
        {
            this.WCTContext = context;
        }

        public void Map()
        {

            if (this.WCTContext.SourceSharePointRootExist)
            {
                FeatureMapper featureMapper = new FeatureMapper(this.WCTContext);
                featureMapper.Map();

                SiteDefinitionsMapper siteDefinitionsMapper = new SiteDefinitionsMapper(this.WCTContext);
                siteDefinitionsMapper.Map();

                HiveFileMapper hiveMapper = new HiveFileMapper(this.WCTContext);
                hiveMapper.Map();

                Map80(this.WCTContext.Solution);
            }
            Logger.LogInformation(StringResources.String_LogMessages_ImportCompleteSuccess);
        }


        private void Map80(SolutionDefinition solution)
        {
            VSItemMapper itemMapper = new VSItemMapper(this.WCTContext);

            if (solution.ApplicationResourceFiles != null)
            {
                MapApplicationResources(this.WCTContext.Solution.ApplicationResourceFiles, itemMapper);
            }

            if (solution.VSWPCatalogItem != null)
            {
                itemMapper.CreateItem(solution.VSWPCatalogItem);
            }
        }



        private void MapApplicationResources(ApplicationResourceFileDefinitions defs, VSItemMapper itemMapper)
        {
            if (defs.VSGlobalItem != null)
            {
                itemMapper.CreateItem(defs.VSGlobalItem);
            }

            if (defs.VSApplicationItem != null)
            {
                itemMapper.CreateItem(defs.VSApplicationItem);
            }
        }




    }
}