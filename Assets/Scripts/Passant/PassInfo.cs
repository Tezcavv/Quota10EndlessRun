[System.Serializable]
public struct PassInfo
{
    public enum PassType
    {
        police = 0,
        actor = 1,
        other = 2,
    }

    public int hitScore;

    public PassType type;
}