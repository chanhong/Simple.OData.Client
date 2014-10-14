﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Simple.OData.Client.Tests
{
    public class InsertTypedTests : TestBase
    {
        [Fact]
        public async Task Insert()
        {
            var product = await _client
                .For<Product>()
                .Set(new { ProductName = "Test1", UnitPrice = 18m })
                .InsertEntryAsync();

            Assert.Equal("Test1", product.ProductName);
        }

        [Fact]
        public async Task InsertAutogeneratedID()
        {
            var product = await _client
                .For<Product>()
                .Set(new { ProductName = "Test1", UnitPrice = 18m })
                .InsertEntryAsync();

            Assert.True(product.ProductID > 0);
            Assert.Equal("Test1", product.ProductName);
        }

        [Fact]
        public async Task InsertWithMappedColumn()
        {
            var product = await _client
                .For<Product>()
                .Set(new Product { ProductName = "Test1", UnitPrice = 18m, MappedEnglishName  = "EnglishTest" })
                .InsertEntryAsync();

            Assert.Equal("Test1", product.ProductName);
            Assert.Equal("EnglishTest", product.MappedEnglishName);
        }

        [Fact]
        public async Task InsertProductWithCategoryByID()
        {
            var category = await _client
                .For<Category>()
                .Set(new { CategoryName = "Test3" })
                .InsertEntryAsync();
            var product = await _client
                .For<Product>()
                .Set(new { ProductName = "Test4", UnitPrice = 18m, CategoryID = category.CategoryID })
                .InsertEntryAsync();

            Assert.Equal("Test4", product.ProductName);
            Assert.Equal(category.CategoryID, product.CategoryID);
            category = await _client
                .For<Category>()
                .Expand(x => new { x.Products })
                .Filter(x => x.CategoryName == "Test3")
                .FindEntryAsync();
            Assert.True(category.Products.Count() == 1);
        }

        [Fact]
        public async Task InsertProductWithCategoryByAssociation()
        {
            var category = await _client
                .For<Category>()
                .Set(new { CategoryName = "Test5" })
                .InsertEntryAsync();
            var product = await _client
                .For<Product>()
                .Set(new { ProductName = "Test6", UnitPrice = 18m, Category = category })
                .InsertEntryAsync();

            Assert.Equal("Test6", product.ProductName);
            Assert.Equal(category.CategoryID, product.CategoryID);
            category = await _client
                .For<Category>()
                .Expand(x => new { x.Products })
                .Filter(x => x.CategoryName == "Test5")
                .FindEntryAsync();
            Assert.True(category.Products.Count() == 1);
        }
        
        [Fact]
        public async Task InsertShip()
        {
            var ship = await _client
                .For<Transport>()
                .As<Ship>()
                .Set(new Ship { ShipName = "Test1" })
                .InsertEntryAsync();

            Assert.Equal("Test1", ship.ShipName);
        }
    }
}
