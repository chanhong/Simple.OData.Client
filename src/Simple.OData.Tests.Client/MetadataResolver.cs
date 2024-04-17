﻿using System.Reflection;

namespace Simple.OData.Tests.Client;

public static class MetadataResolver
{

	private static string GetResourceAsString(string resourceName)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceNames = assembly.GetManifestResourceNames();
		var completeResourceName = resourceNames.FirstOrDefault(o => o.EndsWith("." + resourceName, StringComparison.CurrentCultureIgnoreCase));
		using var resourceStream = assembly.GetManifestResourceStream(completeResourceName);
		var reader = new StreamReader(resourceStream);
		return reader.ReadToEnd();
	}

	public static string GetMetadataDocument(string documentName)
	{
		return GetResourceAsString(@"Resources." + documentName);
	}
}
