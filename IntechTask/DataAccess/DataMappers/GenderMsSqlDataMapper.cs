using IntechTask.Extensions;
using IntechTask.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IntechTask.DataAccess.DataMappers
{
    public sealed class GenderMsSqlDataMapper : IReadOnlyDataMapper<Gender>
    {
        private readonly Func<SqlConnection> _connectionFactory;

        public GenderMsSqlDataMapper(Func<SqlConnection> connectionFactory) => _connectionFactory = connectionFactory;

        public async Task<IReadOnlyList<Gender>> GetAll()
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT gender_id, name FROM gender";

            connection.Open();
            await using var reader = await cmd.ExecuteReaderAsync();

            return reader.AsEnumerable()
                .Select(x =>
                    new Gender(
                        x.GetInt32(0),
                        x.GetString(1))
                )
                .ToArray();
        }
    }
}