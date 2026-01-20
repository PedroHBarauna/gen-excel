namespace GenExcel.Application.Exports.Reports;

public sealed record EventSpreadsheetReportRequest(
    string? ReportTitle,
    List<EventReportItem> Reports
);

public sealed record EventReportItem(
    int EventId,
    List<TicketEventColumn> Columns,
    List<string>? Headers,
    bool IncludeTotal = true
);

