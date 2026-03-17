namespace BookOrbit.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<Student> Students { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

