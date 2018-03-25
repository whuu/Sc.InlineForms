# SmartSitecore.InlineForms

Module allows to use Sitecore 9 Forms directly in Rich Text Field.

Editors have two options to insert the form, both work from Experience and Content Editor:

* **Type in [form id="form-itemid"] tag:**

![Sitecore Inline Forms Experience Editor](documentation/Sitecore-Inline-Forms-Experience-Editor.gif)

*You can find your form item IDs in Content Editor under /sitecore/Forms item*

* **Pick a form from Rich Text Editor:**

![Sitecore Inline Forms Rich Text Editor](documentation/Sitecore-Inline-Forms-Rich-Text-Editor.gif)

## Installation

* In Content Management or Standalone server install [Inline Foms-1.0 CM.zip](sc.package) using Sitecore Installation Wizard.
* In Content Delivery server copy the files from [Inline Foms-1.0 CD.zip](sc.package) to website root folder.

To display Sitecore 9 Form in your page, make sure that you have `@Html.RenderFormStyles()` and `@Html.RenderFormScripts()` in your MVC layout with `@using Sitecore.ExperienceForms.Mvc.Html` directive.

## Manual Installation/Install from Source

* Clone repository
* If needed update nuget packages used in InlineForms projects to match your Sitecore 9 version
* Update `publishUrl` to your Sitecore instance URL in `publishsettings.targets` file
* Update path in `SourceFolderInlineForms` variable to your local repository folder in `SmartSitecore.InlineForms.DevSettings.config` file
* Publish `SmartSitecore.Foundation.InlineFormsRenderer` and `SmartSitecore.Foundation.InlineFormsRichText` projects from Visual Studio
* Publish `SmartSitecore.Foundation.InlineForms.Serialization` project. This project contains Unicorn assemblies and configuration. If you already have Unicorn in your project you can deploy only `App_Config\Include\Foundation\InlineForms` folder.  
* Go to {your-sitecore-instance}/unicorn.aspx and sync `Foundation.InlineFormsRichText` project.

### Test website deployment

* Follow the steps for manual installation of the module
* Publish `SmartSitecore.Project.InlineForms.TestWebsite` project form Visual Studio. Project contains Glass.Mapper configuration, if you already have it, you can remove them.
* Go to {your-sitecore-instance}/unicorn.aspx and sync `Foundation.Serialization` and `Project.InlineFormsTestWebsite` projects.
* Redeploy your site if you use web database
* Test pages are installed under `/sitecore/Content/Home/InlineForms` item, there is also test contact form item under `/sitecore/Forms`. Project contains two pages which show how to render inline form with Standard Sitecore MVC or Glass.Mapper
* If the contact form is not visible in Forms app, rebuild sitecore_master_index in Control Panel
* If the contact form item usage doesn't point to the test pages, rebuild master link database in Control Panel

