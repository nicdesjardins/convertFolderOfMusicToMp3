# Convert Folder Of Music To Mp3
<pre>An application that crawls through a folder structure 
(by default NOT recursively) and creates an mp3 version of any
media file that is not mp3.

Keeps the directory structure clean by moving the non-mp3
file to a sub-directory called .backup after conversion

If an mp3 already exists, checks whether the quality of
the mp3 is equal or better than the non-mp3

  If it's not, then delete that mp3 and 
    convert the non-mp3 anew.
  
  If it is, then move the non-mp3 to .backup

If no mp3 already exists, convert to mp3 and move
file to .backup.</pre>

# Questions / logic that is not yet handled
<pre>Q: What if more than one non-mp3 versions exist?
  Which to pick? 
    If more than one non-mp3 version exists
      What do to?
        Ask the user to confirm?
        Try to figure out the highest quality 
          and pick that one to convert to mp3</pre>

# Pseudocode / flow of actions
<pre><code>
listFiles = getFileList(folder)
foreach(file in listFiles) // where file might actually be a folder
     if(fileExists(file)) // this is so that if the file is moved
                          // during operation, we don't assume it's 
                          // still there
         if(isFolder(file))
             if(crawlRecursively) Crawl(file, crawlRecursively)
         else 
             if(isMediaFile(file))
                 if(!isMp3File(file))
                     if(isExistsMp3VersionOfFileInSameFolder(file))
                         if(isMp3OfEqualQualityOrBetter(file, mp3Version)
                             move(file, BACKUP_FOLDER + basename(file), createFolderIfNonExistent = true)
                         else
                             delete(mp3File)
                             convertToMp3(file, mp3FileName)
                             move(file, BACKUP_FOLDER + basename(file), createFolderIfNonExistent = true)
                 endif // (!isMp3File(file))
             endif (isMediaFile(file))
         endif // (isMediaFile(file))
     endif // (fileExists(file))
endforeach // file in listFiles
</code></pre>