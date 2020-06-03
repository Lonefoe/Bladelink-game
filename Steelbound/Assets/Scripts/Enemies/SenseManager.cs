using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Senses
{
    public class SenseManager
    {
        public event Action onTakeDamageEvent;

        public void Report(Sense sense)
        {
            switch(sense)
            {
                case Sense.Damage:
                onTakeDamageEvent();
                break;

                case Sense.Range:
                break;
            }
        }

    }

    public enum Sense
    {
        Range,
        Damage,
    }
}
