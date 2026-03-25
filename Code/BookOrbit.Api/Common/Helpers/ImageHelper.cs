namespace BookOrbit.Api.Common.Helpers;

public class ImageHelper(
    IWebHostEnvironment environment,
    ILogger<ImageHelper> logger)
{
    private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<Result<string>> UploadImage(IFormFile imageFile, string FolderPath)
    {
        if (imageFile == null || imageFile.Length == 0)
            return Error.Validation("Image.Required", "No Image Was Uploaded");

        var extension = Path.GetExtension(imageFile.FileName).ToLower();

        if (!IsValidImageExtension(extension))
            return Error.Validation("Image.InvalidType", "Only image files are allowed");

        if (imageFile.Length > 2 * 1024 * 1024)
            return Error.Validation("Image.TooLarge", "Max size is 2MB");

        var fileName = $"{Guid.NewGuid()}{extension}";

        var uploadsFolder = Path.Combine(environment.ContentRootPath, FolderPath);

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
            logger.LogError(ex, "Error While Uploading Image Path : {path}", filePath);
            return Error.Failure("ImageUploadFailure", "Failure While Uploading Image");
        }

        return fileName;
    }

    static public bool IsValidImageExtension(string extension)
    {
        return allowedExtensions.Contains(extension);
    }


    public Task DeleteImage(string fileName, string FolderPath)
    {
        try
        {
            var uploadsFolder = Path.Combine(environment.ContentRootPath, FolderPath);
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

    public async Task<Result<byte[]>> GetImage(string fileName, string FolderPath)
    {
        var path = Path.Combine(environment.ContentRootPath, FolderPath, fileName);

        if (!System.IO.File.Exists(path))
        {
            logger.LogWarning("This Image Was not Found , ImageName : {fileName}", fileName);
            return Error.NotFound("Image.NotFound", "This Image Was Not Found ");
        }

        try
        {
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return bytes;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error While Loading Image: {fileName}", fileName);
            return Error.Failure("Image.Failure", "Unexcpected Error While Loading Image");
        }
    }


    public async Task<byte[]?> GetStudentImage(string fileName)
    {
        string folderPath = ApiConstants.StudentImagesUploadFolderPath;
        fileName = Path.GetFileName(fileName);

        //Load Image
        var personalPhotoResult = await GetImage(
            fileName,
            folderPath);

        if (personalPhotoResult.IsSuccess)
            return personalPhotoResult.Value;

        //Load Defualt
        var defaultImageResult = await GetDefaultStudentImage();

        if (defaultImageResult.IsSuccess)
            return defaultImageResult.Value;

        return null;
    }
    public async Task<Result<byte[]>> GetDefaultStudentImage()
    {
        string fileName = ApiConstants.DefaultStudentImageFileName;
        string folderPath = ApiConstants.StudentImagesUploadFolderPath;

        var result = await GetImage(fileName, folderPath);

        if (result.IsFailure)
        {
            logger.LogCritical("Couldnt Load The Default Student Image FolderPath : {folderPath} , FileName : {fileName}", folderPath, fileName);
            return Error.Failure("Image.Failure", "Unexcpected Error While Loading Image");
        }

        return result.Value;
    }
}
