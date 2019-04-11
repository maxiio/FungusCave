﻿using Fungus.Actor;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    // https://stackoverflow.com/questions/10917555/adding-a-new-line-break-tag-in-xml
    public class GameText : MonoBehaviour
    {
        private XElement xFile;

        public string GetHelp()
        {
            string[] elements = new string[]
            { "Title", "Normal", "Examine", "Menu" };
            string[] text = new string[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                text[i] = GetText(xFile.Element("Help").Element(elements[i]));
            }
            return string.Join("\n\n", text);
        }

        public string[] GetOpening()
        {
            XElement xOpening = xFile.Element("Opening");
            string scene = GetText(xOpening.Element("Scene"));
            string modeline = GetText(xOpening.Element("Modeline"));

            string[] text = new string[] { scene, modeline };
            return text;
        }

        public string GetPowerDescription(PowerTag tag)
        {
            return GetText(xFile.Element("PowerDescription").Element(
                tag.ToString()));
        }

        public string GetSettingCursor()
        {
            return GetText(xFile.Element("Setting").Element("Cursor"));
        }

        public string GetSettingOption(bool yesOrNo)
        {
            return yesOrNo
                ? GetText(xFile.Element("Setting").Element("SwitchOn"))
                : GetText(xFile.Element("Setting").Element("SwitchOff"));
        }

        public string GetSettingText(string xElement)
        {
            return GetText(xFile.Element("Setting").Element(xElement));
        }

        private string GetText(XElement xElement)
        {
            string text;
            string myLang = GetComponent<GameSetting>().UserLanguage;

            if (string.IsNullOrEmpty((string)xElement.Element(myLang)))
            {
                myLang = GetComponent<GameSetting>().DefaultLanguage;
            }

            text = xElement.Element(myLang).Value.ToString();
            text = text.Replace(@"\n", "\n");
            return text;
        }

        private void Start()
        {
            xFile = GetComponent<SaveLoad>().LoadXML("text.xml");
        }
    }
}
