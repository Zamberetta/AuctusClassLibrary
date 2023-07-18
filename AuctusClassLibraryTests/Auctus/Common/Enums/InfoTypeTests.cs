// Ignore Spelling: Enums

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Net.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.DataMiner.Library.Common.Enum.Tests
{
    [TestClass]
    public class InfoTypeTests
    {
        [TestMethod]
        public void InfoTypeSingle_Invalid_Enums_Test()
        {
            var infoTypeSingleValues = System.Enum.GetValues(typeof(InfoTypeSingle));

            foreach (InfoTypeSingle infoTypeSingleValue in infoTypeSingleValues)
            {
                var parsedEnum = System.Enum.TryParse(infoTypeSingleValue.ToString(), out InfoType targetInfoType);

                parsedEnum.Should().BeTrue($"'{infoTypeSingleValue}' should exist");

                targetInfoType.Should().HaveValue((int)infoTypeSingleValue);
            }
        }

        [TestMethod]
        public void InfoTypeArray_Invalid_Enums_Test()
        {
            var infoTypeArrayValues = System.Enum.GetValues(typeof(InfoTypeArray));

            foreach (InfoTypeArray infoTypeArrayValue in infoTypeArrayValues)
            {
                var parsedEnum = System.Enum.TryParse(infoTypeArrayValue.ToString(), out InfoType targetInfoType);

                parsedEnum.Should().BeTrue($"'{infoTypeArrayValue}' should exist");

                targetInfoType.Should().HaveValue((int)infoTypeArrayValue);
            }
        }

        [TestMethod]
        public void InfoType_Duplicate_Enums_Test()
        {
            var infoTypeSingleValues = System.Enum.GetValues(typeof(InfoTypeSingle));

            var duplicateEnums = new List<string>();

            foreach (InfoTypeSingle infoTypeSingleValue in infoTypeSingleValues)
            {
                if (System.Enum.IsDefined(typeof(InfoTypeArray), infoTypeSingleValue.ToString()))
                {
                    duplicateEnums.Add(infoTypeSingleValue.ToString());
                }
            }

            duplicateEnums.Should().BeEmpty();
        }

        [TestMethod]
        public void InfoType_Missing_Enums_Test()
        {
            var infoTypeValues = System.Enum.GetValues(typeof(InfoType));

            var missingEnums = new List<string>();

            // Enums that have been excluded due to not being able to determine the expected response type.
            // To be re-evaluated.
            var excludedEnums = new List<string>() {
                "QueuedMessages",
                "RedundancyInfo",
                "ProtocolInformations",
                "SNMPManagers",
                "Plugins",
                "AllPlugins",
                "LocalRedundancyInfo",
                "VideoSources",
                "ProviderTheme",
                "SecureRegisteredClients",
                "SecureUnregisteredClients",
                "SecureClients",
                "AvailableDcpUpdates",
                "AggregatorElementInfo",
                "OEMInfo",
                "ElementConnections",
                "ProviderThemeDefinitions",
                "IndexingConfiguration",
            };

            foreach (InfoType infoTypeValue in infoTypeValues)
            {
                if (System.Enum.IsDefined(typeof(InfoTypeSingle), infoTypeValue.ToString()))
                {
                    continue;
                }

                if (System.Enum.IsDefined(typeof(InfoTypeArray), infoTypeValue.ToString()))
                {
                    continue;
                }

                missingEnums.Add(infoTypeValue.ToString());
            }

            missingEnums = missingEnums.Except(excludedEnums).ToList();
            missingEnums.Should().BeEmpty();
        }
    }
}