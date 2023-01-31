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
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();

            #region Update Usuarios
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"UPDATE Usuarios 
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
                    cmd.Transaction = transaction;

                    cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Rg", usuario.Rg);
                    cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    cmd.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                    cmd.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                    cmd.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

                    cmd.Parameters.AddWithValue("@Id", usuario.Id);

                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Update Contatos
                    sql.Clear();

                    sql.Append(@"UPDATE Contatos 
                                 SET UsuarioId = @UsuarioId, 
                                     Telefone  = @Telefone, 
                                     Celular   = @Celular 
                                 WHERE 
                                    Id = @Id"
                                );

                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    cmd.Parameters.AddWithValue("@Telefone", usuario.Contato.Telefone);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Contato.Celular);

                    cmd.ExecuteNonQuery();

                    #endregion

                    sql.Clear();

                    #region Update Endereços de Entrega

                    foreach (var endereco in usuario.EnderecosEntrega)
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)_connection;
                        cmd.Transaction = transaction;

                        sql.Append(@"UPDATE EnderecosEntrega 
                                         SET UsuarioId = @UsuarioId,
                                             NomeEndereco = @NomeEndereco,
                                             CEP          = @Cep,
                                             Estado       = @Estado,
                                             Cidade       = @Cidade,
                                             Bairro       = @Bairro,
                                             Endereco     = @Endereco,
                                             Numero       = @Numero,
                                             Complemento  = @Complemento 
                                        WHERE Id         = @Id;"
                                );

                        cmd.CommandText = sql.ToString();
                        cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@NomeEndereco", endereco.NomeEndereco);
                        cmd.Parameters.AddWithValue("@Cep", endereco.Cep);
                        cmd.Parameters.AddWithValue("@Estado", endereco.Estado);
                        cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                        cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                        cmd.Parameters.AddWithValue("@Endereco", endereco.Endereco);
                        cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                        cmd.Parameters.AddWithValue("@Complemento", endereco.Complemento);
                        cmd.Parameters.AddWithValue("@Id", endereco.Id);

                        cmd.ExecuteNonQuery();
                    }

                   
                    #endregion

                    sql.Clear();

                    #region Update Departamentos
                    cmd = new SqlCommand();
                    cmd.Connection = (SqlConnection)_connection;
                    cmd.Transaction = transaction;

                    sql.Append(@"DELETE FROM UsuariosDepartamentos WHERE UsuarioId = @UsuarioId;");
                    cmd.CommandText = sql.ToString();
                    cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                    cmd.ExecuteNonQuery();

                   

                    foreach (var departamento in usuario.Departamentos)
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)_connection;
                        cmd.Transaction = transaction;

                        sql.Clear();

                        sql.Append(@"INSERT INTO UsuariosDepartamentos
                                            (UsuarioId, 
                                             DepartamentoId
                                            ) 
                                            VALUES 
                                            (@UsuarioId,
                                             @DepartamentoId
                                            );
                                           ");

                        cmd.CommandText = sql.ToString();

                        cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@DepartamentoId", departamento.Id);

                        cmd.ExecuteNonQuery();
                    }

                    #endregion

                    transaction.Commit();
                }

                

            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    // poderia ser aplicado o registro no log do sistema
                    throw;
                }

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
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction) _connection.BeginTransaction();
            try
            {
                using (_connection)
                {
                   
                    StringBuilder sql = new StringBuilder();
                    sql.Append(@"INSERT INTO Usuarios 
                                (Nome, 
                                    Email, 
                                    Sexo, 
                                    Rg, 
                                    Cpf, 
                                    NomeMae, 
                                    SituacaoCadastro, 
                                    DataCadastro
                                    ) 
                                    VALUES 
                                    (@Nome, 
                                    @Email, 
                                    @Sexo, 
                                    @Rg, 
                                    @Cpf, 
                                    @NomeMae, 
                                    @SituacaoCadastro, 
                                    @DataCadastro);
                                    SELECT CAST(scope_identity() AS int
                                    )");

                    SqlCommand cmd = new SqlCommand();
                    cmd.Transaction = transaction;
                    cmd.Connection = (SqlConnection)_connection;

                    cmd.CommandText = sql.ToString();

                    cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                    cmd.Parameters.AddWithValue("@Rg", usuario.Rg);
                    cmd.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    cmd.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                    cmd.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                    cmd.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

                    usuario.Id = (int)cmd.ExecuteScalar();

                    sql.Clear();
                    sql.Append(@"INSERT INTO Contatos 
                                (usuarioId, 
                                    Telefone, 
                                    Celular
                                )
                                VALUES 
                                (@usuarioId, 
                                    @Telefone, 
                                    @Celular
                                ); 
                                SELECT 
                                CAST
                                (
                                    scope_identity() AS int
                                );"
                                );

                    cmd.CommandText = sql.ToString();
                    cmd.Parameters.AddWithValue("@usuarioId", usuario.Id);
                    cmd.Parameters.AddWithValue("@Telefone", usuario.Contato.Telefone);
                    cmd.Parameters.AddWithValue("@Celular", usuario.Contato.Celular);

                    usuario.Contato.UsuarioId = usuario.Id;
                    usuario.Contato.Id = (int)cmd.ExecuteScalar();


                    sql.Clear();

                    

                    foreach(var endereco in usuario.EnderecosEntrega)
                    {

                        cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)_connection;
                        cmd.Transaction = transaction;

                        sql.Append(@"INSERT INTO EnderecosEntrega 
                                    (UsuarioId, 
                                    NomeEndereco, 
                                    CEP,
                                    Estado, 
                                    Cidade, 
                                    Bairro, 
                                    Endereco, 
                                    Numero, 
                                    Complemento
                                    )
                                    VALUES 
                                    (@UsuarioId, 
                                     @NomeEndereco, 
                                     @Cep, 
                                     @Estado, 
                                     @Cidade,
                                     @Bairro, 
                                     @Endereco, 
                                     @Numero, 
                                     @Complemento); 
                                     SELECT CAST(scope_identity() as int)"
                                    );
                        
                        cmd.CommandText = sql.ToString();
                        

                        cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@NomeEndereco", endereco.NomeEndereco);
                        cmd.Parameters.AddWithValue("@Cep", endereco.Cep);
                        cmd.Parameters.AddWithValue("@Estado", endereco.Estado);
                        cmd.Parameters.AddWithValue("@Cidade", endereco.Cidade);
                        cmd.Parameters.AddWithValue("@Bairro", endereco.Bairro);
                        cmd.Parameters.AddWithValue("@Endereco", endereco.Endereco);
                        cmd.Parameters.AddWithValue("@Numero", endereco.Numero);
                        cmd.Parameters.AddWithValue("@Complemento", endereco.Complemento);

                        endereco.Id = (int)cmd.ExecuteScalar();
                        endereco.UsuarioId = usuario.Id;
                    }


                    foreach(var departamento in usuario.Departamentos)
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = (SqlConnection)_connection;
                        cmd.Transaction = transaction;

                        sql.Clear();

                        sql.Append(@"INSERT INTO UsuariosDepartamentos
                                            (UsuarioId, 
                                             DepartamentoId
                                            ) 
                                            VALUES 
                                            (@UsuarioId,
                                             @DepartamentoId
                                            );
                                           ");

                        cmd.CommandText = sql.ToString();

                        cmd.Parameters.AddWithValue("@UsuarioId", usuario.Id);
                        cmd.Parameters.AddWithValue("@DepartamentoId", departamento.Id);       
                        
                        cmd.ExecuteNonQuery();  
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {

                try
                {
                    transaction.Rollback();
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }

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

            using (_connection)
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
	                             c.Id,
	                             c.Telefone,
	                             c.Celular,
	                             c.UsuarioId,
	                             ee.Id,
	                             ee.UsuarioId,
	                             ee.NomeEndereco,
	                             ee.CEP,
	                             ee.Estado,
	                             ee.Cidade,
	                             ee.Bairro,
	                             ee.Endereco,
	                             ee.Numero,
	                             ee.Complemento,
	                             ud.Id,
	                             ud.DepartamentoId,
	                             ud.UsuarioId,
	                             d.Id,
	                             d.Nome
	                        FROM Usuarios u
	                        LEFT JOIN Contatos c
	                        ON c.UsuarioId = u.Id
	                        LEFT JOIN EnderecosEntrega ee
	                        ON ee.UsuarioId = u.Id
	                        LEFT JOIN UsuariosDepartamentos ud
	                        ON ud.UsuarioId = u.Id
	                        LEFT JOIN Departamentos d
	                        ON d.Id = ud.DepartamentoId
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

                    Dictionary<int, Usuario> usuarios = new Dictionary<int, Usuario>();

                    while (dataReader.Read())
                    {
                        Usuario usuario = new Usuario();

                        if (!usuarios.ContainsKey(dataReader.GetInt32(0)))
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

                            Contato contato = new Contato();
                            contato.Id = dataReader.GetInt32(9);
                            contato.Telefone = dataReader.GetString("Telefone");
                            contato.Celular = dataReader.GetString("Celular");
                            contato.UsuarioId = usuario.Id;

                            usuario.Contato = contato;
                            usuarios.Add(usuario.Id, usuario);
                        }
                        else
                        {
                            usuario = usuarios[dataReader.GetInt32(0)];
                        }

                        EnderecoEntrega enderecoEntrega = new EnderecoEntrega();
                        enderecoEntrega.Id = dataReader.GetInt32(13);
                        enderecoEntrega.UsuarioId = usuario.Id;
                        enderecoEntrega.NomeEndereco = dataReader.GetString("NomeEndereco");
                        enderecoEntrega.Cep = dataReader.GetString("Cep");
                        enderecoEntrega.Estado = dataReader.GetString("Estado");
                        enderecoEntrega.Cidade = dataReader.GetString("Cidade");
                        enderecoEntrega.Bairro = dataReader.GetString("Bairro");
                        enderecoEntrega.Endereco = dataReader.GetString("Endereco");
                        enderecoEntrega.Numero = dataReader.GetString("Numero");
                        enderecoEntrega.Complemento = dataReader.GetString("Complemento");

                        usuario.EnderecosEntrega = (usuario.EnderecosEntrega == null ? new List<EnderecoEntrega>() : usuario.EnderecosEntrega);


                        if (usuario.EnderecosEntrega.FirstOrDefault(e => e.Id == enderecoEntrega.Id) == null)
                        {
                            usuario.EnderecosEntrega.Add(enderecoEntrega);
                        }

                        Departamento departamento = new Departamento();
                        departamento.Id = dataReader.GetInt32(26);
                        departamento.Nome = dataReader.GetString(27);

                        usuario.Departamentos = (usuario.Departamentos == null) ? new List<Departamento>() : usuario.Departamentos;

                        if (usuario.Departamentos.FirstOrDefault(d => d.Id == departamento.Id) == null)
                        {
                            usuario.Departamentos.Add(departamento);
                        }
                    }

                    return usuarios[usuarios.Keys.First()];
                }

                catch (Exception ex)
                {
                    return null;

                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
