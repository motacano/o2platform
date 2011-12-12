// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using System.Reflection; 
using System.Text;
using System.ComponentModel;
using Microsoft.Win32;
using O2.Interfaces.O2Core;
using O2.Interfaces.O2Findings;
using O2.Kernel;
using O2.Kernel.Objects;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.O2Findings;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.H2Scripts;
using O2.DotNetWrappers.Zip;
using O2.Views.ASCX;
using O2.Views.ASCX.CoreControls;
using O2.Views.ASCX.classes.MainGUI;
using O2.Views.ASCX.ExtensionMethods;
using O2.External.SharpDevelop.AST;
using O2.External.SharpDevelop.ExtensionMethods;
using O2.External.SharpDevelop.Ascx;
using O2.API.AST.CSharp;
using O2.API.AST.ExtensionMethods;
using O2.API.AST.ExtensionMethods.CSharp;

using ICSharpCode.TextEditor;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast; 
using ICSharpCode.SharpDevelop.Dom;
using ICSharpCode.SharpDevelop.Dom.CSharp;
using System.CodeDom;

using O2.Views.ASCX.O2Findings;
using O2.Views.ASCX.DataViewers;
using System.Security.Cryptography;

using Ionic.Zip;

//O2Ref:Ionic.Zip.dll
//O2Ref:O2_API_AST.dll

//O2File:_Extra_methods_Collections.cs
//O2File:_Extra_methods_Web.cs
//O2File:_Extra_methods_Misc.cs
//O2File:_Extra_methods_Files.cs
//O2File:_Extra_methods_Items.cs
//O2File:_Extra_methods_Windows.cs
//O2File:_Extra_methods_Reflection.cs
//O2File:_Extra_methods_WinForms_Controls.cs
//O2File:_Extra_methods_WinForms_DataGridView.cs
//O2File:_Extra_methods_WinForms_Misc.cs
//O2File:_Extra_methods_WinForms_TreeView.cs
//O2File:_Extra_methods_WinForms_TableList.cs
//O2File:_Extra_methods_ObjectDetails.cs
//O2File:_Extra_methods_TypeConfusion.cs
//O2File:_Extra_methods_SourceCodeEditor.cs
//O2File:_Extra_methods_AppDomain.cs
//O2File:_Extra_methods_Compilation.cs

namespace O2.XRules.Database.Utils
{		
	
	// Other extension method classes
	
	
	public static class _Extra_XElement_LinqXML_extensionMethods
	{
		public static XElement parent(this XElement element)
		{
			return element.Parent;
		}
		
		public static XElement element(this XElement elementToSearch, string name, bool createIfNotExistant)
		{
			var foundElement = elementToSearch.element(name);
			if (foundElement.notNull())
				return foundElement;
			return createIfNotExistant 
					? elementToSearch.add_Element(name)
					: null;
		}
		
		public static XElement add_Element(this XElement rootElement, string text)
		{
			var newElement = new XElement(text);
			rootElement.Add(newElement);
			return newElement;
		}
		
		public static XAttribute add_Attribute(this XElement rootElement, string text, object value)
		{
			var newAttribute = new XAttribute(text.xName(), value);
			rootElement.Add(newAttribute);
			return newAttribute;
		}
		
		public static XAttribute attribute(this XElement elementToSearch, string name, bool createIfNotExistant)
		{
			return elementToSearch.attribute(name, 	createIfNotExistant, null);
		}
		
		public static XAttribute attribute(this XElement elementToSearch, string name, bool createIfNotExistant, object value)
		{
			var foundAttribute = elementToSearch.attribute(name);
			if (foundAttribute.notNull())
				return foundAttribute;
			return createIfNotExistant 
					? elementToSearch.add_Attribute(name, value ?? "")
					: null;
		}

		public static string add_xmlns(this string name, XElement xElement)				
		{
			return name.prepend_AttributeValue(xElement, "xmlns");
		}
		
		public static string prepend_AttributeValue(this string name, XElement xElement, string attributeName)
		{
			var xmlns =  xElement.attribute(attributeName).value();
			return "{" + xmlns + "}" + name; 
		}
		
		public static XElement innerXml(this XElement xElement, string value)
		{
			return xElement.value(value);			
		}
		
		public static XElement value(this XElement xElement, string value)
		{
			xElement.Value = value;
			return xElement;
		}		
	}
	
