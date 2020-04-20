using System.Collections.Generic;
using System.IO;

namespace BattleFieldSimulator.FileSystem
{
    public interface IFileSystem
    {
        #region File Operations

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        void WriteTextToFile(string filePath, string message);

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        void AppendTextToFile(string filePath, string message);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Deletes the file at the given path. Nothing happens if the target file doesn't exist.
        /// </summary>
        /// <param name="filePath"></param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Creates a new file at <see cref="filePath"/> and writes the <see cref="MemoryStream"/> to that file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="memoryStream"></param>
        void CreateFile(string filePath, MemoryStream memoryStream);

        /// <summary>
        /// Reads file at <see cref="filePath"/> and returns all contents as a string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string ReadAllText(string filePath);

        /// <summary>
        /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        IEnumerable<string> GetFiles(string directoryPath, string searchPattern, SearchOption searchOption);

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        byte[] ReadAllBytes(string filePath);

        /// <summary>
        /// Deletes all files in the specified directory
        /// </summary>
        /// <param name="directoryPath"></param>
        void DeleteDirectoryContents(string directoryPath);

        /// <summary>
        /// Copies from the given original path to the given target path, overwriting the file in the target location, if necessary.
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="targetFilePath"></param>
        void CopyFile(string originalFilePath, string targetFilePath);

        /// <summary>
        /// Renames the file at <see cref="originalFilePath"/> to <see cref="newName"/>
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="newName"></param>
        void RenameFile(string originalFilePath, string newName);

        #endregion File Operations

        #region Directory Operations

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        bool DirectoryExists(string directoryPath);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist.
        /// </summary>
        /// <param name="directoryPath"></param>
        void CreateDirectory(string directoryPath);
        
        /// <summary>
        /// Copies a directory at <see cref="sourceFilePath"/> and all its contents to <see cref="destinationFilePath"/>
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        void CopyDirectoryContents(string sourceFilePath, string destinationFilePath);

        #endregion Directory Operations

        #region Path Operations

        /// <summary>
        /// Returns the file name and extension of the specified path string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetFileName(string filePath);

        /// <summary>
        /// Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        string GetDirectoryName(string directoryPath);

        /// <summary>
        /// Combines an array of strings into a path.
        /// </summary>
        /// <param name="pathParts"></param>
        /// <returns></returns>
        string FormPath(params string[] pathParts);

        string GetUserTempDirectory();

        #endregion Path Operations

    }
}