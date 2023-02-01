using eComerce_API.Controllers.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;

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

        public List<Usuario> Listar()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = (SqlConnection) _connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SelecionarUsuarios";

             List<Usuario> usuarios = new List<Usuario>();

            using (_connection)
            {
                try
                {
                    _connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

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
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }

            return usuarios;
        }


        public Usuario Pesquisar(int id)
        {
            using (_connection)
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "SelecionarUsuario";
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = (SqlConnection)_connection;

                    _connection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();

                    Usuario usuario = new Usuario();
                    while (dataReader.Read())
                    {
                        
                        usuario.Id = dataReader.GetInt32(0);
                        usuario.Nome = dataReader.GetString("Nome");
                        usuario.Email = dataReader.GetString("Email");
                        usuario.Sexo = dataReader.GetString("Sexo");
                        usuario.Rg = dataReader.GetString("RG");
                        usuario.Cpf = dataReader.GetString("CPF");
                        usuario.NomeMae = dataReader.GetString("NomeMae");
                        usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                        usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                    }

                    return usuario; 
                }

                catch (Exception ex)
                {
                    return null;

                    throw new Exception(ex.Message);
                }
            }
        }


        public void Inserir(Usuario usuario)
        {
            _connection.Open();

            try
            {
                using (_connection)
                {

                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)_connection;
                    command.CommandText = "CadastrarUsuario";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@nome", usuario.Nome);
                    command.Parameters.AddWithValue("@email", usuario.Email);
                    command.Parameters.AddWithValue("@sexo", usuario.Sexo);
                    command.Parameters.AddWithValue("@rg", usuario.Rg);
                    command.Parameters.AddWithValue("@cpf", usuario.Cpf);
                    command.Parameters.AddWithValue("@nomeMae", usuario.NomeMae);
                    command.Parameters.AddWithValue("@situacaoCadastro", usuario.SituacaoCadastro);
                    command.Parameters.AddWithValue("@dataCadastro", usuario.DataCadastro);

                    usuario.Id = (int)command.ExecuteScalar();
                }

            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void Atualizar(Usuario usuario)
        {
            _connection.Open();

            try
            {
                using (_connection)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "AtualizarUsuario";
                    cmd.Connection = (SqlConnection)_connection;
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@email", usuario.Email);
                    cmd.Parameters.AddWithValue("@sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@rg", usuario.Rg);
                    cmd.Parameters.AddWithValue("@cpf", usuario.Cpf);
                    cmd.Parameters.AddWithValue("@nomeMae", usuario.NomeMae);
                    cmd.Parameters.AddWithValue("@situacaoCadastro", usuario.SituacaoCadastro);
                    cmd.Parameters.AddWithValue("@dataCadastro", usuario.DataCadastro);

                    cmd.Parameters.AddWithValue("@Id", usuario.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Deletar(int id)
        {
            try
            {
                using (_connection)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "DeletarUsuario";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = (SqlConnection)_connection;

                    cmd.Parameters.AddWithValue("@Id", id);

                    _connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
