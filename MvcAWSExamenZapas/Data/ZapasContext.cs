using Microsoft.EntityFrameworkCore;
using MvcAWSExamenZapas.Models;

namespace MvcAWSExamenZapas.Data
{
    public class ZapasContext: DbContext
    {
        public ZapasContext(DbContextOptions<ZapasContext> options) : base(options) { }
        public DbSet<Zapatilla> Zapatillas { get; set; }
    }
}
