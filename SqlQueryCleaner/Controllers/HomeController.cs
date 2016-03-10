using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlQueryCleaner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public JsonNetResult CleanQuery(string query)
        {
            query = query.Trim();
            query = System.Text.RegularExpressions.Regex.Unescape(query);
            query = query.Replace("\"", "");
            query = ParseQuery(query);

            return new JsonNetResult(new
            {
                success = true,
                message = query
            });

           
        }

        private string ParseQuery(string Query)
        {
            var NewQuery = new List<string>();
            var variableName = "";
            var variableDeclaration = "";

            for (var i = 0; i < Query.Length; i++)
            {
                if (Query.Substring(i, 1) == "@")
                {
                    for (var j = i + 1; j < Query.Length; j++)
                    {
                        if (Query.Substring(j, 1) == " " || Query.Substring(j, 1) == "\r" || Query.Substring(j, 1) == "\n" || Query.Substring(j, 1) == "\t")
                        {
                            variableName = Query.Substring(i, j - i).ToLower();
                            variableDeclaration = "Declare " + variableName;
                            if (variableName.IndexOf("id") != -1)
                            {
                                variableDeclaration += " int=0";
                            }
                            else
                            {
                                variableDeclaration += " varchar =''";
                            }
                            AddIfItDoesntExists(NewQuery, variableDeclaration);
                            break;
                        }
                    }
                }
            }

            NewQuery.Add(Query);

            return string.Join(System.Environment.NewLine, NewQuery);
        }

        private void AddIfItDoesntExists(List<string> query, string newLine)
        {
            foreach (string s in query)
            {
                if (s == newLine)
                {
                    return;
                }
            }
            query.Add(newLine);
        }
    }
}