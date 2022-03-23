using System;
using System.Text.Json;
using Website.Models.DocumentModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Utf8Json;
using RandomDataGenerator;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Website.Services.SettingsProviders;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Net;


namespace FastTestConsoleApp
{
	internal class Program
	{
		/* This project is created for quick testing any methods by simple
         * putting its output to the console
         */
		static void Main(string[] args)
		{
			IPAddress a1 = new IPAddress(new byte[] { 255, 255, 255, 255 });
			IPAddress a2 = new IPAddress(new byte[] { 255, 255, 255, 255 });
			Console.WriteLine(a1 == a2);

			Dictionary<IPAddress, int> dict=new();
			dict.Add(a1, 1);
			Console.WriteLine(dict.ContainsKey(a2));
		}
	}
}
