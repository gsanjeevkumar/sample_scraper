using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.IO;

namespace WebFetch
{
    class Program
    {
        static void Main(string[] args)
        {
            var _url = "https://stocksnap.io/search/flower";
            string _urlXCentium = "https://www.xcentium.com/";
            

            // _client.Headers["User-Agent"] = "MOZILLA/5.0 (WINDOWS NT 6.1; WOW64) APPLEWEBKIT/537.1 (KHTML, LIKE GECKO) CHROME/21.0.1180.75 SAFARI/537.1";

            HtmlWeb _doc = new HtmlWeb();
            string _html = "";

            try
            {
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                WebClient _client = new WebClient();
                _client.Headers.Add("User-Agent: Only a test");
                _html = _client.DownloadString(_url);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = ex.Response;
                    var dataStream = response.GetResponseStream();
                    var reader = new StreamReader(dataStream);
                    var details = reader.ReadToEnd();
                }
            }


            List<string> _searchTags = new List<string>(){"img", "h1"};

            // var _matches = Regex.Match(_html);
            
            Console.WriteLine("Image Tag Count   : " + GetElementCount(_html, TagType.IMG));
            Console.WriteLine("Word Count        : " + GetWordCount(_html));
            Console.WriteLine("Unique Word Count : " + GetUniqueWordCount(_html));
        }

        enum TagType{
            IMG = 0,
            HTML = 1
        }
        static int GetElementCount(string _html, TagType tagType){
        
            var _url = "https://stocksnap.io/search/flower";
            var doc = new HtmlDocument();
            doc.LoadHtml(_html);
            var _images = new List<String>();
            int _length = 0;
            
            String _allWords = "";

            int _wordCount = 0;

            //*[not(self::script or self::style)]/text()
            foreach (HtmlNode style in doc.DocumentNode.Descendants("style").ToArray())
            {
                style.Remove();
            }
            foreach (HtmlNode script in doc.DocumentNode.Descendants("script").ToArray())
            {
                script.Remove();
            }

            foreach (HtmlTextNode node in doc.DocumentNode.SelectNodes("//*[not(self::script or self::style)]/text()[normalize-space()]"))
            {
                _wordCount += GetWordCount(node.InnerText);
                _allWords += " " + node.InnerText;
            }

            Console.WriteLine(@"Word count          : {0}", _wordCount);
            Console.WriteLine(@"Unique Word count   : {0}", GetUniqueWordCount(_allWords));
            try{
                
                foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img")){
                    if(img.Attributes.Contains(new string("src"))){
                        System.Console.WriteLine(@"IMG Tag :{0}", img.Attributes[0].Value);
                        _images.Add(img.Attributes[0].Value);
                    };
                }

                _length = _images.Count;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception at:" + ex.ToString());  
            }

            return _length;
        }

        static int GetWordCount(string content)
        {
            int _count = 0;

            if(content=="")
            {
                _count = 0;
            }

            Array _words = content.Split(" ");

            try
            {  
                _count = _words.Length;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception while counting words: " + ex.ToString());
            }
            

            return _count;
        }

         static int GetUniqueWordCount(string content)
         {
            int _count = 0;
            HashSet<string> _uniqueWordsSet;

            if(content=="")
            {
                return 0;
            }

            Array _words = content.Split(" ");

            try
            {
                 _uniqueWordsSet = new HashSet<string>();

                foreach (string item in _words)
                {
                    _uniqueWordsSet.Add(item);
                }
  
                _count = _uniqueWordsSet.Count;
                    
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception while Unique Word Counting: " + ex.ToString());
            }
            
            return _count;
        }

    }
}
