#File Fuzzer

By Peter O. -- Public Domain -- [http://upokecenter.com](http://upokecenter.com)

If you like this software, consider donating to me at this link: [http://upokecenter.dreamhosters.com/articles/donate-now-2/](http://upokecenter.dreamhosters.com/articles/donate-now-2/)

----

This is a program that generates slightly altered versions
of data files, for testing algorithms that parse file formats.
It's a command line utility.

Usage:
Fuzzer.exe [fuzzer-xml]

fuzzer-xml is an XML file specifying configuration data. If not given, the fuzzer will
read the file fuzzer.xml. It has the following XML format:

    <fuzzer>
     <outputPath></outputPath>
     <validFilesPath></validFilesPath>
     <validFilesPattern></validFilesPattern>
     <startBytes></startBytes>
     <fileExtension></fileExtension>
     <fuzzOffset></fuzzOffset>
     <frequencyMean></frequencyMean>
     <frequencyStdDev></frequencyStdDev>
     <iterations></iterations>
    </fuzzer>

* outputPath - Path of the directory to store the generated file.
* validFilesPath - Path of the directory containing files in a particular file
  format. The fuzzer will randomly manipulate these files to generate its
  test data.
* startBytes - A hexadecimal string specifying the bytes to write at the
  beginning of some of the generated files. Such files will contain random
  data following the header.
* fileExtension - File extension of the generated files.
* fuzzOffset - Byte offset of the point where the fuzzer will modify the
  bytes of the files located in _validFilesPath_.
* frequencyMean - Mean of the frequency of changes, in bytes.
* frequencyStdDev - Standard deviation of the frequency of changes, in bytes.
* iterations - Number of files to generate. Each file will be named XXXXX.YYY
  where XXXXX is a sequence of five or more digits and YYY is the file
  extension given in _fileExtension_.
