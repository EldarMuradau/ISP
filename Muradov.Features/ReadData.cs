using Muradov.Domain;
using Muradov.Mediator;
using System.Text;
using System.Text.Json;

namespace Muradov.Features
{
    public record ReadDataRequest(string FileName) : IRequest<IEnumerable<MuseumExhibit>>;

    public class ReadDataRequestHandler : IRequestHandler<ReadDataRequest, IEnumerable<MuseumExhibit>>
    {
        public async Task<IEnumerable<MuseumExhibit>> Handle(ReadDataRequest request, CancellationToken? cancellationToken = null)
        {
            if (!File.Exists(request.FileName))
            {
                return Enumerable.Empty<MuseumExhibit>();
            }

            var json = await File.ReadAllTextAsync(request.FileName, Encoding.Unicode, cancellationToken ?? default);
            var data = JsonSerializer.Deserialize<IEnumerable<MuseumExhibit>>(json);

            return data ?? Enumerable.Empty<MuseumExhibit>();
        }
    }
}
