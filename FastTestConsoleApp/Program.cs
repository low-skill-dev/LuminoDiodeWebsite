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
			Console.WriteLine(

				String.Join(' ',
				SHA512.HashData(new byte[]
				{
					57,57,57,105,116,105,49,50,49,49,49,52,114,116,64,103,109,97,105,108,46,99,111,109,126,61,251,73,41,15,5,7,109,31,134,217,183,189,47,119,6,220,106,239,234,219,139,102,105,71,181,152,225,84,129,177,7,33,190,167,243,182,70,36,4,105,190,255,33,236,207,96,165,45,29,12,97,82,209,23,171,37,187,160,127,101,130,64
				}
				)));
		}
	}
}
