using OohelpWebApps.Boards.Configurations.Exporting;
using OutOfHome.DataProviders.Boards.Grids.Common;
using OutOfHome.Excel.Common;
using OutOfHome.Excel.Common.Enums;

namespace GridsDownloader.Mapping;
internal static class GridBoardToExcelBoard
{
    public static ExcelBoard ToExcelBoard(this GridBoard gridBoard, ExportToExceSheetSchema schema)
    {
        ExcelBoard excelBoard = new ExcelBoard
        {
            Provider = gridBoard.Provider,
            ProviderID = gridBoard.ProviderID,

            Supplier = gridBoard.Supplier,
            Code = gridBoard.SupplierCode,

            Region = gridBoard.Address.City.Region,
            City = gridBoard.Address.City.Name,
            Street = gridBoard.Address.Street,
            StreetHouse = gridBoard.Address.StreetNumber,
            AddressDescription = gridBoard.Address.Description,

            Photo = gridBoard.Photo,
            Map = gridBoard.Map,

            Latitude = gridBoard.Location.Latitude,
            Longitude = gridBoard.Location.Longitude,
            Angle = gridBoard.Angle,

            Side = gridBoard.Side,
            Size = gridBoard.Size,
            Type = gridBoard.Type,
            Lighting = gridBoard.Lighting,

            DoorsDix = gridBoard.DoorsInfo?.DoorsID,
            OTS = gridBoard.DoorsInfo?.OTS,
            GRP = gridBoard.DoorsInfo?.GRP,
            PhotoDoors = gridBoard.DoorsInfo?.Photo,
            MapDoors = gridBoard.DoorsInfo?.Map,

            Color = System.Drawing.Color.Transparent,
            
        };

        SuppliersInfo si = new SuppliersInfo();

        si.OccupationString = gridBoard.Occupation.OriginOccupationString;
        si.Price = gridBoard.Price.Value;

        HashSet<OutOfHome.Excel.Common.Enums.SuppliersInfoProperty> dynamicProperties = schema.DynamicColumnsProperties.ToHashSet();

        si.InitializeDetailedFields(schema.VisiblePeriods.Count * dynamicProperties.Count);

        if (dynamicProperties.Count > 0)
        {
            foreach (var period in schema.VisiblePeriods)
            {
                OutOfHome.DataProviders.Boards.Grids.Common.OccupationStatus occupationStatus;
                if (dynamicProperties.Contains(SuppliersInfoProperty.Occupation) || dynamicProperties.Contains(SuppliersInfoProperty.Additional))
                {
                    occupationStatus = gridBoard.Occupation.GetStatus(period.Start, period.End);
                }
                else
                {
                    occupationStatus = null;
                }
                SuppliersInfoDetail detail = new SuppliersInfoDetail();
                foreach (var property in schema.DynamicColumnsProperties)
                {
                    switch (property)
                    {
                        case OutOfHome.Excel.Common.Enums.SuppliersInfoProperty.Occupation:
                            detail.Status = occupationStatus.Condition.ToExcelSatus();
                            detail.Value = occupationStatus.Value;
                            break;
                        
                        case OutOfHome.Excel.Common.Enums.SuppliersInfoProperty.Price:
                            detail.Price = gridBoard.Price.GetValue(period.Start, period.End);
                            break;

                        case OutOfHome.Excel.Common.Enums.SuppliersInfoProperty.Additional:
                            detail.Additional = occupationStatus.Description;
                            break;

                        default:
                            break;
                    }
                }
                si.AddDetailedField(detail, period);
            }
        }
        excelBoard.SuppliersInfo = si;
        return excelBoard;
    }

    private static OutOfHome.Excel.Common.Enums.OccupationStatus ToExcelSatus(this OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition condition) => condition switch
    {
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.Free => OutOfHome.Excel.Common.Enums.OccupationStatus.Free,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.Reserved => OutOfHome.Excel.Common.Enums.OccupationStatus.Reserved,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.Booked => OutOfHome.Excel.Common.Enums.OccupationStatus.Booked,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.OutOfRange => OutOfHome.Excel.Common.Enums.OccupationStatus.OutOfRange,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.Unavailable => OutOfHome.Excel.Common.Enums.OccupationStatus.Unavailable,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.Unrecognized => OutOfHome.Excel.Common.Enums.OccupationStatus.Unrecognized,
        OutOfHome.DataProviders.Boards.Grids.Common.Enums.OccupationCondition.None => OutOfHome.Excel.Common.Enums.OccupationStatus.None,
        _ => throw new NotImplementedException(nameof(condition))
    };
}
