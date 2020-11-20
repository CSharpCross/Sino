namespace OrvilleX.FileManager
{
    /// <summary>
    /// 提供文件名转换
    /// </summary>
    public interface IFilenameParser
    {
        string Parse(string filename);
    }
}
