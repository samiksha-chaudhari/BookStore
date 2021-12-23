using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Repository.BookRepository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;

        public bool Register(RegisterModel userData)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));

            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("usp_AddUser", sqlConnection);
                    //userData.Password = EncryptPassword(userData.Password);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@UserName", userData.UserName);
                    sqlCommand.Parameters.AddWithValue("@Email", userData.Email);
                    sqlCommand.Parameters.AddWithValue("@PhoneNo", userData.PhoneNo);
                    sqlCommand.Parameters.AddWithValue("@Password", userData.Password);                   
                    
                    int result = sqlCommand.ExecuteNonQuery();

                    if (result>0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string Login(LoginModel login)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));

            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spUserLogin", sqlConnection);
                   // login.Password = EncryptPassword(login.Password);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@Email", login.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", login.Password);
                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;

                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@user"].Value;
                    if (!(result is DBNull))
                    {
                        if (Convert.ToInt32(result) == 2)
                        {
                            GetUserDetails(login.Email);
                            return "Login is Successfull";
                        }
                        return "Incorrect email or password";
                    }
                    return "Login failed";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string EncryptPassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public void GetUserDetails(string email)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "SELECT * FROM RegUser WHERE Email = @Email";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Email", email);
                    sqlConnection.Open();

                    RegisterModel registerModel = new RegisterModel();
                    SqlDataReader sqlData = sqlCommand.ExecuteReader();
                    if (sqlData.Read())
                    {
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();
                        database.StringSet(key: "UserName", sqlData.GetString("UserName"));
                        database.StringSet(key: "PhoneNo", sqlData.GetString("PhoneNo"));
                        database.StringSet(key: "UserId", sqlData.GetInt32("UserId").ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string GenerateToken(string userName)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(this.Configuration["SecretKey"]);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]{new Claim(ClaimTypes.Name, userName)}),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }

        public string ForgotPassword(string Email)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            try
            {
                using (sqlConnection)
                {
                    SqlCommand sqlCommand = new SqlCommand("spUserForget", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@Email", Email);
                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;

                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@user"].Value;
                                       
                    if (!(result is DBNull))
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress(this.Configuration["Credentials:Email"]);
                        mail.To.Add(Email);
                        mail.Subject = "To Test Out Mail";
                        SendMSMQ();
                        mail.Body = ReceiveMSMQ();

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(this.Configuration["Credentials:Email"], this.Configuration["Credentials:Password"]);
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                    }
                    return "Mail is send";
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        public bool ResetPassword(ResetPasswordModel resetPassword)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spUserReset", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    var password = this.EncryptPassword(resetPassword.Password);
                    sqlCommand.Parameters.AddWithValue("@UserId", resetPassword.UserId);
                    sqlCommand.Parameters.AddWithValue("@Password", password);
  
                    sqlCommand.ExecuteNonQuery();

                    return true;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
        public void SendMSMQ()
        {
            MessageQueue messageQueue;
            if (MessageQueue.Exists(@".\Private$\BookStore"))
            {
                messageQueue = new MessageQueue(@".\Private$\BookStore");
            }
            else
            {
                messageQueue = MessageQueue.Create(@".\Private$\BookStore");
            }
            string body = "http://localhost:4200/forgetpassword";
            messageQueue.Label = "Mail Body";
            messageQueue.Send(body);
        }
        public string ReceiveMSMQ()
        {
            MessageQueue messageQueue = new MessageQueue(@".\Private$\BookStore");
            var receivemsg = messageQueue.Receive();
            receivemsg.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            return receivemsg.Body.ToString();
        }
    }
}
