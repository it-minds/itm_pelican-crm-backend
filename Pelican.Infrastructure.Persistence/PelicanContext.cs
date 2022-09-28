﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Pelican.Domain.Entities;
using Pelican.Domain.Primitives;

namespace Pelican.Infrastructure.Persistence;

public class PelicanContext : DbContext
{
	public DbSet<AccountManager> AccountManagers { get; set; }
	public DbSet<AccountManagerDeal> AccountManagerDeals { get; set; }
	public DbSet<Client> Clients { get; set; }
	public DbSet<ClientContactPerson> ClientContactPersons { get; set; }
	public DbSet<ContactPerson> ContactPersons { get; set; }
	public DbSet<Deal> Deals { get; set; }
	public DbSet<DealContactPerson> DealContactPersons { get; set; }
	public DbSet<Supplier> Suppliers { get; set; }
	private readonly bool _inMemory;
	public PelicanContext(DbContextOptions<PelicanContext> options) : base(options)
	{
		_inMemory = options.Extensions.Any(e => e is InMemoryOptionsExtension);
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}

	public override int SaveChanges()
	{
		if (_inMemory)
		{
			this.ValidateIndexes();
			this.ValidateForeignKeys();
		}

		SetTimeTrackedValues();
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		if (_inMemory)
		{
			this.ValidateIndexes();
			this.ValidateForeignKeys();
		}

		SetTimeTrackedValues();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		if (_inMemory)
		{
			this.ValidateIndexes();
			this.ValidateForeignKeys();
		}

		SetTimeTrackedValues();
		return base.SaveChangesAsync(cancellationToken);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
	{
		if (_inMemory)
		{
			this.ValidateIndexes();
			this.ValidateForeignKeys();
		}

		SetTimeTrackedValues();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	private void SetCreatedAtOnAddedEntities()
	{
		var addedTimeTracked = this.ChangeTracker.Entries()
			.Where(t =>
				t.Entity is ITimeTracked &&
				t.State == EntityState.Added)
			.Select(t => t.Entity as ITimeTracked)
			.ToArray();
		foreach (var entity in addedTimeTracked)
		{
			entity.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}
	}
	private void SetLastUpdatedAtOnUpdatedEntities()
	{
		var updatedTimeTracked = this.ChangeTracker.Entries()
			.Where(t =>
				t.Entity is ITimeTracked &&
				t.State == EntityState.Modified)
			.Select(t => t.Entity as ITimeTracked)
			.ToArray();
		foreach (var entity in updatedTimeTracked)
		{
			entity.LastUpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}
	}
	private void SetTimeTrackedValues()
	{
		SetCreatedAtOnAddedEntities();
		SetLastUpdatedAtOnUpdatedEntities();
	}

}
