using System.Collections.Generic;

public interface IReceiver
{
    List<IOobserver> Observers { get; }
}

public static class ReceiverExtensions
{
    public static void AddObserver(this IReceiver receiver, IOobserver observer)
    {
        receiver.Observers.Add(observer);
    }

    public static void RemoveObserver(this IReceiver receiver, IOobserver observer)
    {
        receiver.Observers.Remove(observer);
    }

    public static void NotifyObservers(this IReceiver receiver, Event gameEvent)
    {
        foreach (var observer in receiver.Observers)
        {
            observer.OnNotify(gameEvent);
        }
    }
}
