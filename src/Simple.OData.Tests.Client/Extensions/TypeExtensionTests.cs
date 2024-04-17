﻿using FluentAssertions;
using Simple.OData.Client.Extensions;
using Xunit;

namespace Simple.OData.Tests.Client;

public class TypeExtensionTests
{
	[Fact]
	public void GetAllProperties_BaseType()
	{
		Assert.Single(typeof(Transport).GetAllProperties());
	}

	[Fact]
	public void GetAllProperties_DerivedType()
	{
		Assert.Equal(2, typeof(Ship).GetAllProperties().Count());
	}

	[Fact]
	public void GetAllProperties_SkipIndexer()
	{
		Assert.Single(typeof(TypeWithIndexer).GetAllProperties());
	}

	[Fact]
	public void GetDeclaredProperties_ExcludeExplicitInterface()
	{
		Assert.Equal(5, typeof(Address).GetAllProperties().Count());
	}

	[Fact]
	public void GetDeclaredProperties_BaseType()
	{
		Assert.Single(typeof(Transport).GetDeclaredProperties());
	}

	[Fact]
	public void GetDeclaredProperties_DerivedType()
	{
		Assert.Single(typeof(Ship).GetDeclaredProperties());
	}

	[Fact]
	public void GetNamedProperty_BaseType()
	{
		typeof(Transport).GetNamedProperty("TransportID").Should().NotBeNull();
	}

	[Fact]
	public void GetNamedProperty_DerivedType()
	{
		typeof(Ship).GetNamedProperty("TransportID").Should().NotBeNull();
		Assert.NotNull(typeof(Ship).GetNamedProperty("ShipName"));
	}

	[Fact]
	public void GetDeclaredProperty_BaseType()
	{
		Assert.NotNull(typeof(Transport).GetDeclaredProperty("TransportID"));
	}

	[Fact]
	public void GetDeclaredProperty_DerivedType()
	{
		Assert.Null(typeof(Ship).GetDeclaredProperty("TransportID"));
		Assert.NotNull(typeof(Ship).GetDeclaredProperty("ShipName"));
	}
}
