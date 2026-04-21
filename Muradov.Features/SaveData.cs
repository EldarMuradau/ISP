using Muradov.Domain;
using Muradov.Mediator;
using System.Text;
using System.Text.Json;

namespace Muradov.Features
{
    public record SaveDataRequest(IEnumerable<MuseumExhibit> Data, string FileName) : IRequest;

    public class SaveDataRequestHandler : IRequestHandler<SaveDataRequest>
    {
        public async Task Handle(SaveDataRequest request, CancellationToken? cancellationToken = null)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(request.Data, options);
            await File.WriteAllTextAsync(request.FileName, json, Encoding.Unicode, cancellationToken ?? default);
        }
    }
}
