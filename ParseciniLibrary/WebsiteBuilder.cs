using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParseciniLibrary.Logging;
using ParseciniLibrary.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseciniLibrary
{
    public class WebsiteBuilder
    {
        public ThemeBuilder ThemeBuilder { get; set; }
        public IConfigurationRoot myConfig { get; }
        private string outputFolder;
        public string WebsiteFolder;
        public Website Website;

        public WebsiteBuilder(string websiteFolder)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            
            myConfig = builder.Build();

            Log.LogFileName = myConfig["LogSettings:LogFileName"];
            Log.LogPath = Path.Join(Directory.GetCurrentDirectory(), myConfig["LogSettings:LogFolder"]);
            outputFolder = Path.Join(Directory.GetCurrentDirectory(), myConfig["FileSettings:OutputFolder"]);

            WebsiteFolder = websiteFolder;
            
            if (File.Exists(Path.Join(websiteFolder, "website.cfg")))
            {
                using (StreamReader r = File.OpenText(Path.Join(websiteFolder, "website.cfg")))
                {
                    string json = r.ReadToEnd();
                    Website = JsonConvert.DeserializeObject<Website>(json);
                }
            }
            else
            {
                throw new FileNotFoundException(Path.Join(websiteFolder, "website.cfg"));
            }

            ThemeBuilder = new ThemeBuilder(myConfig);
            ThemeBuilder.WebsiteVariables.Add("sitename", Website.Name);
            ThemeBuilder.WebsiteVariables.Add("navigation", BuildNavigationList(Website.Pages, Website.Navigation));
        }

        public bool ProcessSite()
        {
            Log.BeginLogging();

            foreach(Page p in Website.Pages)
            {
                if(!p.IsBlogPage)
                    ProcessTemplate(Path.Join(WebsiteFolder, p.Markdown), Path.Join(WebsiteFolder, p.Template));
                //TODO: we're only processing non-blog pages, now we need to add support for parsing all the blog pages, and creating the index pages based on the pagination size
            }

            Log.EndLogging();
            return true;
        }

        private bool ProcessTemplate(string markdownPath, string templatePath)
        {
            try
            {
                ThemeBuilder.SetTemplate(templatePath);
                ThemeBuilder.SetOutputFolder(outputFolder);
                ThemeBuilder.Process(markdownPath, ".mdt");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string BuildNavigationList(List<Page> pages, Navigation nav)
        {
            string navigationString = "";
            foreach(Page p in pages)
            {
                navigationString += nav.OpenTag.Replace("{url}", p.Url);
                navigationString += p.Title;
                navigationString += nav.CloseTag;
            }
            return navigationString;
        }
    }
}
