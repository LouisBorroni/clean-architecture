using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TierListes.Application.Common.Interfaces.Services;

namespace TierListes.Infrastructure.Services;

public class PdfGeneratorService : IPdfGeneratorService
{
    static PdfGeneratorService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GeneratePdfFromImage(byte[] imageData)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);
                page.PageColor(Colors.White);

                page.Header()
                    .Text("Ma Tier List")
                    .FontSize(24)
                    .Bold()
                    .FontColor(Colors.Grey.Darken3)
                    .AlignCenter();

                page.Content()
                    .PaddingVertical(10)
                    .AlignCenter()
                    .AlignMiddle()
                    .Image(imageData)
                    .FitArea();

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Généré le ");
                        text.Span(DateTime.Now.ToString("dd/MM/yyyy à HH:mm"));
                    });
            });
        });

        return document.GeneratePdf();
    }
}
