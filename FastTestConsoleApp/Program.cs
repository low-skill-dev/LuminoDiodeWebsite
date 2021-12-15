using System;
using System.Text.Json;
using Website.Models.DocumentModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Utf8Json;
using RandomDataGenerator;


namespace FastTestConsoleApp
{
	internal class Program
	{
		/* This project is created for quick testing any methods by simple
         * putting its output to the console
         */
		static void Main(string[] args)
		{
			Console.Write(FuzzySharp.Fuzz.TokenSetRatio("купить апельсины москва","апельсины купить адрес"));
		}
	}
}
