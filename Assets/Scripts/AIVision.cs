﻿using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class AIVision : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private FieldOfView fov;

        public bool CanSeeTarget(MainObjectTag targetTag)
        {
            return CanSeeTarget<MainObjectTag>(targetTag);
        }

        public bool CanSeeTarget(SubObjectTag targetTag)
        {
            return CanSeeTarget<SubObjectTag>(targetTag);
        }

        private bool CanSeeTarget<T>(T targetTag)
        {
            int[] position = coord.Convert(transform.position);
            int x = position[0];
            int y = position[1];
            int range = fov.MaxRange;

            for (int i = x - range; i < x + range + 1; i++)
            {
                for (int j = y - range; j < y + range + 1; j++)
                {
                    if ((i == x && j == y)
                        || !fov.CheckFOV(FOVStatus.Insight, i, j))
                    {
                        continue;
                    }

                    if (actor.CheckActorTag(targetTag, i, j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();

            fov = GetComponent<FieldOfView>();
        }
    }
}
