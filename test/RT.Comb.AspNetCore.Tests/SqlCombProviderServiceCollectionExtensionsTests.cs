﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace RT.Comb.AspNetCore.Tests
{
	public sealed class SqlCombProviderServiceCollectionExtensionsTests
	{
		[Fact]
		public void AddSqlCombGuidWithSqlDateTime_ShouldAddSqlProviderWithDefaultStrategies()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddSqlCombGuidWithSqlDateTime();

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var comb = serviceProvider.GetService<ICombProvider>();

			var sqlCombProvider = comb as SqlCombProvider;

			Assert.NotNull(sqlCombProvider);
			Assert.IsType<SqlDateTimeStrategy>(GetCurrentDateTimeStrategy(sqlCombProvider));
			Assert.IsType<TimestampProvider>(sqlCombProvider.TimestampProvider);
			Assert.IsType<GuidProvider>(sqlCombProvider.GuidProvider);
		}

		[Fact]
		public void AddSqlCombGuidWithSqlDateTime_CustomStrategies_CreateShouldInvokeCustomStrategies()
		{
			// Arrange
			var services = new ServiceCollection();

			// custom timestamp and guid strategies
			var customTimestampProviderMock = new Mock<TimestampProvider>();
			customTimestampProviderMock.Setup(p => p.Invoke()).Returns(DateTime.Now);

			var customGuidProviderMock = new Mock<GuidProvider>();
			customGuidProviderMock.Setup(p => p.Invoke()).Returns(Guid.NewGuid());

			services.AddSqlCombGuidWithSqlDateTime(customTimestampProviderMock.Object, customGuidProviderMock.Object);
			var serviceProvider = services.BuildServiceProvider();
			var comb = serviceProvider.GetService<ICombProvider>();

			// Act
			var _ = comb.Create();

			// Assert
			customTimestampProviderMock.Verify(p => p.Invoke(), Times.Once);
			customGuidProviderMock.Verify(p => p.Invoke(), Times.Once);
		}

		[Fact]
		public void AddSqlCombGuidWithUnixDateTime_ShouldAddSqlProviderWithDefaultStrategies()
		{
			// Arrange
			var services = new ServiceCollection();

			// Act
			services.AddSqlCombGuidWithUnixDateTime();

			// Assert
			var serviceProvider = services.BuildServiceProvider();
			var comb = serviceProvider.GetService<ICombProvider>();

			var sqlCombProvider = comb as SqlCombProvider;

			Assert.NotNull(sqlCombProvider);
			Assert.IsType<UnixDateTimeStrategy>(GetCurrentDateTimeStrategy(sqlCombProvider));
			Assert.IsType<TimestampProvider>(sqlCombProvider.TimestampProvider);
			Assert.IsType<GuidProvider>(sqlCombProvider.GuidProvider);
		}

		[Fact]
		public void AddSqlCombGuidWithUnixDateTime_CustomStrategies_CreateShouldInvokeCustomStrategies()
		{
			// Arrange
			var services = new ServiceCollection();

			// custom timestamp and guid strategies
			var customTimestampProviderMock = new Mock<TimestampProvider>();
			customTimestampProviderMock.Setup(p => p.Invoke()).Returns(DateTime.Now);

			var customGuidProviderMock = new Mock<GuidProvider>();
			customGuidProviderMock.Setup(p => p.Invoke()).Returns(Guid.NewGuid());

			services.AddSqlCombGuidWithUnixDateTime(customTimestampProviderMock.Object, customGuidProviderMock.Object);
			var serviceProvider = services.BuildServiceProvider();
			var comb = serviceProvider.GetService<ICombProvider>();

			// Act
			var _ = comb.Create();

			// Assert
			customTimestampProviderMock.Verify(p => p.Invoke(), Times.Once);
			customGuidProviderMock.Verify(p => p.Invoke(), Times.Once);
		}

		private ICombDateTimeStrategy GetCurrentDateTimeStrategy(ICombProvider combProvider)
		{
			var dateTimeStragetyField = combProvider
				.GetType()
				.GetField("_dateTimeStrategy", BindingFlags.NonPublic | BindingFlags.Instance);

			return dateTimeStragetyField.GetValue(combProvider) as ICombDateTimeStrategy;
		}
	}
}
