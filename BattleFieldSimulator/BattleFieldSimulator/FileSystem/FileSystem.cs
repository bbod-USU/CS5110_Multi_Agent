using System.Collections.Generic;
using System.IO;

namespace BattleFieldSimulator.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public FileSystem()
        {
            Directory.CreateDirectory(FileSystemConstants.LogDirectory);
        }

        /// <inheritdoc/>
        public void WriteTextToFile(string filePath, string message)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!DirectoryExists(directoryPath))
                CreateDirectory(directoryPath);
            File.WriteAllText(filePath, message);
        }

        /// <inheritdoc/>
        public void AppendTextToFile(string filePath, string message)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!DirectoryExists(directoryPath))
                CreateDirectory(directoryPath);
            File.AppendAllText(filePath, message);
        }

        /// <inheritdoc/>
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <inheritdoc/>
        public void RenameFile(string originalFilePath, string newName)
        {
            File.Move(originalFilePath, FormPath(Path.GetDirectoryName(originalFilePath), newName));
        }

        /// <inheritdoc/>
        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <inheritdoc/>
        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        /// <inheritdoc/>
        public void CreateFile(string filePath, MemoryStream memoryStream)
        {
            var file = new FileStream($"{filePath}temp",
                FileMode.Create, FileAccess.Write);
            memoryStream.WriteTo(file);
            file.Close();
            File.Move($"{filePath}temp",
                filePath);
        }
        
        /// <inheritdoc/>
        public void CopyDirectoryContents(string sourceDirectoryPath, string destinationDirectoryPath)
        {
            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);

            if (!sourceDirectory.Exists)
                throw new DirectoryNotFoundException(
                    $"Source directory does not exist or could not be found: {sourceDirectoryPath}");

            var sourceSubdirectories = sourceDirectory.GetDirectories();

            if (!Directory.Exists(destinationDirectoryPath))
                Directory.CreateDirectory(destinationDirectoryPath);

            var sourceDirectoryFiles = sourceDirectory.GetFiles();

            foreach (var file in sourceDirectoryFiles)
            {
                var tempPath = FormPath(destinationDirectoryPath, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (var subdirectory in sourceSubdirectories)
            {
                var tempPath = FormPath(destinationDirectoryPath, subdirectory.Name);
                CopyDirectoryContents(subdirectory.FullName, tempPath);
            }
        }

        /// <inheritdoc/>
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetFiles(string directoryPath, string searchPattern, SearchOption searchOption) =>
            DirectoryExists(directoryPath)
                ? Directory.GetFiles(directoryPath, searchPattern, searchOption)
                : new string[0];

        /// <inheritdoc/>
        public byte[] ReadAllBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        /// <inheritdoc/>
        public void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        /// <inheritdoc/>
        public void DeleteDirectoryContents(string directoryPath)
        {
            if (!DirectoryExists(directoryPath))
                return;
            var directory = new DirectoryInfo(directoryPath);
            foreach (var file in directory.GetFiles()) file.Delete();
            foreach (var dir in directory.GetDirectories()) dir.Delete(true);
        }

        /// <inheritdoc/>
        public string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <inheritdoc/>
        public string GetDirectoryName(string directoryPath)
        {
            return Path.GetDirectoryName(directoryPath);
        }

        /// <inheritdoc/>
        public void CopyFile(string originalFilePath, string targetFilePath) =>
            File.Copy(originalFilePath, targetFilePath, true);

        /// <inheritdoc/>
        public string FormPath(params string[] pathParts) => Path.Combine(pathParts);

        public string GetUserTempDirectory() => Path.GetTempPath();
    }
}