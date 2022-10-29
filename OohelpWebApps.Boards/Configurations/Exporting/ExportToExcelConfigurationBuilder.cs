using OutOfHome.DataProviders.Boards.Grids.Common;
using OutOfHome.Excel.Common;
using OutOfHome.Excel.Common.Enums;
using OutOfHome.Excel.DocumentModel.Fields;
using OutOfHome.Excel.DocumentModel.Interfaces;

namespace OohelpWebApps.Boards.Configurations.Exporting;
internal class ExportToExcelConfigurationBuilder
{    
    public static ExportToExceSheetSchema Build(IEnumerable<GridBoard> boards)
    {
        DateOnly start = boards.Min(a => a.Occupation.VisiblePeriod.Start);
        DateOnly end = boards.Max(a => a.Occupation.VisiblePeriod.End);

        var periods = ExcelDatesOnlyPeriod.SplitPeriodByOneMonth(start, end);
        var staticColumns = BoardExcelField.GetDefaultExportProperties();
        var dynamicColumns = new SuppliersInfoProperty[] { SuppliersInfoProperty.Occupation };       

        int multiplier = dynamicColumns.Length;

        List<IExcelField> columns = new List<IExcelField>(periods.Count * dynamicColumns.Length + staticColumns.Length);
        foreach (var c in staticColumns)
        {
            columns.Add(new BoardExcelField(c));
        }

        if (multiplier > 0)
        {
            foreach (var period in periods)
            {
                foreach (var property in dynamicColumns)
                {
                    columns.Add(new SuppliersInfoField(property, period));
                }
            }
        }


        return new ExportToExceSheetSchema
        {
            TableColumns = columns,
            VisiblePeriods = periods,
            DynamicColumnsProperties = dynamicColumns ?? Array.Empty<SuppliersInfoProperty>(),
        };
    }

}
