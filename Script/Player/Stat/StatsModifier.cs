namespace Player.CharacterStats
{
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }
    public class StatsModifier
    {
        public readonly float Value;
        public StatModType Type;
        public readonly int Order;
        public readonly object Source; //object variable so that we can store anything we want suchas: int, enum, float, ...

        public StatsModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        //(int)type is the default value of order
        public StatsModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

        public StatsModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        public StatsModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }

    }
}
