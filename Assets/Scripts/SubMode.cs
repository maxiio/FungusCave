﻿using Fungus.Actor.Render;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem
{
    public enum SubModeUITag { Power, Log, Help, Setting };

    public class SubMode : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private StaticActor getActor;
        private UIObject getUI;

        private delegate GameObject StaticActor(SubObjectTag tag);

        private delegate GameObject UIObject(UITag tag);

        public GameObject ExamineTarget
        {
            get
            {
                int[] pos = coord.Convert(
                    getActor(SubObjectTag.Examiner).transform.position);

                return actor.GetActor(pos[0], pos[1]);
            }
        }

        public void SwitchModeBuyPower(bool switchOn)
        {
            SwitchUINormal(!switchOn);
            SwitchUIBuyPower(switchOn);
            SwitchUISubModeHeader(switchOn);

            GetComponent<UISubModeHeader>().SetMode(SubModeUITag.Power);
            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.BuyPower).SetActive(switchOn);
        }

        public void SwitchModeExamine(bool switchOn)
        {
            SwitchUIExamineModeline(switchOn);
            SwitchUIExamineMessage(false);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.Examiner).transform.position
                = FindObjects.PC.transform.position;
            getActor(SubObjectTag.Examiner).SetActive(switchOn);
        }

        public void SwitchModeLog(bool switchOn)
        {
            SwitchUINormal(!switchOn);
            SwitchUILog(switchOn);
            SwitchUISubModeHeader(switchOn);

            GetComponent<UISubModeHeader>().SetMode(SubModeUITag.Log);
            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            getActor(SubObjectTag.ViewLog).SetActive(switchOn);
        }

        public void SwitchUIExamineMessage(bool switchOn)
        {
            getUI(UITag.ExamineMessage).SetActive(switchOn);
            getUI(UITag.Message1).SetActive(!switchOn);
        }

        public void SwitchUIExamineModeline(bool switchOn)
        {
            getUI(UITag.ExamineModeline).SetActive(switchOn);
        }

        private void Start()
        {
            actor = GetComponent<ActorBoard>();
            coord = GetComponent<ConvertCoordinates>();
            getUI = FindObjects.GetUIObject;
            getActor = FindObjects.GetStaticActor;
        }

        private void SwitchUIBuyPower(bool switchOn)
        {
            getUI(UITag.BuyPowerSlotLabel).SetActive(switchOn);

            GetComponent<UIBuyPower>().ResetCursorPosition();
        }

        private void SwitchUILog(bool switchOn)
        {
            getUI(UITag.Log1).SetActive(switchOn);
        }

        private void SwitchUINormal(bool switchOn)
        {
            // UI
            getUI(UITag.Message1).SetActive(switchOn);
            getUI(UITag.Status).SetActive(switchOn);

            // Dungeon actors
            RenderSprite[] render = FindObjectsOfType<RenderSprite>();
            foreach (RenderSprite rs in render)
            {
                rs.GetComponentInParent<SpriteRenderer>().enabled = switchOn;
                rs.enabled = switchOn;
            }
        }

        private void SwitchUISubModeHeader(bool switchOn)
        {
            getUI(UITag.SubModeHeader).SetActive(switchOn);
            getUI(UITag.SubModeHeader).GetComponent<Text>().text
                = "Invalid header.";
        }
    }
}
