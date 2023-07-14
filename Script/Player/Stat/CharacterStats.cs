using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Player.CharacterStats {
    [Serializable]
    public class CharacterStats
    {
        public float BaseValue;

        public float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true; //check if we need to return the value or not
        protected float _value; //store the most recent calculated value
        protected float lastBaseValue = float.MinValue;

        protected readonly List<StatsModifier> statsModifiers;
        public readonly ReadOnlyCollection<StatsModifier> StatsModifiers;

        public CharacterStats()
        {
            statsModifiers = new List<StatsModifier>();
            StatsModifiers = statsModifiers.AsReadOnly();
        }

        public CharacterStats(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatsModifier mod)
        {
            isDirty = true;
            statsModifiers.Add(mod);
            statsModifiers.Sort(CompareModifierOrder);
        }

        protected virtual int CompareModifierOrder(StatsModifier a, StatsModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if (a.Order == b.Order)
        }

        public virtual bool RemoveModifier(StatsModifier mod)
        {
            if (statsModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statsModifiers.Count - 1; i >= 0; i--)
            {
                if (statsModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statsModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        //The idea for this function is for example we get base: +10 - level: +20 - talent: +10%
        //We will want the funciton is (10 +20)*1.1 instead of 10*1.1 +20
        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statsModifiers.Count; i++)
            {
                StatsModifier mod = statsModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;
                    //check until we end of the list or theres no more PercentAdd value
                    //PercentAdd is increased damage & PercentMult is more damage
                    if (i + 1 >= statsModifiers.Count || statsModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;

                        //reset value of sumPercentAdd
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
            //18.0001f != 18f
            return (float)Math.Round(finalValue, 4);
        }
    }
    
}
