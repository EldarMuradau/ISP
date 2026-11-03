namespace ipr2;

// Ad images are stored as Images/{adId}.{ext} inside the platform app data folder
public static class ImageStorage
{
	public static string Folder => Path.Combine(FileSystem.Current.AppDataDirectory, "Images");

	public static string? Find(int adId)
	{
		if (!Directory.Exists(Folder)) return null;
		return Directory.GetFiles(Folder, $"{adId}.*").FirstOrDefault();
	}

	public static async Task SaveAsync(int adId, FileResult file)
	{
		Directory.CreateDirectory(Folder);
		Delete(adId);

		var path = Path.Combine(Folder, adId + Path.GetExtension(file.FileName));
		using var source = await file.OpenReadAsync();
		using var target = File.Create(path);
		await source.CopyToAsync(target);
	}

	public static void Delete(int adId)
	{
		var existing = Find(adId);
		if (existing != null) File.Delete(existing);
	}
}
