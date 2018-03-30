using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Model.Query
{
    public class BaleZipQuery
    {
        /// <summary>
        /// 生成zip文件的路径>虚拟路径
        /// </summary>
        public string ZipPath { get; set; }

        /// <summary>
        /// 原文件的上级目录>虚拟路径
        /// </summary>
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
        /// 文件或文件夹集合>虚拟路径
        /// </summary>
        public List<string> FilesOrDirectoriesPaths { get; set; }
    }
}
