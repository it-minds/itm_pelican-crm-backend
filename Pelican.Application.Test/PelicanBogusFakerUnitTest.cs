﻿using Pelican.Application.Common;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test;
public class PelicanBogusFakerUnitTest
{
	private IPelicanBogusFaker uut;
	[Fact]
	public void PelicanBogusFakerCreateSingleSupplierTest()
	{
		//Arrange
		uut = new PelicanBogusFaker();
		//Act
		IEnumerable<Supplier> result = uut.SupplierFaker(1);
		//Assert
		Assert.Single(result);
	}
	[Fact]
	public void PelicanBogusFakerCreateManySuppliersTest()
	{
		//Arrange
		uut = new PelicanBogusFaker();
		//Act
		IEnumerable<Supplier> result = uut.SupplierFaker(50);
		//Assert
		Assert.Equal(50, result.Count());
	}
}
