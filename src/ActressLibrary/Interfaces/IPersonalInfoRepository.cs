using ActressLibrary.Models;

namespace ActressLibrary.Interfaces;

public interface IPersonalInfoRepository
{
    Task BatchAddAsync(List<PersonalInfo> personalInfos, CancellationToken cancellationToken = default);
    Task AddAsync(PersonalInfo personalInfo, Stream stream, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PersonalInfo>> GetListAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<PersonalInfo> GetAsync(string id, CancellationToken cancellationToken = default);
}
