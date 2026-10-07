using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios
{
    public class LibroRepositorio
    {
        private readonly string _connectionString;

        public LibroRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Libro>> ObtenerTodosAsync()
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Libro>("sp_ListarLibros", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Libro>("sp_BuscarLibrosPorTitulo", new { Titulo = titulo }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Libro?> ObtenerPorIdAsync(int id)
        {
            using var db = CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Libro>("sp_ObtenerLibroPorId", new { LibroId = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task InsertarAsync(Libro libro)
        {
            using var db = CreateConnection();
            await db.ExecuteAsync("sp_InsertarLibro", new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
        }

        public async Task ActualizarAsync(Libro libro)
        {
            using var db = CreateConnection();
            await db.ExecuteAsync("sp_ActualizarLibro", new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
        }

        public async Task EliminarAsync(int id)
        {
            using var db = CreateConnection();
            await db.ExecuteAsync("sp_EliminarLibro", new { LibroId = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Autor>> ObtenerAutoresAsync()
        {
            using var db = CreateConnection();
            return await db.QueryAsync<Autor>("sp_ListarAutores", commandType: CommandType.StoredProcedure);
        }
    }
}