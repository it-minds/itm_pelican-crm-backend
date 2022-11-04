using NetArchTest.Rules;
using Xunit;

namespace Pelican.Architecture.Test;

public class ProjectReferencesTests
{
	private const string APP_NAMESPACE = "Pelican.App";
	private const string APPLICATION_NAMESPACE = "Pelican.Application";
	private const string INFRASTRUCTURE_HUBSPOT_NAMESPACE = "Pelican.Infrastructure.HubSpot";
	private const string INFRASTRUCTURE_PERSISTENCE_NAMESPACE = "Pelican.Infrastructure.Persistence";
	private const string PRESENTATIION_API_NAMESPACE = "Pelican.Presentation.Api";
	private const string PRESENTATIION_GRAPHQL_NAMESPACE = "Pelican.Presentation.GraphQL";

	[Fact]
	public void Domain_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Domain.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			APPLICATION_NAMESPACE,
			INFRASTRUCTURE_HUBSPOT_NAMESPACE,
			INFRASTRUCTURE_PERSISTENCE_NAMESPACE,
			PRESENTATIION_API_NAMESPACE,
			PRESENTATIION_GRAPHQL_NAMESPACE,
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Application_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Application.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			INFRASTRUCTURE_HUBSPOT_NAMESPACE,
			INFRASTRUCTURE_PERSISTENCE_NAMESPACE,
			PRESENTATIION_API_NAMESPACE,
			PRESENTATIION_GRAPHQL_NAMESPACE,
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void InfrastructureHubSpot_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Infrastructure.HubSpot.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			INFRASTRUCTURE_PERSISTENCE_NAMESPACE,
			PRESENTATIION_API_NAMESPACE,
			PRESENTATIION_GRAPHQL_NAMESPACE,
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void InfrastructurePersistence_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Infrastructure.Persistence.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			INFRASTRUCTURE_HUBSPOT_NAMESPACE,
			PRESENTATIION_API_NAMESPACE,
			PRESENTATIION_GRAPHQL_NAMESPACE,
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void PresentationApi_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Presentation.Api.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			INFRASTRUCTURE_HUBSPOT_NAMESPACE,
			INFRASTRUCTURE_PERSISTENCE_NAMESPACE,
			PRESENTATIION_GRAPHQL_NAMESPACE,
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void PresentationGraphQL_ShouldNotDependOn()
	{
		// Arrange
		var assembly = typeof(Presentation.GraphQL.DependencyInjection).Assembly;

		var otherNamespaces = new[]
		{
			APP_NAMESPACE,
			INFRASTRUCTURE_HUBSPOT_NAMESPACE,
			INFRASTRUCTURE_PERSISTENCE_NAMESPACE,
			PRESENTATIION_API_NAMESPACE
		};

		// Act 
		var result = Types
			.InAssembly(assembly)
			.ShouldNot()
			.HaveDependencyOnAny(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

}
