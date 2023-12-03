using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;


namespace telePars
{
    public class Program
    {
        public static List<string> GetUrlsList(string sourceText, string domen)
        {
            List<string> urls = new List<string>();
            string pattern = "img src=";
            Regex reg = new Regex($@"{pattern}\S*" + "\"");
            var matches = reg.Matches(sourceText);
            urls = matches.Select(x => x.Value).Distinct().ToList();
            urls = urls.Select(str => domen + str.Replace(pattern,"").Replace("\"","").Substring(1)).ToList();
            return urls;
        }
        public static Regex FileNameRegex = new Regex(@"(\w+)\.(\w+)$");
        public static string GetNameOfFile(string url)
        {
            return FileNameRegex.Match(url).Value;            
        }
        public static string GetNumberOfPost(int num)
        {
            return num == 0 ? "" : "-" + num.ToString();
        }
        public static void Main()
        {
            DirectoryInfo di = new DirectoryInfo("/home/pathToSave");
            string[] dirEnum = di.EnumerateFiles().Select(x => x.Name).ToArray();
 
            string destDirPath = @"/home/pathToSave";
            string domen = @"https://telegra.ph/";

            //FileInfo fi = new FileInfo("/home/pathToSave/w_names.txt");
            //StreamReader sr = fi.OpenText();    
            //string[] names = sr.ReadToEnd().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int border = 5;
            Regex reg = new Regex(@"http(\S+)\.(\w+)$");

            using(WebClient wc = new WebClient())
            {
                foreach (var name in /*names*/new string[] {"Viktoriya"})
                {
                StringBuilder fullUrl;
                for(int x = 1;x < 13;x++)
                    for(int y = 1;y < 31;y++)
                        for(int z = 0;z < border;z++)
                        {
                            fullUrl = new StringBuilder();
                            fullUrl.Append(domen + name + "-" + 
                                x.ToString().PadLeft(2,'0') + "-" + 
                                y.ToString().PadLeft(2,'0')
                                 + GetNumberOfPost(z));
                            System.Console.WriteLine(fullUrl.ToString());
                            string res = "";
                            try
                            {
                                res = wc.DownloadString(fullUrl.ToString());
                            }
                            catch
                            {
                                System.Console.WriteLine("   -");
                                continue;
                            }
                                
                            
                            if(res.Contains("dont exist"))
                            {
                                System.Console.WriteLine("=");
                                continue;
                            }
                            string[] urls = GetUrlsList(res, domen).ToArray();
                            foreach(string url in urls)
                            {
                                try
                                {
                                    string nameOfFile = GetNameOfFile(url);
                                    if(dirEnum.Contains(nameOfFile))
                                        continue;
                                    System.Console.WriteLine(url);
                                    wc.DownloadFile(url, destDirPath + "/" + nameOfFile);
                                    System.Console.WriteLine("dowloaded");
                                }
                                catch
                                {

                                }
                            }
                        }
                            
                }
            }

        }
    }
}
