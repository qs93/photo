using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Model.Query
{
    public class BaleZipQuery
    {
        public string ZipPath { get; set; }

        public string ZipTopDirectoryPath { get; set; }

        /// <summary>
        /// 压缩包等级
        /// </summary>
        public int ZipLevel { get; set; } = 6;

        /// <summary>
        /// 压缩包密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 文件或文件夹集合
        /// </summary>
        public List<string> FilesOrDirectoriesPaths { get; set; }
    }
}
