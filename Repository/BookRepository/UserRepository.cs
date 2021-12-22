using Microsoft.Extensions.Configuration;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    }
}
