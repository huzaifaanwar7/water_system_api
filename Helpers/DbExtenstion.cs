using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

public static class DbSetExtensions
{
    public static async Task<List<T>> ExecuteStoredProcedureAsync<T>(this DbContext context, string procedureName, List<SqlParameter> parameters) where T : class, new()
    {
        using (var connection = new SqlConnection(context.Database.GetConnectionString()))
        {
            using (var command = new SqlCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                var resultList = new List<T>();

                try
                {
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var entityType = typeof(T);
                        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        while (await reader.ReadAsync())
                        {
                            var entity = new T();

                            foreach (var property in properties)
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                {
                                    property.SetValue(entity, reader.GetValue(reader.GetOrdinal(property.Name)));
                                }
                            }

                            resultList.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (log it, rethrow it, or handle it as per your requirements)
                    throw new Exception($"Error executing stored procedure {procedureName}: {ex.Message}", ex);
                }

                return resultList;
            }
        }
    }

    public static SqlParameter CreateParameter(string parameterName, SqlDbType dbType, object value)
    {
        return new SqlParameter(parameterName, dbType)
        {
            Value = value ?? DBNull.Value
        };
    }
}
