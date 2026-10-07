using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios
{
    public class SocioRepositorio
    {
        private readonly string _connectionString;

        public SocioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Socio>> ObtenerTodosAsync()
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Socio>("sp_ListarSocios", commandType: CommandType.StoredProcedure);
        }

        public async Task InsertarAsync(Socio socio)
        {
            using var db = CreateConnection();
            await db.ExecuteAsync("sp_InsertarSocio", new { socio.DNI, socio.Nombre, socio.Email }, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ExisteDNIAsync(string dni)
        {
            using var db = CreateConnection();
            var count = await db.ExecuteScalarAsync<int>("sp_ExisteSocioDNI", new { DNI = dni }, commandType: CommandType.StoredProcedure);
            return count > 0;
        }

        public async Task<IEnumerable<PrestamoReporte>> ObtenerReportePrestamosAsync(DateTime desde, DateTime hasta)
        {
            using var db = CreateConnection();
            return await db.QueryAsync<PrestamoReporte>("sp_ReportePrestamos", new { FechaDesde = desde, FechaHasta = hasta }, commandType: CommandType.StoredProcedure);
        }
    }
}