﻿using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.WorldBuilding
{
    public enum FOVShape { Rhombus, Square };

    // Create a 2D array. Provide methods to inspect and change its content.
    public class DungeonBoard : MonoBehaviour, ISaveLoadBinary
    {
        private GameObject[,] blocks;
        private SubObjectTag[,] blueprint;
        private ConvertCoordinates coordinate;

        public int Height { get; private set; }

        public int Width { get; private set; }

        public bool ChangeBlock(GameObject go, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            blocks[x, y] = go;
            return true;
        }

        public bool ChangeBlueprint(SubObjectTag block, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            blueprint[x, y] = block;
            return true;
        }

        public bool CheckBlock(SubObjectTag block, int[] position)
        {
            int x = position[0];
            int y = position[1];

            return CheckBlock(block, x, y);
        }

        public bool CheckBlock(SubObjectTag block, int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return false;
            }

            return blueprint[x, y] == block;
        }

        public bool CheckBlock(SubObjectTag block, Vector3 position)
        {
            int[] index = coordinate.Convert(position);

            return CheckBlock(block, index[0], index[1]);
        }

        public List<int[]> FilterPositions(List<int[]> positions)
        {
            List<int[]> validPositions = new List<int[]>();

            foreach (var pos in positions)
            {
                if (!IndexOutOfRange(pos[0], pos[1]))
                {
                    validPositions.Add(pos);
                }
            }

            return validPositions;
        }

        public GameObject GetBlockObject(Vector3 position)
        {
            int[] index = coordinate.Convert(position);
            return GetBlockObject(index[0], index[1]);
        }

        public GameObject GetBlockObject(int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return null;
            }

            return blocks[x, y];
        }

        public SubObjectTag GetBlockTag(int x, int y)
        {
            if (IndexOutOfRange(x, y))
            {
                return SubObjectTag.NONE;
            }

            return blueprint[x, y];
        }

        public int GetDistance(int[] source, int[] target)
        {
            int x = Math.Abs(source[0] - target[0]);
            int y = Math.Abs(source[1] - target[1]);

            return Math.Max(x, y);
        }

        public int GetDistance(GameObject source, GameObject target)
        {
            int[] sourcePos = coordinate.Convert(source.transform.position);
            int[] targetPos = coordinate.Convert(target.transform.position);

            return GetDistance(sourcePos, targetPos);
        }

        public bool IndexOutOfRange(int x, int y)
        {
            bool checkWidth = (x < 0) || (x > Width - 1);
            bool checkHeight = (y < 0) || (y > Height - 1);

            return checkWidth || checkHeight;
        }

        public bool IsInsideRange(FOVShape shape, int maxRange,
            int[] source, int[] target)
        {
            int distance;

            if (IndexOutOfRange(target[0], target[1]))
            {
                return false;
            }

            switch (shape)
            {
                case FOVShape.Rhombus:
                    distance = Math.Abs(target[0] - source[0])
                        + Math.Abs(target[1] - source[1]);
                    break;

                case FOVShape.Square:
                    distance = Math.Max(
                        Math.Abs(target[0] - source[0]),
                        Math.Abs(target[1] - source[1]));
                    break;

                default:
                    return false;
            }
            return distance <= maxRange;
        }

        public void LoadBinary(IDataTemplate[] dt)
        {
            foreach (IDataTemplate d in dt)
            {
                if (d.DTTag == DataTemplateTag.Dungeon)
                {
                    DTDungeonBoard value = d as DTDungeonBoard;
                    blueprint = value.Blueprint;
                    return;
                }
            }
        }

        public void SaveBinary(Stack<IDataTemplate> dt)
        {
            DTDungeonBoard data = new DTDungeonBoard
            {
                Blueprint = blueprint
            };
            dt.Push(data);
        }

        private void Awake()
        {
            Height = 17;
            Width = 24;
        }

        private void DungeonBoard_LoadingGame(object sender, LoadEventArgs e)
        {
            LoadBinary(e.GameData);
        }

        private void DungeonBoard_SavingGame(object sender, SaveEventArgs e)
        {
            SaveBinary(e.GameData);
        }

        private void Start()
        {
            coordinate = GetComponent<ConvertCoordinates>();

            blueprint = new SubObjectTag[Width, Height];
            blocks = new GameObject[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    blueprint[i, j] = SubObjectTag.Floor;
                }
            }

            GetComponent<SaveLoadGame>().SavingGame += DungeonBoard_SavingGame;
            GetComponent<SaveLoadGame>().LoadingGame += DungeonBoard_LoadingGame;
        }
    }
}
