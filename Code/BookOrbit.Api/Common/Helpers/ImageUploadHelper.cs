namespace BookOrbit.Api.Common.Helpers;

public class ImageUploadHelper(
    IWebHostEnvironment environment,
    ILogger<ImageUploadHelper>logger)
{
    public async Task<Result<string>> UploadImage(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return Error.Failure("Image.Required", "No Image Was Uploaded");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(imageFile.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return Error.Failure("Image.InvalidType", "Only image files are allowed");

        if (imageFile.Length > 2 * 1024 * 1024)
            return Error.Failure("Image.TooLarge", "Max size is 2MB");

        var fileName = $"{Guid.NewGuid()}{extension}";

        var uploadsFolder = Path.Combine(environment.ContentRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, fileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Error While Uploading Image Path : {path}", filePath);
            return Error.Failure("ImageUploadFailure", "Failure While Uploading Image");
        }

        return fileName;
    }

    public Task DeleteImage(string fileName)
    {
        try
        {
            var uploadsFolder = Path.Combine(environment.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error While Deleting Image: {fileName}", fileName);
        }

        return Task.CompletedTask;
    }
}
