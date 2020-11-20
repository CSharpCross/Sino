using System.IO;

namespace OrvilleX.FileManager
{
    public class DefaultFileEntry : IFileEntry
    {
        public string FileName { get; set; }

        public long Length { get; set; }

        public long StartPosition { get; set; }

        public Stream Stream { get; set; }
    }
}
