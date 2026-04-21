using System.Reflection;
using Bogus;
using Microsoft.Extensions.Configuration;
using Muradov.Domain;
using Muradov.Features;
using Muradov.Mediator;

var builder = new ConfigurationBuilder();
builder.AddJsonFile("appsettings.json", false, true);
var configuration = builder.Build();

var fileName = configuration["TestStandData:FileName"] ?? "museum_exhibits.json";

var exhibits = GenerateExhibits(75);

var featuresAssembly = Assembly.GetAssembly(typeof(SaveDataRequest));
var sender = new Sender(featuresAssembly);

Console.WriteLine($"Total exhibits in collection: {exhibits.Count}\n");

var filterVariants = CreateFilterVariants();

foreach (var filterVariant in filterVariants)
{
    Console.WriteLine($"Filter: {filterVariant.Name}");
    Console.WriteLine($"Parameters: {filterVariant.Description}");

    var predicate = ExpressionBuilder.Build(filterVariant.Filter);
    var filteredExhibits = exhibits.Where(predicate.Compile()).ToList();

    Console.WriteLine($"Results found: {filteredExhibits.Count} exhibit(s)");

    if (filteredExhibits.Count != 0)
    {
        Console.WriteLine("Filtered exhibits:");
        foreach (var exhibit in filteredExhibits.Take(30))
        {
            Console.WriteLine($"  - {exhibit.Name} ({exhibit.Century}th century, ${exhibit.EstimatedValue:N2}, Restoration needed: {exhibit.RequiresRestoration})");
        }

        if (filteredExhibits.Count > 30)
        {
            Console.WriteLine($"  ... and {filteredExhibits.Count - 10} more");
        }
    }
    else
    {
        Console.WriteLine("  No exhibits match this filter");
    }

    Console.WriteLine();
}

static List<MuseumExhibit> GenerateExhibits(int count)
{
    var faker = new Faker<MuseumExhibit>()
        .RuleFor(e => e.Id, f => f.IndexFaker + 1)
        .RuleFor(e => e.Name, f => $"{f.Commerce.ProductName()} {f.Random.Word()}")
        .RuleFor(e => e.Century, f => f.Random.Int(1, 21))
        .RuleFor(e => e.EstimatedValue, f => f.Random.Decimal(100, 1000000))
        .RuleFor(e => e.RequiresRestoration, f => f.Random.Bool(0.3f))
        .RuleFor(e => e.Origin, f => f.Address.Country())
        .RuleFor(e => e.Description, f => f.Lorem.Sentence());

    return faker.Generate(count);
}

static List<FilterVariant> CreateFilterVariants()
{
    return new List<FilterVariant>
    {
        new FilterVariant
        {
            Name = "Ancient Artifacts",
            Description = "Century: 5, Requires restoration: true",
            Filter = new FilterData
            {
                Century = 5,
                MinEstimatedValue = null,
                MaxEstimatedValue = null,
                RequiresRestoration = true
            }
        },
        new FilterVariant
        {
            Name = "Valuable Paintings",
            Description = "Min value: $100,000",
            Filter = new FilterData
            {
                MinEstimatedValue = 100000,
                RequiresRestoration = null
            }
        },
        new FilterVariant
        {
            Name = "Restored Medieval Items",
            Description = "Century: 10, Requires restoration: false, Max value: $50,000",
            Filter = new FilterData
            {
                Century = 10,
                MaxEstimatedValue = 50000,
                RequiresRestoration = false
            }
        },
        new FilterVariant
        {
            Name = "Premium Sculptures",
            Description = "Min value: $200,000, Century: 17",
            Filter = new FilterData
            {
                Century = 17,
                MinEstimatedValue = 200000
            }
        }
    };
}