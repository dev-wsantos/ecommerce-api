using eComerce_API.Controllers.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Cryptography;
using System.Reflection.PortableExecutable;

namespace eComerce_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
      
     
        public UsuarioRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);       
        }

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
            List<Usuario> usuarios = new List<Usuario>();

            StringBuilder sql = new StringBuilder();
           
            sql.Append(@"SELECT Id, 
                                Nome, 
                                Email, 
                                Sexo, 
                                RG, 
                                CPF, 
                                NomeMae, 
                                SituacaoCadastro, 
                                DataCadastro 
                            FROM Usuarios");

            using(_connection)
            {
                try
                {
                    _connection.Open();
                    
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql.ToString();
                    cmd.Connection = (SqlConnection)_connection;

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.Id = Convert.ToInt32(reader["Id"]);
                        usuario.Nome = reader.GetString("Nome");
                        usuario.Email = reader.GetString("Email");
                        usuario.Sexo = reader.GetString("Sexo");
                        usuario.Rg = reader.GetString("RG");
                        usuario.Cpf = reader.GetString("CPF");
                        usuario.NomeMae = reader.GetString("NomeMae");
                        usuario.SituacaoCadastro = reader.GetString("SituacaoCadastro");
                        usuario.DataCadastro = reader.GetDateTimeOffset(8);

                        usuarios.Add(usuario);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
           

            return usuarios;
        }

        public Usuario Pesquisar(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@$"SELECT Id, 
                                Nome, 
                                Email, 
                                Sexo, 
                                RG, 
                                CPF, 
                                NomeMae, 
                                SituacaoCadastro, 
                                DataCadastro 
                            FROM Usuarios
                            WHERE Id = @Id;");


            using (_connection)
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql.ToString();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Connection = (SqlConnection)_connection;
                
                _connection.Open();
                SqlDataReader dataReader = cmd.ExecuteReader();

                while(dataReader.Read())
                {
                    Usuario usuario = new Usuario();

                    usuario.Id = Convert.ToInt32(dataReader["Id"]);
                    usuario.Nome = dataReader.GetString("Nome");
                    usuario.Email = dataReader.GetString("Email");
                    usuario.Sexo = dataReader.GetString("Sexo");
                    usuario.Rg = dataReader.GetString("RG");
                    usuario.Cpf = dataReader.GetString("CPF");
                    usuario.NomeMae = dataReader.GetString("NomeMae");
                    usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                    usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                    return usuario;
                }
            }

            return null;


        }

        private static List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){ Id = 1, Email = "alana.stella.cavalcanti@archosolutions.com.br", Nome = "Alana Stella Cavalcanti"},
            new Usuario(){ Id = 2, Email = "mateus.calebe.caldeira@univap.br", Nome = "Mateus Calebe Caldeira" },
            new Usuario(){ Id = 3, Email = "filipe.ian.fernandes@iaru.com.br", Nome = "Filipe Ian Fernandes"},
            new Usuario(){ Id = 4, Email = "stefany_nina_dapaz@iclaud.com", Nome = "Stefany Nina Tânia da Paz"},
            new Usuario(){ Id = 5, Email = "gael-damota87@andrelam.com.br", Nome = "Gael Alexandre da Mota"}
        };
    }
}
