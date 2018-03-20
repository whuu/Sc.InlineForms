using Sitecore.Diagnostics;
using Sitecore.Shell.Controls.RichTextEditor.Pipelines.LoadRichTextContent;
using SmartSitecore.Foundation.InlineFormsRichText.Services;

namespace SmartSitecore.Foundation.InlineFormsRichText.Pipelines.Processors.LoadRichTextContent
{
    public class AddFormsHelperProcessor
    {
        protected readonly IProvideForms _provider;

        public AddFormsHelperProcessor(IProvideForms provider)
        {
            _provider = provider;
        }

        public void Process(LoadRichTextContentArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            args.Content = _provider.AddContainerTags(args.Content);
        }
    }
}