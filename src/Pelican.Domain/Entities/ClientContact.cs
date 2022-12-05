﻿using HotChocolate;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public class ClientContact : Entity, ITimeTracked
{
	public ClientContact(Guid id) : base(id) { }

	public ClientContact() { }


	public bool IsActive { get; set; }


	public Guid ClientId { get; set; }

	public string SourceClientId { get; set; } = string.Empty;

	public Client Client { get; set; }


	public string SourceContactId { get; set; } = string.Empty;

	public Guid ContactId { get; set; }

	public Contact Contact { get; set; }


	public long CreatedAt { get; set; }

	public long? LastUpdatedAt { get; set; }


	[GraphQLIgnore]
	public static ClientContact Create(Client client, Contact contact)
	{
		return new(Guid.NewGuid())
		{
			Client = client,
			ClientId = client.Id,
			SourceClientId = client.SourceId,
			Contact = contact,
			ContactId = contact.Id,
			SourceContactId = contact.SourceId,
			IsActive = true,
		};
	}

	[GraphQLIgnore]
	public virtual void Deactivate() => IsActive = false;
}
