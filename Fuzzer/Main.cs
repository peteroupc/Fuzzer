using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using PeterO;

namespace Fuzzer {
  class MainClass {
    public static int NextGaussian(Random r, int mean, int sd) {
             double left = Math.Cos((Math.PI + Math.PI)*r.NextDouble());
             double right = Math.Sqrt(-2*Math.Log(r.NextDouble()));
            return (int)Math.Round(mean + sd*left*right);
    }

    public static void WriteFuzzedFileWithHeader(byte[] startbytes, Random rnd,
      string outputFile) {
      using(var fs = new FileStream(outputFile, FileMode.Create)) {
        Console.WriteLine(outputFile);
        fs.Write(startbytes, 0, startbytes.Length);
        int count = rnd.Next(8000);
        var array = new byte[count];
        rnd.NextBytes(array);
        fs.Write(array, 0, count);
      }
    }
    public static void WriteVariation(
    byte[] bytes,
    Random rnd,
    string outputFile,
    int fuzzStart,
    int frequency) {
      int mode = rnd.Next(3);
      int realSize = bytes.Length;
      if (frequency< 1) {
 frequency = 1;
}
      if (mode == 1) {
 realSize-=rnd.Next(bytes.Length)+1;
  } else if (mode == 2) {
 realSize+=rnd.Next(bytes.Length/2)+1;
}
      byte[] clonedBytes=(byte[])bytes.Clone();
      int bytesLength = Math.Min(realSize, bytes.Length);
      using(var fs = new FileStream(outputFile, FileMode.Create)) {
        Console.WriteLine(outputFile);
        for (int i = Math.Max(0, fuzzStart); i < bytesLength; ++i) {
          if (rnd.Next(frequency) == 0) {
 clonedBytes[i]=(byte)rnd.Next(256);
}
        }
        fs.Write(clonedBytes, 0, bytesLength);
        if (realSize>bytes.Length) {
          var randomBytes = new byte[realSize-bytes.Length];
          rnd.NextBytes(randomBytes);
          fs.Write(randomBytes, 0, randomBytes.Length);
        }
      }
    }

    public static IList<byte[]> ReadAllBytesInDir(string path, string pattern) {
      IList<byte[]> list = new List<byte[]>();
      if (!Directory.Exists(path)) {
 return list;
}
      foreach(string file in Directory.GetFiles(path, pattern)) {
        list.Add(File.ReadAllBytes(file));
      }
      return list;
    }

    public static int Main(string[] args) {
      try {
        var random = new Random();
        XmlConfigFile config = XmlConfigFile.Create(
           (args.Length>0) ? args[0] : "fuzzer.xml",
           "fuzzer");
        string path=config.GetValue("outputPath");
        byte[] startbytes=config.GetValueOrEmptyAsByteArray("startBytes");
        IList<byte[]> fuzzing = ReadAllBytesInDir(
        config.GetValue("validFilesPath"),
        config.GetValue("validFilesPattern","*.*"));
          string extension=config.GetValue("fileExtension","dat");
        Directory.CreateDirectory(path);
      int fuzzOffset=config.GetValueAsInt32("fuzzOffset",0);
        int iterations=config.GetValueAsInt32("iterations",1000);
        int freqMean=config.GetValueAsInt32("frequencyMean",32);
        int freqStdDev=config.GetValueAsInt32("frequencyStdDev",16);
        for (int i = 0;i<Math.Max(0, iterations); ++i) {
          string name = Path.Combine(
          path,
          i.ToString("d5"
            ,System.Globalization.CultureInfo.InvariantCulture)+"." +extension);
          int index = random.Next(fuzzing.Count + 1);
          if (index == fuzzing.Count) {
            WriteFuzzedFileWithHeader(startbytes, random, name);
          } else {
            WriteVariation(fuzzing[index], random, name, fuzzOffset,
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
