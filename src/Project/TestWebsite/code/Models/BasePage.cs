using System;

namespace SmartSitecore.Project.InlineForms.TestWebsite.Models
{
    public class BasePage
    {
        public virtual Guid Id { get; set; }

        public virtual string Url { get; set; }

        public virtual string Title { get; set; }

        public virtual string Text { get; set; }
    }
}