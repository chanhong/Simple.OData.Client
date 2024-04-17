﻿using FluentAssertions;
using Simple.OData.Client;
using Xunit;

namespace Simple.OData.Tests.Client.Core;

public class RequestWriterBatchV3Tests : RequestWriterBatchTests
{
	public override string MetadataFile => "Northwind3.xml";
	public override IFormatSettings FormatSettings => new ODataV3Format();

	protected async override Task<IRequestWriter> CreateBatchRequestWriter()
	{
		return new OData.Client.V3.Adapter.RequestWriter(
			_session,
			await _client.GetMetadataAsync<Microsoft.Data.Edm.IEdmModel>().ConfigureAwait(false),
			new Lazy<IBatchWriter>(() => _session.Adapter.GetBatchWriter(
				new Dictionary<object, IDictionary<string, object>>())));
	}
}

public class RequestWriterBatchV4Tests : RequestWriterBatchTests
{
	public override string MetadataFile => "Northwind4.xml";
	public override IFormatSettings FormatSettings => new ODataV4Format();

	protected async override Task<IRequestWriter> CreateBatchRequestWriter()
	{
		return new OData.Client.V4.Adapter.RequestWriter(
			_session,
			await _client.GetMetadataAsync<Microsoft.OData.Edm.IEdmModel>().ConfigureAwait(false),
			new Lazy<IBatchWriter>(() => BatchWriter));
	}
}

public abstract class RequestWriterBatchTests : CoreTestBase
{
	protected Dictionary<object, IDictionary<string, object>> BatchContent { get; } = new(3);

	protected abstract Task<IRequestWriter> CreateBatchRequestWriter();

	protected IBatchWriter BatchWriter => _session.Adapter.GetBatchWriter(
				BatchContent);

	[Fact]
	public async Task CreateUpdateRequest_NoPreferredVerb_AllProperties_OperationHeaders_Patch()
	{
		var requestWriter = await CreateBatchRequestWriter();

		var result = await requestWriter.CreateUpdateRequestAsync("Products", "",
					new Dictionary<string, object>() { { "ProductID", 1 } },
					new Dictionary<string, object>()
					{
							{ "ProductID", 1 },
							{ "SupplierID", 2 },
							{ "CategoryID", 3 },
							{ "ProductName", "Chai" },
							{ "EnglishName", "Tea" },
							{ "QuantityPerUnit", "10" },
							{ "UnitPrice", 20m },
							{ "UnitsInStock", 100 },
							{ "UnitsOnOrder", 1000 },
							{ "ReorderLevel", 500 },
							{ "Discontinued", false },
					},
					false,
					new Dictionary<string, string>()
					{
							{ "Header1","HeaderValue1"}
					});

		result.Method.Should().Be("PATCH");
		(result.Headers.TryGetValue("Header1", out var value) && value == "HeaderValue1").Should().BeTrue();
		Assert.True(result.RequestMessage.Headers.TryGetValues("Header1", out var values) && values.Contains("HeaderValue1"));
	}
}
