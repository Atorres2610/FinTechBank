using Cliente.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cliente.Tests
{
    public class BasePruebas
    {
        protected FinTechBankContext ConstruirContext(string nombreDb)
        {
            var opciones = new DbContextOptionsBuilder<FinTechBankContext>()
                .UseInMemoryDatabase(nombreDb).Options;

            return new FinTechBankContext(opciones);
        }
    }
}