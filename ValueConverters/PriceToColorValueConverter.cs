using System.Globalization;

namespace ipr2.ValueConverters;

internal class PriceToColorValueConverter : IValueConverter
{
	public const decimal MaxPrice = 30000;

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is decimal price && price > MaxPrice)
			return Colors.OrangeRed;
		return Colors.LightGray;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
