﻿using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class SpawnBeetle : MonoBehaviour
    {
        private readonly string dataNode = "SpawnBeetle";
        private readonly string textNode = "SpawnBeetle";
        private int count;
        private int distance;
        private int maxBeetle;
        private int maxCount;
        private bool notWarned;

        public void BeetleEmerge()
        {
            if ((count == 1) && notWarned)
            {
                StoreMessage("Warning");

                notWarned = false;
            }
            else if (count < 1)
            {
                StoreMessage("Emerge");
                CreateBeetle();

                count = maxCount;
                notWarned = true;
            }
        }

        private void CreateBeetle()
        {
            int[][] position = GetPosition();

            int actor = GetComponent<SchedulingSystem>().CountActor;
            int maxActor = GetComponent<GameData>().GetIntData("Dungeon",
                "MaxActor");

            if ((position.Length < maxBeetle) || (actor > maxActor))
            {
                return;
            }
            position = FilterPosition(position);

            DungeonLevel dl = GetComponent<DungeonProgressData>()
                .GetDungeonLevel();
            SubObjectTag minion = GetComponent<ActorGroupData>()
                .GetActorGroup(dl)[CombatRoleTag.Minion][0];

            foreach (int[] p in position)
            {
                GetComponent<ObjectPool>().CreateObject(
                    MainObjectTag.Actor,
                    minion,
                    p);
            }
        }

        private int[][] FilterPosition(int[][] position)
        {
            SeedTag seed = GetComponent<DungeonProgressData>().GetDungeonSeed();
            int[][] randomized = position.OrderBy(
                p => GetComponent<RandomNumber>().Next(seed))
                .ToArray();
            Stack<int[]> result = new Stack<int[]>();

            for (int i = 0; i < maxBeetle; i++)
            {
                result.Push(randomized[i]);
            }
            return result.ToArray();
        }

        private int[][] GetPosition()
        {
            int[] pcPosition = GetComponent<ConvertCoordinates>().Convert(
                FindObjects.PC.transform.position);
            Stack<int[]> position = new Stack<int[]>();

            for (int i = pcPosition[0] - distance;
                i < pcPosition[0] + distance;
                i++)
            {
                for (int j = pcPosition[1] - distance;
                    j < pcPosition[1] + distance;
                    j++)
                {
                    if (GetComponent<DungeonTerrain>().IsPassable(i, j))
                    {
                        position.Push(new int[] { i, j });
                    }
                }
            }
            return position.ToArray();
        }

        private bool HasFungus(int[] position)
        {
            List<int[]> neighbor = GetComponent<ConvertCoordinates>()
                 .SurroundCoord(Surround.Diagonal, position);
            neighbor = GetComponent<DungeonBoard>().FilterPositions(neighbor);

            foreach (int[] n in neighbor)
            {
                if (GetComponent<DungeonBoard>().CheckBlock(
                    SubObjectTag.Fungus, n))
                {
                    return true;
                }
            }
            return false;
        }

        private void NourishFungus_SpawnCountDown(object sender,
            ActorInfoEventArgs e)
        {
            count--;

            if (HasFungus(e.Position))
            {
                count--;
            }
        }

        private void Start()
        {
            GetComponent<NourishFungus>().SpawnCountDown
                += NourishFungus_SpawnCountDown;

            maxCount = GetComponent<GameData>().GetIntData(dataNode, "MaxCount");
            distance = GetComponent<GameData>().GetIntData(dataNode, "Distance");
            maxBeetle = GetComponent<GameData>().GetIntData(dataNode, "Beetle");

            count = maxCount;
            notWarned = true;
        }

        private void StoreMessage(string node)
        {
            string text = GetComponent<GameText>().GetStringData(textNode,
                node);
            text = GetComponent<GameColor>().GetColorfulText(text,
                ColorName.Orange);
            GetComponent<CombatMessage>().StoreText(text);
        }
    }
}
