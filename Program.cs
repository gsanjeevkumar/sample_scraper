using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WebFetch
{
    class Program
    {
        static void Main(string[] args)
        {
            var _url = "https://stocksnap.io/search/flower";
            WebClient _client = new WebClient();
            string _html = _client.DownloadString(_url);


            List<string> _searchTags = new List<string>(){"img", "h1"};

            // var _matches = Regex.Match(_html);
            
            Console.WriteLine("HTML Tag Count    : " + GetElementCount(_html, TagType.IMG));
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
            List<string> _images = new List<string>();
            string n = doc.DocumentNode.SelectNodes("//img").First().Attributes["src"].Value;

            foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img")){
                HtmlAttribute att = img.Attributes["src"];
                _images.Add(new string(att.Value));
            }

            Regex _rx;
            string _pattern;
            int _length = 0;
            MatchCollection _matches;
            try
            {
                switch(tagType)
                {
                    case TagType.HTML:
                        _pattern = @"(/?([^>/]*)/?>)";
                        break;
                    case TagType.IMG:
                        _pattern = @"img.*";
                        break;
                    default:
                        _pattern = @"(</?([^>/]*)/?>)";
                        break;
                }

                _matches = new Regex(_pattern).Matches(_html);
                var tags = _matches.OfType<Match>().Select(m => m.Groups[2].Value);
                                
                // _rs = Regex.Matches(_html, _pattern, RegexOptions.IgnoreCase);
                _length = 1;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Exception at:" + ex.ToString());  
            }
            finally
            {
                _length = 0;
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
