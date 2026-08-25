using MyReadingNookApp.Dtos.DailyShelf;

namespace MyReadingNookApp.Services;

public interface IDailyShelfService
{
    Task<List<ShelfBookDisplayDto>> GetTodayShelfAsync();
}