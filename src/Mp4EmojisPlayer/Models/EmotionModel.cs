using System.Text.Json.Serialization;

namespace Mp4EmojisPlayer.Models
{
    public class EmotionModel
    {
        [JsonPropertyName("cmd_tag")]
        public string CmdTag { get; set; } = "";

        [JsonPropertyName("cmd_tag_name")]
        public string CmdTagName { get; set; } = "";

        public string VideoPath => $"videos/{CmdTag}.mp4";
        
        public string DisplayName => string.IsNullOrEmpty(CmdTagName) ? CmdTag : CmdTagName;
    }

    public class EmotionCategory
    {
        public string Name { get; set; } = "";
        public List<EmotionModel> Emotions { get; set; } = new();
    }
}
