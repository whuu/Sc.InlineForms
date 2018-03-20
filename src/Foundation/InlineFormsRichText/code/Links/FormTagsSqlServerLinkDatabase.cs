using Sitecore.Data.Items;
using Sitecore.Data.SqlServer;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using SmartSitecore.Foundation.InlineFormsRichText.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSitecore.Foundation.InlineFormsRichText.Links
{
    public class FormTagsSqlServerLinkDatabase : SqlServerLinkDatabase
    {
        private readonly IProvideForms _provider;

        public FormTagsSqlServerLinkDatabase(string connectionString) : base(connectionString)
        {
            _provider = (IProvideForms)ServiceLocator.ServiceProvider.GetService(typeof(IProvideForms));
        }

        /// <summary>
        /// After save item update item version references.
        /// </summary>
        /// <param name="item"></param>
        public override void UpdateItemVersionReferences(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var links = item.Links.GetAllLinks(false)?.ToList();
            var formLinks = _provider.ReferencedForms(item);
            if (formLinks?.Count > 0)
            {
                links?.AddRange(formLinks);
            }
            Task.Factory.StartNew(() => UpdateItemVersionLink(item, links?.ToArray()));
        }

        /// <summary>
        /// If rebuild, update the references.
        /// </summary>
        /// <param name="item"></param>
        public override void UpdateReferences(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var allLinks = item.Links.GetAllLinks()?.ToList();
            var formLinks = _provider.ReferencedForms(item);
            if (formLinks?.Count > 0)
            {
                allLinks?.AddRange(formLinks);
            }
            UpdateLinks(item, allLinks?.ToArray());
        }
    }
}