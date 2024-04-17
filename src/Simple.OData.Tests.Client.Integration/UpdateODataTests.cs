﻿using FluentAssertions;
using Simple.OData.Client;
using Xunit;
using Entry = System.Collections.Generic.Dictionary<string, object>;

namespace Simple.OData.Tests.Client;

// Not implemented by OData.org service
//public class UpdateODataTestsV2Atom : UpdateODataTests
//{
//    public UpdateODataTestsV2Atom() : base(ODataV2ReadWriteUri, ODataPayloadFormat.Atom, 2) { }
//}

// Not implemented by OData.org service
//public class UpdateODataTestsV2Json : UpdateODataTests
//{
//    public UpdateODataTestsV2Json() : base(ODataV2ReadWriteUri, ODataPayloadFormat.Json, 2) { }
//}

public class UpdateODataTestsV3Atom : UpdateODataTests
{
	public UpdateODataTestsV3Atom() : base(ODataV3ReadWriteUri, ODataPayloadFormat.Atom, 3) { }
}

public class UpdateODataTestsV3Json : UpdateODataTests
{
	public UpdateODataTestsV3Json() : base(ODataV3ReadWriteUri, ODataPayloadFormat.Json, 3) { }
}

public class UpdateODataTestsV4Json : UpdateODataTests
{
	public UpdateODataTestsV4Json() : base(ODataV4ReadWriteUri, ODataPayloadFormat.Json, 4) { }
}

