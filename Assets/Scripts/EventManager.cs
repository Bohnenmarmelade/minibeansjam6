using Framework;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityEvent OnPlayerDeath { get; } = new UnityEvent();
    public UnityEvent OnPlayerAdult { get; } = new UnityEvent();
}