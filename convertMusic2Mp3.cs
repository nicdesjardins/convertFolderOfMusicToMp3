using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.nicdesjardins.convertmusic2mp3
{
    public class convertMusic2Mp3
    {
        public const string BACKUP_FOLDER_NAME = ".backup";
        public const string MP3_FILE_EXTENSION = ".mp3";
        public const string RESOURCES_FOLDER_NAME = "resources";
        public const string FFMPEG_MEDIA_EXTENSIONS_FILE = "ffmpegMediaFileExtensions.txt";

        public static void CrawlFoldersAndConvertNonMp3sToMp3(string startingFolderPath, bool crawlRecursively = true)
        {
            List<string> listFiles = getFileAndFolderList(startingFolderPath);

            foreach (string filePath in listFiles)
            {
                if (File.Exists(filePath) || Directory.Exists(filePath))
                {
                    if (isFolder(filePath))
                    {
                        if (Path.GetFileName(filePath) != BACKUP_FOLDER_NAME && crawlRecursively)
                        {
                            CrawlFoldersAndConvertNonMp3sToMp3(
                                filePath
                                , crawlRecursively
                            );
                        }
                    }
                    else
                    {
                        if (isMediaFile(filePath))
                        {
                            if (!isMp3File(filePath))
                            {
                                HandleNonMp3File(filePath);
                            }
                        }
                    }
                }
            }
        }

        public static void HandleNonMp3File(string filePath)
        {
            string mp3FileName = getNameForMp3VersionOfNonMp3(filePath);
            if (File.Exists(mp3FileName))
            {
                if (isMp3OfEqualQualityOrBetter(filePath, mp3FileName))
                {
                    HandleNonMp3WhereHigherQualityMp3Exists(filePath, mp3FileName);
                }
                else
                {
                    HandleNonMp3WhereLowerQualityMp3Exists(filePath, mp3FileName);
                }
            } else
            {
                ConvertToMp3AndMoveNonMp3ToBackup(filePath, mp3FileName);
            }
        }

        public static void HandleNonMp3WhereHigherQualityMp3Exists(string filePath, string mp3FileName)
        {
            File.Move(
                filePath
                , mp3FileName
                    + "/" + BACKUP_FOLDER_NAME
                    + "/" + Path.GetFileName(filePath)
            );
        }

        public static void ConvertToMp3AndMoveNonMp3ToBackup(string filePath, string mp3FileName)
        {
            convertToMp3(filePath, mp3FileName);
            ConfirmOrCreateBackupFolder(filePath);
            File.Move(
                filePath
                , getParentFolderPathForFile(filePath)
                    + "/" + BACKUP_FOLDER_NAME
                    + "/" + Path.GetFileName(filePath)
            );
        }

        public static void ConfirmOrCreateBackupFolder(string filePath)
        {
            string folderPath = getParentFolderPathForFile(filePath);
            string backupFolderPath = folderPath + "/" + BACKUP_FOLDER_NAME;
            if (!Directory.Exists(backupFolderPath))
                Directory.CreateDirectory(backupFolderPath);
            //
        }

        public static void HandleNonMp3WhereLowerQualityMp3Exists(string filePath, string mp3FileName)
        {
            File.Delete(mp3FileName);
            ConvertToMp3AndMoveNonMp3ToBackup(filePath, mp3FileName);
        }

        public static string getParentFolderPathForFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string parentFolderPath = filePath.Replace(fileName, "");
            return parentFolderPath;
        }

        public static List<string> getFileAndFolderList(string folderPath) {
            
            // get files (doesn't include folders)
            List<string> filesAndFolders = 
                Directory.GetFiles(
                    folderPath, "*", SearchOption.TopDirectoryOnly
                ).ToList()
            ;

            // add list of folders
            filesAndFolders.AddRange(
                Directory.GetDirectories(
                    folderPath, "*", SearchOption.TopDirectoryOnly).ToList()
                )
            ;

            return filesAndFolders;
        }

        public static bool isFolder(string filePath) {
            return Directory.Exists(filePath);
        }

        private static string __binaryPath = null;
        private static string binaryPath
        {
            get
            {
                if(__binaryPath == null)
                {
                    __binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                }
                return __binaryPath;
            }
        }

        private static string binaryFolderPath
        {
            get
            {
                return getParentFolderPathForFile(binaryPath);
            }
        }

        private static string resourcesFolderPath
        {
            get
            {
                return binaryFolderPath + "\\" + RESOURCES_FOLDER_NAME;
            }
        }

        private static string ffmpegMediaFileExtensionsFilePath
        {
            get
            {
                return resourcesFolderPath + "\\" + FFMPEG_MEDIA_EXTENSIONS_FILE;
            }
        }

        public static List<string> listOfMediaFileExtensions {
            get {
                return new List<string>
                {
                    "wma", "m4a"
                };
                //return ConvertFileLinesToList(ffmpegMediaFileExtensionsFilePath);
            }
        }

        public static List<string> ConvertFileLinesToList(string filePath, bool skipEmptyLines = true)
        {
            List<string> list = new List<string>();

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (!skipEmptyLines || line.Trim().Length > 0)
                    list.Add(line.Trim());
            }
            file.Close();

            return list;
        }

        public static bool isMediaFile(string filePath)
        {
            string baseFileName = Path.GetFileName(filePath);
            string fileExtension = Path.GetExtension(baseFileName).Replace(".", "");
            bool isMediaFile = listOfMediaFileExtensions.Contains(fileExtension);
            return isMediaFile;
        }

        public static bool isMp3File(string filePath)
        {
            return Path.GetExtension(filePath).ToLower() == MP3_FILE_EXTENSION;
        }

        public static string getNameForMp3VersionOfNonMp3(string filePath)
        {
            return StripExtentionFromFileName(filePath) + MP3_FILE_EXTENSION;
        }

        public static string StripExtentionFromFileName(string filePath)
        {
            return filePath.Replace(Path.GetExtension(filePath), String.Empty);
        }

        public static bool isExistsMp3VersionOfFileInSameFolder(string filePath)
        {
            return File.Exists(getNameForMp3VersionOfNonMp3(filePath));
        }

        public static bool isMp3OfEqualQualityOrBetter(
            string filePath
            , string filePathMp3Version
        )
        {
            return true;
        }
        public static void convertToMp3(string filePath, string mp3FileName)
        {
            Console.WriteLine(string.Format(@"Would convert {0} to {1}.", filePath, mp3FileName));
        }


    }
}