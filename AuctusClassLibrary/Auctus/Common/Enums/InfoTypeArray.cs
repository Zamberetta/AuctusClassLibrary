namespace Auctus.DataMiner.Library.Common
{
    /// <summary>
    /// Enum used in conjunction with the GetInfo SLNet call.
    /// </summary>
    public enum InfoTypeArray
    {
        /// <returns>
        ///   GetDataMinerInfoResponseMessage
        /// </returns>
        DataMinerInfo = 0,

        /// <returns>
        ///   ElementInfoEventMessage
        /// </returns>
        ElementInfo = 3,

        /// <returns>
        ///   ServiceInfoEventMessage
        /// </returns>
        ServiceInfo = 4,

        /// <returns>
        ///   ViewInfoEventMessage
        /// </returns>
        ViewInfo = 6,

        /// <returns>
        ///   GeneralInfoEventMessage
        /// </returns>
        GeneralInfoMessage = 7,

        /// <returns>
        ///   GetTrendingTemplatesResponseMessage
        /// </returns>
        TrendingTemplates = 9,

        /// <returns>
        ///   GetProtocolInformationsResponseMessage
        /// </returns>
        ProtocolInformations = 10,

        /// <returns>
        ///   GetProtocolsResponseMessage
        /// </returns>
        Protocols = 11,

        /// <returns>
        ///   DataMinerStateEventMessage
        /// </returns>
        DataMinerStates = 23,

        /// <returns>
        ///   ElementInfoEventMessage
        /// </returns>
        LocalElementInfo = 24,

        /// <returns>
        ///   ServiceInfoEventMessage
        /// </returns>
        LocalServiceInfo = 25,

        /// <returns>
        ///   LoginInfoResponseMessage
        /// </returns>
        ClientList = 0x1F,

        /// <returns>
        ///   LoginInfoResponseMessage
        /// </returns>
        LocalClientList = 35,

        /// <returns>
        ///   VideoServerType
        /// </returns>
        VideoServerTypes = 39,

        /// <returns>
        ///   ServiceInfoEventMessage
        /// </returns>
        ServiceTemplates = 41,

        /// <returns>
        ///   ServiceElementInfoEventMessage
        /// </returns>
        ServiceElementInfo = 47,

        /// <returns>
        ///   InterfaceInfoEventMessage
        /// </returns>
        LocalInterfaceInfo = 49,

        /// <returns>
        ///   InterfaceConnectionInfoEventMessage
        /// </returns>
        LocalInterfaceConnectionInfo = 54,

        /// <returns>
        ///   InterfaceInfoEventMessage
        /// </returns>
        InterfaceInfo = 55,

        /// <returns>
        ///   InterfaceConnectionInfoEventMessage
        /// </returns>
        InterfaceConnectionInfo = 56,

        /// <returns>
        ///   DcpUpdateBlobEvent
        /// </returns>
        AvailableDcpUpdates = 57,

        /// <returns>
        ///   AvailableUpgradePackageEvent
        /// </returns>
        AvailableUpgradePackages = 61,

        /// <returns>
        ///   ElementConnectionInfoEventMessage
        /// </returns>
        ElementConnections = 63,

        /// <returns>
        ///   LiteProtocolInfoEventMessage
        /// </returns>
        LiteProtocols = 0x40,
    }
}