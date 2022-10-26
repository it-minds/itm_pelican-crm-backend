using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class DealTests
{
	[Fact]
	public void UpdateProperty_NoUpdates_ReturnInputDeal()
	{
		/// Arrange
		string name = "";

		string value = "";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			inputDeal,
			returnDeal);
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
		long value = 1666690255141;

		string name = "closedate";

		string stringValue = value.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		Deal updatedDeal = inputDeal;
		updatedDeal.EndDate = new DateTime(value, DateTimeKind.Utc);

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, stringValue);

		/// Assert
		Assert.Equal(
			updatedDeal,
			returnDeal);
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

		string stringValue = value.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		Deal updatedDeal = inputDeal;
		updatedDeal.Revenue = value;

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, stringValue);

		/// Assert
		Assert.Equal(
			updatedDeal,
			returnDeal);
	}

	[Fact]
	public void UpdateProperty_DealStatusUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "dealstage";

		string value = "newStatus";

		Deal inputDeal = new(Guid.NewGuid());

		Deal updatedDeal = inputDeal;
		updatedDeal.DealStatus = value;

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			updatedDeal,
			returnDeal);
	}

	[Fact]
	public void UpdateProperty_CurrencyCodeUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "deal_currency_code";

		string value = "newCurrencyCode";

		Deal inputDeal = new(Guid.NewGuid());

		Deal updatedDeal = inputDeal;
		updatedDeal.CurrencyCode = value;

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			updatedDeal,
			returnDeal);
	}
}
