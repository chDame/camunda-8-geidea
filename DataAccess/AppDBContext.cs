using Microsoft.EntityFrameworkCore;
using tasklistDotNetReact.DataAccess.Entities;

namespace tasklistDotNetReact.DataAccess
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{
		}

		public DbSet<TaskModel> TaskModels { get; set; }
	}
}
