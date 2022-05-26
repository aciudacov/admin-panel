using OSS.IPTV.Ministra.Admin.Models.Package;
using OSS.IPTV.Ministra.Repository.Entities;

namespace OSS.IPTV.Ministra.Admin.Interfaces
{
    public interface IPackage
    {
        Task<PackageFindResponse> ListTvPackages(PackageFindRequest request);
        Task<List<PackageMin>> GetPackagesById(IEnumerable<int> ids);
        Task<int> AddTvPackage(PackageAddRequest request);
        Task EditTvPackage(PackageEditRequest request);
        Task DeleteTvPackage();
        Task<PackageLinkedChannels> GetLinkedChannels(int packageId);
        Task EditPackageChannels(PackageEditChannelsRequest request);
        Task<List<TvPackageType>> GetPackageTypes();
    }
}
