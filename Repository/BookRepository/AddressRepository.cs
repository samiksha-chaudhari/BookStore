using Microsoft.Extensions.Configuration;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.BookRepository
{
    public class AddressRepository : IAddressRepository
    {
        public AddressRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;

        public bool AddAddress(AddressModel addressDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));

            using (sqlConnection)

                try
                {
                    SqlCommand sqlCommand = new SqlCommand("SpAddUserAddress", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Address", addressDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressDetails.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressDetails.State);
                    sqlCommand.Parameters.AddWithValue("@Type", addressDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@UserId", addressDetails.UserId);

                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    else
                        return false;

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
