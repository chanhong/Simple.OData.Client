﻿using FluentAssertions;
using Xunit;

namespace Simple.OData.Tests.Client.Core;

public class StreamTests : CoreTestBase
{
	public override string MetadataFile => "TripPin.xml";
	public override IFormatSettings FormatSettings => new ODataV4Format();

	private class Photo
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public byte[] Media { get; set; }
	}

	[Fact]
	public async Task GetMediaStream()
	{
		var command = _client
			.For<Photo>()
			.Key(1)
			.QueryOptions(new Dictionary<string, object>() { { "IntOption", 42 }, { "StringOption", "xyz" } })
			.Media();
		var commandText = await command.GetCommandTextAsync();
		commandText.Should().Be("Photos(1)/$value?IntOption=42&StringOption='xyz'");
	}
}
