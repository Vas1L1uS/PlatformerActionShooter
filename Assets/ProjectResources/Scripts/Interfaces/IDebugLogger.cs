using System.Collections.Generic;

public interface IDebugLogger
{
    List<GameobjectLog> GameobjectLog_list { get; set; }
    bool EnabledPrintDebugLogInEditor { get; set; }
    bool EnabledAddingLogs { get; set; }
    void PrintLogInEditor(string text);
    void AddLog(GameobjectLog log);
}