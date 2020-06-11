using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace OnvifServicesTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            Message m = Message.CreateMessage(XmlReader.Create(GetTestDataFolder("GetServices/HanwhaTechwin_PNM-9020V.xml")), int.MaxValue, MessageVersion.Soap12WSAddressing10);
            SoapReflectionImporter importer = new SoapReflectionImporter(new SoapAttributeOverrides(), "http://www.onvif.org/ver10/device/wsdl");
            XmlTypeMapping mapp = importer.ImportTypeMapping(typeof(Onvif.GetServicesResponse));
            XmlSerializer xmlSerializer = new XmlSerializer(mapp); //typeof(T), xr
            var o = (Onvif.GetServicesResponse)xmlSerializer.Deserialize(m.GetReaderAtBodyContents());
        }

        public static string GetTestDataFolder(string testDataFolder)
        {
            string startupPath = ApplicationEnvironment.ApplicationBasePath;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, "TestsData", testDataFolder);
        }
    }
}
