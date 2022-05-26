namespace OSS.IPTV.Ministra.Admin.Enums
{
    public class LogEnums
    {
        public enum ChangeAction
        {
            Insert = 1,
            Update = 2,
            Delete = 3
        }

        public enum ChangeType
        {
            CreateChannel = 1,
            CreatePackage = 2,
            UpdateChannel = 3,
            UpdatePackage = 4,
            AddChannelToPackages = 5,
            RemoveChannelFromPackages = 6,
            AddChannelsToPackage = 7,
            RemoveChannelsFromPackage = 8
        }

        public enum ItemType
        {
            Channel = 1,
            Package = 2,
            PackageChannels = 3
        }
    }
}
