using UnityEngine;

public class LightStateContructor
{
    public bool lightsOn;
    public bool leftTurnSignalsOn;
    public bool rightTurnSignalsOn;
    public LightStateContructor(bool lightsOn, bool leftTurnSignalsOn, bool rightTurnSignalsOn)
    {
        this.lightsOn = lightsOn;
        this.leftTurnSignalsOn = leftTurnSignalsOn;
        this.rightTurnSignalsOn = rightTurnSignalsOn;
    }
}
