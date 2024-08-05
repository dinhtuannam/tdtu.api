using System.Data.SqlTypes;

namespace TDTU.API.Interfaces;

public interface IDataContext
{
	DbSet<User> Users { get; }
	DbSet<Role> Roles { get; }
	Task<int> SaveChangesAsync();
	Task<int> SaveChangesAsync(CancellationToken token);
}

