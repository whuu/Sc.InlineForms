using Sitecore.Data;

namespace SmartSitecore.Foundation.InlineFormsRenderer.Services
{
    public interface IRenderForms
    {
        /// <summary>
        /// Render forms for [form id=""] tags inside given text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="contextDatabase"></param>
        /// <returns></returns>
        string ReplaceTagsWithForms(string text, Database contextDatabase);
    }
}
