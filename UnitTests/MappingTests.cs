using API.Mappings;
using AutoMapper;
using Integrations.CCP.DTO;
using Integrations.Crayon.Database.Models;
using NUnit.Framework;
using ReadFacade.CCP.DTO;
using WriteFacade.Crayon.DTO;

namespace UnitTests
{
    [TestFixture]
    public class MappingTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Test]
        public void Should_Map_CCPSoftware_To_PurchasableSoftware()
        {
            var source = new CCPSoftware
            {
                Name = "TestSoftware",
                Author = "Test",
                Description = "A test software",
                Id = 1,
                Price = 500
            };

            var result = _mapper.Map<PurchasableSoftware>(source);

            Assert.That(source.Name == result.Name);
            Assert.That(source.Id == result.Id);
            Assert.That(source.Author == result.Author);
            Assert.That(source.Description == result.Description);
            Assert.That(source.Price == result.Price);
        }

        [Test]
        public void Should_Map_License_To_LicenseDto()
        {
            var source = new License
            {
                Id = 1,
                SubscriptionId = 2,
                Key = "ABC123"
            };

            var result = _mapper.Map<LicenseDto>(source);

            Assert.That(source.Id == result.Id);
            Assert.That(source.SubscriptionId == result.SubscriptionId);
            Assert.That(source.Key == result.Key);
        }

        [Test]
        public void Should_Map_Subscription_To_SubscriptionDto()
        {
            var license = new License
            {
                Id = 1,
                SubscriptionId = 2,
                Key = "XYZ789"
            };

            var source = new Subscription
            {
                Id = 1,
                SoftwareId = 1,
                ValidUntil = DateTime.UtcNow.AddMonths(1),
                SoftwareName = "AutoCAD",
                Active = true,
                SoftwareDescription = "CAD tool",
                Licenses = new List<License> { license }
            };

            var result = _mapper.Map<SubscriptionDto>(source);

            Assert.That(source.Id == result.Id);
            Assert.That(source.SoftwareId == result.SoftwareId);
            Assert.That(source.ValidUntil == result.ValidUntil);
            Assert.That(source.SoftwareName == result.SoftwareName);
            Assert.That(source.Active == result.Active);
            Assert.That(source.SoftwareDescription == result.SoftwareDescription);
            Assert.That(result.Licenses is not null);
            Assert.That(source.Licenses.Count == result?.Licenses?.Count);
            Assert.That(license.Id == result?.Licenses?[0]?.Id);
        }

        [Test]
        public void Should_Map_Account_To_AccountDto()
        {
            var subscription = new Subscription
            {
                Id = 1,
                SoftwareId = 2,
                ValidUntil = DateTime.UtcNow.AddMonths(1),
                SoftwareName = "Photoshop",
                Active = true,
                SoftwareDescription = "Image Editor",
                Licenses = new List<License>()
            };

            var source = new Account
            {
                Id = 1,
                CustomerId = 2,
                Name = "John's Account",
                Description = "Business software",
                Subscriptions = new List<Subscription> { subscription }
            };

            var result = _mapper.Map<AccountDto>(source);

            Assert.That(source.Id == result.Id);
            Assert.That(source.CustomerId == result.CustomerId);
            Assert.That(source.Name == result.Name);
            Assert.That(source.Description == result.Description);
            Assert.That(result.Subscriptions is not null);
            Assert.That(subscription.Licenses.Count == result?.Subscriptions?[0].Licenses.Count);
            Assert.That(subscription.Id == result?.Subscriptions?[0].Id);
        }

        [Test]
        public void Should_Map_Customer_To_CustomerDto()
        {
            var now = DateTime.Now;

            var customer = new Customer
            {
                Email = "test@test.com",
                Id = 1,
                Name = "Test",
                PasswordHash = "passwordHash",
                RefreshToken = "123456789",
                RefreshTokenExpiry = now,
                Salt = "salty"
            };

            var result = _mapper.Map<CustomerDto>(customer);

            Assert.That(customer.Salt == result.Salt);
            Assert.That(customer.Email == result.Email);
            Assert.That(customer.Name == result.Name);
            Assert.That(customer.PasswordHash == result.PasswordHash);
            Assert.That(customer.RefreshToken == result.RefreshToken);
            Assert.That(customer.Id == result.Id);
            Assert.That(customer.RefreshTokenExpiry == result.RefreshTokenExpiry);
        }
    }
}
