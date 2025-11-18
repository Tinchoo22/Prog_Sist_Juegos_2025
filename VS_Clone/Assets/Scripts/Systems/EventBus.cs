using System;
using System.Collections.Generic;

public static class EventBus
{
    public static event Action<int, int> OnXPChanged;
    public static event Action<int> OnLevelUp;
    public static event Action OnPlayerDied;

    public static event Action<int, int> OnPlayerHPChanged;
    public static void RaisePlayerHPChanged(int current, int max)
        => Enqueue(() => OnPlayerHPChanged?.Invoke(current, max));

    public static event Action<int> OnEnemyKilled;
    public static void RaiseEnemyKilled(int total)
        => Enqueue(() => OnEnemyKilled?.Invoke(total));

    private static readonly Queue<Action> _queued = new();

    public static void RaiseXPChanged(int current, int toNext)
        => Enqueue(() => OnXPChanged?.Invoke(current, toNext));

    public static void RaiseLevelUp(int lvl)
        => Enqueue(() => OnLevelUp?.Invoke(lvl));

    public static void RaisePlayerDied()
        => Enqueue(() => OnPlayerDied?.Invoke());

    private static void Enqueue(Action a) => _queued.Enqueue(a);

    public static void Pump()
    {
        while (_queued.Count > 0)
            _queued.Dequeue()?.Invoke();
    }

    public static void ClearQueue()
    {
        _queued.Clear();
    }
}
