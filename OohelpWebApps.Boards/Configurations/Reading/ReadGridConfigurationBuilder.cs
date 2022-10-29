using OutOfHome.DataProviders.Boards.Grids.Common.Enums;
using OutOfHome.DataProviders.Boards.Grids.Properties.Reading;
using OutOfHome.DataProviders.Boards.Grids.Properties.Reading.Enums;

namespace OohelpWebApps.Boards.Configurations.Reading;
public class ReadGridConfigurationBuilder
{
    private readonly GridProvider provider;
    public ReadGridConfigurationBuilder(GridProvider provider)
    {
        this.provider = provider;
    }
    public ReadProperties Build() => provider switch
    {
        GridProvider.Outhub => GetOuthubReadProperties(),
        GridProvider.Octagon => GetOctagonReadProperties(),
        GridProvider.Bigmedia => GetBigmediaReadProperties(),
        _ => throw new NotImplementedException()
    };

    private OuthubReadProperties GetOuthubReadProperties() =>
        new OuthubReadProperties
        {
            Language = OutOfHome.DataProviders.Language.Ukrainian,
            RemoveInactiveRecords = true,
            FilteringMode = OuthubFilteringMode.Prime
        };
    private OctagonReadProperties GetOctagonReadProperties() => OctagonReadProperties.Default;
    private BigmediaReadProperties GetBigmediaReadProperties() =>
        new BigmediaReadProperties
        {
            Language = OutOfHome.DataProviders.Language.Ukrainian,
            RemoveInactiveRecords = true,
            FilteringMode = BmaFilteringMode.Bigmedia
        };

}
