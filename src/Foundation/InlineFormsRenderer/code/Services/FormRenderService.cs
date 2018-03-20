using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using Sitecore.Pipelines;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartSitecore.Foundation.InlineFormsRenderer.Services
{
    public class FormRenderService : IRenderForms
    {
        public string ReplaceTagsWithForms(string text, Database contextDatabase)
        {
            var formTags = Regex.Matches(text, Constants.FormRegEx, RegexOptions.IgnoreCase);
            foreach (var formTag in formTags.Cast<Match>().Reverse())
            {
                var idMatch = Regex.Match(formTag.Value, Constants.GuidRegEx, RegexOptions.IgnoreCase);

                ID formId;
                if (!ID.TryParse(idMatch, out formId))
                {
                    text = ReplaceMatch(text, formTag.Index, formTag.Length, string.Empty);
                    continue;
                }
                var html = RenderControllerRendering(contextDatabase, formId);
                text = ReplaceMatch(text, formTag.Index, formTag.Length, html);
            }
            return text;
        }

        protected string ReplaceMatch(string text, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(text.Substring(0, index));
            builder.Append(replacement);
            builder.Append(text.Substring(index + length));
            return builder.ToString();
        }
        
        protected string RenderControllerRendering(Database contextDatabase, ID dataSourceItemId)
        {
            var ri = new RenderingItem(contextDatabase.GetItem(ID.Parse(Constants.FormMvcRenderingId)));
            var rendering = new Rendering { RenderingType = "Controller", UniqueId = Guid.NewGuid(), DataSource = dataSourceItemId.ToString(), Placeholder = "forms", RenderingItem = ri };
            var stringBuilder = new StringBuilder();
            var renderingArgs = new RenderRenderingArgs(rendering, new StringWriter(stringBuilder));
            CorePipeline.Run("mvc.renderRendering", renderingArgs);
            return stringBuilder.ToString();
        }
    }
}