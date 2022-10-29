using OutOfHome.Excel.Common;
using OutOfHome.Excel.Common.Enums;
using OutOfHome.Excel.DocumentModel;

namespace OohelpWebApps.Boards.Configurations.Exporting;
internal class ExportToExceSheetSchema : SheetSchema
{
    public List<ExcelDatesOnlyPeriod> VisiblePeriods { get; set; }
    public SuppliersInfoProperty[] DynamicColumnsProperties { get; internal set; }
}
