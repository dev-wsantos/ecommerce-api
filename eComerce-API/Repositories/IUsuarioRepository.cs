using eComerce_API.Controllers.Models;

namespace eComerce_API.Repositories
{
    interface IUsuarioRepository
    {
        public List<Usuario> Listar();
        public Usuario Pesquisar(int id);
        public void Inserir(Usuario usuario);
        public void Atualizar(Usuario usuario);
        public void Deletar(int id);
    }
}
