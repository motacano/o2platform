// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using O2.Interfaces.O2Core;
using O2.Kernel;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Network;
using O2.DotNetWrappers.Zip;

namespace O2.XRules.Database.APIs
{
    public class API_FuzzDB
    {    
    	public string _currentVersion = "fuzzdb-1.08";
    	public string currentVersionZipFile = "fuzzdb-1.08.zip";
    	public string installDir = PublicDI.config.O2TempDir.pathCombine("..\\");
    	
    	public string PathToFuzzDB { get; set;}
    	
    	public API_FuzzDB()
    	{
    		PathToFuzzDB = installDir.pathCombine(_currentVersion);
    		install();
    	}
    	    	    	
    	public bool isInstalled()
    	{
    		return PathToFuzzDB.dirExists();
    	}
    	
    	public bool install()
    	{
    		if (isInstalled())
    			return true;
    		var localFilePath = PublicDI.config.O2TempDir.pathCombine(currentVersionZipFile);
    		var webLocation = "{0}{1}".format(PublicDI.config.O2SVN_FilesWithNoCode, currentVersionZipFile);
    		"downloading file {0} from {1} to {2}".info(currentVersionZipFile, webLocation,localFilePath);
            if (new Web().httpFileExists(webLocation))
            {
                new Web().downloadBinaryFile(webLocation, localFilePath);
                if (localFilePath.fileExists())                                	
                	new zipUtils().unzipFile(localFilePath,installDir);                               
            }
            if (isInstalled())
            {
            	"{0} installed ok".info(_currentVersion);
            	return true;
            }
            "There was a problem installing the {0}".error(_currentVersion);
            return false;
    	}    	    	    	    	    
    	
    	public List<String> getPayloads(params string[] virtualFilePaths)
    	{
    		var payloads = new List<String>();
    		foreach(var virtualFilePath in virtualFilePaths)
    		{
    			var targetFile = PathToFuzzDB.pathCombine(virtualFilePath);
    			if (targetFile.fileExists())    		
    				payloads.AddRange(targetFile.fileContents()
								  				.fixCRLF()
							      				.lines()
							 	  				.remove(0));
				else
					"[API_FuzzDB] in getPayloads the request virtual path could not be found: {0}".error(virtualFilePath);
			}
			return payloads;
    	}
    }
    
    public static class API_FuzzDB_ExtensionMethods_Fuzzing_Databases
    {
    	public static List<String> payloads_Xss(this API_FuzzDB fuzzDB)
    	{
    		var xssPayloads = fuzzDB.getPayloads(@"attack-payloads\xss\xss-rsnake.txt",
    											 @"attack-payloads\xss\xss-uri.txt");
    		    		
    		
											   
			return xssPayloads;
    	}
    }
}