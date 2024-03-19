using System;

public enum Map
{
    Default
}

public enum GameMode
{
    Default
}

public enum GameQueue
{
    Solo,
    Team
}

[Serializable]
public class UserData
{
    public string UserAuthId;
    public string UserName;
    public GameInfo UserGamePreferences = new GameInfo();
}

[Serializable]
public class GameInfo
{
    public Map Map;
    public GameMode Mode;
    public GameQueue Queue;

    public string ToMultiplayQueue()
    {
        return Queue switch
        {
            GameQueue.Solo => "solo-queue",
            GameQueue.Team => "team-queue",
            _ => "solo-queue"
        };
    }
}