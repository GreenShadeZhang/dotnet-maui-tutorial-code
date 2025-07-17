namespace Mp4EmojisPlayer.Services
{
    public interface IVideoService
    {
        Task<string> GetVideoUriAsync(string videoFileName);
        Task<bool> VideoExistsAsync(string videoFileName);
    }
}
