using System;
using System.Text.Json;
using Website.Models.ArticleModel;
using LuminoDiodeRandomDataGenerators;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace FastTestConsoleApp
{
    internal class Program
    {
        /* This project is created for quick testing any methods by simple
         * putting its output to the console
         */
        static void Main(string[] args)
        {
            ArticleImage testArticleImage = new ArticleImage() { String64Image = RandomDataGenerator.String(20) };
            Article testArticle = new Article() { Title = "TestArticle", Id = 0, AuthorUserId = 0, Tags = new[] { "tag1", "tag2" }, BodyParts = (new IArticleBodyPart[] { testArticleImage }).ToList() };
            Console.WriteLine(JsonSerializer.Serialize(testArticle));
        }
    }
}
