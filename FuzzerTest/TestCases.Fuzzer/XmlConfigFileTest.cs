using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterO;

namespace TestCases.Fuzzer {
  [TestClass]
  public class XmlConfigFileTest {
    //
    //  Public Methods
    //
    [TestMethod]
    public void Create() {
      // Not implemented yet.
    }
    [TestMethod]
    public void Exists() {
      XmlConfigFile f = XmlConfigFile.FromXmlString(
        "<root><element>ElementValue</element></root>",
        "root");
      Assert.IsTrue(f.Exists("element"));
      Assert.IsFalse(f.Exists("notfound"));
    }
    [TestMethod]
    public void GetAllElementConfig() {
      // Not implemented yet.
    }
    [TestMethod]
    public void GetAttribute() {
      XmlConfigFile f = XmlConfigFile.FromXmlString(
        "<root><element a='b' c='d'>ElementValue</element></root>",
        "root");
      {
string stringTemp = f.GetAttribute("element", "a");
Assert.AreEqual(
  "b",
  stringTemp);
}
      {
string stringTemp = f.GetAttribute("element", "c");
Assert.AreEqual(
  "d",
  stringTemp);
}
      try {
        f.GetAttribute("notfound", "");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
    }
    [TestMethod]
    public void GetElementConfig() {
      // Not implemented yet.
    }
    [TestMethod]
    public void GetRootAttribute() {
      // Not implemented yet.
    }
    [TestMethod]
    public void GetRootValue() {
      // Not implemented yet.
    }
    [TestMethod]
    public void GetValue() {
      XmlConfigFile f = XmlConfigFile.FromXmlString(
        "<root><element>ElementValue</element></root>",
        "root");
      try {
        f.GetValue("notfound");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      if ((f.GetValue("notfound", null)) != null) {
 Assert.Fail();
 }
      {
string stringTemp = f.GetValue("element");
Assert.AreEqual(
  "ElementValue",
  stringTemp);
}
    }
    [TestMethod]
    public void GetValueAsByteArray() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>01020A2b</element></root>",
        "root");
      try {
        f.GetValueAsByteArray(null);
        Assert.Fail("Should have failed");
      } catch (ArgumentNullException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsByteArray("notfound");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      if ((f.GetValueAsByteArray("notfound", null)) != null) {
 Assert.Fail();
 }
      Assert.AreEqual(new byte[] { 0x01, 0x02, 0x0a, 0x2b },
                    f.GetValueAsByteArray("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>0102030s</element></root>",
        "root");
      try {
        f.GetValueAsByteArray("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsByteArray("element", null);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      f = XmlConfigFile.FromXmlString(
        "<root><element>0102030</element></root>",
        "root");
      try {
        f.GetValueAsByteArray("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsByteArray("element", null);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      f = XmlConfigFile.FromXmlString(
        "<root><element>good0102030</element></root>",
        "root");
      try {
        f.GetValueAsByteArray("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsByteArray("element", null);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
    }
    [TestMethod]
    public void GetValueAsDouble() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789.5</element></root>",
        "root");
      try {
        f.GetValueAsDouble(null);
        Assert.Fail("Should have failed");
      } catch (ArgumentNullException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsDouble("notfound");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      Assert.AreEqual(0.1, f.GetValueAsDouble("notfound", 0.1));
      Assert.AreEqual(36789.5,
                    f.GetValueAsDouble("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789.5dsfe</element></root>",
        "root");
      try {
        f.GetValueAsDouble("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsDouble("element", 0.1);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      f = XmlConfigFile.FromXmlString(
        "<root><element>dde36789.5</element></root>",
        "root");
      try {
        f.GetValueAsDouble("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsDouble("element", 0.1);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
    }
    [TestMethod]
    public void GetValueAsInt32() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789</element></root>",
        "root");
      try {
        f.GetValueAsDouble(null);
        Assert.Fail("Should have failed");
      } catch (ArgumentNullException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsInt32("notfound");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      Assert.AreEqual(0, f.GetValueAsInt32("notfound", 0));
      Assert.AreEqual(36789,
                    f.GetValueAsInt32("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789dsfe</element></root>",
        "root");
      try {
        f.GetValueAsInt32("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsInt32("element", 0);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      f = XmlConfigFile.FromXmlString(
        "<root><element>dde36789</element></root>",
        "root");
      try {
        f.GetValueAsInt32("element");
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
      try {
        f.GetValueAsInt32("element", 0);
        Assert.Fail("Should have failed");
      } catch (XmlConfigException) {
new Object();
} catch (Exception ex) {
        Assert.Fail(ex.ToString());
        throw new InvalidOperationException(String.Empty, ex);
      }
    }
    [TestMethod]
    public void GetValueOrEmptyAsByteArray() {
      // Not implemented yet.
    }
  }
}
