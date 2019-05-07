﻿using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IHP : ITurnCounter
    {
        void RestoreAfterKill();
    }

    public class HP : MonoBehaviour, IResetData, ILoadActorData
    {
        private ActorData actorData;

        public int CurrentHP { get; private set; }

        public bool LoadedActorData { get; private set; }

        public int MaxHP
        {
            get
            {
                return actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.HP);
            }
        }

        public void GainHP(int hp)
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + hp);
        }

        public void Load(DTActor data)
        {
            CurrentHP = data.HP;
            LoadedActorData = true;
            if (GetComponent<MetaInfo>().SubTag == SubObjectTag.PC)
            {
                Debug.Log("Load HP: " + data.HP);
                Debug.Log("Current HP: " + CurrentHP);
            }
        }

        public void LoseHP(int hp)
        {
            CurrentHP = Math.Max(0, CurrentHP - hp);

            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().ReviveSelf();
            }
            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().BurySelf();
            }
        }

        public void Reset()
        {
            CurrentHP = MaxHP;
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();

            if (!LoadedActorData)
            {
                Reset();
            }
            if (GetComponent<MetaInfo>().SubTag == SubObjectTag.PC)
            {
                Debug.Log("Start HP");
            }
        }
    }
}
