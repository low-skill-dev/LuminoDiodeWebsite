﻿using RandomDataGenerator;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Update;
using NpgsqlTypes;
using Website.Models.UserModel;

namespace Website.Models.DocumentModel
{
	public class Document
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public User Author { get; set; }
		public string[] Tags { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public DocumentParagraph[] Paragraphs { get; set; }

		public override string ToString()
		{
			return Utf8Json.JsonSerializer.ToJsonString(this);
		}

#if DEBUG
		static Random rnd = new Random();
		public static Document GenerateRandom()
		{
			return new Document
			{
				Title = new RandomDataGenerator.Randomizers.RandomizerTextWords(new RandomDataGenerator.FieldOptions.FieldOptionsTextWords { }).Generate(),
				Author = new UserModel.User() { Id=null, FirstName="AdminFN", LastName="AdminLN"},
				Tags = new string[rnd.Next(1, 6)]
					.Select(x => new string(new RandomDataGenerator.Randomizers.RandomizerTextLipsum(new RandomDataGenerator.FieldOptions.FieldOptionsTextLipsum { }).Generate()
					.Split(' ')[rnd.Next(0,7)].Where(x=> char.IsLetterOrDigit(x)).ToArray())).ToArray(),
				CreatedDateTime = DateTime.Now,
				Paragraphs = new DocumentParagraph[rnd.Next(2, 15)].Select(x => DocumentParagraph.GenerateRandom()).ToArray()
			};
		}
#endif
	}
}
