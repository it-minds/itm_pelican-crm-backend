using System;
using System.Linq;
using System.Reflection.Metadata;
using NetArchTest.Rules;
using Xunit;
using Pelican.Application;

namespace Pelican.Architecture.Test;

public class ProjectReferencesTests
{
	private const string APP_NAMESPACE = "Pelican.App";
	private const string DOMAIN_NAMESPACE = "Pelican.Domain";
	private const string APPLICATION_NAMESPACE = "Pelican.Application";
	private const string INFRASTRUCTURE_HUBSPOT_NAMESPACE = "Pelican.Infrastructure.HubSpot";
	private const string INFRASTRUCTURE_PERSISTENCE_NAMESPACE = "Pelican.Infrastructure.Persistence";
	private const string PRESENTATIION_API_NAMESPACE = "Pelican.Presentation.Api";
	private const string PRESENTATIION_GRAPHQL_NAMESPACE = "Pelican.Presentation.GraphQL";

	[Fact]
	public void Domain_NoProjectReferences()
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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

	[Fact]
	public void Application_NoProjectReferences()
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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}
	
	[Fact]
	public void InfrastructureHubSpot_NoProjectReferences()
	{
		// Arrange
		var assembly = typeof(Application.DependencyInjection).Assembly;

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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}
	
	[Fact]
	public void InfrastructurePersistence_NoProjectReferences()
	{
		// Arrange
		var assembly = typeof(Application.DependencyInjection).Assembly;

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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}
	
	[Fact]
	public void PresentationApi_NoProjectReferences()
	{
		// Arrange
		var assembly = typeof(Application.DependencyInjection).Assembly;

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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}
	
	[Fact]
	public void PresentationGraphQL_NoProjectReferences()
	{
		// Arrange
		var assembly = typeof(Application.DependencyInjection).Assembly;

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
			.HaveDependencyOnAll(otherNamespaces)
			.GetResult();

		// Assert
		Assert.True(result.IsSuccessful);
	}

}
