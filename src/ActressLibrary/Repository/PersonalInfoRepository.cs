using ActressLibrary.Interfaces;
using ActressLibrary.Models;
using LiteDB;

namespace ActressLibrary.Infrastructure.Repository
{
    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        private readonly LiteDatabase _liteDatabase;
        public PersonalInfoRepository(string dbDataPath)
        {
            _liteDatabase = new LiteDatabase(dbDataPath);
        }

        public Task AddAsync(PersonalInfo personalInfo, Stream stream, CancellationToken cancellationToken = default)
        {
            var infos = _liteDatabase.GetCollection<PersonalInfo>();

            infos.Insert(personalInfo);

            var fs = _liteDatabase.GetStorage<string>("dataFiles", "dataChunks");

            if (!fs.Exists($"$/Data/{personalInfo.AvatarName}"))
            {
                if (stream != null)
                {
                    fs.Upload($"$/Data/{personalInfo.AvatarName}", personalInfo.AvatarName, stream);
                }
            }

            return Task.CompletedTask;
        }

        public Task BatchAddAsync(List<PersonalInfo> personalInfos, CancellationToken cancellationToken = default)
        {
            var infos = _liteDatabase.GetCollection<PersonalInfo>();

            if (personalInfos != null && personalInfos.Count > 0)
            {
                var fs = _liteDatabase.GetStorage<string>("dataFiles", "dataChunks");

                foreach (var item in personalInfos)
                {

                    if (!fs.Exists($"$/Data/{item.AvatarName}"))
                    {
                        if (item.AvatarStream != null)
                        {
                            fs.Upload($"$/Data/{item.AvatarName}", item.AvatarName, item.AvatarStream);

                            item.AvatarStream = null;
                        }
                    }

                }
            }

            infos.InsertBulk(personalInfos);



            return Task.CompletedTask;
        }

        public Task<PersonalInfo> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var infos = _liteDatabase.GetCollection<PersonalInfo>();

            var person = infos.FindOne(p => p.Name == id);

            var fs = _liteDatabase.GetStorage<string>("dataFiles", "dataChunks");

            LiteFileInfo<string> file = fs.FindById($"$/Data/{person.AvatarName}");

            Stream stream = new MemoryStream();

            fs.Download(file.Id, stream);

            stream.Seek(0, SeekOrigin.Begin);

            person.AvatarStream = stream;

            return Task.FromResult(person);
        }

        public Task<IReadOnlyCollection<PersonalInfo>> GetListAsync(
            int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _liteDatabase.GetCollection<PersonalInfo>().Query();

            var list = query
                .OrderByDescending(p => p.Name)
                .Skip((pageIndex-1) * pageSize)
                .Limit(pageSize)
                .ToList();


            if (list != null && list.Count > 0)
            {
                var fs = _liteDatabase.GetStorage<string>("dataFiles", "dataChunks");

                foreach (var item in list)
                {
                    if (fs.Exists($"$/Data/{item.AvatarName}"))
                    {
                        LiteFileInfo<string> file = fs.FindById($"$/Data/{item.AvatarName}");

                        var stream = new MemoryStream();

                        fs.Download(file.Id, stream);

                        stream.Seek(0, SeekOrigin.Begin);

                        item.AvatarStream = stream;
                    }
                }
            }

            return Task.FromResult((IReadOnlyCollection<PersonalInfo>)list);
        }
    }
}
