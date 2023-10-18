using Core.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbOracleContext : DbContext
    {
        public DbOracleContext()
        {
        }

        public DbOracleContext(DbContextOptions<DbOracleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apoteosys> apoteosys { get; set; }
        public virtual DbSet<OrdenesCompraDetalleApoteosys> requisiconesApoteosys { get; set; }
        public virtual DbSet<OrdenesCompraMaestraApoteosys> OrdenesMaestraApoteosys { get; set; }
        public virtual DbSet<OrdenesCompraAdjudicacionApoteosys> OrdenesCompraAdjudicacions { get; set; }

        public virtual DbSet<EstadoCuentasXPorPagar> EstadoCuentasXPorPagar { get; set; }
        public virtual DbSet<EstadoCuentasXPorPagarDte> EstadoCuentasXPorPagarDte { get; set; }
        public virtual DbSet<EstadoCuentasXPagarDetalle> EstadoCuentasXPagarDetalle { get; set; }

        public virtual DbSet<EstadoCuentasPagadas> EstadoCuentasPagadas { get; set; }
        public virtual DbSet<EstadoCuentasPagadasDte> EstadoCuentasPagadasDte { get; set; }
        public virtual DbSet<EstadoCuentasPagadasDetalle> EstadoCuentasPagadasDetalle { get; set; }
        public virtual DbSet<EstadoCuentasPagadasDetalle_Reporte> EstadoCuentasPagadasDetalle_Reportes { get; set; }

        #region Descuentos Aplicados en Facturas Pagadas
        public virtual DbSet<EstadoCuentasXPagar_FactPagas_Maestro> EstadoCuentasXPagar_FactPagas_Maestro { get; set; }
        public virtual DbSet<EstadoCuentasXPagar_FactPagas_Detalle> EstadoCuentasXPagar_FactPagas_Detalle { get; set; }
        #endregion

        #region Consultas desde SP SQL Server
        public virtual DbSet<FacturasPagas_SQL> FacturasPagas_SQL { get; set; }
        #endregion

        public virtual DbSet<CertificadoRetencionMaestro> CertificadoRetencionMaestros { get; set; }
        public virtual DbSet<CertificadoRetencionFuenteDte> CertificadoRetencionFuenteDtes { get; set; }
        public virtual DbSet<CertificadoRetencionIvaDte> CertificadoRetencionIvaDtes { get; set; }
        public virtual DbSet<CertificadoRetencionIcaDte> CertificadoRetencionIcaDtes { get; set; }
        public virtual DbSet<CertificadoRetencionEstampillaBoyacaDte> CertificadoRetencionEstampillaBoyacaDtes { get; set; }
        public virtual DbSet<SolicitudesApoteosys> SolicitudesApoteosys { get; set; }
        public virtual DbSet<CertificadoExperiencia> CertificadoExperiencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}

