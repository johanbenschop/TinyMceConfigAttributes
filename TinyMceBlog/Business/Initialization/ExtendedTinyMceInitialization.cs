using TinyMceBlog.Models.Blocks;
using TinyMceBlog.Models.Pages;
using EPiServer.Cms.TinyMce.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using TinyMceBlog.Business.Attributes;
using TinyMceBlog.Business.TinyMceConfig;

namespace TinyMceBlog.Business.Initialization
{
    [ModuleDependency(typeof(DependencyResolverInitialization), typeof(TinyMceInitialization))]
    public class ExtendedTinyMceInitialization : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Configure<TinyMceConfiguration>(config =>
            {
                // Add content CSS to the default settings.
                config.Default()
                    .ContentCss("/static/css/editor.css");

                // This will clone the default settings object and extend it by
                // limiting the block formats for the MainBody property of an ArticlePage.
                //config.For<ArticlePage>(t => t.MainBody)
                //    .BlockFormats("Paragraph=p;Header 1=h1;Header 2=h2;Header 3=h3");

                // Passing a second argument to For<> will clone the given settings object
                // instead of the default one and extend it with some basic toolbar commands.
                config.For<EditorialBlock>(t => t.MainBody, config.Empty())
                    .AddEpiserverSupport()
                    .DisableMenubar()
                    .Toolbar("bold italic underline strikethrough");

                var simpleConfig = config.Default().Clone()
                    .AddPlugin("wordcount code")
                    .AppendToolbar("code")
                    .RemovePlugin("image")
                    .RemovePlugin("epi-image-editor")
                    .Height(125);

                var verySimpleConfig = config.Empty().Clone()
                    .Height(400);

                // For one property of one page type:
                // config.For<StandardPage>(x => x.MainBody, simpleConfig);

                TinyMceCustomSettingsAttributeRegistration<SimpleTinyMceConfigAttribute>.RegisterCustomTinyMceSettingsAttribute(config, simpleConfig );

                TinyMceCustomSettingsAttributeRegistration<VerySimpleTinyMceConfigAttribute>.RegisterCustomTinyMceSettingsAttribute(config, verySimpleConfig);

            });
        }
    }
}
