using System.Collections.Generic;

public class AchievmentObserver : IOobserver
{
    private Dictionary<Event, Achievment> acheivments;

    public AchievmentObserver()
    {
        acheivments = new Dictionary<Event, Achievment>();
        acheivments[Event.Playerjumped] = new Achievment("First Jump");
        acheivments[Event.FiveRolls] = new Achievment("Five Rolls", 5);
    }

    public void OnNotify(Event gameEvent)
    {
        if(acheivments.ContainsKey(gameEvent) && !acheivments[gameEvent].IsUnlocked)
        {
            acheivments[gameEvent].Progress();
        }
    }
}
