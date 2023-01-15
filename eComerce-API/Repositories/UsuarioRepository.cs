using eComerce_API.Controllers.Models;

namespace eComerce_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // Simulando o banco de dados
        private List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){ Id = 1, Email = "fulano.sicrano@gmail.com", Nome = "Fulano Sicrano"}
        };

        public void Atualizar(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public void Inserir(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> Pesquisar()
        {
            throw new NotImplementedException();
        }

        public Usuario Pesquisar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
