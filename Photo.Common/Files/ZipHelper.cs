using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Photo.Model.Query;
using System.IO;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace Photo.Common.Files
{
    public class ZipHelper
    {
        /// <summary>
        /// 生成压缩文件
        /// </summary>
        /// <param name="zipPath">保存路径</param>
        /// <param name="zipTopDirectoryPath">源文件的上級目錄</param>
        /// <param name="intZipLevel">压缩包的等级</param>
        /// <param name="password">压缩包的密码，为空则没有</param>
        /// <param name="filesOrDirectoriesPaths"></param>
        /// <returns></returns>
        public static bool BaleZip(BaleZipQuery query)
        {
            try
            {
                var allFilesPath = new List<string>();
                if (query.FilesOrDirectoriesPaths.Count > 0)
                {
                    query.FilesOrDirectoriesPaths.ForEach(p =>
                    {
                        var filePath = HttpContext.Current.Server.MapPath(p);
                        if (File.Exists(filePath))  //文件是否存在
                        {
                            allFilesPath.Add(filePath);
                        }
                        else if (Directory.Exists(filePath))  //文件夹是否存在
                        {
                            GetDirectoryFiles(filePath, allFilesPath);
                        }
                    });
                }
                if (allFilesPath.Count > 0)
                {
                    if (File.Exists(query.ZipPath))  //该压缩包以存在时，先删除
                    {
                        File.Delete(query.ZipPath);
                    }
                    ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(query.ZipPath));
                    zipOutputStream.SetLevel(query.ZipLevel);
                    zipOutputStream.Password = query.Password;
                    foreach (var file in allFilesPath)
                    {
                        if (file.Substring(file.Length - 1) == "")  //文件夹
                        {
                            var fileName = file.Replace(query.ZipTopDirectoryPath, "");
                            if (fileName.StartsWith(""))
                            {
                                fileName = fileName.Substring(1);
                            }
                            var entry = new ZipEntry(fileName);
                            entry.DateTime = DateTime.Now;
                            zipOutputStream.PutNextEntry(entry);
                        }
                        else //文件
                        {
                            var fs = File.OpenRead(file);
                            var buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            var fileName = file.Replace(query.ZipTopDirectoryPath, "");
                            if (fileName.StartsWith(""))
                            {
                                fileName = fileName.Substring(0);
                            }
                            var entry = new ZipEntry(fileName);
                            entry.DateTime = DateTime.Now;
                            zipOutputStream.PutNextEntry(entry);
                            zipOutputStream.Write(buffer, 0, buffer.Length);

                            fs.Close();
                            fs.Dispose();
                        }
                    }

                    zipOutputStream.Finish();
                    zipOutputStream.Close();

                    return true;
                }
                return false;  //未找到可用的文件夹
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="parentDirectoryPath">文件夹路径</param>
        /// <param name="AllFilesPath">所以文件路徑</param>
        public static void GetDirectoryFiles(string parentDirectoryPath, List<string> filesPath)
        {
            string[] files = Directory.GetFiles(parentDirectoryPath);
            for (int i = 0; i < files.Length; i++)
            {
                filesPath.Add(files[i]);
            }
            string[] directorys = Directory.GetDirectories(parentDirectoryPath);
            for (int i = 0; i < directorys.Length; i++)
            {
                GetDirectoryFiles(directorys[i], filesPath);
            }
            if (files.Length == 0 && directorys.Length == 0) //空文件夹
            {
                filesPath.Add(parentDirectoryPath);
            }
        }
    }
}
