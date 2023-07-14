using UnityEngine;
using Player.CharacterStats;

public class PlayerStats : MonoBehaviour
{
    //public CharacterStats MaximumHealth { get; private set; }
    //public CharacterStats currentHealth;
    //public CharacterStats MaximumMana { get; private set; }
    //public CharacterStats currentMana;

    public CharacterStats Strength;
    public CharacterStats Int;

}





/*
public class Item
{
    public void Equip(DaxuaStats c)
    {
        c.Strength.AddModifier(new StatsModifier(10, StatModType.Flat, this));
        c.Strength.AddModifier(new StatsModifier(0.1f, StatModType.PercentMult, this));
    }

    public void Unequip(DaxuaStats c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
    }
}
*/
