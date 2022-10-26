using OutOfHome.DataProviders.Boards.Grids.Common.Enums;
using OutOfHome.DataProviders.Boards.Grids.Properties.Downloading;
using OutOfHome.DataProviders.Boards.Grids.Properties.Downloading.Common;

namespace OohelpWebApps.Boards.Configurations.Downloading;
public class DownloadConfigurationBuilder
{
    private readonly IConfiguration configuration;

    public DownloadConfigurationBuilder(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public DownloadProperties Build(GridProvider provider) => provider switch
    {
        GridProvider.Outhub => GetOuthubDownloadProperties(),
        GridProvider.Octagon => GetOctagonDownloadProperties(),
        GridProvider.Bigmedia => GetBigmediaDownloadProperties(),
        _ => throw new NotImplementedException()
    };

    private OuthubDownloadProperties GetOuthubDownloadProperties() => new OuthubDownloadProperties();
    //{
    //    var config = AppSettings.Instance.DownloadConfigurationOuthub
    //        ?? DownloadConfigurationOuthub.Default;

    //    return new OuthubDownloadProperties
    //    {
    //        RemoveInactiveRecords = config.RemoveInactiveRecords,
    //        RemoveNonHubRecords = config.RemoveNonHubRecords,
    //    };
    //}
    private OctagonDownloadProperties GetOctagonDownloadProperties()
    {
        var defaultStart = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1);
        var defaultEnd = new DateOnly(DateTime.Now.Year, 12, 31);

        return new OctagonDownloadProperties
        {
            Cities = OctagonDownloadProperties.GetAvailableCities().ToList(),

            PeriodStart = defaultStart,
            PeriodEnd = defaultEnd
        };
    }
    private BigmediaDownloadProperties GetBigmediaDownloadProperties()
    {
        ProviderCredential credential = this.configuration.GetSection("BigmediaCredentials").Get<ProviderCredential>();

        return new BigmediaDownloadProperties
        {
            Credential = credential,
            Language = OutOfHome.DataProviders.Language.Ukrainian
        };
    }
}
