using application_infrastructure.Entities;

namespace application_infrastructure.Repositories.Interfaces;

public interface ISearchHistoryRepository : IGenericRepository<SearchHistory>
{
    IEnumerable<SearchHistory> Save(SearchHistory search);
}
