using System.Text.Json;
using Mp4EmojisPlayer.Models;

namespace Mp4EmojisPlayer.Services
{
    public class EmotionService
    {
        private List<EmotionModel> _allEmotions = new();
        private List<EmotionCategory> _emotionCategories = new();

        public async Task<List<EmotionModel>> LoadEmotionsAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("emotions.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                
                _allEmotions = JsonSerializer.Deserialize<List<EmotionModel>>(json) ?? new();
                
                // 创建情绪分类
                CreateEmotionCategories();
                
                return _allEmotions;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading emotions: {ex.Message}");
                return new List<EmotionModel>();
            }
        }

        private void CreateEmotionCategories()
        {
            var happyEmotions = _allEmotions.Where(e => 
                e.CmdTagName.Contains("笑") || 
                e.CmdTagName.Contains("开心") ||
                e.CmdTagName.Contains("爱心") ||
                e.CmdTagName.Contains("期待")).ToList();

            var angryEmotions = _allEmotions.Where(e => 
                e.CmdTagName.Contains("愤怒") || 
                e.CmdTagName.Contains("皱眉") ||
                e.CmdTagName.Contains("嫉妒")).ToList();

            var sadEmotions = _allEmotions.Where(e => 
                e.CmdTagName.Contains("哭") || 
                e.CmdTagName.Contains("伤心") ||
                e.CmdTagName.Contains("难过")).ToList();

            var surpriseEmotions = _allEmotions.Where(e => 
                e.CmdTagName.Contains("疑惑") || 
                e.CmdTagName.Contains("惊") ||
                e.CmdTagName.Contains("害怕") ||
                e.CmdTagName.Contains("眩晕")).ToList();

            var sleepEmotions = _allEmotions.Where(e => 
                e.CmdTagName.Contains("睡") || 
                e.CmdTagName.Contains("闭眼")).ToList();

            // 获取已分类的表情ID列表
            var categorizedEmotionIds = new HashSet<string>();
            categorizedEmotionIds.UnionWith(happyEmotions.Select(e => e.CmdTag));
            categorizedEmotionIds.UnionWith(angryEmotions.Select(e => e.CmdTag));
            categorizedEmotionIds.UnionWith(sadEmotions.Select(e => e.CmdTag));
            categorizedEmotionIds.UnionWith(surpriseEmotions.Select(e => e.CmdTag));
            categorizedEmotionIds.UnionWith(sleepEmotions.Select(e => e.CmdTag));

            var otherEmotions = _allEmotions.Where(e => !categorizedEmotionIds.Contains(e.CmdTag)).ToList();

            _emotionCategories = new List<EmotionCategory>();

            if (happyEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "开心快乐", Emotions = happyEmotions });
            
            if (angryEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "生气愤怒", Emotions = angryEmotions });
            
            if (sadEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "伤心难过", Emotions = sadEmotions });
            
            if (surpriseEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "惊讶疑惑", Emotions = surpriseEmotions });
            
            if (sleepEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "睡觉休息", Emotions = sleepEmotions });
            
            if (otherEmotions.Any())
                _emotionCategories.Add(new EmotionCategory { Name = "其他表情", Emotions = otherEmotions });
        }

        public List<EmotionCategory> GetEmotionCategories()
        {
            return _emotionCategories;
        }

        public EmotionModel? GetRandomEmotionByCategory(string categoryName)
        {
            var category = _emotionCategories.FirstOrDefault(c => c.Name == categoryName);
            if (category == null || !category.Emotions.Any())
                return null;

            var random = new Random();
            return category.Emotions[random.Next(category.Emotions.Count)];
        }

        public EmotionModel? GetEmotionById(string cmdTag)
        {
            return _allEmotions.FirstOrDefault(e => e.CmdTag == cmdTag);
        }
    }
}
