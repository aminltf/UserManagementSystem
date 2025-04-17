namespace UserManagement.Application.Common.Interfaces;

public interface IFilterableQuery
{

}

public interface IFilterableQuery<TFilter> : IFilterableQuery
{
    TFilter Filter { get; set; }
}