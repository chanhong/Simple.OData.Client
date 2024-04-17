using BenchmarkDotNet.Running;
using Simple.OData.Client;
using Simple.OData.Tests.Client;

namespace Simple.OData.Benchmarks.Client;

internal class Program
{
	private static void Main()
	{
		//BenchmarkRunner.Run<CrmEmployee>();
		BenchmarkRunner.Run<TripPinPeople>();
	}
}

public static class Utils
{
	public static ODataClient GetClient(string metadataFilename, string responseFilename)
	{
		return new ODataClient(new ODataClientSettings(new Uri("http://localhost/odata"))
		{
			MetadataDocument = MetadataResolver.GetMetadataDocument(metadataFilename),
			IgnoreUnmappedProperties = true
		}.WithHttpResponses([@"..\..\..\..\..\..\..\Resources\" + responseFilename]));
	}
}
