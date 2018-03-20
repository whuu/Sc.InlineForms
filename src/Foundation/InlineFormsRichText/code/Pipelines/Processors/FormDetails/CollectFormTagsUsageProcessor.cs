using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Client.Models.Builder;
using Sitecore.ExperienceForms.Client.Pipelines.FormDetails;
using Sitecore.Mvc.Pipelines;
using System;
using System.Linq;

namespace SmartSitecore.Foundation.InlineFormsRichText.Pipelines.Processors.FormDetails
{
    public class CollectFormTagsUsageProcessor : MvcPipelineProcessor<FormDetailsEventArgs>
    {
        public override void Process(FormDetailsEventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (args.Item == null)
            {
                args.AbortPipeline();
            }
            else
            {
                var referrers = Globals.LinkDatabase.GetReferrers(args.Item);
                foreach (var linkItem in referrers)
                {
                    if (!linkItem.SourceDatabaseName.Equals(args.Item.Database.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    if (linkItem.SourceFieldID == FieldIDs.FinalLayoutField || linkItem.SourceFieldID == FieldIDs.LayoutField)
                    {
                        continue;
                    }
                    if (args.FormLinks.Any(l => l.Id == linkItem.SourceItemID))
                    {
                        continue;
                    }
                    var item = args.Item.Database.GetItem(linkItem.SourceItemID, args.Item.Language);
                    if (item == null || item.Fields[linkItem.SourceFieldID].TypeKey != Constants.RichTextType)
                    {
                        continue;
                    }
                    
                    args.FormLinks.Add(new FormReferrer(linkItem.SourceItemID, new ID(args.FormId))
                    {
                        Name = item.DisplayName,
                        Path = item.Paths.ContentPath
                    });
                } 
            }
        }
    }
}