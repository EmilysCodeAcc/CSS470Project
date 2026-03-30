using UnityEngine;
[System.Serializable]
public class LevelProgress
{
    public int unlockedLevel;
    public string levelName;
       public LevelProgress(int unlockedLevel, string levelName)
    {
       this.levelName = levelName;
       this.unlockedLevel = unlockedLevel;

    }
}
