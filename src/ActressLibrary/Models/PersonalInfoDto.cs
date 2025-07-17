using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActressLibrary.Models
{
    public class PersonalInfoDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
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
        /// 头像
        /// </summary>
        public byte[] AvatarBitmap { get; set; }

        public ImageSource ImageSource { get; set; }
    }
}
