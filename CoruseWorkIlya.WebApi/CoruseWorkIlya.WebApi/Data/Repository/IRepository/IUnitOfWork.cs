namespace CourseWork.WebApi.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task Commit();
    }
}
