using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ApoteosysService : IApoteosysService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApoteosysService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Apoteosys>> ConsultaGeneral(string inicio,string fin)
        {
            return _unitOfWork.ApoteosysRepository.ConsultaGeneral(inicio, fin);
        }

        public Task<List<CentroCosto>> GetCCO()
        {
            return _unitOfWork.ApoteosysRepository.GetCCO();
        }
        public Task<List<CuentasContables>> GetCuentasContables()
        {
            return _unitOfWork.ApoteosysRepository.GetCuentasContables();
        }

        public Task<List<FamiliaCuentas>> GetFamiliaCuentas()
        {
            return _unitOfWork.ApoteosysRepository.GetFamiliaCuentas();
        }

        public Task<List<Articulos>> GetArticulos()
        {
            return _unitOfWork.ApoteosysRepository.GetArticulos();
        }

        public Task<List<Terceros>> GetTerceros()
        {
            return _unitOfWork.ApoteosysRepository.GetTerceros();
        }
        public Task<List<Empresas>> GetEmpresas()
        {
            return _unitOfWork.ApoteosysRepository.GetEmpresas();
        }
        public Task<List<Familias>> GetFamilias()
        {
            return _unitOfWork.ApoteosysRepository.GetFamilias();
        }
        public Task<List<TiposTransaccion>> GetTiposTransaccion()
        {
            return _unitOfWork.ApoteosysRepository.GetTiposTransaccion();
        }

        #region XANDER
        public Task<List<Proveedores>> GetProveedores()
        {
            return _unitOfWork.ApoteosysRepository.GetProveedores();
        }

        public Task<List<Proveedores>> GetProveedoresFindOne(string TERCER_IDENTIFIC_B, string TERCER_NOMBEXTE__B)
        {
            return _unitOfWork.ApoteosysRepository.GetProveedoresFindOne(TERCER_IDENTIFIC_B, TERCER_NOMBEXTE__B);
        }

        public Task<List<ArticulosApo>> GetArticulosByEmpresa(string empresa)
        {
            return _unitOfWork.ApoteosysRepository.GetArticulosByEmpresa(empresa);
        }

        public Task<List<ArticulosApo>> GetArticulosByEmpresaAndArticulo(string empresa, string articulo)
        {
            return _unitOfWork.ApoteosysRepository.GetArticulosByEmpresaAndArticulo(empresa, articulo);
        }

        public Task<List<SpQuerysApoteosys>> GetSpQuerysApoteosys(FiltroSP filtroSP)
        {
            return _unitOfWork.ApoteosysRepository.GetSpQuerysApoteosys(filtroSP);
        }
        #endregion XANDER
    }
}
