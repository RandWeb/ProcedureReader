using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace ReadProcedure.Services;

public interface IProcedureReaderService
{
    Task<string> ReadAsync(string procedureName);
}

public class ProcedureReaderService : IProcedureReaderService
{
    private SqlConnection _sqlConnection;

    public ProcedureReaderService()
    {

    }

    public async Task<string> ReadAsync(string procedureName)
    {
        try
        {
            string readProcedureInformationQuery =
                $"SELECT * FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME='{procedureName}' ORDER BY ORDINAL_POSITION";

            _sqlConnection =
                new(
                    "server=serverName; database=databaseName; user id=Sa; password=@nara1402;Connection Timeout=60;TrustServerCertificate=True;");
            _sqlConnection.Open();
            SqlCommand sqlCommand = _sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = readProcedureInformationQuery;
            var reader = await sqlCommand.ExecuteReaderAsync();
            StringBuilder stringBuilder = new StringBuilder($"public class {procedureName.Substring(2).ToPascalCase()} "+"{");
            while (await reader.ReadAsync())
            {
                if (reader["PARAMETER_MODE"].ToString().Equals("IN"))
                {
                    string type = GetDataType(reader["DATA_TYPE"].ToString());
                    stringBuilder.Append($"\n\t public {type} {reader["PARAMETER_NAME"].ToString().Split("@")[1].ToPascalCase()}"+" {set;get;}");
                }

            }

            stringBuilder.Append("\n}");
            
            return stringBuilder.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
        finally
        {
            await _sqlConnection.CloseAsync();
        }

    }

    private string GetDataType(string type)
    {
        if (type.Equals("varchar"))
            return "string";
        if (type.Equals("date"))
            return "DateTime";
        if (type.Equals("int"))
            return "int";
        return "string";
    }
}

public static class StringExtensions
{
    public static string ToPascalCase(this string name)
    {
        return Regex.Replace(name, @"^\w|_\w", 
            (match) => match.Value.Replace("_", "").ToUpper());
    }
}