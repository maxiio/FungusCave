﻿using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor
{
    public class PCInfection : MonoBehaviour, IInfection
    {
        private InfectionData data;
        private int lowRate;
        private int normalDuration;
        private int shortDuration;

        public int InfectionDuration
        {
            get
            {
                return GetComponent<Power>().IsActive(PowerTag.DefInfection2)
                    ? shortDuration : normalDuration;
            }
        }

        public string GetHealthReport(HealthTag status)
        {
            switch (status)
            {
                case HealthTag.Stressed:
                    return "You feel stressed.";

                case HealthTag.Infected:
                    return "You are infected.";

                case HealthTag.Overflowed:
                    return "You feel exhausted.";

                default:
                    return "";
            }
        }

        public int GetInfectionRate(GameObject attacker)
        {
            // The universal base rate is decided by victim's current HP and
            // environment.
            int baseRate = GetComponent<InfectionRate>().GetInfectionRate();

            // PC's defending power, which is optional and can be purchased by
            // potions.
            int defend
                = GetComponent<Power>().IsActive(PowerTag.DefInfection1)
                ? lowRate : 0;

            // NPC's attacking power (attack) is a static value.
            int attack
                = FindObjects.GameLogic.GetComponent<ActorData>().GetIntData(
                    attacker.GetComponent<MetaInfo>().SubTag,
                    DataTag.InfectionAttack);

            int final = baseRate + attack - defend;
            return final;
        }

        public int GetInfectionRate()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            data = FindObjects.GameLogic.GetComponent<InfectionData>();

            lowRate = data.LowInfectionRate;
            normalDuration = data.NormalDuration;
            shortDuration = data.ShortDuration;
        }
    }
}
