using Core.CustomEntities;
using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApoteosysService
    {
        #region Órdenes
        Task<List<OrdenesCompraDetalleApoteosys>> ConsultaOrdenesCompra(QuerySearchOrdenesCompra parameters);
        Task<List<OrdenesCompraMaestraApoteosys>> ConsultaMaestraOrdenesCompra(QuerySearchMaestraOrdenesCompra parameters);
        Task<List<OrdenesCompraAdjudicacionApoteosys>> ConsultaOrdenesParaAdjudicacionRequerimiento(QuerySearchOrdenesAdjudicacion parameters);
        #endregion

        #region Facturación
        Task<List<Empresa>> GetParametrosIniciales(int idUser);
        Task<List<Empresa>> GetParametrosInicialesDashboard(int idUser);

        #region Facturas Por Pagar
        Task<FacturasXPagarDetalle> EstadoCuentasXPorPagarDetalleApoteosys(QuerySearchEstadoCuentasXPagarDetalle parameters);
        Task<FacturasXPagarDetalle> EstadoCuentasXPorPagarDetalleApoteosys_Reporte(QuerySearchEstadoCuentasXPagarDetalle parameters);
        #endregion

        #region Facturas Pagadas
        Task<List<EstadoCuentasPagadas>> EstadoCuentasPagadasApoteosys(QuerySearchEstadoCuentasPagadas parameters);
        Task<List<EstadoCuentasPagadasDetalle>> EstadoCuentasPagadasDetalleApoteosys(QuerySearchEstadoCuentasPagadasDetalle parameters);
        Task<List<EstadoCuentasPagadasDetalle_Reporte>> EstadoCuentasPagadasDetalleApoteosys_Reporte(QuerySearchEstadoCuentasPagadasDetalle parameters);

        #region Descuentos Aplicados en Facturas Pagadas
        Task<Detalle_FactPagas> EstadoCuentasXPorPagarDetalleApoteosysFactPagas(QuerySearchEstadoCuentasXPagar_FactPagas parameters);
        #endregion

        #region Consultas desde SP SQL Server
        Task<List<FacturasPagas_SQL>> GetFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters);
        Task<List<FacturasPagas_SQL>> GetRetencionesFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters);
        #endregion
        #endregion
        #endregion

        #region Certificados
        Task<List<CertificadoRetencionMaestro>> CertificadosRetencionApoteosys(QuerySearchCertificados parameters);
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
