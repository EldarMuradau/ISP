using System.Globalization;

namespace ipr2.ValueConverters;

internal class IdToImageValueConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is int id && ImageStorage.Find(id) is string path)
		{
			var bytes = File.ReadAllBytes(path);
			return ImageSource.FromStream(() => new MemoryStream(bytes));
		}
		return ImageSource.FromFile("car_placeholder.png");
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
