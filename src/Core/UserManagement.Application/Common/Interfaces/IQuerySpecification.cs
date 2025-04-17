namespace UserManagement.Application.Common.Interfaces;

public interface IQuerySpecification
{

}

public interface IQuerySpecification<T> : IQuerySpecification
{
    IQueryable<T> Apply(IQueryable<T> query);
}
