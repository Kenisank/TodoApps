using Backend.Models;
using Backend.Services;
using Moq;

namespace Backend.Test
{
    public class AuditServiceTests
    {
        private readonly Mock<TodoDbContext> _mockDbContext;
        private readonly AuditService _auditService;

        public AuditServiceTests()
        {
            _mockDbContext = new Mock<TodoDbContext>();
            _auditService = new AuditService(_mockDbContext.Object);
        }

        [Fact]
        public async Task LogActionAsync_ShouldAddAuditRecord()
        {
            // Arrange
            var action = "Logging an action";
            var user = "user@gmail.com";
            var details = "Successful";

            // Act
            await _auditService.LogActionAsync(action, user, details);

            // Assert
            _mockDbContext.Verify(m => m.Audits.Add(It.Is<Audit>(a => a.Action == action && a.User == user && a.Details == details)), Times.Once);
            _mockDbContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}