using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using PeterO;

namespace PeterO
{
	public sealed class XmlConfigFile {
		XmlElement root;
		Dictionary<string,XmlElement> dict;
		private void Initialize(XmlElement element){
			if(element==null)
				throw new ArgumentNullException("element");
			this.root=element;
			this.dict=new Dictionary<string, XmlElement>();
			foreach(XmlNode node in this.root.ChildNodes){
				XmlElement e=node as XmlElement;
				if(e!=null){
					this.dict[e.Name]=e;
				}
			}
		}
		private XmlConfigFile(XmlElement element){
			Initialize(element);
		}
		private XmlConfigFile(XmlReader reader, string rootElement){
			if(reader==null)
				throw new ArgumentNullException("reader");
			if(rootElement.Length==0)
				throw new ArgumentException("'rootElement' is empty.");
			XmlDocument doc;
			try {
				doc=new XmlDocument();
				doc.Load(reader);
			} catch(IOException e){
				throw new XmlConfigException(e.Message,e);
			} catch(UnauthorizedAccessException e){
				throw new XmlConfigException(e.Message,e);
			} catch(XmlException e){
				throw new XmlConfigException(e.Message,e);
			}
			if(doc.DocumentElement==null)
				throw new XmlConfigException("The file contains no XML element.");
			if(rootElement!=null && !doc.DocumentElement.Name.Equals(rootElement))
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The XML file's root element is {0}, not {1}.",
					              doc.DocumentElement.Name,rootElement));
			Initialize(doc.DocumentElement);
		}
		private XmlConfigFile(string path, string rootElement){
			if(path==null)
				throw new ArgumentNullException("path");
			if(rootElement.Length==0)
				throw new ArgumentException("'rootElement' is empty.");
			XmlDocument doc;
			try {
				doc=new XmlDocument();
				doc.Load(path);
			} catch(IOException e){
				throw new XmlConfigException(e.Message,e);
			} catch(UriFormatException e){
				throw new XmlConfigException(e.Message,e);
			} catch(System.Security.SecurityException e){
				throw new XmlConfigException(e.Message,e);
			} catch(UnauthorizedAccessException e){
				throw new XmlConfigException(e.Message,e);
			} catch(XmlException e){
				throw new XmlConfigException(e.Message,e);
			}
			if(doc.DocumentElement==null)
				throw new XmlConfigException("The file contains no XML element.");
			if(rootElement!=null && !doc.DocumentElement.Name.Equals(rootElement))
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The XML file's root element is {0}, not {1}.",
					              doc.DocumentElement.Name,rootElement));
			Initialize(doc.DocumentElement);
		}
		public static XmlConfigFile Create(XmlReader reader, string rootElement){
			return new XmlConfigFile(reader,rootElement);
		}
		public static XmlConfigFile Create(Stream stream, string rootElement){
			using(XmlReader reader=XmlReader.Create(stream)){
				return new XmlConfigFile(reader,rootElement);
			}
		}
		public static XmlConfigFile Create(TextReader textReader, string rootElement){
			using(XmlReader reader=XmlReader.Create(textReader)){
				return new XmlConfigFile(reader,rootElement);
			}
		}
		public static XmlConfigFile Create(string path, string rootElement){
			return new XmlConfigFile(path,rootElement);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="rootElement">If not null, throws an XmlConfigException
		/// if the document's root element name doesn't match this value.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">'xml' is null.</exception>
		/// <exception cref="XmlConfigException">The method failed to create the
		/// config file. The exception's InnerException contains more information.</exception>
		public static XmlConfigFile FromXmlString(string xml, string rootElement){
			if(xml==null)
				throw new ArgumentNullException("xml");
			using(StringReader reader=new StringReader(xml)){
				return Create(reader,rootElement);
			}	
		}
		/// <summary>
		/// Gets the value of the specified attribute of 
		/// the specified child element.
		/// </summary>
		/// <param name="attributeName">Name of the attribute to retrieve.
		/// </param>
		/// <returns>The attribute's value. Returns an empty string
		/// if the attribute doesn't exist.</returns>
		public string GetRootAttribute(string attributeName){
			return root.GetAttribute(attributeName);
		}
		/// <summary>
		/// Gets the text content of the root element as a string.
		/// </summary>
		/// <returns>The root's text content.</returns>
		public string GetRootValue(){
			return root.InnerText;
		}
		public IEnumerable<XmlConfigFile> GetAllElementConfig(string elementName){
			foreach(XmlNode node in this.root.ChildNodes){
				XmlElement e=node as XmlElement;
				if(e!=null && e.Name.Equals(elementName)){
					yield return new XmlConfigFile(e);
				}
			}
		}
		public XmlConfigFile GetElementConfig(string elementName){
			if(!dict.ContainsKey(elementName))
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' can't be found.",elementName));
			return new XmlConfigFile(dict[elementName]);
		}
		public bool Exists(string elementName){
			return dict.ContainsKey(elementName);
		}
		/// <summary>
		/// Gets the value of the specified attribute of 
		/// the specified child element.
		/// </summary>
		/// <param name="elementName">A child element's name</param>
		/// <param name="attributeName">Name of the attribute to retrieve.
		/// </param>
		/// <returns>The attribute's value. Returns an empty string
		/// if the attribute doesn't exist.</returns>
		/// <exception cref="ArgumentNullException">elementName is null.</exception>
		/// <exception cref="XmlConfigException">The child element
		/// does not exist.</exception>
		public string GetAttribute(string elementName, string attributeName){
			if(elementName==null)
				throw new ArgumentNullException("elementName");
			if(!dict.ContainsKey(elementName))
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' can't be found.",elementName));
			return dict[elementName].GetAttribute(attributeName);
		}
		public string GetValue(string elementName, string defaultValue){
			return Exists(elementName) ? GetValue(elementName) : defaultValue;
		}
		
		public string GetValue(string elementName){
			if(!dict.ContainsKey(elementName))
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' can't be found.",elementName));
			return dict[elementName].InnerText;
		}
		public int GetValueAsInt32(string elementName, int defaultValue){
			return Exists(elementName) ? GetValueAsInt32(elementName) : defaultValue;
		}
		public int GetValueAsInt32(string elementName){
			try {
				return XmlConvert.ToInt32(GetValue(elementName));
			} catch(FormatException e){
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' doesn't contain a valid integer.",elementName),e);
			} catch(OverflowException e){
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' doesn't contain a valid integer.",elementName),e);
			}
		}
		public double GetValueAsDouble(string elementName, double defaultValue){
			return Exists(elementName) ? GetValueAsDouble(elementName) : defaultValue;
		}
		public double GetValueAsDouble(string elementName){
			try {
				return XmlConvert.ToDouble(GetValue(elementName));
			} catch(FormatException e){
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' doesn't contain a valid number.",elementName),e);
			} catch(OverflowException e){
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The element named '{0}' doesn't contain a valid number.",elementName),e);
			}
		}
		private static int ToHex(char c){
			int ic=(int)c;
			if(ic>=(int)'0' && ic<=(int)'9')
				return ic-(int)'0';
			if(ic>=(int)'A' && ic<=(int)'f')
				return ic+10-(int)'A';
			if(ic>=(int)'a' && ic<=(int)'f')
				return ic+10-(int)'a';
			return -1;
		}
		public byte[] GetValueAsByteArray(string elementName, byte[] defaultValue){
			return Exists(elementName) ? GetValueAsByteArray(elementName) : defaultValue;
		}
		public byte[] GetValueOrEmptyAsByteArray(string elementName){
			return Exists(elementName) ? GetValueAsByteArray(elementName) : new byte[]{};
		}
		public byte[] GetValueAsByteArray(string elementName){
			string value=GetValue(elementName);
			if(value.Length%2!=0)
				throw new XmlConfigException(
					String.Format(System.Globalization.CultureInfo.CurrentCulture,
					              "The hex string in '{0}' contains an odd number of characters.",elementName));
			byte[] data=new byte[value.Length/2];
			for(int i=0;i<value.Length;i+=2){
				int num=ToHex(value[i]);
				int num2=ToHex(value[i+1]);
				if(num<0||num2<0){
					throw new XmlConfigException(
						String.Format(System.Globalization.CultureInfo.CurrentCulture,
						              "The hex string in '{0}' contains an illegal character.",elementName));
				}
				data[i>>1]=(byte)((num<<4)|num2);
			}
			return data;
		}
	}
}
