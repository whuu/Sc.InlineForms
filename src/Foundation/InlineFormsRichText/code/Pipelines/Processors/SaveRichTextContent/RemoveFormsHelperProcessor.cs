using Sitecore.Diagnostics;
using Sitecore.Shell.Controls.RichTextEditor.Pipelines.SaveRichTextContent;
using SmartSitecore.Foundation.InlineFormsRichText.Services;

namespace SmartSitecore.Foundation.InlineFormsRichText.Pipelines.Processors.SaveRichTextContent
{
    public class RemoveFormsHelperProcessor
    {
        protected readonly IProvideForms _provider;

        public RemoveFormsHelperProcessor(IProvideForms provider)
        {
            _provider = provider;
        }

        public void Process(SaveRichTextContentArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            args.Content = _provider.RemoveContainerTags(args.Content);
        }
    }
}