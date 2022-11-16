using FluentValidation;
using MediatR;
using Moq;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Behaviours;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Behaviours;

public class ValidationBehaviourTests
{
	[Fact]
	public async void Handle_RequestTypeQuery_ThrowNoException()
	{
		// Arrange
		Mock<IEnumerable<IValidator<IRequest<Deal>>>> validatorsMock = new();

		ValidationBehaviour<IRequest<Deal>, Deal> uut = new(validatorsMock.Object);

		Mock<IQuery<Deal>> queryMock = new();

		Mock<RequestHandlerDelegate<Deal>> delegateMock = new();


		Deal deal = new Deal(Guid.NewGuid());

		delegateMock
			.Setup(d => d())
			.Returns(Task.Run(() => deal));

		// Act
		var result = await uut.Handle(
			queryMock.Object,
			delegateMock.Object,
			default);

		// Assert
		Assert.Equal(
			deal,
			result);
	}

	[Fact]
	public async void Handle_RequestTypeCommand_ThrowValidationException()
	{
		// Arrange
		Mock<List<IValidator<IRequest<Result<Deal>>>>> validatorsMock = new();

		ValidationBehaviour<IRequest<Result<Deal>>, Result<Deal>> uut = new(validatorsMock.Object);

		Mock<IValidator<IRequest<Result<Deal>>>> validatorMock = new();

		Mock<FluentValidation.Results.ValidationResult> validationResultMock = new();

		validationResultMock.Object.Errors = new List<FluentValidation.Results.ValidationFailure>()
		{
			new FluentValidation.Results.ValidationFailure(),
		};

		validatorsMock.Object.Add(validatorMock.Object);

		validatorMock
			.Setup(v => v.Validate(It.IsAny<ValidationContext<IRequest<Result<Deal>>>>()))
			.Returns(validationResultMock.Object);

		Mock<ICommand<Deal>> commandMock = new();

		Mock<RequestHandlerDelegate<Result<Deal>>> delegateMock = new();

		Result<Deal> dealResult = new Deal(Guid.NewGuid());

		delegateMock
			.Setup(d => d())
			.Returns(Task.Run(() => dealResult));

		// Act
		var result = await Record.ExceptionAsync(() => uut
			.Handle(
				commandMock.Object,
				delegateMock.Object,
				default));

		// Assert
		Assert.Equal(
			typeof(ValidationException),
			result.GetType());
	}

	[Fact]
	public async void Handle_RequestTypeCommand_ThrowNoException()
	{
		// Arrange
		Mock<List<IValidator<IRequest<Result<Deal>>>>> validatorsMock = new();

		ValidationBehaviour<IRequest<Result<Deal>>, Result<Deal>> uut = new(validatorsMock.Object);

		Mock<FluentValidation.Results.ValidationResult> validationResultMock = new();

		Mock<IValidator<IRequest<Result<Deal>>>> validatorMock = new();

		validatorMock
			.Setup(v => v.Validate(It.IsAny<ValidationContext<IRequest<Result<Deal>>>>()))
			.Returns(validationResultMock.Object);

		validatorsMock.Object.Add(validatorMock.Object);

		Mock<ICommand<Deal>> commandMock = new();

		Mock<RequestHandlerDelegate<Result<Deal>>> delegateMock = new();

		Result<Deal> dealResult = new Deal(Guid.NewGuid());

		delegateMock
			.Setup(d => d())
			.Returns(Task.Run(() => dealResult));

		// Act
		var result = await uut.Handle(
			commandMock.Object,
			delegateMock.Object,
			default);

		// Assert
		Assert.Equal(
			dealResult,
			result);
	}

}
