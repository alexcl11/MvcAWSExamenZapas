using Microsoft.EntityFrameworkCore;
using MvcAWSExamenZapas.Data;
using MvcAWSExamenZapas.Models;

namespace MvcAWSExamenZapas.Repositories
{
    public class RepositoryZapas
    {
        private ZapasContext context;
        public RepositoryZapas(ZapasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync() 
        {
            return await this.context.Zapatillas.ToListAsync();
        }
        public async Task<int> GetMaxIdAsync()
        {
            return await this.context.Zapatillas.MaxAsync(x => x.IdProducto) + 1;
        }
        public async Task CreateZapatillaAsync(string nombre, string desripcion, string imagen)
        {
            Zapatilla zapa = new Zapatilla
            {
                IdProducto = await this.GetMaxIdAsync(),
                Nombre = nombre,
                Descripcion = desripcion,
                Imagen = imagen
            };
            await this.context.Zapatillas.AddAsync(zapa);
            await this.context.SaveChangesAsync();
        }
    }
}
