using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

public static class WordImageInserter
{
    public static async Task<MemoryStream> InsertImageAtBookmarkAsync(
        MemoryStream wordDocStream,
        string bookmarkName,
        Stream imageStream)
    {
        var outputDocStream = new MemoryStream();
        wordDocStream.Position = 0;
        await wordDocStream.CopyToAsync(outputDocStream);
        outputDocStream.Position = 0;

        using (var wordDoc = WordprocessingDocument.Open(outputDocStream, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart ?? throw new InvalidOperationException("The document does not contain a MainDocumentPart.");;
              
               // 1. Add image part
            var imagePart = mainPart.AddImagePart(ImagePartType.Png);
            imageStream.Position = 0;
            imagePart.FeedData(imageStream);
            var relationshipId = mainPart.GetIdOfPart(imagePart);

            // 2. Find the bookmark
            if (mainPart.Document == null || mainPart.Document.Body == null)
                throw new InvalidOperationException("The document or its body is missing.");

            BookmarkStart bookmarkStart = mainPart.Document.Body
                .Descendants<BookmarkStart>()
                .FirstOrDefault(b => b.Name == bookmarkName) ?? throw new ArgumentException($"Bookmark '{bookmarkName}' not found.");                

            // 3. Insert the image after the bookmark
            OpenXmlElement parent = bookmarkStart.Parent ?? throw new InvalidOperationException("Bookmark parent is null.");
            var imageDrawing = CreateImageDrawing(relationshipId);
            var run = new Run(imageDrawing);
            parent.InsertAfter(run, bookmarkStart);

            mainPart.Document.Save();
        }

        outputDocStream.Position = 0;
        return outputDocStream;
    }

    private static Drawing CreateImageDrawing(string relationshipId)
    {
        long widthEmus = 1000000L;  // ~1.1 inches
        long heightEmus = 300000L;  // ~0.33 inches

        return new Drawing(
            new DW.Inline(
                new DW.Extent() { Cx = widthEmus, Cy = heightEmus },
                new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
                new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Inserted Image" },
                new DW.NonVisualGraphicFrameDrawingProperties(
                    new A.GraphicFrameLocks() { NoChangeAspect = true }),
                new A.Graphic(
                    new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties()
                                {
                                    Id = 0U,
                                    Name = "signature.png"
                                },
                                new PIC.NonVisualPictureDrawingProperties()),
                            new PIC.BlipFill(
                                new A.Blip()
                                {
                                    Embed = relationshipId,
                                    CompressionState = A.BlipCompressionValues.Print
                                },
                                new A.Stretch(new A.FillRectangle())),
                            new PIC.ShapeProperties(
                                new A.Transform2D(
                                    new A.Offset() { X = 0L, Y = 0L },
                                    new A.Extents() { Cx = widthEmus, Cy = heightEmus }),
                                new A.PresetGeometry(new A.AdjustValueList())
                                {
                                    Preset = A.ShapeTypeValues.Rectangle
                                }))
                    ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }
                )
            )
            {
                DistanceFromTop = 0U,
                DistanceFromBottom = 0U,
                DistanceFromLeft = 0U,
                DistanceFromRight = 0U
            });
    }
}
