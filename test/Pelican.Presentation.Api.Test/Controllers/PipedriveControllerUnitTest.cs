namespace Pelican.Presentation.Api.Test.Controllers;
public class PipedriveControllerUnitTest
{
	private readonly PipedriveController _uut;
	private readonly Mock<ISender> _senderMock;

	public PipedriveControllerUnitTest()
	{
		_senderMock = new Mock<ISender>();
		_uut = new(_senderMock.Object);
	}
	[Theory]
	[InlineData("code", "0", "fail")]
	public async void NewInstallation_SenderReturnsFailure_ReturnsBadRequest(
		string code,
		string errorCode,
		string errorMessage)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationPipedriveCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationPipedriveCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("code")]
	public async void NewInstallation_SenderReturnsSuccess_ReturnsOk(string code)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationPipedriveCommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationPipedriveCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<RedirectResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void UpdateDeal_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateDealRequest dealRequest = new();

		//Act
		IActionResult result = await _uut.UpdateDeal(dealRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateDealPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("TestDescription", "TestName", 1, "TestContactDate", 1, 1, 1)]
	public async void UpdateDeal_ReceivesCallAndReturnsSuccess_SuccessIsReturned(
		string dealDescription,
		string dealName,
		int dealStatusId,
		string lastContactDate,
		int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);
		UpdateDealCurrentProperties updateDealCurrentProperties = new()
		{
			DealDescription = dealDescription,
			DealName = dealName,
			DealStatusId = dealStatusId,
			LastContactDate = lastContactDate,
		};
		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		UpdateDealRequest dealRequest = new()
		{
			CurrentProperties = updateDealCurrentProperties,
			MetaProperties = metaProperties,
		};
		UpdateDealPipedriveCommand expectedCommand = new(
			supplierPipedriveId,
			objectId,
			userId,
			dealStatusId,
			dealDescription,
			dealName,
			lastContactDate,
			null,
			null);

		//Act
		IActionResult result = await _uut.UpdateDeal(dealRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
					expectedCommand,
					default),
				Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void DeleteDeal_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		DeleteDealRequest dealRequest = new();

		//Act
		IActionResult result = await _uut.DeleteDeal(dealRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<DeleteDealPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData(1, 1, 1)]
	public async void DeleteDeal_ReceivesCallAndReturnsSuccess_SuccessIsReturned(int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		DeleteDealRequest dealRequest = new()
		{
			MetaProperties = metaProperties,
		};
		DeleteDealPipedriveCommand expectedCommand = new(objectId);

		//Act
		IActionResult result = await _uut.DeleteDeal(dealRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				expectedCommand,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void UpdateAccountManager_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateAccountManagerRequest accountManagerRequest = new();

		//Act
		IActionResult result = await _uut.UpdateAccountManager(accountManagerRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateAccountManagerPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("Anton Meldgaard Esbjerg", "TestMail", "TestPhoneNumber", "TestPictureUrl", 1, 1, 1)]
	public async void UpdateAccountManager_ReceivesCallAndReturnsSuccess_SuccessIsReturned(
		string testFullName,
		string testEmail,
		string testPhoneNumber,
		string testPictureUrl,
		int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);
		UpdateAccountManagerCurrentProperties updateAccountManagerCurrentProperties = new()
		{
			AccountManagerFullName = testFullName,
			Email = testEmail,
			PhoneNumber = testPhoneNumber,
			PictureUrl = testPictureUrl,
		};
		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		UpdateAccountManagerRequest accountManagerRequest = new()
		{
			CurrentProperties = updateAccountManagerCurrentProperties,
			MetaProperties = metaProperties,
		};
		UpdateAccountManagerPipedriveCommand expectedCommand = new(
			supplierPipedriveId,
			objectId,
			userId,
			"Anton Meldgaard",
			"Esbjerg",
			testPictureUrl,
			testPhoneNumber,
			testEmail,
			null);

		//Act
		IActionResult result = await _uut.UpdateAccountManager(accountManagerRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
					expectedCommand,
					default),
				Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void DeleteAccountManager_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		DeleteAccountManagerRequest accountManagerRequest = new();

		//Act
		IActionResult result = await _uut.DeleteAccountManager(accountManagerRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<DeleteAccountManagerPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData(1, 1, 1)]
	public async void DeleteAccountManager_ReceivesCallAndReturnsSuccess_SuccessIsReturned(int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		DeleteAccountManagerRequest accountManagerRequest = new()
		{
			MetaProperties = metaProperties,
		};
		DeleteAccountManagerPipedriveCommand expectedCommand = new(objectId);

		//Act
		IActionResult result = await _uut.DeleteAccountManager(accountManagerRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				expectedCommand,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void UpdateClient_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateClientRequest clientRequest = new();

		//Act
		IActionResult result = await _uut.UpdateClient(clientRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateClientPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("TestName", "TestLocation", 1, 1, 1)]
	public async void UpdateClient_ReceivesCallAndReturnsSuccess_SuccessIsReturned(
		string clientName,
		string officeLocation,
		int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);
		UpdateClientCurrentProperties updateClientCurrentProperties = new()
		{
			ClientName = clientName,
			OfficeLocation = officeLocation
		};
		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		UpdateClientRequest clientRequest = new()
		{
			CurrentProperties = updateClientCurrentProperties,
			MetaProperties = metaProperties,
		};
		UpdateClientPipedriveCommand expectedCommand = new(
			supplierPipedriveId,
			objectId,
			userId,
			clientName,
			null,
			officeLocation,
			null);

		//Act
		IActionResult result = await _uut.UpdateClient(clientRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				expectedCommand,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void DeleteClient_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		DeleteClientRequest clientRequest = new();

		//Act
		IActionResult result = await _uut.DeleteClient(clientRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<DeleteClientPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData(1, 1, 1)]
	public async void DeleteClient_ReceivesCallAndReturnsSuccess_SuccessIsReturned(int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		DeleteClientRequest clientRequest = new()
		{
			MetaProperties = metaProperties,
		};
		DeleteClientPipedriveCommand expectedCommand = new(
			objectId);

		//Act
		IActionResult result = await _uut.DeleteClient(clientRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				expectedCommand,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void UpdateContact_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateContactRequest contactRequest = new();

		//Act
		IActionResult result = await _uut.UpdateContact(contactRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateContactPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("TestFirstName", "TestLastName", "TestUrl", "TestLabelOfNotPrimary", "TestValueOfNotPrimary", false, "TestLabelOfPrimary", "TestValueOfPrimary", true, 1, 1, 1)]
	public async void UpdateContact_ReceivesCallAndReturnsSuccess_SuccessIsReturned(
		string firstName,
		string lastName,
		string pictureUrl,
		string labelOfNotPrimary,
		string valueOfNotPrimary,
		bool isPrimaryOfNotPrimary,
		string labelOfPrimary,
		string valueOfPrimary,
		bool isPrimaryOfPrimary,
		int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		List<ContactItem> testContactItem = new();
		testContactItem.Add(new()
		{
			Label = labelOfNotPrimary,
			Value = valueOfNotPrimary,
			Primary = isPrimaryOfNotPrimary
		});
		testContactItem.Add(new()
		{
			Label = labelOfPrimary,
			Value = valueOfPrimary,
			Primary = isPrimaryOfPrimary
		});

		UpdateContactCurrentProperties updateContactCurrentProperties = new()
		{
			FirstName = firstName,
			LastName = lastName,
			PictureUrl = pictureUrl,
			Email = testContactItem,
			PhoneNumber = testContactItem,
		};
		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		UpdateContactRequest contactRequest = new()
		{
			CurrentProperties = updateContactCurrentProperties,
			MetaProperties = metaProperties,
		};
		UpdateContactPipedriveCommand expectedCommand = new(
			supplierPipedriveId,
			objectId,
			userId,
			firstName,
			lastName,
			pictureUrl,
			valueOfPrimary,
			valueOfPrimary,
			null);

		//Act
		IActionResult result = await _uut.UpdateContact(contactRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
					expectedCommand,
					default),
				Times.Once);

		Assert.IsType<OkResult>(result);
	}
}
