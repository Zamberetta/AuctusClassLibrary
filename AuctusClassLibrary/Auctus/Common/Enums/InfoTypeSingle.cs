namespace Auctus.DataMiner.Library.Common
{
    public enum InfoTypeSingle
    {
        /// <returns>
        ///   ClientSecurityChangeEventMessage
        /// </returns>
        ClientSecurity = 2,

        /// <returns>
        ///   GetUserInfoResponseMessage
        /// </returns>
        SecurityInfo = 8,

        /// <returns>
        ///   GetScriptsResponseMessage
        /// </returns>
        Scripts = 12,

        /// <returns>
        ///   GetDataBaseInfoResponseMessage
        /// </returns>
        Database = 13,

        /// <returns>
        ///   GetElementTypesResponseMessage
        /// </returns>
        ElementTypes = 0xF,

        /// <returns>
        ///   GetLicensesResponseMessage
        /// </returns>
        Licenses = 0x10,

        /// <returns>
        ///   GetCorrelationRulesResponseMessage
        /// </returns>
        CorrelationRules = 17,

        /// <returns>
        ///   GetSchedulerTasksResponseMessage
        /// </returns>
        SchedulerTasks = 18,

        /// <returns>
        ///   GetDmsConfigurationResponseMessage
        /// </returns>
        DmsConfiguration = 20,

        /// <returns>
        ///   GetDataMinerInfoResponseMessage
        /// </returns>
        LocalDataMinerInfo = 21,

        /// <returns>
        ///   GeneralInfoEventMessage
        /// </returns>
        LocalGeneralInfoMessage = 27,

        /// <returns>
        ///   VersionInfoResponseMessage
        /// </returns>
        VersionInfo = 28,

        /// <returns>
        ///   GetDocumentsResponseMessage
        /// </returns>
        Documents = 29,

        /// <returns>
        ///   ScriptMemoryNamesResponseMessage
        /// </returns>
        ScriptMemoryNames = 30,

        /// <returns>
        ///   GetDocumentsResponseMessage
        /// </returns>
        DocumentFolders = 0x20,

        /// <returns>
        ///   TopologyInfoMessage
        /// </returns>
        Topology = 33,

        /// <returns>
        ///   TopologyMaxDepthInfo
        /// </returns>
        TopologyMaxDepth = 34,

        /// <returns>
        ///   ExtraMenuItemsMessage
        /// </returns>
        ExtraMenuItems = 36,

        /// <returns>
        ///   GetPropertyConfigurationResponse
        /// </returns>
        PropertyConfiguration = 37,

        /// <returns>
        ///   FailoverConfigMessage
        /// </returns>
        FailoverConfig = 38,

        /// <returns>
        ///   VdxFileChangeInfoResponse
        /// </returns>
        VdxFileChangeInfo = 42,

        /// <returns>
        ///   GetProtocolIconsResponse
        /// </returns>
        ProtocolIcons = 43,

        /// <returns>
        ///   GetAvailableAssetManagerConfigsResponse
        /// </returns>
        AssetManagersConfigs = 44,

        /// <returns>
        ///   GetAvailableMapConfigsResponse
        /// </returns>
        MapConfigs = 45,

        /// <returns>
        ///   LocalFailoverStatusResponse
        /// </returns>
        LocalFailoverStatus = 48,

        /// <returns>
        ///   GetActivatedLicenseCountersResponseMessage
        /// </returns>
        ActivatedLicenseCounters = 53,

        /// <returns>
        ///   BackupSettings
        /// </returns>
        BackupSettings = 60,

        /// <returns>
        ///   GetUpgradeDefaultOptionsResponseMessage
        /// </returns>
        DefaultUpgradeOptions = 62,

        /// <returns>
        ///   TopologyInfoMessage
        /// </returns>
        DCFTopology = 65,

        /// <returns>
        ///   ConfigurationDataEvent
        /// </returns>
        ConfigurationData = 67,

        /// <returns>
        ///   IndexingConfiguration
        /// </returns>
        IndexingConfiguration = 68,

        /// <returns>
        ///   GetIconsResponse
        /// </returns>
        Icons = 69,

        /// <returns>
        ///   GetProtocolFunctionsResponseMessage
        /// </returns>
        ProtocolFunctions = 70,
    }
}