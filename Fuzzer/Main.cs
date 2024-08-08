using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using PeterO;

namespace Fuzzer {
  /// <summary>Not documented yet.</summary>
  internal class MainClass {
    /// <summary>Not documented yet.</summary>
    private static void Usage() {
      var usageString = @"File Fuzzer

By Peter O. -- Public Domain -- http://peteroupc.github.io/Fuzzer

This is a program that generates slightly altered versions
of data files, for testing algorithms that parse file formats.
It's a command line utility.

Usage:
Fuzzer.exe [fuzzer-xml]

fuzzer-xml is an XML file specifying configuration data. If not given, the
  fuzzer will
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
  extension given in _fileExtension_.";
      Console.WriteLine(usageString);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='r'>The parameter <paramref name='r'/> is not
    /// documented yet.</param>
    /// <param name='mean'>The parameter <paramref name='mean'/> is not
    /// documented yet.</param>
    /// <param name='sd'>The parameter <paramref name='sd'/> is not
    /// documented yet.</param>
    /// <returns>A 32-bit signed integer.</returns>
    public static int NextGaussian(Random r, int mean, int sd) {
      double left = Math.Cos((Math.PI + Math.PI) * r.NextDouble());
      double right = Math.Sqrt(-2 * Math.Log(r.NextDouble()));
      return (int)Math.Round(mean + (sd * left * right));
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='startbytes'>The parameter <paramref
    /// name='startbytes'/> is not documented yet.</param>
    /// <param name='rnd'>The parameter <paramref name='rnd'/> is not
    /// documented yet.</param>
    /// <param name='outputFile'>The parameter <paramref
    /// name='outputFile'/> is not documented yet.</param>
    public static void WriteFuzzedFileWithHeader(
      byte[] startbytes,
      Random rnd,
      string outputFile) {
      using (var fs = new FileStream(outputFile, FileMode.Create)) {
        Console.WriteLine(outputFile);
        fs.Write(startbytes, 0, startbytes.Length);
        int count = rnd.Next(8000);
        var array = new byte[count];
        rnd.NextBytes(array);
        fs.Write(array, 0, count);
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='bytes'>The parameter <paramref name='bytes'/> is not
    /// documented yet.</param>
    /// <param name='rnd'>The parameter <paramref name='rnd'/> is not
    /// documented yet.</param>
    /// <param name='outputFile'>The parameter <paramref
    /// name='outputFile'/> is not documented yet.</param>
    /// <param name='fuzzStart'>The parameter <paramref name='fuzzStart'/>
    /// is not documented yet.</param>
    /// <param name='frequency'>The parameter <paramref name='frequency'/>
    /// is not documented yet.</param>
    public static void WriteVariation(
      byte[] bytes,
      Random rnd,
      string outputFile,
      int fuzzStart,
      int frequency) {
      int mode = rnd.Next(3);
      int realSize = bytes.Length;
      if (frequency < 1) {
        frequency = 1;
      }
      if (mode == 1) {
        realSize -= rnd.Next(bytes.Length) + 1;
      } else if (mode == 2) {
        realSize += rnd.Next(bytes.Length / 2) + 1;
      }
      byte[] clonedBytes = (byte[])bytes.Clone();
      int bytesLength = Math.Min(realSize, bytes.Length);
      using (var fs = new FileStream(outputFile, FileMode.Create)) {
        Console.WriteLine(outputFile);
        for (int i = Math.Max(0, fuzzStart); i < bytesLength; ++i) {
          if (rnd.Next(frequency) == 0) {
            clonedBytes[i] = (byte)rnd.Next(256);
          }
        }
        fs.Write(clonedBytes, 0, bytesLength);
        if (realSize > bytes.Length) {
          var randomBytes = new byte[realSize - bytes.Length];
          rnd.NextBytes(randomBytes);
          fs.Write(randomBytes, 0, randomBytes.Length);
        }
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='path'>The parameter <paramref name='path'/> is not
    /// documented yet.</param>
    /// <param name='pattern'>The parameter <paramref name='pattern'/> is
    /// not documented yet.</param>
    /// <returns>An IList(byte[]) object.</returns>
    public static IList<byte[]> ReadAllBytesInDir(string path,
      string pattern) {
      IList<byte[]> list = new List<byte[]>();
      if (!Directory.Exists(path)) {
        return list;
      }
      foreach (string file in Directory.GetFiles(path, pattern)) {
        list.Add(File.ReadAllBytes(file));
      }
      return list;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='args'>The parameter <paramref name='args'/> is not
    /// documented yet.</param>
    /// <returns>A 32-bit signed integer.</returns>
    public static int Main(string[] args) {
      try {
        var random = new Random();
        string configFile = (args.Length > 0) ? args[0] : "fuzzer.xml";
        if (!File.Exists(configFile)) {
          Usage();
          return 1;
        }
        XmlConfigFile config = XmlConfigFile.Create(
          configFile,
          "fuzzer");
        string path = config.GetValue("outputPath");
        byte[] startbytes = config.GetValueOrEmptyAsByteArray("startBytes");
        IList<byte[]> fuzzing = ReadAllBytesInDir(
            config.GetValue("validFilesPath"),
            config.GetValue("validFilesPattern", "*.*"));
        string extension = config.GetValue("fileExtension", "dat");
        Directory.CreateDirectory(path);
        int fuzzOffset = config.GetValueAsInt32("fuzzOffset", 0);
        int iterations = config.GetValueAsInt32("iterations", 1000);
        int freqMean = config.GetValueAsInt32("frequencyMean", 32);
        int freqStdDev = config.GetValueAsInt32("frequencyStdDev", 16);
        for (int i = 0; i < Math.Max(0, iterations); ++i) {
          string number = i.ToString(
            "d5",
            System.Globalization.CultureInfo.InvariantCulture);
          string name = Path.Combine(
            path,
            number + "." + extension);
          int index = random.Next(fuzzing.Count + 1);
          if (index == fuzzing.Count) {
            WriteFuzzedFileWithHeader(startbytes, random, name);
          } else {
            WriteVariation(
              fuzzing[index],
              random,
              name,
              fuzzOffset,
              NextGaussian(random, freqMean, freqStdDev));
          }
        }
        return 0;
      } catch (XmlConfigException e) {
        Console.Write(e.Message);
        return 1;
      }
    }
  }
}
