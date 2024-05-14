using System;

namespace evantage.Models;

public sealed class LogRecord
{
    public string id { get; set; } = string.Empty;
    public string exception_text { get; set; } = string.Empty;
    public string exception_message { get; set; } = "OOOPS!"; // = string.Empty;

    public string exception_severity { get; set; } = "HIGH";

    public string sql_parameters { get; set; } = string.Empty;
    public string payload { get; set; } = string.Empty;
    public string diff { get; set; } = "{}";
    public string operation_name { get; set; } = string.Empty;
    public string breadcrumb { get; set; } = string.Empty;
    public string table_name { get; set; } = string.Empty;
    public string server_name { get; set; } = string.Empty;
    public string database_name { get; set; } = "railway"; // = string.Empty;
    public string application_name { get; set; } = string.Empty;
    public string modified_by { get; set; } = string.Empty;
    public string created_by { get; set; } = string.Empty;
    public DateTime modified_at { get; set; }
    public DateTime created_at { get; set; }

    public string commit_url { get; set; } = string.Empty;
    public string issue_url { get; set; } = string.Empty;
    public bool is_deleted { get; set; }
    public bool is_archived { get; set; }
    public bool is_enabled { get; set; }
}