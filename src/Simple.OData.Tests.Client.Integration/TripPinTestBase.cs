using Simple.OData.Client;

namespace Simple.OData.Tests.Client;

public class TripPinTestBase : TestBase
{
	protected TripPinTestBase(string serviceUri, ODataPayloadFormat payloadFormat)
		: base(serviceUri, payloadFormat)
	{
	}

#pragma warning disable 1998
	protected async override Task DeleteTestData()
	{
		try
		{
		}
		catch (Exception)
		{
		}
	}
#pragma warning restore 1998
}
