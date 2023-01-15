using eComerce_API.Controllers.Models;
using eComerce_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eComerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private UsuarioRepository _usuarioRepository;

        public UsuariosController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        /**
         *  CRUD 
         *  
         *  - GET       -> Obter uma lista de usuários
         *  - GET       -> Obter um usuário passando o ID.
         *  - POST      -> Cadastrar um usuário
         *  - PUT       -> Atualizar um usuário
         *  - DELETE    -> Remover um usuário.
         *  
         *  
         *  Exemplo de forma de acesso.
         *  
         *  HTTP METHOD: www.minhaapi.com.br/api/Usuarios
         *               www.minhaapi.com.br/api/Usuarios/5
         *  
         */

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_usuarioRepository.Listar());
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var _usuario = _usuarioRepository.Pesquisar(id);

            if (_usuario == null)
            {
                return NotFound(); // Erro HTTP 404 - Not Found
            }
            
            return Ok(_usuario);
        }


        [HttpPost]
        public IActionResult Insert([FromBody] Usuario usuario)
        {
            _usuarioRepository.Inserir(usuario);

            return Ok(usuario);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Usuario usuario)
        {
            _usuarioRepository.Atualizar(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _usuarioRepository.Deletar(id);

            return Ok();
        }
     
    }
}
