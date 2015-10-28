#if DEBUG || TEST
#if !CODE_ANALYSIS
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeterO;
using PeterO.Testing;

namespace TestCases.Fuzzer {
  [TestClass]
  public class XmlConfigFileTest {
    //
    //  Public Methods
    //
    [TestMethod]
    public void Create() {
      Assert.Ignore("This test is not implemented yet.");
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
      Assert.Ignore("This test is not implemented yet.");
    }
    [TestMethod]
    public void GetAttribute() {
      XmlConfigFile f = XmlConfigFile.FromXmlString(
        "<root><element a='b' c='d'>ElementValue</element></root>",
        "root");
      Assert.AreEqual("b",f.GetAttribute("element","a"));
      Assert.AreEqual("d",f.GetAttribute("element","c"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetAttribute("notfound",""));
    }
    [TestMethod]
    public void GetElementConfig() {
      Assert.Ignore("This test is not implemented yet.");
    }
    [TestMethod]
    public void GetRootAttribute() {
      Assert.Ignore("This test is not implemented yet.");
    }
    [TestMethod]
    public void GetRootValue() {
      Assert.Ignore("This test is not implemented yet.");
    }
    [TestMethod]
    public void GetValue() {
      XmlConfigFile f = XmlConfigFile.FromXmlString(
        "<root><element>ElementValue</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValue("notfound"));
      Assert.IsNull(f.GetValue("notfound",null));
      Assert.AreEqual("ElementValue",f.GetValue("element"));
    }
    [TestMethod]
    public void GetValueAsByteArray() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>01020A2b</element></root>",
        "root");
      TestUtil.ArgumentNull(()=>f.GetValueAsByteArray(null));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("notfound"));
      Assert.IsNull(f.GetValueAsByteArray("notfound",null));
      Assert.AreEqual(new byte[] { 0x01, 0x02, 0x0a, 0x2b },
                      f.GetValueAsByteArray("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>0102030s</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element",null));
      f = XmlConfigFile.FromXmlString(
        "<root><element>0102030</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element",null));
      f = XmlConfigFile.FromXmlString(
        "<root><element>good0102030</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsByteArray("element",null));
    }
    [TestMethod]
    public void GetValueAsDouble() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789.5</element></root>",
        "root");
      TestUtil.ArgumentNull(()=>f.GetValueAsDouble(null));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsDouble("notfound"));
      Assert.AreEqual(0.1,f.GetValueAsDouble("notfound",0.1));
      Assert.AreEqual(36789.5,
                      f.GetValueAsDouble("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789.5dsfe</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsDouble("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsDouble("element",0.1));
      f = XmlConfigFile.FromXmlString(
        "<root><element>dde36789.5</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsDouble("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsDouble("element",0.1));
    }
    [TestMethod]
    public void GetValueAsInt32() {
      XmlConfigFile f;
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789</element></root>",
        "root");
      TestUtil.ArgumentNull(()=>f.GetValueAsDouble(null));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsInt32("notfound"));
      Assert.AreEqual(0,f.GetValueAsInt32("notfound",0));
      Assert.AreEqual(36789,
                      f.GetValueAsInt32("element"));
      f = XmlConfigFile.FromXmlString(
        "<root><element>36789dsfe</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsInt32("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsInt32("element",0));
      f = XmlConfigFile.FromXmlString(
        "<root><element>dde36789</element></root>",
        "root");
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsInt32("element"));
      TestUtil.AssertThrow(typeof(XmlConfigException),()=>f.GetValueAsInt32("element",0));
    }
    [TestMethod]
    public void GetValueOrEmptyAsByteArray() {
      Assert.Ignore("This test is not implemented yet.");
    }
  }
}
#endif
#endif
