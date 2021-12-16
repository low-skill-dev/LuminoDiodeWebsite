using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FuzzySharp;
using Website.Models.UserModel;
using Website.Models.DocumentModel;

namespace Website.Models.ProjectsGroupModel
{
	public class ProjectsGroup
	{
		public int Id { get; set; }
		public User OwnerId { get; set; }
		public User[] Admins { get; set; }
		public DbDocument[] OrderedProjects { get; set; }
		public string ShortDescription { get; set; }
	}
}
