using ipr1.Models;

namespace ipr1.Services;

public interface IRateService
{
    IEnumerable<Rate> GetRates(DateTime date);
}
