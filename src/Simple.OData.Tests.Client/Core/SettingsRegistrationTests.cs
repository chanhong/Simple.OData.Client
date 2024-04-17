using FluentAssertions;
using Simple.OData.Client;
using Simple.OData.Tests.Client.Entities;
using Xunit;

namespace Simple.OData.Tests.Client.Core;

public class SettingsRegistrationTests
{
	[Fact]
	public void RegisterContainer()
	{
		var settings = new ODataClientSettings { BaseUri = new Uri("http://localhost") };

		settings.TypeCache.Register<Animal>();

		// NB Stored under AbsoluteUri so need trailing /
		var typeCache = TypeCaches.TypeCache("http://localhost/", null);

		typeCache.DynamicContainerName(typeof(Animal)).Should().Be("DynamicProperties");
	}

	[Fact]
	public void RegisterNamedContainer()
	{
		var settings = new ODataClientSettings { BaseUri = new Uri("http://localhost") };

		settings.TypeCache.Register<Animal>("Foo");

		// NB Stored under AbsoluteUri so need trailing /
		var typeCache = TypeCaches.TypeCache("http://localhost/", null);

		typeCache.DynamicContainerName(typeof(Animal)).Should().Be("Foo");
	}
}
