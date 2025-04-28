using Common.Requests;
using Integrations.Crayon;
using Integrations.Crayon.Database.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class CrayonIntegrationTests
    {
        private CrayonClient _client;
        private CrayonDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            // Setup InMemory Database for testing
            var options = new DbContextOptionsBuilder<CrayonDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new CrayonDbContext(options);
            _client = new CrayonClient(_dbContext);
        }

        [Test]
        public async Task CreateCustomer_ShouldCreateAndDeleteCustomer()
        {
            // Arrange: Create a new customer request
            var newCustomerRequest = new NewCustomerRequest
            {
                Email = "test@test.com",
                Name = "Test Customer",
                PasswordHash = "hashedPassword",
                Salt = "randomSalt"
            };

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

            // Act: Create the new customer
            var createdCustomer = await _client.CreateNewCustomer(newCustomerRequest, cancellationTokenSource.Token);

            // Assert: Check if the customer was created successfully
            var customerFromDb = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Email == newCustomerRequest.Email, cancellationTokenSource.Token);

            Assert.That(customerFromDb is not null);
            Assert.That(newCustomerRequest.Email == customerFromDb.Email);
            Assert.That(newCustomerRequest.Name == customerFromDb.Name);

            // cleanup
            _dbContext.Customers.Remove(customerFromDb);
            await _dbContext.SaveChangesAsync(cancellationTokenSource.Token);

            // ensure the customer is deleted
            var deletedCustomer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Email == newCustomerRequest.Email, cancellationTokenSource.Token);

            Assert.That(deletedCustomer is null);

            cancellationTokenSource.Dispose();
        }
    }
}
