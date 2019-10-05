
public struct Level
{
    public string Id;
    public int WinPoints;

    public Level(string id, int winPoints)
    {
        Id = id;
        WinPoints = winPoints;
    }
}

public static class SceneConstants
{
    public static readonly Level[] Levels = {
        new Level("level1", 10),
        new Level("level2", 15),
    };
}