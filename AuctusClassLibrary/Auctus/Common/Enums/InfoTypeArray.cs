namespace Auctus.DataMiner.Library.Common
{
    public enum InfoTypeArray
    {
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
        ///   GeneralInfoEventMessage
        /// </returns>
        LocalGeneralInfoMessage = 27,

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
        ///   LiteProtocolInfoEventMessage
        /// </returns>
        LiteProtocols = 0x40,
    }
}