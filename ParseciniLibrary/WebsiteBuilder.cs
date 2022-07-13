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
        private string outputFolderRoot;
        public string WebsiteFolder;
        public Website Website;

        public WebsiteBuilder(string websiteFolder)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false, true);
            
            myConfig = builder.Build();

            Log.LogFileName = myConfig["LogSettings:LogFileName"];
            Log.LogPath = Path.Join(Directory.GetCurrentDirectory(), myConfig["LogSettings:LogFolder"]);
            outputFolderRoot = Path.Join(Directory.GetCurrentDirectory(), myConfig["FileSettings:OutputFolder"]);

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
                ThemeBuilder.WebsiteVariables["title"] = p.Title;

                if (!p.IsBlogPage)
                    ProcessTemplate(Path.Join(WebsiteFolder, p.Markdown), Path.Join(WebsiteFolder, p.Template), outputFolderRoot);
                else
                    ProcessBlogPosts(p.Title, p.RootUrl, p.Template, p.Posts, p.Pagination, p.PreviewTemplate);
            }

            Log.EndLogging();
            return true;
        }

        private void ProcessBlogPosts(string title, string rooturl, string templatePath, string posts, int pagination, string previewTemplate)
        {
            BlogIndex blogIndex = new BlogIndex();

            blogIndex.TemplatePath = Path.Join(WebsiteFolder, templatePath);
            blogIndex.PreviewTemplatePath = Path.Join(WebsiteFolder, previewTemplate);

            string postsPath = Path.Join(WebsiteFolder, posts);

            foreach (string yearDir in Directory.EnumerateDirectories(postsPath).OrderByDescending(x => x))
            {
                DirectoryInfo yearDirInfo = new DirectoryInfo(yearDir);
                string yearDirName = yearDirInfo.Name;

                foreach(string monthDir in Directory.EnumerateDirectories(yearDir).OrderByDescending(x => x))
                {
                    DirectoryInfo monthDirInfo = new DirectoryInfo(monthDir);
                    string monthDirName = monthDirInfo.Name;

                    foreach (string dayFile in Directory.EnumerateFiles(monthDir).OrderByDescending(x => x))
                    {
                        FileInfo dayFileInfo = new FileInfo(dayFile);
                        string dayFileName = dayFileInfo.Name;
                        string dayHtmlName = dayFileInfo.Name.Replace(dayFileInfo.Extension, ".html");

                        Page newPage = new Page($"{dayFileName}", Path.Join(rooturl, $"{posts}/{yearDirName}/{monthDirName}/"), templatePath, $"/{posts}/{yearDirName}/{monthDirName}/{dayFileName}");

                        string dateString = $"{yearDirName} {monthDirName} {dayFileName.Replace(dayFileInfo.Extension, "")}";

                        Post newPost = new Post(DateTime.Parse(dateString), new Uri($"{outputFolderRoot}/{posts}/{yearDirName}/{monthDirName}/{dayHtmlName}").ToString());
                        newPost.Variables = new Dictionary<string, string>();
                        newPost.Variables.Add("previewLink", newPost.Url);
                        newPost.Variables.Add("previewDate", newPost.PostDate.ToString("MMMM dd, yyyy"));

                        ProcessPost(Path.Join(WebsiteFolder, newPage.Markdown), Path.Join(WebsiteFolder, newPage.Template), Path.Join(outputFolderRoot, newPage.RootUrl), newPost.Variables);

                        blogIndex.Posts.Add(newPost);
                    }
                }
            }
            string path = Path.GetFileName(templatePath).Replace(Path.GetExtension(templatePath), ".html");

            ThemeBuilder.SetOutputFolder(Path.Join(outputFolderRoot, rooturl));
            ThemeBuilder.WriteBlogIndexToTemplate(blogIndex, path);
        }

        private bool ProcessTemplate(string markdownPath, string templatePath, string outputPath)
        {
            try
            {
                ThemeBuilder.SetTemplate(templatePath);
                ThemeBuilder.SetOutputFolder(outputPath);
                ThemeBuilder.Process(markdownPath, ".mdt");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ProcessPost(string markdownPath, string templatePath, string outputPath, Dictionary<string,string> postVariables)
        {
            try
            {
                ThemeBuilder.SetTemplate(templatePath);
                ThemeBuilder.SetOutputFolder(outputPath);
                ThemeBuilder.SetPostVariableDictionary(postVariables);
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
                navigationString += nav.OpenTag.Replace("{url}", new Uri($"{Path.Join(outputFolderRoot,p.RootUrl,p.Title)}.html").ToString());
                navigationString += p.Title;
                navigationString += nav.CloseTag;
            }
            return navigationString;
        }
    }
}
