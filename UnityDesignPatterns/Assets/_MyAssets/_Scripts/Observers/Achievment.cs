using UnityEngine;

public class Achievment 
{
    public string Name {  get; private set; } 
    public bool IsUnlocked {  get; private set; }
    private int progress;
    public int maxProgress;

    public Achievment(string name, int maxProg = 1)
    {
        Name = name;
        IsUnlocked = false;
        progress = 0;
        maxProgress = maxProg;
    }

    public void Progress()
    {
        if (++progress == maxProgress)
        {
            IsUnlocked |= true;
            Debug.Log("Acheivment Unlocked! " + Name);
        }
    }
}
