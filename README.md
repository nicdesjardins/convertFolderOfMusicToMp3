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

I've been thinking about below in different ways, because I didn't want to have to run ffmpeg from within a c# app, and I had just been listening to a talk about using pipes to connect apps that do one thing. So here goes:


(1)Crawl | (2)Decide | (3)Convert | (4)Cleanup

(1) Crawl what? (a folder structure, recursively)

(2)Decide what? 
  (do we have a non-mp3?
      Yes:
          (is there a >= quality version mp3?)
              if no, re-convert; 
              if yes, cleanup non-mp3
          )
      No: nothing to do, duh!
  )
  So basically, build a list of shit to convert, and a list of shit to clean up
  Then, pipe that list into our convert

(3)Convert what? (non-mp3 audio files)
Convert to what? (to mp3)
  Then, pipe our list to our cleanup crew who will cleanup appropriately

(4)How to cleanup? (move to .backup)

Put concisely:
 Crawl for conversion (pass a list of file types: e.g. wma, m4a, ogg, etc)
        >>> [find, right?]
    {Pipe}
 Decision-maker (exists mp3 < quality ? convert to mp3 [overwrite] : do nothing)
        >>> [Is there a way to make this decision in-line?]
    {Pipe}
 Converter 
        >>> [ffmpeg: did some research and should be able to pipe in a list of files to convert]
    {Pipe?}
 Crawl for cleanup (i.e. the file that were converted)
        >>> [find, again!]
    {Pipe} 
 Cleanup (modes: --moveToTrash=<TRASH FOLDER> OR --delete)
        >>> [mv OR rm -rf]
            +++ The trick is... how do I move into a .trash folder 
                in the folder that the file was, not just all in 
                one .trash folder?

... This is my previous spec where I'm thinking I'll do it all in one app, which I may still do
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