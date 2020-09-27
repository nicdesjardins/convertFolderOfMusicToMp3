using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace convertmusic2mp3
{
    public class cnvrtMscTuMpIII
    {
        // put EXE in folder where the art is
        // iterate folders

        public const string BACKUP_FOLDER = ".backup";

        public static void CrawlFoldersAndConvertNonMp3sToMp3(string startingFolderPath, bool crawlRecursively = true)
        {
            // fileList = getFileList(folder)
            // foreach(file in listFiles) // where file might actually be a folder
            //      if(fileExists(file)) // this is so that if the file is moved
            //                           // during operation, we don't assume it's 
            //                           // still there
            //          if(isFolder(file))
            //              if(crawlRecursively) Crawl(file, crawlRecursively)
            //          else 
            //              if(isMediaFile(file))
            //                  if(!isMp3File(file))
            //                      if(isExistsMp3VersionOfFileInSameFolder(file))
            //                          if(isMp3OfEqualQualityOrBetter(file, mp3Version)
            //                              move(file, BACKUP_FOLDER + basename(file), createFolderIfNonExistent = true)
            //                          else
            //                              delete(mp3File)
            //                              convertToMp3(file, mp3FileName)
            //                              move(file, BACKUP_FOLDER + basename(file), createFolderIfNonExistent = true)
            //                  endif // (!isMp3File(file))
            //              endif (isMediaFile(file))
            //          endif // (isMediaFile(file))
            //      endif // (fileExists(file))

            List<string> listFiles = getFileList(startingFolderPath);

            foreach (string filePath in listFiles)
            {
                if (fileExists(filePath))
                {
                    if (isFolder(filePath))
                    {
                        if (crawlRecursively)
                        {
                            CrawlFoldersAndConvertNonMp3sToMp3(filePath, crawlRecursively);
                        }
                    }
                    else
                    {// (!isFolder)
                        if (isMediaFile(filePath))
                        {
                            if (!isMediaFile(filePath))
                            {
                                if (!isMp3File(filePath))
                                {
                                    if (isExistsMp3VersionOfFileInSameFolder(filePath))
                                    {
                                        if(isMp3OfEqualQualityOrBetter(filePath, getNameForMp3VersionOfNonMp3(filePath)))
                                        {
                                            File.Move(filePath, getParentFolderPathForFile(filePath) + "/" + BACKUP_FOLDER + "/" + Path.GetFileName(filePath));
                                        }
                                        else
                                        { // (!isMp3OfEqualQualityOrBetter(filePath, getNameForMp3VersionOfNonMp3(filePath)))

            //                              delete(mp3File)
            //                              convertToMp3(file, mp3FileName)
            //                              move(file, BACKUP_FOLDER + basename(file), createFolderIfNonExistent = true)
                                        }//endif: isMp3OfEqualQualityOrBetter(filePath, getNameForMp3VersionOfNonMp3(filePath))
                                    }//endif: isExistsMp3VersionOfFileInSameFolder(filePath)
                                }//endif: !isMp3File(filePath)
                            }//endif: !isMediaFile(filePath)
                        }//endif: isMediaFile(filePath)
                    } //endif: isFolder(filePath)
                }//endif: !fileExists(filePath)
            }//end: foreach(string filePath in listFiles)

                
        }

        public static string getParentFolderPathForFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string parentFolderPath = filePath.Replace(fileName, "");
            return parentFolderPath;
        }

        public static List<string> getFileList(string folder) { return new List<string>(); }
        public static bool fileExists(string filePath) { return true; }
        public static bool isFolder(string filePath) { return true;  }
        public static bool isMediaFile(string filePath) { return true; }
        public static bool isMp3File(string filePath) { return true; }

        public static string getNameForMp3VersionOfNonMp3(string filePath) { return StripExtentionFromFileName(filePath) + ".mp3"; }
        public static string StripExtentionFromFileName(string filePath) { return filePath.Replace(Path.GetExtension(filePath), ""); }
        public static bool isExistsMp3VersionOfFileInSameFolder(string filePath) { return fileExists(getNameForMp3VersionOfNonMp3(filePath)); }
        public static bool isExistsNonMp3VersionOfFileInSameFolder(string filePath) { return true; }
        public static bool isMp3OfEqualQualityOrBetter(string filePath, string filePathMp3Version) { return true; }
        public static void move(string filePath, string newFilePath) { }
        public static void delete(string filePath) { }
        public static void convertToMp3(string filePath) { }

        public static string Mp3VersionOfFileInSameFolder(string filePath) {return "";}


    }
}
