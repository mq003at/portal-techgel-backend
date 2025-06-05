using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Drawing;
using System.IO;
using NonVisualGraphicFrameDrawingProperties = DocumentFormat.OpenXml.Drawing.NonVisualGraphicFrameDrawingProperties;
using Picture = DocumentFormat.OpenXml.Drawing.Picture;

// .2 inch margin = 288 twips (1 inch = 1440 twips)
public class WordHeaderLogoHelper
{
    private const int LOGO_MARGIN_TWIPS = 288; // 0.2 inch
    private const int DEFAULT_LOGO_WIDTH_EMUS = 3000000; // ~3.3 inch
    private const int DEFAULT_LOGO_HEIGHT_EMUS = 800000; // ~0.88 inch

    public string LogoPath { get; set; }
    public int LogoWidthEmus { get; set; } = DEFAULT_LOGO_WIDTH_EMUS;
    public int LogoHeightEmus { get; set; } = DEFAULT_LOGO_HEIGHT_EMUS;

    public WordHeaderLogoHelper(string logoPath)
    {
        LogoPath = logoPath ?? throw new ArgumentNullException(nameof(logoPath));
    }

    public void InsertHeaderLogo(WordprocessingDocument doc)
    {
        var mainPart = doc.MainDocumentPart ?? throw new ArgumentException("Missing MainDocumentPart");

        var imagePart = mainPart.AddImagePart(ImagePartType.Png);
        using (var stream = new FileStream(LogoPath, FileMode.Open, FileAccess.Read))
        {
            imagePart.FeedData(stream);
        }
        var relId = mainPart.GetIdOfPart(imagePart);

        // Build Drawing for the image
        var drawing = new Drawing(
            new Inline(
                new Extent() { Cx = LogoWidthEmus, Cy = LogoHeightEmus },
                new EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                new DocProperties() { Id = (UInt32Value)1U, Name = "Logo" },
                new NonVisualGraphicFrameDrawingProperties(new GraphicFrameLocks() { NoChangeAspect = true }),
                new Graphic(
                    new GraphicData(
                        new Picture(
                            new DocumentFormat.OpenXml.Drawing.NonVisualPictureProperties(
                                new DocumentFormat.OpenXml.Drawing.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = System.IO.Path.GetFileName(LogoPath) },
                                new DocumentFormat.OpenXml.Drawing.NonVisualPictureDrawingProperties()),
                            new DocumentFormat.OpenXml.Drawing.BlipFill(
                                new Blip() { Embed = relId },
                                new Stretch(new FillRectangle())),
                            new DocumentFormat.OpenXml.Drawing.ShapeProperties(
                                new Transform2D(
                                    new Offset() { X = 0L, Y = 0L },
                                    new Extents() { Cx = LogoWidthEmus, Cy = LogoHeightEmus }),
                                new PresetGeometry(new AdjustValueList()) { Preset = ShapeTypeValues.Rectangle }))
                    )
                    { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
            )
            {
                DistanceFromTop = (UInt32Value)0U,
                DistanceFromBottom = (UInt32Value)0U,
                DistanceFromLeft = (UInt32Value)0U,
                DistanceFromRight = (UInt32Value)0U,
            });

        // Paragraph properties for custom margins
        var paraProps = new DocumentFormat.OpenXml.Drawing.ParagraphProperties(
            new Justification() { Val = JustificationValues.Center },
            new Indentation() { Left = LOGO_MARGIN_TWIPS.ToString(), Right = LOGO_MARGIN_TWIPS.ToString() }
        );

        var logoPara = new DocumentFormat.OpenXml.Drawing.Paragraph(paraProps, new DocumentFormat.OpenXml.Drawing.Run(drawing));

        var body = mainPart.Document.Body;
        // Insert at the very top
        if (body.HasChildren)
            body.InsertAt(logoPara, 0);
        else
            body.Append(logoPara);
    }
}