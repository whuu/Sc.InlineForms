using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using SmartSitecore.Foundation.InlineFormsRenderer.Services;

namespace SmartSitecore.Foundation.InlineFormsRenderer.Pipelines.Processors.RenderField
{
    public class FormsReplacerProcessor
    {
        protected readonly IRenderForms _renderer;

        public FormsReplacerProcessor(IRenderForms renderer)
        {
            _renderer = renderer;
        }

        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (ShouldFieldBeProcessed(args))
            {
                ReplaceFormTags(args);
            }
        }

        private bool ShouldFieldBeProcessed(RenderFieldArgs args)
        {
            return args.FieldTypeKey.ToLower() == Constants.RichTextType && !Sitecore.Context.PageMode.IsExperienceEditorEditing;
        }

        private void ReplaceFormTags(RenderFieldArgs args)
        {
            args.Result.FirstPart = _renderer.ReplaceTagsWithForms(args.Result.FirstPart, args.Item.Database);
        }
    }
}