using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApoteosysRepository
    {
        #region Órdenes
        Task<List<OrdenesCompraDetalleApoteosys>> ConsultaOrdenesCompra(QuerySearchOrdenesCompra parameters);
        Task<List<OrdenesCompraMaestraApoteosys>> ConsultaMaestraOrdenesCompra(QuerySearchMaestraOrdenesCompra parameters);
        Task<List<OrdenesCompraAdjudicacionApoteosys>> ConsultaOrdenesParaAdjudicacionRequerimiento(QuerySearchOrdenesAdjudicacion parameters);
        #endregion

        #region Facturación
        #region Facturas Por Pagar
        Task<List<EstadoCuentasXPagarDetalle>> EstadoCuentasXPorPagarDetalleApoteosys(QuerySearchEstadoCuentasXPagarDetalle parameters);
        Task<List<EstadoCuentasXPagarDetalle>> EstadoCuentasXPorPagarDetalleApoteosys_Reporte(QuerySearchEstadoCuentasXPagarDetalle parameters);        
        #endregion

        #region Facturas Pagadas
        Task<List<EstadoCuentasPagadas>> EstadoCuentasPagadasApoteosys(QuerySearchEstadoCuentasPagadas parameters, List<Empresa> empresasActivas);
        Task<List<EstadoCuentasPagadasDetalle>> EstadoCuentasPagadasDetalleApoteosys(QuerySearchEstadoCuentasPagadasDetalle parameters);
        Task<List<EstadoCuentasPagadasDetalle_Reporte>> EstadoCuentasPagadasDetalleApoteosys_Reporte(QuerySearchEstadoCuentasPagadasDetalle parameters);

        #region Descuentos Aplicados en Facturas Pagadas
        Task<List<EstadoCuentasXPagar_FactPagas_Maestro>> EstadoCuentasXPorPagar_FactPagas_Maestro(QuerySearchEstadoCuentasXPagar_FactPagas parameters);
        Task<List<EstadoCuentasXPagar_FactPagas_Detalle>> EstadoCuentasXPorPagar_FactPagas_Detalle(QuerySearchEstadoCuentasXPagar_FactPagas parameters);
        #endregion

        #region Consultas desde SP SQL Server
        Task<List<FacturasPagas_SQL>> GetFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters);
        Task<List<FacturasPagas_SQL>> GetRetencionesFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters);
        #endregion
        #endregion
        #endregion

        #region Certificados
        Task<List<CertificadoRetencionMaestro>> CertificadosRetencionApoteosys(QuerySearchCertificados parameters);
        Task<List<CertificadoExperiencia>> CertificadosExperienciaApoteosys(QuerySearchCertificados parameters);
        Task<List<CertificadoRetencionFuenteDte>> CertificadosRetencionFuenteApoteosys(QuerySearchCertificados parameters);
        Task<List<CertificadoRetencionIvaDte>> CertificadosRetencionIvaApoteosys(QuerySearchCertificados parameters);
        Task<List<CertificadoRetencionIcaDte>> CertificadosRetencionIcaApoteosys(QuerySearchCertificadosICA parameters);
        Task<List<CertificadoRetencionEstampillaBoyacaDte>> CertificadosRetencionEstampillaBoyacaApoteosys(QuerySearchCertificados parameters);
        #endregion

        #region Requerimientos
        Task<List<SolicitudesApoteosys>> GetSolicitudes(QuerySearchSolicitudesApoteosys parameters);
        #endregion
    }
}

