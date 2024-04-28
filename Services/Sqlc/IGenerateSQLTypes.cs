using System.Data.SqlClient;

namespace CodeMechanic.Sqlc;

public interface IGenerateSQLTypes
{
    Task<ProposedScript> ScriptTypeAs<T>(ScriptOptions scriptOptions);
    List<ParsedSqlQuery> FindAllProcs(string directory_path);
    List<SqlParameter> ParseParametersFromSQL(string sql);
    List<SqlParameter> ParseTypesFromSQL(string sql);
    bool ValidateSQL();
    string OutFolderPath { get; set; }
}