namespace portal.Helpers;

public class FileHandling
{
    public static async Task<MemoryStream> ToMemoryStreamAsync(Stream input)
    {
        if (input is MemoryStream ms)
            return ms;

        var memoryStream = new MemoryStream();
        await input.CopyToAsync(memoryStream);
        memoryStream.Position = 0; // Reset for reading
        return memoryStream;
    }
    
    public static Task<MemoryStream> ToMemoryStreamAsync(MemoryStream input)
    {
        input.Position = 0; // Reset for reading
        return Task.FromResult(input);
    }

    public static bool IsPngHeader(MemoryStream stream)
    {
        byte[] pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47 }; // ‰PNG
        byte[] buffer = new byte[4];

        stream.Position = 0;
        stream.Read(buffer, 0, 4);
        stream.Position = 0;

        return buffer.SequenceEqual(pngHeader);
    }
}