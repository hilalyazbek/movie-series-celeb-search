using application_infrastructure.Entities;
using application_infrastructure.PagingAndSorting;

namespace application_infrastructure.Repositories.Interfaces;

public interface ISearchHistoryRepository : IGenericRepository<SearchHistory>
{
    IEnumerable<SearchHistory> Save(SearchHistory search);

    Task<IEnumerable<SearchHistory>> GetAllAsync(PagingParameters paging, SortingParameters sorting);
}
