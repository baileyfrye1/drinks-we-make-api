using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace DrinksWeMake.Api.Infrastructure;

public sealed class SupabaseStorageClient(HttpClient httpClient, IConfiguration configuration) : IStorageClient
{
    private readonly string _bucketName = configuration["Storage:Bucket"];
    private readonly string _publicUrl = configuration["Storage:PublicUrl"];
    
    public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        await using var fileStream = file.OpenReadStream();
        using var content = new StreamContent(fileStream);

        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => throw new InvalidOperationException("File type not supported.")
        };

        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        
        var response = await httpClient.PostAsync($"{_bucketName}/{fileName}", content, cancellationToken);

        response.EnsureSuccessStatusCode();

        return $"{_publicUrl}/{_bucketName}/{fileName}"; 
    }

    public async Task<string> UpdateFileAsync(IFormFile file, string path, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> DeleteFileAsync(string imageUrl, CancellationToken cancellationToken)
    {
        var prefix = $"{_publicUrl}/{_bucketName}/";

        if (!imageUrl.StartsWith(prefix))
        {
            throw new ArgumentException(
                "Invalid Supabase Storage URL.",
                nameof(imageUrl));
        }

        var fileName = imageUrl[prefix.Length..];

        var response = await httpClient.DeleteAsync(
            $"{_bucketName}/{fileName}",
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return Results.NoContent();
    }
}

public interface IStorageClient
{
    Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);
    Task<string> UpdateFileAsync(IFormFile file, string path, CancellationToken cancellationToken);
    Task<IResult> DeleteFileAsync(string imageUrl, CancellationToken cancellationToken);
}