using IntechTask.Extensions;
using IntechTask.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace IntechTask.DataAccess.DataMappers
{
    public sealed class EmployeeMsSqlDataMapper : IEmployeeDataMapper
    {
        private readonly Func<SqlConnection> _connectionFactory;

        public EmployeeMsSqlDataMapper(Func<SqlConnection> connectionFactory) => _connectionFactory = connectionFactory;

        public async Task<IReadOnlyList<Employee>> GetAll()
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT employee_id, gender_id, fullname, date_of_birth FROM employee";

            connection.Open();
            await using var reader = cmd.ExecuteReader();

            return reader.AsEnumerable()
                .Select(ReadEmployee)
                .ToArray();
        }

        public async Task Insert(Employee entity)
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "INSERT INTO employee (gender_id, fullname, date_of_birth) OUTPUT INSERTED.employee_id VALUES (@gender_id, @fullname, @date_of_birth)";
            AddDataParameters(cmd, entity);

            connection.Open();
            entity.ID = (int) await cmd.ExecuteScalarAsync();
        }

        public async Task Update(Employee entity)
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "UPDATE employee SET gender_id = @gender_id, fullname = @fullname, date_of_birth = @date_of_birth WHERE employee_id = @employee_id";
            AddDataParameters(cmd, entity);
            AddIdentityParameter(cmd, entity);

            connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(Employee entity)
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "DELETE FROM employee WHERE employee_id = @employee_id";
            AddIdentityParameter(cmd, entity);

            connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IReadOnlyList<Employee>> FindByFullName(string searchTerm)
        {
            await using var connection = _connectionFactory();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT employee_id, gender_id, fullname, date_of_birth FROM employee WHERE fullname LIKE @search_term";
            cmd.Parameters.Add("@search_term", SqlDbType.NText).Value = $"%{searchTerm}%";

            connection.Open();
            await using var reader = cmd.ExecuteReader();

            return reader.AsEnumerable()
                .Select(ReadEmployee)
                .ToArray();
        }

        private static Employee ReadEmployee(IDataReader reader) =>
            new Employee
            {
                ID = reader.GetInt32(0),
                GenderID = reader.GetInt32(1),
                FullName = reader.GetString(2),
                DateOfBirth = reader.GetDateTime(3)
            };

        private static void AddIdentityParameter(SqlCommand cmd, Employee entity) =>
            cmd.Parameters.Add("@employee_id", SqlDbType.Int).Value = entity.ID;

        private static void AddDataParameters(SqlCommand cmd, Employee entity)
        {
            cmd.Parameters.Add("@gender_id", SqlDbType.Int).Value = entity.GenderID;
            cmd.Parameters.Add("@fullname", SqlDbType.NText).Value = entity.FullName;
            cmd.Parameters.Add("@date_of_birth", SqlDbType.Date).Value = entity.DateOfBirth;
        }
    }
}