using System.Collections.Generic;


public class PlayerBuildMemento
{
    public readonly Dictionary<string, float> values;

    public PlayerBuildMemento(Dictionary<string, float> source)
    {
      values = new Dictionary<string, float>(source ?? new Dictionary<string, float>());
    }
}
