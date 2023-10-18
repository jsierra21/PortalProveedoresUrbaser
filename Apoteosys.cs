namespace Core.Entities
{
    public class Apoteosys
    {
        public string CodEmpresa { get; set; }
        public string CodigoPlanContable { get; set; }
    }

    #region Órdenes
    public class OrdenesCompraDetalleApoteosys
    {
        public int IdLinea { get; set; }
        public string Empresa { get; set; }
        public string NumOrden { get; set; }
        public string Fecha { get; set; }
        public string CodArticulo { get; set; }
        public string NomArticulo { get; set; }
        public int Cantidad { get; set; }
        public float ValorUnitario { get; set; }
        /*public float DescuentoLinea { get; set; }*/
        public float DescuentoMonto { get; set; }
        public float OtrosImpuestoMonto { get; set; }
        public float ImpuestoVentaMonto { get; set; }
        public float TotalLinea { get; set; }
        public string MedidaCantidadMedida { get; set; }
    }

    public class OrdenesCompraMaestraApoteosys
    {
        public string Empresa { get; set; }
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public string Nit { get; set; }
        public string Moneda { get; set; }
        public string Tipo_Documento { get; set; }
        public string Numero_documento { get; set; }
        public string Tipo_Orden { get; set; }
        public string Estado_Orden { get; set; }
        public string Condicion_Pago { get; set; }
        public float Sub_Total { get; set; }
        public float Descuento_Global { get; set; }
        public float Descuento { get; set; }
        public float Otro_Impuesto { get; set; }
        public float Iva { get; set; }
        public float Total { get; set; }
        public string Observaciones { get; set; }
        public string Departamento { get; set; }
    }

    public class OrdenesCompraAdjudicacionApoteosys
    {
        public string Empresa { get; set; }
        public string Num_Solicitud { get; set; }
        public string Tipo_Solicitud { get; set; }
        public string Linea_Articulo { get; set; }
        public string Codigo_Articulo { get; set; }
        public string Descripcion_Dte { get; set; }
        public string Descripcion_Alt_Dte { get; set; }
        public string Unidad_Medida_Articulo { get; set; }
        public string Num_Orden { get; set; }
        public string Tipo_Orden { get; set; }
        public string Proveedor_Orden { get; set; }
        public string Estado_Orden { get; set; }
        public string Subtotal_Orden { get; set; }
        public string Total_Orden { get; set; }
        public string Contrato_Orden { get; set; }
        public string Fecha_Orden { get; set; }
        public string Estado_Orden_Dte { get; set; }
        public string Linea_Orden { get; set; }
        public string Cantidad_Pedida_Dte { get; set; }
        public string Cantidad_Recibida_Dte { get; set; }
        public string Cantidad_Devuelta_Dte { get; set; }
        public string Total_Linea_Dte { get; set; }
    }
    #endregion

    #region Facturas Por Pagar
    public class EstadoCuentasXPorPagar
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        //public string Numero_Proceso { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha_Emision { get; set; }
        public string Fecha_Generacion { get; set; }
        public string Fecha_Vencimiento { get; set; }
        public string Fecha_Estimado_Pago { get; set; }
        //public string Plan_Contable { get; set; }
        //public string Cuenta_Contable { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Tipo_Documento { get; set; }
        public string Codigo_Td { get; set; }
        public string Numero_B { get; set; }
        public string Numero_Documento { get; set; }
        //public string Naturaleza_Saldo { get; set; }
        //public string Estado { get; set; }
        //public string Tipo_Giro { get; set; }
        //public string Tipo_Registro { get; set; }
        //public string Ind_Tipo_Registro { get; set; }
        //public string Codigo_Efectivo { get; set; }
        //public string Numero_Cuenta { get; set; }
        //public string Tipo_Cuenta { get; set; }
        //public string Sec_Interna_Documento { get; set; }
        //public string Idenbeme { get; set; }
        //public string Numero_Radicado { get; set; }
        //public string Saldo_Inicial { get; set; }
        //public string Saldo_Acu_Debito { get; set; }
        //public string Saldo_Acu_Credito { get; set; }
        public string Valor_A_Pagar { get; set; }
        //public string Valor_Iva { get; set; }
        //public string Valor_Retencion { get; set; }
        //public string Usuario_Crea { get; set; }
    }

    public class EstadoCuentasXPorPagarDte
    {
        public string Empresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha_Emision { get; set; }
        public string Fecha_Vencimiento { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Tipo_Documento { get; set; }
        public string Codigo_Tipo_Td { get; set; }
        public string Numero_B { get; set; }
        public string Numero_Documento { get; set; }
    }

    public class EstadoCuentasXPagarDetalle
    {
        public string Empresa { get; set; }
        public string Tipo_Documento { get; set; }
        public string Fecha { get; set; }
        public string Numero { get; set; }
        public string Consecutivo { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Codigo_Plan_Contable { get; set; }
        public string Cuenta_Contable { get; set; }
        public string Nombre_Cuenta_Contable { get; set; }
        public string Centro_Costo { get; set; }
        public string Nombre_Centro_Costo { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Fecha_Emision { get; set; }
        public string Fecha_Vencimiento { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string Observaciones { get; set; }
        public string Iva_Mayor_Valor { get; set; }

        //public string Codigo_Tipo_Td { get; set; }
        //public string Codigo_Tipo_Cd { get; set; }
        //public string Documento_Soporte { get; set; }
        //public string Secuencia { get; set; }
        //public string Saldo_Aplicado { get; set; }
        //public string Valor_Descuento { get; set; }
        //public string Valor_Iva { get; set; }
        //public string Valor_Retencion { get; set; }
        //public string Debito_Moneda_Original { get; set; }
        //public string Credito_Moneda_Original { get; set; }
        //public string Debito_Moneda_Local { get; set; }
        //public string Credito_Moneda_Local { get; set; }
        //public string Tipo_Movimiento { get; set; }
        //public string Usuario_Crea { get; set; }
        //public string Usuario_Modifica { get; set; }
    }
    #endregion

    #region Facturas Pagadas
    public class EstadoCuentasPagadas
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        //public string Fecha_Emision { get; set; }
        //public string Fecha_Vencimiento { get; set; }
        public string Fecha_Pago { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        //public string Total_Movimiento { get; set; }
    }

    public class EstadoCuentasPagadasDte
    {
        public string Empresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
    }

    public class EstadoCuentasPagadasDetalle
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Cuenta_Consignacion { get; set; }
        public string Nombre_Banco { get; set; }
        public string Tipo_Cuenta_Banco { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string TotalCredito { get; set; }
        public string IsTotal { get; set; }
        
    }

    public class EstadoCuentasPagadasDetalle_Reporte
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Cuenta { get; set; }
        public string Cuenta_Consignacion { get; set; }
        public string Nombre_Banco { get; set; }
        public string Tipo_Cuenta_Banco { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string TotalDebito { get; set; }
        public string IsTotal { get; set; }        
    }

    #region Descuentos Aplicados en Facturas Pagadas
    public class EstadoCuentasXPagar_FactPagas_Maestro
    {
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
    }

    public class EstadoCuentasXPagar_FactPagas_Detalle
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha { get; set; }
        public string Fecha_Vence { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta_Con { get; set; }
        public string Cuenta_Consignacion { get; set; }
        public string Nombre_Banco { get; set; }
        public string Tipo_Cuenta_Banco { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
    }
    #endregion

    #region Consultas desde SP SQL Server
    public class FacturasPagas_SQL
    {
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string Anio { get; set; }
        public string Periodo { get; set; }
        public string Fecha { get; set; }
        //public string Fecha_Vence { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta_Con { get; set; }
        public string Cuenta_Consignacion { get; set; }
        public string Nombre_Banco { get; set; }
        public string Tipo_Cuenta_Banco { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
    }

    public class FacturasPagas
    {
        public string Tipo_Movimiento { get; set; }
        public string Empresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int Anio { get; set; }
        public int Periodo { get; set; }
        public string Fecha { get; set; }
        //public string Fecha_Vence { get; set; }
        public string Codigo_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta_Con { get; set; }
        public string Cuenta_Consignacion { get; set; }
        public string Nombre_Banco { get; set; }
        public string Tipo_Cuenta_Banco { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
        public string Observaciones { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string Tdc { get; set; }
        public string Ndc { get; set; }
        public string Total_Movimiento { get; set; }
    }
    #endregion
    #endregion

    #region Certificados
    public class CertificadoRetencionMaestro
    {
        public string Nit { get; set; }
        public string Razon_Social { get; set; }
        public string Clase { get; set; }
    }

    public class CertificadoExperiencia
    {
        public string Anio { get; set; }
        public string Nit_Tercero { get; set; }
        public string Nombre_Tercero { get; set; }
    }

    public class CertificadoRetencionFuenteDte
    {
        public string Empresa { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Porcentaje_Practicado { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string CreditoSinFormato { get; set; }
        public string Total { get; set; }
    }

    public class CertificadoRetencionIvaDte
    {
        public string Empresa { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Porcentaje_Practicado { get; set; }
        public string Base_Origen { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string CreditoSinFormato { get; set; }
        public string Total { get; set; }
    }

    public class CertificadoRetencionIcaDte
    {
        public string Empresa { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Porcentaje_Practicado { get; set; }
        public string Base_Origen { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string CreditoSinFormato { get; set; }
        public string Total { get; set; }
    }

    public class CertificadoRetencionEstampillaBoyacaDte
    {
        public string Empresa { get; set; }
        public string Cuenta { get; set; }
        public string Nombre_Cuenta { get; set; }
        public string Porcentaje_Practicado { get; set; }
        public string Base_Origen { get; set; }
        public string Base_Retencion { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string CreditoSinFormato { get; set; }
        public string Total { get; set; }
    }
    #endregion

    #region Requerimientos
    public class SolicitudesApoteosys
    {
        public int Id { get; set; } // Id Autoincrementable
        public string Scm_Status { get; set; } // Etado de Solicitud
        public string Scm_Dep_Departamento { get; set; } // Departamento de la Solicitud
        public string Scm_Solicitud_Compra { get; set; } // Numero de Solicitud
        public string Scm_Observaciones { get; set; } // Observaciones de la Solicitud
        public string Scm_Fecha_Creacion { get; set; } // Fecha de la Solicitud
        public string Scm_Emp_Empresa { get; set; } // Empresa de la Solicitud
        public string Dsc_Linea { get; set; } // Línea(item) del Articulo en la Solicitud
        public string Dsc_Ato_Articulo { get; set; } // Codigo del Articulo
        public string Art_Unidad_Medida { get; set; } // Unidad de Medida del Articulo
        public string Dsc_Descripcion { get; set; } // Descripción del articulo
        public string Dsc_Descripcion_Alterna { get; set; } // Descripción alterna del articulo
        public string Dsc_Cantidad { get; set; } // Cantidad Solicitada del Articulo
        public string Dsc_Aprobada { get; set; } // Indica si la Solictud es Aprobada
        public string Dsc_Observaciones { get; set; } // Observaciones del articulo

        public string Tipo_Solicitud { get; set; } // Tipo de Solicitud (Servicio, Inventario)
        public string Valor_Solicitud { get; set; } // Valor de la Solicitud
        public string Cantidad_Aprobada { get; set; } // Cantidad de articulos aprobados
    }
    #endregion
}
