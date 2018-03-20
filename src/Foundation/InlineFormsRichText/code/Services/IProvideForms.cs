using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;

namespace SmartSitecore.Foundation.InlineFormsRichText.Services
{
    public interface IProvideForms
    {
        /// <summary>
        /// Get all items representing forms placed in parent item path from settings "InlineForms.FormsParentPath"
        /// </summary>
        /// <returns></returns>
        IEnumerable<Item> GetFormsItems();

        /// <summary>
        /// Get helper container tags to improve usage in RTE
        /// </summary>
        /// <returns>Opening and closing tag defined in settings "InlineForms.FormContainerTag" or return two empty strings if setting is empty</returns>
        Tuple<string,string> FormContainerTags();

        /// <summary>
        /// Add helper container tags around [form id=""] tag to improve usage in RTE
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string AddContainerTags(string text);

        /// <summary>
        /// Add helper container tags around [form id=""] tag to improve usage in RTE
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string RemoveContainerTags(string text);

        /// <summary>
        /// List all form used in given item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        List<ItemLink> ReferencedForms(Item item);

    }
}
