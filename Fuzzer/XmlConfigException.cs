using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace PeterO {
    /// <summary>The exception that is thrown when an error occurs when
    /// creating or reading an XML configuration file.</summary>
  [Serializable]
  public sealed class XmlConfigException : SystemException {
    /// <summary>Initializes a new instance of the XmlConfigException
    /// class.</summary>
    public XmlConfigException() {
}

    /// <summary>Initializes a new instance of the XmlConfigException
    /// class.</summary>
    /// <param name='message'>A string object.</param>
    public XmlConfigException(string message) : base(message) {
}

    /// <summary>Initializes a new instance of the XmlConfigException
    /// class.</summary>
    /// <param name='message'>A string object.</param>
    /// <param name='innerException'>An Exception object.</param>
    public XmlConfigException(string message, Exception innerException) :
      base(message, innerException) {
}

    /// <summary>Initializes a new instance of the XmlConfigException
    /// class.</summary>
    /// <param name='info'>A System.Runtime.Serialization.SerializationInfo
    /// object.</param>
    /// <param name='context'>A
    /// System.Runtime.Serialization.StreamingContext object.</param>
private XmlConfigException(
  System.Runtime.Serialization.SerializationInfo info,
  System.Runtime.Serialization.StreamingContext context) :
      base(info, context) {
}
  }
}
