namespace PrivateDataServer.Module.FileModule.Interface;
public interface IFileService
{
    /// <summary>
    /// ファイルを保存する
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool SaveFile(IFormFile file);

    public bool DeleteFile(string FileId);
}