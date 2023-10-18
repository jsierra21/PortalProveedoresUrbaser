using Api.Responses;
using Core.CustomEntities;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Core.Tools;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApoteosysController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IApoteosysService _apoteosysService;
        private readonly ISispoSevice _sispoSevice;
        private readonly IReportingService _reportingService;
        private readonly ITokenService _tokenService;
        private readonly IEmpresasService _empresasService;

        public ApoteosysController(
            IConfiguration configuration,
            IApoteosysService apoteosysService,
            ISispoSevice sispoSevice,
            IReportingService reportingService,
            ITokenService tokenService,
            IEmpresasService empresasService
        )
        {
            _configuration = configuration;
            _apoteosysService = apoteosysService;
            _sispoSevice = sispoSevice;
            _reportingService = reportingService;
            _tokenService = tokenService;
            _empresasService = empresasService;
        }

        #region Órdenes
        /// <summary>
        /// Buscar Ordenes Compra (Detalle de un documento escogido)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SearchOrdenesCompra", Name = "SearchOrdenesCompraApoteosys")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchOrdenesCompraApoteosys([FromBody] QuerySearchOrdenesCompra parameters)
        {
            try
            {
                List<OrdenesCompraDetalleApoteosys> listConsulta = await _apoteosysService.ConsultaOrdenesCompra(parameters);
                var response = new ApiResponse<List<OrdenesCompraDetalleApoteosys>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Buscar Maestro Ordenes Compra (Maestro de documentos)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SearchMaestraOrdenesCompra", Name = "SearchMaestraOrdenesCompraApoteosys")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchMaestraOrdenesCompraApoteosys([FromBody] QuerySearchMaestraOrdenesCompra parameters)
        {
            try
            {
                List<OrdenesCompraMaestraApoteosys> listConsulta = await _apoteosysService.ConsultaMaestraOrdenesCompra(parameters);
                var response = new ApiResponse<List<OrdenesCompraMaestraApoteosys>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar las órdenes asociadas a un requerimiento, en Apoteosys.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SearchOrdenArticulo", Name = "SearchOrdenesArticuloAdjudicacion")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchOrdenesArticuloAdjudicacion([FromBody] QuerySearchOrdenesAdjudicacion parameters)
        {
            try
            {
                List<OrdenesCompraAdjudicacionApoteosys> listConsulta = await _apoteosysService.ConsultaOrdenesParaAdjudicacionRequerimiento(parameters);
                var response = new ApiResponse<List<OrdenesCompraAdjudicacionApoteosys>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Facturación
        /// <summary>
        /// Método para consultar los parámtetros iniciales del modulo de Facturación.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParamsFacturacion", Name = "SearchParametrosInicialesFacturacion")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchParametrosInicialesFacturacion()
        {
            try
            {
                int user = int.Parse(HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0");
                string tipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                List<Empresa> entities = new List<Empresa>();

                if (tipoUsuario == "Proveedor")
                {
                    entities = await _apoteosysService.GetParametrosIniciales(user);
                }
                else
                {
                    entities = await _apoteosysService.GetParametrosInicialesDashboard(user);
                }

                var response = new ApiResponse<List<Empresa>>(entities, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        #region Facturas Por Pagar
        /// <summary>
        /// Método para consultar en Apoteosys, las facturas por pagar de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasXPagar", Name = "SearchFacturasXPagarProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchFacturasXPagarProveedor([FromQuery] QuerySearchEstadoCuentasXPagar parameters)
        {
            try
            {
                parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                parameters.TipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                if (parameters.TipoUsuario == "Proveedor")
                {
                    parameters.NitProveedor = HttpContext.Items.ContainsKey("CodUser") ? HttpContext.Items["CodUser"]?.ToString() : "";
                }
                List<EstadoCuentasXPorPagar> listConsulta = await _sispoSevice.EstadoCuentasXPorPagarApoteosys(parameters);
                var response = new ApiResponse<List<EstadoCuentasXPorPagar>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar el detalle de las facturas por pagar de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasXPagarDetalle", Name = "FacturasXPagarProveedorDetalle")]
        [Consumes("application/json")]
        public async Task<IActionResult> FacturasXPagarProveedorDetalle([FromQuery] QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                FacturasXPagarDetalle entity = await _apoteosysService.EstadoCuentasXPorPagarDetalleApoteosys(parameters);
                var response = new ApiResponse<FacturasXPagarDetalle>(entity, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar el detalle de las facturas por pagar de un proveedor en especifico. Agrupado por cuenta contable, sumando valores de Base retención, Débito y Crédito (Minificado)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasXPagarDetalleMinificado", Name = "FacturasXPagarProveedorDetalleMinificado")]
        [Consumes("application/json")]
        public async Task<IActionResult> FacturasXPagarProveedorDetalleMinificado([FromQuery] QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                FacturasXPagarDetalle entity = await _apoteosysService.EstadoCuentasXPorPagarDetalleApoteosys_Reporte(parameters);
                var response = new ApiResponse<FacturasXPagarDetalle>(entity, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar el detalle de las facturas por pagar de un proveedor en especifico. Agrupado por cuenta contable, sumando valores de Base retención, Débito y Crédito (Minificado)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasXPagarDetalleMinificadoFactPagas", Name = "FacturasXPagarProveedorDetalleMinificadoFactPagas")]
        [Consumes("application/json")]
        public async Task<IActionResult> FacturasXPagarProveedorDetalleMinificadoFactPagas([FromQuery] QuerySearchEstadoCuentasXPagar_FactPagas parameters)
        {
            try
            {
                Detalle_FactPagas entity = await _apoteosysService.EstadoCuentasXPorPagarDetalleApoteosysFactPagas(parameters);
                var response = new ApiResponse<Detalle_FactPagas>(entity, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar detalle de Facturas por Pagar (Pdf)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateFacturaPorPagarPdf", Name = "DescargarFacturaXPagarPdf")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarFacturaXPagarPdf([FromQuery] QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                var ms = await _reportingService.FacturaPorPagarPdf(parameters);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "detalleFacturaXPagar.pdf"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la factura. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar detalle de Facturas por Pagar (Excel)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateFacturaPorPagarXlsx", Name = "DescargarFacturaXPagarXlsx")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarFacturaXPagarXlsx([FromQuery] QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            try
            {
                var ms = await _reportingService.FacturaPorPagarXlsx(parameters);
                return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "detalleFacturaXPagar.xlsx"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la factura. Detalle: {e.Message}");
            }
        }
        #endregion

        #region Facturas Pagadas
        /// <summary>
        /// Método para consultar en Apoteosys, las facturas pagadas de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasPagadas", Name = "SearchFacturasPagadasProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchFacturasPagadasProveedor([FromQuery] QuerySearchEstadoCuentasPagadas parameters)
        {
            try
            {
                parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                parameters.TipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                if (parameters.TipoUsuario == "Proveedor")
                {
                    parameters.NitProveedor = HttpContext.Items.ContainsKey("CodUser") ? HttpContext.Items["CodUser"]?.ToString() : "";
                }
                List<EstadoCuentasPagadas> listConsulta = await _apoteosysService.EstadoCuentasPagadasApoteosys(parameters);
                var response = new ApiResponse<List<EstadoCuentasPagadas>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar el detalle de las facturas pagadas de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasPagadasDetalle", Name = "SearchFacturasPagadasProveedorDetalle")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchFacturasPagadasProveedorDetalle([FromQuery] QuerySearchEstadoCuentasPagadasDetalle parameters)
        {
            try
            {
                List<EstadoCuentasPagadasDetalle> entity = await _apoteosysService.EstadoCuentasPagadasDetalleApoteosys(parameters);
                var response = new ApiResponse<List<EstadoCuentasPagadasDetalle>>(entity, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar detalle de Facturas Pagadas (PDF)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateFacturaPagadaPdf", Name = "DescargarFacturaPagadaPdf")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarFacturaPagadaPdf([FromQuery] QueryGenerateFacturaPagada parameters)
        {
            try
            {
                GenerateFactPag param = new()
                {
                    apoteosysService = _apoteosysService,
                    empresasActivas = _empresasService.EmpresasActivas,
                    parameters = parameters
                };
                var ms = await _reportingService.FacturaPagadaPdf(param);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "detalleFacturaPagada.pdf"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la factura. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar detalle de Facturas Pagadas (Excel)
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateFacturaPagadaXlsx", Name = "DescargarFacturaPagadaXlsx")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarFacturaPagadaXlsx([FromQuery] QueryGenerateFacturaPagada parameters)
        {
            try
            {
                GenerateFactPag param = new()
                {
                    apoteosysService = _apoteosysService,
                    empresasActivas = _empresasService.EmpresasActivas,
                    parameters = parameters
                };
                var ms = await _reportingService.FacturaPagadaXlsx(param);
                return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "detalleFacturaPagada.xlsx"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la factura. Detalle: {e.Message}");
            }
        }

        #region Consultas desde SP SQL Server
        /// <summary>
        /// Método para consultar en Apoteosys, desde SQL Server, las facturas pagadas de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchFacturasPagadas_SQL", Name = "SearchFacturasPagadasProveedor_SQL")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchFacturasPagadasProveedor_SQL([FromQuery] QuerySearchFactPagas_SQL parameters)
        {
            List<FacturasPagas_SQL> listConsulta = await _apoteosysService.GetFacturasPagadas_SpSQL(parameters);
            var response = new ApiResponse<List<FacturasPagas_SQL>>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para consultar en Apoteosys, desde SQL Server, las retenciones de facturas pagadas de un proveedor en especifico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("SearchRetencionFacturasPagadas_SQL", Name = "SearchRetencionFacturasPagadasProveedor_SQL")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchRetencionFacturasPagadasProveedor_SQL([FromQuery] QuerySearchFactPagas_SQL parameters)
        {
            List<FacturasPagas_SQL> listConsulta = await _apoteosysService.GetRetencionesFacturasPagadas_SpSQL(parameters);
            var response = new ApiResponse<List<FacturasPagas_SQL>>(listConsulta, 200);
            return Ok(response);
        }
        #endregion
        #endregion
        #endregion

        #region Certificados
        /// <summary>
        /// Método para generar certificado de retencion de Fuente
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoRetFuentePdf", Name = "DescargarCertificadoRetencionFuentePdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoRetencionFuentePdf([FromQuery] QuerySearchCertificados parameters)
        {
            return await DescargarCertificado(parameters, "reteFuente");
        }

        /// <summary>
        /// Método para generar certificado de retencion de IVA
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoRetIvaPdf", Name = "DescargarCertificadoRetencionIvaPdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoRetencionIvaPdf([FromQuery] QuerySearchCertificados parameters)
        {
            return await DescargarCertificado(parameters, "reteIva");
        }

        /// <summary>
        /// Método para generar certificado de retencion de ICA
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoRetIcaPdf", Name = "DescargarCertificadoRetencionIcaPdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoRetencionIcaPdf([FromQuery] QuerySearchCertificados parameters)
        {
            parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
            return await DescargarCertificado(parameters, "reteIca");
        }

        [HttpGet("GenerateCertificadoRetIca2Pdf", Name = "DescargarCertificadoRetencionIca2Pdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoRetencionIca2Pdf([FromQuery] QuerySearchCertificados parameters)
        {
            parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
            parameters.TipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
            return await DescargarCertificado(parameters, "reteIca2");
        }

        /// <summary>
        /// Método para generar certificado de retencion de Estampilla Boyacá
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoRetEstampillaPdf", Name = "DescargarCertificadoRetencionEstampillaBoyacaPdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoRetencionEstampillaBoyacaPdf([FromQuery] QuerySearchCertificados parameters)
        {
            return await DescargarCertificado(parameters, "reteEstampillaBoyaca");
        }

        /// <summary>
        /// Método para validación de certificados generados por los proveedores.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ValidateCertificado", Name = "ValidarCertificado")]
        [Consumes("application/json")]
        public async Task<IActionResult> ValidarCertificado([FromQuery] QueryToken query)
        {
            try
            {
                List<HashCertifiedValidation> listConsulta = await _tokenService.ValidateTokenCertificado(query);
                var response = new ApiResponse<List<HashCertifiedValidation>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar validar el certificado: {e.Message}");
            }
        }

        private async Task<IActionResult> DescargarCertificado(QuerySearchCertificados parameters, string tipoCert)
        {
            try
            {
                // Generamos Token en Base de datos
                List<ResponseActionUrl> responseToken = await _tokenService.GenerateToken();

                if (!responseToken[0].estado)
                {
                    return Ok(ErrorResponse.GetError(false, $"Hubo un inconveniente para generar el certificado: {responseToken[0].mensaje}", 400));
                }

                parameters.UrlToken = responseToken[0].url;

                string html = string.Empty;
                string nombreArchivo = string.Empty;
                byte[] resIca = Array.Empty<byte>();

                parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                parameters.TipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                if (parameters.TipoUsuario == "Proveedor")
                {
                    parameters.NitProveedor = HttpContext.Items.ContainsKey("CodUser") ? HttpContext.Items["CodUser"]?.ToString() : "";
                }

                switch (tipoCert)
                {
                    case "reteFuente":
                        html = await _reportingService.CertificadoRetencionFuentePdf(parameters);
                        nombreArchivo = "detalleCertFuente.pdf";
                        break;
                    case "reteIva":
                        html = await _reportingService.CertificadoRetencionIVAPdf(parameters);
                        nombreArchivo = "detalleCertIva.pdf";
                        break;
                    case "reteIca":
                        html = await _reportingService.CertificadoRetencionIcaPdf(parameters);
                        nombreArchivo = "detalleCertIca.pdf";
                        break;
                    case "reteIca2":
                        var t = await _reportingService.CertificadoRetencionIca2Pdf(parameters);
                        html = t.Item1;
                        resIca = t.Item2;
                        nombreArchivo = "detalleCertIca2.pdf";
                        break;
                    case "reteEstampillaBoyaca":
                        html = await _reportingService.CertificadoRetencionEstampillaBoyacaPdf(parameters);
                        nombreArchivo = "detalleCertEstampillaBoyaca.pdf";
                        break;
                    default:
                        break;
                }                

                // Actualizamos el campo CertifiedHtml para relacionarlo con el token
                List<ResponseAction> responseUpdate = await _tokenService.UpdateToken(html, (int)responseToken[0].Id);

                if (!responseUpdate[0].estado)
                {
                    return Ok(ErrorResponse.GetError(false, $"Hubo un inconveniente para generar el certificado: {responseUpdate[0].mensaje}", 400));
                }

                MemoryStream ms;

                if (tipoCert == "reteIca2")
                {
                    // Creamos el stream del pdf
                    ms = new(resIca);
                }
                else
                {
                    // Creamos el stream del pdf
                    ms = new(Funciones.PdfSharpConvertWithoutCreateFile(html));
                }

                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = nombreArchivo
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del certificado. Detalle: {e.Message}");
            }
        }
        #endregion

        #region Requerimientos
        /// <summary>
        /// Método para consultar las solicitudes existentes en Apoteosys.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SearchSolicitudes", Name = "SearchSolicitudes")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchSolicitudes([FromBody] QuerySearchSolicitudesApoteosys parameters)
        {
            try
            {
                List<SolicitudesApoteosys> listConsulta = await _apoteosysService.GetSolicitudes(parameters);
                var response = new ApiResponse<List<SolicitudesApoteosys>>(listConsulta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        /// <summary>
        /// Método de Prueba.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateVariasHojas", Name = "DescargarVariasHojas")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarVariasHojas()
        {
            try
            {
                var ms = await _reportingService.Prueba();
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "prueba.pdf"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }
    }
}