public abstract class UpdateODataTests(string serviceUri, ODataPayloadFormat payloadFormat, int version) : ODataTestBase(serviceUri, payloadFormat, version)
{
	[Fact]
	public async Task UpdateByKey()
	{
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2001, "Test1"))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product["ID"])
			.Set(new { Price = 123m })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.FindEntryAsync();

		product["Price"].Should().Be(123d);
	}

	[Fact]
	public async Task UpdateByFilter()
	{
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2002, "Test1"))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Filter("Name eq 'Test1'")
			.Set(new { Price = 123 })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.FindEntryAsync();

		product["Price"].Should().Be(123d);
	}

	[Fact]
	public async Task UpdateMultipleWithResult()
	{
		_ = await _client
			.For("Products")
			.Set(CreateProduct(2003, "Test1"))
			.InsertEntryAsync();

		var product = (await _client
			.For("Products")
			.Filter("Name eq 'Test1'")
			.Set(new { Price = 123 })
			.UpdateEntriesAsync()).Single();

		Assert.Equal(123d, product["Price"]);
	}

	[Fact]
	public async Task UpdateByObjectAsKey()
	{
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2004, "Test1"))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product)
			.Set(new { Price = 456 })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.FindEntryAsync();

		Assert.Equal(456d, product["Price"]);
	}

	[Fact]
	public async Task UpdateDate()
	{
		var tomorrow = DateTime.Now.AddDays(1);

		var product = await _client
			.For("Products")
			.Set(CreateProduct(2008, "Test1"))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product["ID"])
			.Set(new { ReleaseDate = tomorrow })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.FindEntryAsync();

		Assert.Equal(tomorrow.Date, DateTime.Parse(product["ReleaseDate"].ToString()).Date);
	}

	[Fact]
	public async Task AddSingleAssociation()
	{
		var category = await _client
			.For("Categories")
			.Set(CreateCategory(2011, "Test1"))
			.InsertEntryAsync();
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2012, "Test2"))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product["ID"])
			.Set(new Entry { { ProductCategoryName, ProductCategoryLinkFunc(category) } })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.Expand(ProductCategoryName)
			.FindEntryAsync();
		Assert.Equal(category["ID"], ProductCategoryFunc(product)["ID"]);
	}

	[Fact]
	public async Task UpdateSingleAssociation()
	{
		var category1 = await _client
			.For("Categories")
			.Set(CreateCategory(2013, "Test1"))
			.InsertEntryAsync();
		var category2 = await _client
			.For("Categories")
			.Set(CreateCategory(2014, "Test2"))
			.InsertEntryAsync();
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2015, "Test3", category1))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product["ID"])
			.Set(new Entry { { ProductCategoryName, ProductCategoryLinkFunc(category2) } })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.Expand(ProductCategoryName)
			.FindEntryAsync();
		Assert.Equal(category2["ID"], ProductCategoryFunc(product)["ID"]);
	}

	[Fact]
	public async Task UpdateSingleAssociation_IgnoreNullNavProperties()
	{
		var category1 = await _client
			.For("Categories")
			.Set(CreateCategory(2013, "Test1"))
			.InsertEntryAsync();
		var category2 = await _client
			.For("Categories")
			.Set(CreateCategory(2014, "Test2"))
			.InsertEntryAsync();
		var product = await _client
			.For("Products")
			.Set(CreateProduct(2015, "Test3", category1))
			.InsertEntryAsync();

		await _client
			.For("Products")
			.Key(product["ID"])
			.Set(new Entry { { ProductCategoryName, ProductCategoryLinkFunc(category2) } })
			.UpdateEntryAsync();

		product = await _client
			.For("Products")
			.Key(product["ID"])
			.Expand(ProductCategoryName)
			.FindEntryAsync();
		Assert.Equal(category2["ID"], ProductCategoryFunc(product)["ID"]);

		//@robertmclaws: At this point, we've verified entries and associations are available.
		//               Now, we're going grab another copy to edit, make changes, and verify the removal was ignored with no side effects.

		var _client2 = new ODataClient(CreateDefaultSettings((settings) => settings.IgnoreNavigationPropertiesOnUpdate = true));

		var productToEdit = await _client2
			.For("Products")
			.Key(product["ID"])
			.Expand(ProductCategoryName)
			.FindEntryAsync();

		productToEdit["Name"] = "Test99";
		productToEdit["Categories"] = null;

		await _client2
			.For("Products")
			.Key(product["ID"])
			.Set(productToEdit)
			.UpdateEntryAsync();

		var product2 = await _client2
			.For("Products")
			.Key(product["ID"])
			.Expand(ProductCategoryName)
			.FindEntryAsync();
		Assert.Equal(ProductCategoryFunc(product2)["ID"], ProductCategoryFunc(product)["ID"]);
		Assert.Equal("Test99", product2["Name"]);
	}

	// Not supported for one-to-many relationships
	//[Fact]
	//public async Task RemoveSingleAssociation()
	//{
	//    var category = await _client
	//        .For("Categories")
	//        .Set(CreateCategory(2016, "Test5"))
	//        .InsertEntryAsync();
	//    var product = await _client
	//        .For("Products")
	//        .Set(CreateProduct(2017, "Test6", category))
	//        .InsertEntryAsync();

	//    await _client
	//        .For("Products")
	//        .Key(product["ID"])
	//        .Set(new Entry { { ProductCategoryName, null } })
	//        .UpdateEntryAsync();

	//    product = await _client
	//        .For("Products")
	//        .Key(product["ID"])
	//        .Expand(ProductCategoryName)
	//        .FindEntryAsync();
	//    Assert.Null(product[ProductCategoryName]);
	//}

	[Fact]
	public async Task UpdateMultipleAssociations()
	{
		var category = await _client
			.For("Categories")
			.Set(CreateCategory(2018, "Test3"))
			.InsertEntryAsync();
		var product1 = await _client
			.For("Products")
			.Set(CreateProduct(2019, "Test4"))
			.InsertEntryAsync();
		var product2 = await _client
			.For("Products")
			.Set(CreateProduct(2020, "Test5"))
			.InsertEntryAsync();

		await _client
			.For("Categories")
			.Key(category["ID"])
			.Set(new { Products = new[] { product1, product2 } })
			.UpdateEntryAsync();

		category = await _client
			.For("Categories")
			.Key(category["ID"])
			.Expand("Products")
			.FindEntryAsync();
		Assert.Equal(2, (category["Products"] as IEnumerable<object>).Count());
	}
}
