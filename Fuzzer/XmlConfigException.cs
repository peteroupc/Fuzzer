using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace PeterO {
    /// <summary>The exception that is thrown when an error occurs when creating or
    /// reading an XML configuration file.</summary>
  [Serializable]
  public sealed class XmlConfigException : SystemException {
    public XmlConfigException() {}
    public XmlConfigException(string message):base(message) {}
    public XmlConfigException(string message, Exception innerException):
    base(message, innerException) {}
private XmlConfigException(System.Runtime.Serialization.SerializationInfo info,
                        System.Runtime.Serialization.StreamingContext context):
    base(info, context) {}
  }
}
