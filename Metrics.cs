

public class Metrics
{
    public string collisions;
    public string speed;
    public string wrongLaneCount;
    public string redLightVal;
    public string parkingAccuracy;

    public Metrics(string collisions, string speed, string wrongLaneCount, string redLightVal, string parkingAccuracy)
    {
        this.collisions = collisions;
        this.speed = speed;
        this.wrongLaneCount = wrongLaneCount;
        this.redLightVal = redLightVal;
        this.parkingAccuracy = parkingAccuracy;
    }
}
