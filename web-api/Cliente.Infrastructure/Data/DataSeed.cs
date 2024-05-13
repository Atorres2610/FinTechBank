using Cliente.Infrastructure.Entities;

namespace Cliente.Infrastructure.Data
{
    public class DataSeed
    {
        private readonly FinTechBankContext context;

        public DataSeed(FinTechBankContext context)
        {
            this.context = context;
        }

        public async Task EjecutarAsync()
        {
            await context.Database.EnsureCreatedAsync();
            await CheckGeneros();
            await CheckTiposCliente();
        }

        private async Task CheckGeneros()
        {
            if (!context.Generos.Any())
            {
                List<Genero> generos =
                [
                    new Genero { Nombre = "Masculino" },
                    new Genero { Nombre = "Femenino" },
                    new Genero { Nombre = "Prefiero no decirlo" },
                    new Genero { Nombre = "Otro" },
                ];

                await context.Generos.AddRangeAsync(generos);
                await context.SaveChangesAsync();
            }
        }

        private async Task CheckTiposCliente()
        {
            if (!context.TiposCliente.Any())
            {
                List<TipoCliente> tiposCliente =
                [
                    new TipoCliente { Nombre = "Individual" },
                    new TipoCliente { Nombre = "Corporativo" }
                ];

                await context.TiposCliente.AddRangeAsync(tiposCliente);
                await context.SaveChangesAsync();
            }
        }
    }
}