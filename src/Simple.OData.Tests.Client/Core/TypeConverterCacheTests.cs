using FluentAssertions;
using Simple.OData.Client;
using Xunit;

namespace Simple.OData.Tests.Client.Core;

public class TypeConverterCacheTests
{
	[Fact]
	public void CachePerUri()
	{
		var c1 = CustomConverters.Converter("foo");
		var c2 = CustomConverters.Converter("bar");

		c2.Should().NotBeSameAs(c1);
	}

	[Fact]
	public void SameCacheForUri()
	{
		var c1 = CustomConverters.Converter("foo");
		var c2 = CustomConverters.Converter("foo");

		c2.Should().BeSameAs(c1);
	}

	[Fact]
	public void GlobalConverters()
	{
		var c1 = CustomConverters.Converter("global");
		var c2 = CustomConverters.Global;

		Assert.Same(c1, c2);
	}
}
