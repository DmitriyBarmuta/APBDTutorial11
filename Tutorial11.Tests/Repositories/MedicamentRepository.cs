using Tutorial11.Models;
using Tutorial11.Tests.Utils;

namespace Tutorial11.Tests.Repositories;

public class MedicamentRepository
{
    [Fact]
    public async Task AllExists_ReturnsTrue_WhenAllExists()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        var medicament = new Medicament
        {
            IdMedicament = 1,
            Description = "Some description",
            Name = "Some med name #1",
            Type = "I don't even know what to write here"
        };
        var medicament2 = new Medicament
        {
            IdMedicament = 2,
            Description = "Some description",
            Name = "Some med name #1",
            Type = "I don't even know what to write here"
        };
        context.Medicaments.AddRange(medicament, medicament2);
        await context.SaveChangesAsync();

        var repo = new Tutorial11.Repositories.MedicamentRepository(context);

        var exists = await repo.AllExistAsync(new[] { 1, 2 }, CancellationToken.None);
        
        Assert.True(exists);
    }

    [Fact]
    public async Task AllExists_ReturnsFalse_WhenAnyNotFound()
    {
        var context = TestUtils.CreateInMemoryDatabaseContext();
        
        var repo = new Tutorial11.Repositories.MedicamentRepository(context);

        var exists = await repo.AllExistAsync(new[] { 1, 2 }, CancellationToken.None);
        
        Assert.False(exists);
    }
}