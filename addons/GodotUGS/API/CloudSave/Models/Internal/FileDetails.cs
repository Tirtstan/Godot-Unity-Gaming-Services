namespace Unity.Services.CloudSave.Internal.Models;

using System.Text.Json.Serialization;

public class FileDetails
{
    /// <summary>
    /// Creates an instance of FileDetails.
    /// </summary>
    /// <param name="contentType">The MIME type of the file that will be uploaded</param>
    /// <param name="contentLength">The content length in bytes of the file that will be uploaded</param>
    /// <param name="contentMd5">The base64 encoded MD5 checksum of the file contents that will be uploaded</param>
    /// <param name="writeLock">The expected writeLock value of the currently stored file</param>
    public FileDetails(string contentType, long contentLength, string contentMd5, string writeLock = default)
    {
        ContentType = contentType;
        ContentLength = contentLength;
        ContentMd5 = contentMd5;
        WriteLock = writeLock;
    }

    /// <summary>
    /// The MIME type of the file that will be uploaded
    /// </summary>
    [JsonPropertyName("contentType")]
    public string ContentType { get; }

    /// <summary>
    /// The content length in bytes of the file that will be uploaded
    /// </summary>
    [JsonPropertyName("contentLength")]
    public long ContentLength { get; }

    /// <summary>
    /// The base64 encoded MD5 checksum of the file contents that will be uploaded
    /// </summary>
    [JsonPropertyName("contentMd5")]
    public string ContentMd5 { get; }

    /// <summary>
    /// The expected writeLock value of the currently stored file
    /// </summary>
    [JsonPropertyName("writeLock")]
    public string WriteLock { get; }
}
