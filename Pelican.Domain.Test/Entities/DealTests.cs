using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class DealTests
{
	[Fact]
	public void UpdateProperty_NoUpdates_ThrowsNoException()
	{
		/// Arrange
		string name = "";

		string value = "";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Null(exceptionResult);
	}

	[Fact]
	public void UpdateProperty_EndDateUpdatedInvalidValueFormat_ThrowsException()
	{
		/// Arrange
		string name = "closedate";

		string value = "Hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_EndDateUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		DateTime date = new(2022, 11, 25);

		long ticks = 1669382373249; //Timestamp in milliseconds Friday, November 25, 2022 1:19:33.249 PM

		string name = "closedate";

		string value = ticks.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			date,
			returnDeal.EndDate);
	}

	[Fact]
	public void UpdateProperty_RevenueUpdatedInvalidValueFormat_ThrowsException()
	{
		/// Arrange
		string name = "amount";

		string value = "hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_RevenueUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		decimal value = (decimal)10.4;

		string name = "amount";

		string stringValue = "10.4";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, stringValue);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.Revenue);
	}

	[Fact]
	public void UpdateProperty_DealStatusUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "dealstage";

		string value = "newStatus";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.DealStatus);
	}

	[Fact]
	public void UpdateProperty_CurrencyCodeUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "deal_currency_code";

		string value = "newCurrencyCode";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.CurrencyCode);
	}
}
