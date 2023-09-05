using System;

public struct GameobjectLog
{
    public GameobjectLog(DateTime logTime, string gameobjectName, string componentName, string logText)
    {
        _logTime = logTime;
        _gameobjectName = gameobjectName;
        _componentName = componentName;
        _logText = logText;
    }

    public readonly DateTime _logTime;
    public readonly string _gameobjectName;
    public readonly string _componentName;
    public readonly string _logText;

    public string GetLogInStringFormat()
    {
        return $"{_logTime}. Gameobject {_gameobjectName}. Component {_componentName}. Text: {_logText}";
    }
}