using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartSitecore.Foundation.InlineFormsRichText.Services
{
    public class FormsItemService : IProvideForms
    {
        public IEnumerable<Item> GetFormsItems()
        {
            var formsPath = Settings.GetSetting("InlineForms.FormsParentPath", "/sitecore/Forms");
            return Context.ContentDatabase.GetItem(formsPath).Children.Where(i => i.TemplateID == ID.Parse(Constants.FormsTemplateId));
        }

        public Tuple<string,string> FormContainerTags()
        {
            var container = Settings.GetSetting("InlineForms.FormContainerTag");
            if (string.IsNullOrWhiteSpace(container))
            {
                return Tuple.Create(string.Empty, string.Empty);
            }
            return Tuple.Create($"<{container}>", $"</{container}>");
        }

        public string AddContainerTags(string text)
        {
            var formTags = Regex.Matches(text, Constants.FormRegEx, RegexOptions.IgnoreCase);
            if (formTags.Count > 0)
            {
                var tags = FormContainerTags();
                foreach (var formTag in formTags.Cast<Match>().Reverse())
                {
                    var replacement = tags.Item1 + formTag.Value + tags.Item2;
                    text = ReplaceMatch(text, formTag.Index, formTag.Length, replacement);
                }
            }
            return text;
        }

        public string RemoveContainerTags(string text)
        {
            var tags = FormContainerTags();
            var formTags = Regex.Matches(text, tags.Item1 + Constants.FormRegEx + tags.Item2, RegexOptions.IgnoreCase);
            if (formTags.Count == 0)
            {
                return text;
            }
            foreach (var formTag in formTags.Cast<Match>().Reverse())
            {
                var replacement = formTag.Value.Substring(tags.Item1.Length, formTag.Value.Length - tags.Item1.Length - tags.Item2.Length);
                text = ReplaceMatch(text, formTag.Index, formTag.Length, replacement);
            }
            return text;
        }

        public List<ItemLink> ReferencedForms(Item item)
        {
            var links = new List<ItemLink>();
            if (item == null)
            {
                return links;
            }

            foreach (var field in item.Fields.Where(f => f.TypeKey == Constants.RichTextType))
            {
                var usedForms = ReferencedFormIds(field.Value);
                if (usedForms == null || usedForms.Count == 0)
                {
                    continue;
                }
                foreach (var id in usedForms)
                {
                    var targetItem = item.Database.GetItem(id);
                    if (targetItem == null)
                    {
                        continue;
                    }
                    links.Add(new ItemLink(item, field.ID, targetItem, targetItem.ID.ToString()));
                }
            }
            return links;
        }

        protected List<ID> ReferencedFormIds(string text)
        {
            var usedForms = new List<ID>();
            var formTags = Regex.Matches(text, Constants.FormRegEx, RegexOptions.IgnoreCase);
            foreach (var formTag in formTags.Cast<Match>())
            {
                var idMatch = Regex.Match(formTag.Value, Constants.GuidRegEx, RegexOptions.IgnoreCase);

                ID formId;
                if (!ID.TryParse(idMatch, out formId))
                {
                    continue;
                }
                usedForms.Add(formId);
            }
            return usedForms;
        }

        protected string ReplaceMatch(string text, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(text.Substring(0, index));
            builder.Append(replacement);
            builder.Append(text.Substring(index + length));
            return builder.ToString();
        }
    }
}