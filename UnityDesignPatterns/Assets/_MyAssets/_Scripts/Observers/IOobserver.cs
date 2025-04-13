public enum Event
{
    // achievment events
    Playerjumped, 
    FiveRolls,
    // other events
    ClassDismissed
}
public interface IOobserver 
{
    void OnNotify(Event gameEvent); // prototype. no definition 
}