	public static class _Extra_ConfigFiles_extensionMethods
	{
		// Config files (can't easily put this on the main
        public static Panel editLocalConfigFile(this string file)
        {
            var panel = O2Gui.open<Panel>("Editing local config file: {0}".format(file), 700, 300);
            return file.editLocalConfigFile(panel);
        }
	}
		
	 

	
	public static class _Extra_XmlLinq_ExtensiomMethods
	{
		public static XAttribute value(this XAttribute xAttribute, string value)
		{	
			if (xAttribute.notNull())
				xAttribute.SetValue(value);
			return xAttribute;
		}		
	}
	
	public static class _Extra_Xml_XSD_ExtensionMethods
	{
		//replace current xml_CreateCSharpFile with this one (inside O2.External.SharpDevelop.ExtensionMethods)
		public static string xmlCreateCSharpFile_Patched(this string xmlFile)
		{
			var csharpFile = "{0}.cs".format(xmlFile); //xmlFile.replace(".xml",".cs");
			return xmlFile.xmlCreateCSharpFile_Patched(csharpFile);
		}
		
		public static string xmlCreateCSharpFile_Patched(this string xmlFile, string csharpFile)
		{
			var xsdFile = "{0}.xsd".format(xmlFile) ;// xmlFile.replace(".xml",".xsd");
			return xmlFile.xmlCreateCSharpFile_Patched(xsdFile, csharpFile);
		}
		
		public static string xmlCreateCSharpFile_Patched(this string xmlFile, string xsdFile, string csharpFile)
		{								
			if (xsdFile.dirExists())
				xsdFile = xsdFile.pathCombine("{0}.xsd".format(xmlFile.fileName()));
			if (csharpFile.dirExists())
				csharpFile = csharpFile.pathCombine("{0}.cs".format(xmlFile.fileName()));
				
			xmlFile.xmlCreateXSD().saveAs(xsdFile);
			if (xsdFile.fileExists())
			{
				"Created XSD for Xml File: {0}".info(xmlFile);	 
				var tempCSharpFile = xsdFile.xsdCreateCSharpFile();
				tempCSharpFile.fileContents()
					          .insertBefore("//O2Ref:O2_Misc_Microsoft_MPL_Libs.dll".line())
					   	      .saveAs(csharpFile);				
				if (csharpFile.fileExists())
				{
					"Created CSharpFile for Xml File: {0}".info(csharpFile);	
					if (tempCSharpFile != csharpFile)
						File.Delete(tempCSharpFile);
				}	
				return csharpFile;
			}
			return null;	
		}
	}
	
	
		
			
	public static class _Extra_H2_ExtensionMethods
	{
		public static string scriptSourceCode(this string file)
		{
			if (file.extension(".h2"))
				return file.h2_SourceCode();
			return file.fileContents();
		}
		public static string h2_SourceCode(this string file)
		{
			if (file.extension(".h2"))
			{
				"return source code of H2 file".info();
				return H2.load(file).SourceCode;
			}
			return null;
		}			
	}	
		
	public static class _Extra_Zip_ExtensionMethods
	{	
		public static string zip_File(this string filesToZip)
		{
			return filesToZip.zip_File(".zip".tempFile());
		}
		
		public static string zip_Folder(this string filesToZip)
		{
			return filesToZip.zip_Folder(".zip".tempFile());
		}
		
		public static string zip_Files(this List<string> filesToZip)
		{
			return filesToZip.zip_Files(".zip".tempFile());
		}
		
		public static string zip_Files(this List<string> filesToZip, string targetZipFile)//, string baseFolder)
		{		
			"Creating ZipFile with {0} files to {1}".info(filesToZip.size(), targetZipFile);
			if (targetZipFile.fileExists())
				Files.deleteFile(targetZipFile);
            var zpZipFile = new ZipFile(targetZipFile);
            foreach(var fileToZip in filesToZip)
            {            	
            	{
            		zpZipFile.AddFile(fileToZip);            
            	}
            	//catch(Exception ex)
            	{
            	//	"[zip_Files] {0} in file {1}".error(ex.Message, fileToZip);
            	}
            }
            zpZipFile.Save();
            zpZipFile.Dispose();
            return targetZipFile;        
		}
	}	
}
    	