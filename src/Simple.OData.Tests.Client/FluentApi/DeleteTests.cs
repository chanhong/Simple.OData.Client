﻿using FluentAssertions;
using Simple.OData.Client;
using Xunit;

namespace Simple.OData.Tests.Client.FluentApi;

public class DeleteTests : TestBase
{
	[Fact]
	public async Task DeleteByKey()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For("Products")
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync();

		await client
			.For("Products")
			.Key(product["ProductID"])
			.DeleteEntryAsync();

		product = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.FindEntryAsync();

		product.Should().BeNull();
	}

	[Fact(Skip = "Cannot be mocked")]
	public async Task DeleteByKeyClearMetadataCache()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For("Products")
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync();

		client.Session.ClearMetadataCache();
		await client
			.For("Products")
			.Key(product["ProductID"])
			.DeleteEntryAsync();

		product = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.FindEntryAsync();

		product.Should().BeNull();
	}

	[Fact]
	public async Task DeleteByFilter()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		_ = await client
			.For("Products")
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync();

		await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.DeleteEntriesAsync();

		var product = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.FindEntryAsync();

		Assert.Null(product);
	}

	[Fact]
	public async Task DeleteByFilterFromCommand()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		_ = await client
			.For("Products")
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync();

		var commandText = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.GetCommandTextAsync();

		await client
			.DeleteEntriesAsync("Products", commandText);

		var product = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.FindEntryAsync();

		Assert.Null(product);
	}

	[Fact]
	public async Task DeleteByObjectAsKey()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var product = await client
			.For("Products")
			.Set(new { ProductName = "Test1", UnitPrice = 18m })
			.InsertEntryAsync();

		await client
			.For("Products")
			.Key(product)
			.DeleteEntryAsync();

		product = await client
			.For("Products")
			.Filter("ProductName eq 'Test1'")
			.FindEntryAsync();

		Assert.Null(product);
	}

	[Fact]
	public async Task DeleteDerived()
	{
		var client = new ODataClient(CreateDefaultSettings().WithHttpMock());
		var ship = await client
			.For("Transport")
			.As("Ship")
			.Set(new { ShipName = "Test1" })
			.InsertEntryAsync();

		await client
			.For("Transport")
			.As("Ship")
			.Key(ship["TransportID"])
			.DeleteEntryAsync();

		ship = await client
			.For("Transport")
			.As("Ship")
			.Filter("ShipName eq 'Test1'")
			.FindEntryAsync();

		Assert.Null(ship);
	}
}
