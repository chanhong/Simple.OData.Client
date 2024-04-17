﻿using System.Text;
using FluentAssertions;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Moq;
using Simple.OData.Client.V4.Adapter;
using Xunit;

namespace Simple.OData.Tests.Client.Core;

public class OpenTypesTests : CoreTestBase
{
	public override string MetadataFile => "OpenTypes.xml";
	public override IFormatSettings FormatSettings => new ODataV4Format();

	[Fact]
	public async Task ReadUntypedAsStrings()
	{
		_session.Settings.ReadUntypedAsString = false;
		var response = SetUpResourceMock("OpenTypeV401.json");
		var edmModel = await _client.GetMetadataAsync<IEdmModel>();
		var responseReader = new ResponseReader(_session, edmModel);
		var result = (await responseReader.GetResponseAsync(response)).Feed;
		var entry = result.Entries.First();
		entry.Should().NotBeNull();
		entry.Data["Id"].Should().Be(42m);
		Assert.Equal(43m, entry.Data["IntegerProperty"]);
		Assert.Null(entry.Data["NullProperty"]);
		Assert.Equal("some string", entry.Data["StringProperty"]);
	}

	private static new IODataResponseMessageAsync SetUpResourceMock(string resourceName)
	{
		var document = GetResourceAsString(resourceName);
		var mock = new Mock<IODataResponseMessageAsync>();
		mock.Setup(x => x.GetStreamAsync()).ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(document)));
		mock.Setup(x => x.GetStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(document)));
		mock.Setup(x => x.GetHeader("Content-Type")).Returns(() => "application/json; type=feed; charset=utf-8");
		return mock.Object;
	}
}
