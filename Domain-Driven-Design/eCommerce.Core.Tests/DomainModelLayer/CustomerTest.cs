﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCommerce.DomainModelLayer.Customers;
using Moq;
using FluentAssertions;
using eCommerce.DomainModelLayer.Purchases;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions.Equivalency;
using eCommerce.Helpers.Domain;
using eCommerce.DomainModelLayer.Countries;
using eCommerce.DomainModelLayer.Carts;

namespace eCommerce.Tests.DomainModelLayer
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod, TestCategory("Unit")]
        public void Create_WithCorrectInformation_ReturnsConstructedCustomer()
        {
            //Mock, don't open your "customer object" up just to set properties up.
            //Instead use Mock to setup returns. Mock creates a proxy object against which we can validate.
            Mock<Customer> expected = new Mock<Customer>();
            expected.SetupGet(x => x.FirstName).Returns("John");
            expected.SetupGet(x => x.LastName).Returns("Smith");
            expected.SetupGet(x => x.Email).Returns("john.smith@microsoft.com");
            expected.SetupGet(x => x.CreditCards).Returns(new ReadOnlyCollection<CreditCard>(new List<CreditCard>()));
            expected.SetupGet(x => x.CountryId).Returns(Guid.Empty);

            //call a method 
            Customer actual = Customer.Create(Guid.Empty, "John", "Smith", "john.smith@microsoft.com", new Country());

            //single assert thanks to fluent assertions framework
            actual.Should().BeEquivalentTo(expected.Object);
            //, options => options.Excluding(x => x.PropertyPath == "Cart"));
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithEmailNull_ThrowsException()
        {
            Customer actual = Customer.Create("John", "Smith", null, new Country());
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithFirstNameNull_ThrowsException()
        {
            Customer actual = Customer.Create(null, "Smith", "john.smith@microsoft.com", new Country());
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_WithLastNameNull_ThrowsException()
        {
            Customer actual = Customer.Create("John", null, "Smith", new Country());
        }

        [TestMethod, TestCategory("Unit")]
        public void ChangeEmail_PassedNewEmailAddress_EmailChanged()
        {
            //notice we are not mocking up the entire object but just one area
            Mock<Customer> expected = new Mock<Customer>();
            expected.SetupGet(x => x.Email).Returns("smith.john@microsoft.com");

            //we are creating a while object, but we only care about one property
            Customer actual = Customer.Create("john", "smith", "john.smith@microsoft.com", new Country());
            actual.ChangeEmail("smith.john@microsoft.com");

            //comparing only one property
            actual.Email.Should().BeEquivalentTo(expected.Object.Email);
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(Exception))]
        public void ChangeEmail_PassedNewEmailAddress_ThrowsExceptionEventRaised()
        {
            DomainEvents.Register<CustomerChangedEmail>((evnt) => { throw new Exception("Event Raised"); });

            Customer actual = Customer.Create("john", "smith", "john.smith@microsoft.com", new Country());
            actual.ChangeEmail("smith.john@microsoft.com");
        }

        [TestMethod, TestCategory("Unit")]
        public void ChangeEmail_PassedSameEmailAddress_DoesNotThrowEventRaisedException()
        {
            DomainEvents.Register<CustomerChangedEmail>((evnt) => { throw new Exception("Event Raised"); });

            Customer actual = Customer.Create("john", "smith", "john.smith@microsoft.com", new Country());
            actual.ChangeEmail("john.smith@microsoft.com");
        }

    }
}
