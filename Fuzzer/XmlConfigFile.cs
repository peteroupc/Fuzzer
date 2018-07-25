using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using PeterO;

namespace PeterO {
    /// <summary>Not documented yet.</summary>
  public sealed class XmlConfigFile {
    private XmlElement root;
    private Dictionary<string, XmlElement> dict;

    /// <summary>Not documented yet.</summary>
    /// <param name='element'>The parameter <paramref name='element'/> is
    /// not documented yet.</param>
    /// <exception cref='ArgumentNullException'>The parameter <paramref
    /// name='element'/> is null.</exception>
    private void Initialize(XmlElement element) {
      if (element == null) {
        throw new ArgumentNullException(nameof(element));
      }
      this.root = element;
      this.dict = new Dictionary<string, XmlElement>();
      foreach (XmlNode node in this.root.ChildNodes) {
        var e = node as XmlElement;
        if (e != null) {
          this.dict[e.Name] = e;
        }
      }
    }

    /// <summary>Initializes a new instance of the XmlConfigFile
    /// class.</summary>
    /// <param name='element'>A XmlElement object.</param>
    private XmlConfigFile(XmlElement element) {
      this.Initialize(element);
    }

    /// <summary>Initializes a new instance of the XmlConfigFile
    /// class.</summary>
    /// <param name='reader'>A XmlReader object.</param>
    /// <param name='rootElement'>A string object.</param>
    /// <exception cref='ArgumentNullException'>The parameter <paramref
    /// name='reader'/> is null.</exception>
    private XmlConfigFile(XmlReader reader, string rootElement) {
      if (reader == null) {
        throw new ArgumentNullException(nameof(reader));
      }
      if (rootElement.Length == 0) {
        throw new ArgumentException("'rootElement' is empty.");
      }
      XmlDocument doc;
      try {
        doc = new XmlDocument();
        doc.Load(reader);
      } catch (IOException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (UnauthorizedAccessException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (XmlException e) {
        throw new XmlConfigException(e.Message, e);
      }
      if (doc.DocumentElement == null) {
        throw new XmlConfigException("The file contains no XML element.");
      }
    if (rootElement != null && !doc.DocumentElement.Name.Equals(rootElement)) {
    string msg = "The XML file's root element is " +
        doc.DocumentElement.Name + ", not " + rootElement + ".";
 throw new XmlConfigException(msg);
 }
      this.Initialize(doc.DocumentElement);
    }

    /// <summary>Initializes a new instance of the XmlConfigFile
    /// class.</summary>
    /// <param name='path'>A string object.</param>
    /// <param name='rootElement'>Another string object.</param>
    /// <exception cref='ArgumentNullException'>The parameter <paramref
    /// name='path'/> is null.</exception>
    private XmlConfigFile(string path, string rootElement) {
      if (path == null) {
        throw new ArgumentNullException(nameof(path));
      }
      if (rootElement.Length == 0) {
        throw new ArgumentException("'rootElement' is empty.");
      }
      XmlDocument doc;
      try {
        doc = new XmlDocument();
        doc.Load(path);
      } catch (IOException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (UriFormatException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (System.Security.SecurityException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (UnauthorizedAccessException e) {
        throw new XmlConfigException(e.Message, e);
      } catch (XmlException e) {
        throw new XmlConfigException(e.Message, e);
      }
      if (doc.DocumentElement == null) {
        throw new XmlConfigException("The file contains no XML element.");
      }
    if (rootElement != null && !doc.DocumentElement.Name.Equals(rootElement)) {
    string msg = "The XML file's root element is " +
        doc.DocumentElement.Name + ", not " + rootElement + ".";
 throw new XmlConfigException(msg);
 }
      this.Initialize(doc.DocumentElement);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='reader'>The parameter <paramref name='reader'/> is not
    /// documented yet.</param>
    /// <param name='rootElement'>The parameter <paramref
    /// name='rootElement'/> is not documented yet.</param>
    /// <returns>A XmlConfigFile object.</returns>
    public static XmlConfigFile Create(XmlReader reader, string rootElement) {
      return new XmlConfigFile(reader, rootElement);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='stream'>The parameter <paramref name='stream'/> is not
    /// documented yet.</param>
    /// <param name='rootElement'>The parameter <paramref
    /// name='rootElement'/> is not documented yet.</param>
    /// <returns>A XmlConfigFile object.</returns>
    public static XmlConfigFile Create(Stream stream, string rootElement) {
      using (XmlReader reader = XmlReader.Create(stream)) {
        return new XmlConfigFile(reader, rootElement);
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='textReader'>The parameter <paramref
    /// name='textReader'/> is not documented yet.</param>
    /// <param name='rootElement'>The parameter <paramref
    /// name='rootElement'/> is not documented yet.</param>
    /// <returns>A XmlConfigFile object.</returns>
public static XmlConfigFile Create(
  TextReader textReader,
  string rootElement) {
      using (XmlReader reader = XmlReader.Create(textReader)) {
        return new XmlConfigFile(reader, rootElement);
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='path'>The parameter <paramref name='path'/> is not
    /// documented yet.</param>
    /// <param name='rootElement'>The parameter <paramref
    /// name='rootElement'/> is not documented yet.</param>
    /// <returns>A XmlConfigFile object.</returns>
    public static XmlConfigFile Create(string path, string rootElement) {
      return new XmlConfigFile(path, rootElement);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='xml'>A string object.</param>
    /// <param name='rootElement'>If not null, throws an XmlConfigException
    /// if the document's root element name doesn't match this
    /// value.</param>
    /// <returns>A XmlConfigFile object.</returns>
    /// <exception cref='ArgumentNullException'>The parameter <paramref
    /// name='xml'/> is null.</exception>
    /// <exception cref='XmlConfigException'>The method failed to create
    /// the config file. The exception's InnerException contains more
    /// information.</exception>
    public static XmlConfigFile FromXmlString(string xml, string rootElement) {
      if (xml == null) {
        throw new ArgumentNullException(nameof(xml));
      }
      using (var reader = new StringReader(xml)) {
        return Create(reader, rootElement);
      }
    }

    /// <summary>Gets the value of the specified attribute of the specified
    /// child element.</summary>
    /// <param name='attributeName'>Name of the attribute to
    /// retrieve.</param>
    /// <returns>The attribute's value. Returns an empty string if the
    /// attribute doesn't exist.</returns>
    public string GetRootAttribute(string attributeName) {
      return this.root.GetAttribute(attributeName);
    }

    /// <summary>Gets the text content of the root element as a
    /// string.</summary>
    /// <returns>The root's text content.</returns>
    public string GetRootValue() {
      return this.root.InnerText;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <returns>An IEnumerable(XmlConfigFile) object.</returns>
    public IEnumerable<XmlConfigFile> GetAllElementConfig(string elementName) {
      foreach (XmlNode node in this.root.ChildNodes) {
        var e = node as XmlElement;
        if (e != null && e.Name.Equals(elementName)) {
          yield return new XmlConfigFile(e);
        }
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <returns>A XmlConfigFile object.</returns>
    public XmlConfigFile GetElementConfig(string elementName) {
      if (!this.dict.ContainsKey(elementName)) {
 throw new XmlConfigException(
          "The element named '" + elementName + "' can't be found.");
 }
      return new XmlConfigFile(this.dict[elementName]);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <returns>A Boolean object.</returns>
    public bool Exists(string elementName) {
      return this.dict.ContainsKey(elementName);
    }

    /// <summary>Gets the value of the specified attribute of the specified
    /// child element.</summary>
    /// <param name='elementName'>A child element's name.</param>
    /// <param name='attributeName'>Name of the attribute to
    /// retrieve.</param>
    /// <returns>The attribute's value. Returns an empty string if the
    /// attribute doesn't exist.</returns>
    /// <exception cref='ArgumentNullException'>ElementName is
    /// null.</exception>
    /// <exception cref='XmlConfigException'>The child element does not
    /// exist.</exception>
    public string GetAttribute(string elementName, string attributeName) {
      if (elementName == null) {
        throw new ArgumentNullException(nameof(elementName));
      }
      if (!this.dict.ContainsKey(elementName)) {
 throw new XmlConfigException(
          "The element named '" + elementName + "' can't be found.");
 }
      return this.dict[elementName].GetAttribute(attributeName);
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>Another string object.</param>
    /// <param name='defaultValue'>A string object. (3).</param>
    /// <returns>A string object.</returns>
    public string GetValue(string elementName, string defaultValue) {
   return this.Exists(elementName) ? this.GetValue(elementName) :
        defaultValue;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>Another string object.</param>
    /// <returns>A string object.</returns>
    public string GetValue(string elementName) {
      if (!this.dict.ContainsKey(elementName)) {
 throw new XmlConfigException(
          "The element named '" + elementName + "' can't be found.");
 }
      return this.dict[elementName].InnerText;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <param name='defaultValue'>A 32-bit signed integer. (2).</param>
    /// <returns>A 32-bit signed integer.</returns>
    public int GetValueAsInt32(string elementName, int defaultValue) {
      return this.Exists(elementName) ? this.GetValueAsInt32(elementName) :
        defaultValue;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <returns>A 32-bit signed integer.</returns>
    public int GetValueAsInt32(string elementName) {
      try {
        return XmlConvert.ToInt32(this.GetValue(elementName));
      } catch (FormatException e) {
        throw new XmlConfigException(
  "The element named '" + elementName + "' doesn't contain a valid integer.",
  e);
      } catch (OverflowException e) {
        throw new XmlConfigException(
  "The element named '" + elementName + "' doesn't contain a valid integer.",
  e);
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <param name='defaultValue'>A 64-bit floating-point number.
    /// (2).</param>
    /// <returns>A 64-bit floating-point number.</returns>
    public double GetValueAsDouble(string elementName, double defaultValue) {
      return this.Exists(elementName) ? this.GetValueAsDouble(elementName) :
        defaultValue;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>A string object.</param>
    /// <returns>A 64-bit floating-point number.</returns>
    public double GetValueAsDouble(string elementName) {
      try {
        return XmlConvert.ToDouble(this.GetValue(elementName));
      } catch (FormatException e) {
        throw new XmlConfigException(
   "The element named '" + elementName + "' doesn't contain a valid number.",
   e);
      } catch (OverflowException e) {
        throw new XmlConfigException(
   "The element named '" + elementName + "' doesn't contain a valid number.",
   e);
      }
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='c'>The parameter <paramref name='c'/> is not
    /// documented yet.</param>
    /// <returns>A 32-bit signed integer.</returns>
    private static int ToHex(char c) {
      var ic = (int)c;
      if (ic >= (int)'0' && ic <= (int)'9') {
        return ic - (int)'0';
      }
      return (ic >= (int)'A' && ic <= (int)'f') ? (ic + 10 - (int)'A') : ((ic
        >= (int)'a' && ic <= (int)'f') ? (ic + 10 - (int)'a') : (-1));
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>The parameter <paramref
    /// name='elementName'/> is not documented yet.</param>
    /// <param name='defaultValue'>The parameter <paramref
    /// name='defaultValue'/> is not documented yet.</param>
    /// <returns>A byte array.</returns>
    public byte[] GetValueAsByteArray(string elementName, byte[] defaultValue) {
  return this.Exists(elementName) ? this.GetValueAsByteArray(elementName) :
    defaultValue;
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>The parameter <paramref
    /// name='elementName'/> is not documented yet.</param>
    /// <returns>A byte array.</returns>
    public byte[] GetValueOrEmptyAsByteArray(string elementName) {
  return this.Exists(elementName) ? this.GetValueAsByteArray(elementName) :
        new byte[] { };
    }

    /// <summary>Not documented yet.</summary>
    /// <param name='elementName'>The parameter <paramref
    /// name='elementName'/> is not documented yet.</param>
    /// <returns>A byte array.</returns>
    public byte[] GetValueAsByteArray(string elementName) {
      string value = this.GetValue(elementName);
      if (value.Length % 2 != 0) {
          string msg = "The hex string in '" + elementName +
            "' contains an odd number of characters.";
 throw new XmlConfigException(msg);
 }
      var data = new byte[value.Length / 2];
      for (int i = 0; i < value.Length; i += 2) {
        int num = ToHex(value[i]);
        int num2 = ToHex(value[i + 1]);
        if (num < 0 || num2 < 0) {
          throw new XmlConfigException(
   "The hex string in '" + elementName + "' contains an illegal character.");
        }
        data[i >> 1] = (byte)((num << 4) | num2);
      }
      return data;
    }
  }
}
