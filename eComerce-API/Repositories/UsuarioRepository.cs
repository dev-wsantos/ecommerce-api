using eComerce_API.Controllers.Models;

namespace eComerce_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // Simulando o banco de dados
        private static List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){ Id = 1, Email = "alana.stella.cavalcanti@archosolutions.com.br", Nome = "Alana Stella Cavalcanti"}, 
            new Usuario(){ Id = 2, Email = "mateus.calebe.caldeira@univap.br", Nome = "Mateus Calebe Caldeira" },
            new Usuario(){ Id = 3, Email = "filipe.ian.fernandes@iaru.com.br", Nome = "Filipe Ian Fernandes"},
            new Usuario(){ Id = 4, Email = "stefany_nina_dapaz@iclaud.com", Nome = "Stefany Nina Tânia da Paz"}, 
            new Usuario(){ Id = 5, Email = "gael-damota87@andrelam.com.br", Nome = "Gael Alexandre da Mota"}
        };

        public void Atualizar(Usuario usuario)
        {
            _db.Remove(_db.FirstOrDefault(a => a.Id == usuario.Id));
            _db.Add(usuario);

        }

        public void Deletar(int id)
        {
            _db.Remove(_db.FirstOrDefault(a => a.Id == id));
        }

        public void Inserir(Usuario usuario)
        {
            var ultimoUsuario = _db.LastOrDefault();

            if (usuario == null)
            {
                usuario.Id = 1;
            }
            else
            {
                usuario.Id = ultimoUsuario.Id;
                usuario.Id++;
            }

            _db.Add(usuario);
        }

        public List<Usuario> Listar()
        {
            return _db;
        }

        public Usuario Pesquisar(int id)
        {
            return _db.FirstOrDefault(p => p.Id == id);
        }
    }
}
