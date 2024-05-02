namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepositories<T> GenericRepositories<T>() where T : class;
        void Save();

        Task<int> SaveChangesAsync();
    }
}
