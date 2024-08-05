using System.Reflection;
using TDTU.API.Interfaces;

namespace TDTU.API.Data;

public class DataContext : DbContext, IDataContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options) { }

	public DbSet<User> Users => Set<User>();

	public DbSet<Role> Roles => Set<Role>();

	public Task<int> SaveChangesAsync()
	{
		return base.SaveChangesAsync();
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

}
