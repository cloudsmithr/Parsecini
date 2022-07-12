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

            Log.LogPath = Path.Combine(Directory.GetCurrentDirectory(), myConfig["LogSettings:LogFolder"]);
            Log.LogFileName = myConfig["LogSettings:LogFileName"];
            outputFolder = Path.Combine(Directory.GetCurrentDirectory(), myConfig["FileSettings:OutputFolder"]);

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
        }

        public bool ProcessSite()
        {
            Log.BeginLogging();

            foreach(Page p in Website.Pages)
            {
                if(!p.IsBlogPage)
                    ProcessTemplate(Path.Join(WebsiteFolder, p.Markdown), Path.Join(WebsiteFolder, p.Template));
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
    }
}
