using eComerce_API.Controllers.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Cryptography;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc.Formatters;

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
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(@"UPDATE Usuarios 
                                        SET Nome = @Nome, 
                                            Email = @Email, 
                                            Sexo = @Sexo, 
                                            Rg = @Rg, 
                                            Cpf = @Cpf, 
                                            NomeMae = @NomeMae, 
                                            SituacaoCadastro = @SituacaoCadastro, 
                                            DataCadastro = @DataCadastro
                                        WHERE Id = @Id");
                using (_connection)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql.ToString();
                    cmd.Connection = (SqlConnection)_connection;

                    cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Rg", usuario.Rg);
                    cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    cmd.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                    cmd.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                    cmd.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

                    cmd.Parameters.AddWithValue("@Id", usuario.Id);
                    
                    _connection.Open(); 
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
                    StringBuilder sql = new StringBuilder();
                    sql.Append("DELETE FROM Usuarios WHERE Id = @Id;");

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql.ToString();
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

        public void Inserir(Usuario usuario)
        {
            try
            {
                using (_connection)
                {
                    _connection.Open();

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine(@"INSERT INTO Usuarios (Nome, Email, Sexo, Rg, Cpf, NomeMae, SituacaoCadastro, DataCadastro) 
                                        VALUES (@Nome, @Email, @Sexo, @Rg, @Cpf, @NomeMae, @SituacaoCadastro, @DataCadastro);
                                        SELECT CAST(scope_identity() AS int) ");

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql.ToString();
                    cmd.Connection = (SqlConnection)_connection;

                    cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Rg", usuario.Rg);
                    cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    cmd.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                    cmd.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                    cmd.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

                    usuario.Id = (int)cmd.ExecuteScalar();
                }
                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                
            }
           

            return usuarios;
        }

        public Usuario Pesquisar(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@$"SELECT u.Id, 
                                 u.Nome, 
                                 u.Email, 
                                 u.Sexo, 
                                 u.RG, 
                                 u.CPF, 
                                 u.NomeMae, 
                                 u.SituacaoCadastro, 
                                 u.DataCadastro,
                                 c.Id AS contatoId,
                                 c.Telefone,
                                 c.Celular,
                                 c.UsuarioId AS usuarioId
                            FROM Usuarios u
                            LEFT JOIN Contatos c
                            ON c.usuarioId = u.id
                            WHERE u.Id = @Id;");

            using (_connection)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql.ToString();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = (SqlConnection)_connection;

                    _connection.Open();
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.Id = dataReader.GetInt32(0);
                        usuario.Nome = dataReader.GetString("Nome");
                        usuario.Email = dataReader.GetString("Email");
                        usuario.Sexo = dataReader.GetString("Sexo");
                        usuario.Rg = dataReader.GetString("RG");
                        usuario.Cpf = dataReader.GetString("CPF");
                        usuario.NomeMae = dataReader.GetString("NomeMae");
                        usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                        usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                        Contato contato = new Contato();
                        contato.Id = dataReader.GetInt32("contatoId");
                        contato.Telefone = dataReader.GetString("Telefone");
                        contato.Celular = dataReader.GetString("Celular");
                        contato.UsuarioId = usuario.Id;

                        usuario.Contato = contato;

                        return usuario;
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
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
