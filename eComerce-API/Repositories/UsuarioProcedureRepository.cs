using eComerce_API.Controllers.Models;
using System.Data.SqlClient;
using System.Data;

namespace eComerce_API.Repositories
{
    public class UsuarioProcedureRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        private readonly IConfiguration Configuration;

        public UsuarioProcedureRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }


        public Usuario Pesquisar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> Listar()
        {
            throw new NotImplementedException();
        }

        public void Inserir(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
