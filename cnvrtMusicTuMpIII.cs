﻿using System;
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
        public const string MP3_FILE_EXTENSION = ".mp3";

        public static void CrawlFoldersAndConvertNonMp3sToMp3(string startingFolderPath, bool crawlRecursively = true)
        {
            List<string> listFiles = getFileList(startingFolderPath);

            foreach (string filePath in listFiles)
            {
                if (File.Exists(filePath))
                {
                    if (isFolder(filePath))
                    {
                        if (crawlRecursively)
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

        public static string getParentFolderPathForFile(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string parentFolderPath = filePath.Replace(fileName, "");
            return parentFolderPath;
        }

        public static void HandleNonMp3File(string filePath)
        {
            if (isExistsMp3VersionOfFileInSameFolder(filePath))
            {
                string mp3FileName =
                    getNameForMp3VersionOfNonMp3(filePath)
                ;
                if (isMp3OfEqualQualityOrBetter(
                    filePath
                    , mp3FileName
                ))
                {
                    HandleNonMp3WhereHigherQualityMp3Exists(filePath, mp3FileName);
                }
                else
                {
                    HandleNonMp3WhereLowerQualityMp3Exists(filePath, mp3FileName);
                }
            }
        }

        public static void HandleNonMp3WhereHigherQualityMp3Exists(string filePath, string mp3FileName)
        {
            File.Move(
                filePath
                , mp3FileName
                    + "/" + BACKUP_FOLDER
                    + "/" + Path.GetFileName(filePath)
            );
        }


        public static void HandleNonMp3WhereLowerQualityMp3Exists(string filePath, string mp3FileName)
        {
            File.Delete(
                        mp3FileName
                    );
            convertToMp3(
                filePath
                , mp3FileName
            );
            File.Move(
                filePath
                , getParentFolderPathForFile(filePath)
                    + "/" + BACKUP_FOLDER
                    + "/" + Path.GetFileName(filePath)
            );
        }

        public static List<string> getFileList(string folder) { 
            return new List<string>(); 
        }

        public static bool isFolder(string filePath) { 
            return Directory.Exists(filePath);  
        }

        public List<string> getListOfMediaFileExtensions()
        {
            List<string> list = new List<string>();



            return list;
        }

        public static bool isMediaFile(string filePath) { 
            return true; 
        }

        public static bool isMp3File(string filePath) { 
            return Path.GetExtension(filePath).ToLower() == MP3_FILE_EXTENSION; 
        }

        public static string getNameForMp3VersionOfNonMp3(string filePath) { 
            return StripExtentionFromFileName(filePath) + MP3_FILE_EXTENSION; 
        }

        public static string StripExtentionFromFileName(string filePath) { 
            return filePath.Replace(Path.GetExtension(filePath), String.Empty ); 
        }

        public static bool isExistsMp3VersionOfFileInSameFolder(string filePath) { 
            return File.Exists(getNameForMp3VersionOfNonMp3(filePath)); 
        }

        public static bool isMp3OfEqualQualityOrBetter(
            string filePath
            , string filePathMp3Version
        ) {
            return true; 
        }
        public static void convertToMp3(string filePath, string mp3FileName) { }


    }
}