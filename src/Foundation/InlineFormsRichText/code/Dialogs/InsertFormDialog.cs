using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using SmartSitecore.Foundation.InlineFormsRichText.Services;
using System;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartSitecore.Foundation.InlineFormsRichText.Dialogs
{
    public class InsertFormDialog : DialogForm
    {
        protected static string CurrentFormId => WebUtil.GetQueryString("formID", string.Empty);

        protected Scrollbox FormListView;

        protected DataContext FormId;

        protected string Mode
        {
            get => Assert.ResultNotNull(StringUtil.GetString(ServerProperties["Mode"], "shell"));
            set
            {
                Assert.ArgumentNotNull(value, "value");
                ServerProperties["Mode"] = value;
            }
        }

        protected readonly IProvideForms _formsProvider;

        public InsertFormDialog() 
        {
            _formsProvider = (IProvideForms)ServiceLocator.ServiceProvider.GetService(typeof(IProvideForms));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            LoadForms(CurrentFormId);
        }

        /// <summary>
        /// Handles category click
        /// </summary>
        /// <param name="id"></param>
        protected void FormListView_Click(string id)
        {
            FormId.Value = id;
        }

        /// <summary>Handles a click on the OK button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var formId = FormId.Value;
            if (!ID.TryParse(formId, out ID selectedFormId))
            {
                SheerResponse.Alert("You need to select a form.");
                return;
            }
            var form = _formsProvider.GetFormsItems().FirstOrDefault(i => i.ID == selectedFormId);
            var formNode = string.Format(Constants.FormTag, formId);
            var tags = _formsProvider.FormContainerTags();
            SheerResponse.Eval($"scClose('{HttpUtility.HtmlEncode(formNode)}', '{tags.Item1}', '{tags.Item2}')");
        }

        /// <summary>Handles a click on the Cancel button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>When the user clicksCancel, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.</remarks>
        protected override void OnCancel(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            if (Mode == "webedit")
            {
                base.OnCancel(sender, args);
            }
            else
            {
                SheerResponse.Eval("scCancel()");
            }
        }

        private void LoadForms(string selectedId)
        {
            var sb = new StringBuilder();
            if (ID.TryParse(selectedId, out ID selected))
            {
                FormId.Value = selected.ToString();
            }

            foreach (var form in _formsProvider.GetFormsItems())
            {
                sb.Append(RenderItemSelection(form, form.ID == selected));
            }
            FormListView.InnerHtml = sb.ToString();
        }

        private string RenderItemSelection(Item item, bool selected)
        {
            var formImageItem = Context.ContentDatabase.GetItem(Constants.FormsMediaImageId);
            var formImage = new MediaItem(formImageItem);
            var icon = MediaManager.GetMediaUrl(formImage);
            var click = $"javascript:return scSelect(this, event, 'FormListView_Click(&quot;{item.ID}&quot;)')";
            var selection = selected ? "border: 2px solid gray;" : string.Empty;
            return $"<a href='#' class='scTile' onclick=\"{click}\">" +
            $"<div class='scTileImage' style='position:relative; {selection}'>" + 
            $"<img src = '{icon}' alt='{item.Name}' style='width:100%;' /></div>" + 
            $"<div class='scTileHeader'>{item.Name}</div></a>";
        }
    }
}
 