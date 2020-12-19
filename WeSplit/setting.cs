﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WeSplit
{
    public class setting
    {
        public static void readSettingDB(string settingName, ref string value)
        {
            if (File.Exists("setting.xml"))
            {
                XDocument settingDocument = XDocument.Load("setting.xml");

                if (settingDocument.Element("setting").Elements(settingName).Any())
                {
                    value = settingDocument.Element("setting").Element(settingName).Value;
                }
                else
                {
                    //Lay gia tri mac dinh
                }
            }
            else
            {
                XDocument settingDocument =
                new XDocument(
                    new XElement("setting",
                      new XElement(settingName, value)
                    )
                  );
                settingDocument.Save("setting.xml");
            }
        }

        public static void SaveSetting(string settingName, string value)
        {
            XDocument setting = XDocument.Load("setting.xml");

            if (setting.Element("setting").Elements(settingName).Any())
            {
                setting.Element("setting").Element(settingName).Value = value;
                setting.Save("setting.xml");
                return;
            }
            else
            {
                setting.Element("setting").AddFirst(new XElement(settingName, value));
                setting.Save("setting.xml");
            }
        }
    }
}
