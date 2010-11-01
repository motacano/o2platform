// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;
using O2.Kernel;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.Views.ASCX.classes.MainGUI;
using O2.Kernel.ExtensionMethods;
using O2.API.Visualization.ExtensionMethods;
using Microsoft.Windows.Controls.Ribbon;
using O2.XRules.Database.APIs;
using O2.External.SharpDevelop.ExtensionMethods;

//O2File:WPF_Ribbon.cs

namespace O2.XRules.Database.Utils
{
	public class CustomO2
	{
		public static WPF_Ribbon create(string title)
		{
			return create(title, 800,263);
		}
		
		public static WPF_Ribbon create(string title, int width, int height)
		{				
			var formTitle = "{0} (Custom O2)".format(title);		
			var panel = O2Gui.open<WinForms.Panel>(title, width, height);       
			return create(panel, title);
		}
		
		public static WPF_Ribbon create(WinForms.Control control, string title)
		{
			var ribbon = control.add_Ribbon_WithLogViewer(title);       
			ribbon.title(title);    
			return ribbon;
		}
	}
	
	//number of template Ribbon Tabs and Groups
	public static class CustomO2_ExtensionMethods_Tabs
	{
		public static  WPF_Ribbon add_Tab_BrowserAutomation(this WPF_Ribbon ribbon)
		{
			var browserAutomation = ribbon.add_Tab("Browser Automation");   
			browserAutomation.add_RibbonGroup("IE Script development environments") 				 
							 .add_RibbonButton_Script("IE Automation","ascx_IE_ScriptExecution.cs")
							 .add_RibbonButton_H2Script("IE Automation Development","IE Automation using WatiN.h2")
							 .add_RibbonButton_H2Script("WatiN Recorder","WatiN - Open 'Test Recorder' in new process.h2");
			browserAutomation.add_RibbonGroup("Javascript")
							 .add_RibbonButton_H2Script("XSS PoC Builder","Web - XSS PoC Builder.h2") 
							 .add_RibbonButton_H2Script("Javascript AST Viewer","Web - Javascript AST Viewer.h2")	  
							 .add_RibbonButton_H2Script("JavaScript Stats Viewer","Web - JavaScript Stats Viewer.h2");  
				  
			browserAutomation.add_RibbonGroup("Html") 
							 .add_RibbonButton_H2Script("Html Tag Viewer","ascx_HtmlTagViewer.cs")
							 .add_RibbonButton_H2Script("View WebPage details","ascx_View_WebPage_Details.cs.o2")
							 .add_RibbonButton_H2Script("Quick HtmlCode Viewer","Quick HtmlCode Viewer.h2")
							 .add_RibbonButton_H2Script("Simple Html Viewer","Util - Simple Html Viewer.h2")
							 .add_RibbonButton_H2Script("Html Editor","ascx_Html_Editor.cs.o2");      
			return ribbon;		
		}		
	}
	
	public static class CustomO2_ExtensionMethods_Groups
	{
		public static WPF_Ribbon add_Group_developmentGuis(this WPF_Ribbon ribbon, string tabName)
		{
			ribbon.tab(tabName).add_Group_developmentGuis();
			return ribbon;
		}
		
		public static RibbonTab add_Group_developmentGuis(this RibbonTab tab)
		{		
			if (tab.notNull())
				tab.add_RibbonGroup("Development GUIs")
						.add_RibbonButton_Script("Simple Script Editor","ascx_Simple_Script_Editor.cs.o2")
						.add_RibbonButton_Script("Quick development GUI","ascx_Quick_Development_GUI.cs.o2")			
						.add_RibbonButton_H2Script("O2 Development Environment","Util - O2 Development Environment.h2")					
						.add_RibbonButton_H2Script("Source Code Viewer","Util - SourceCodeViewer.h2")
						.add_RibbonButton_H2Script("Source Code Editor","Util - SourceCodeEditor.h2")
						.add_RibbonButton("O2 Object Model", () => open.o2ObjectModel());
			return tab;
		}
	}
}