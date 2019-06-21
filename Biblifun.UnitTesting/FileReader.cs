using System;
using System.IO;
using System.Reflection;

namespace Biblifun.UnitTesting
{
    /// <summary>
    /// Helper class for unit tests to load test file content.
    /// </summary>
    public class FileReader
    {
        readonly string _baseFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public string BaseFolder => _baseFolder;

        /// <summary>
        /// Optional folder if you want to reuse a FileReader for files 
        /// from a single folder. If provided, will be appened to the base 
        /// folder before the specified file path.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Returns the file content from the specified file. Assumes the file will be
        /// found at {BaseFolder}\{Folder}\{FileName}.
        /// 
        /// - BaseFolder is the bin folder of the executing assembly.
        /// - Folder is optional and will be included if supplied.
        /// - FileName may be either the file name or a relative path from 
        ///   the parent folder, such as "MyTestData\testfile.txt"
        ///   
        ///   Note: Passing either null or an empty string will result in that value being returned.
        /// </summary>
        public string ReadFile(string fileName)
        {
            // all
            if (fileName == null)
                return null;

            if (string.IsNullOrWhiteSpace(fileName))
                return "";

            var filePath = Path.Combine(_baseFolder, Folder ?? "", fileName);

            return File.ReadAllText(filePath);
        }
    }
}
