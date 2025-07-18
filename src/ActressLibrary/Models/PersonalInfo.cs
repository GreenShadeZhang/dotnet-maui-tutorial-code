using System.Collections.Generic;
using System.IO;

namespace ActressLibrary.Models;

public class PersonalInfo
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 头像名称
    /// </summary>
    public string AvatarName { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }
    /// <summary>
    /// 兴趣爱好
    /// </summary>
    public string Hobbies { get; set; }
    /// <summary>
    /// 标签
    /// </summary>
    public List<string> Tags { get; set; }
    /// <summary>
    /// 头像流
    /// </summary>
    public Stream AvatarStream { get; set; }
    /// <summary>
    /// 头像字节数组
    /// </summary>
    public byte[] AvatarBytes { get; set; }

    /// <summary>
    /// 从流中获取字节数组
    /// </summary>
    /// <returns>字节数组</returns>
    public byte[] GetAvatarBytes()
    {
        // 优先返回已缓存的字节数组
        if (AvatarBytes != null && AvatarBytes.Length > 0)
            return AvatarBytes;

        // 如果没有缓存，从流中读取
        if (AvatarStream != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                AvatarStream.Position = 0;
                AvatarStream.CopyTo(memoryStream);
                AvatarBytes = memoryStream.ToArray();
                return AvatarBytes;
            }
        }

        return null;
    }
}

public class PersonalInfoModel
{
    public List<PersonalInfo> Data { get; set; }
}
