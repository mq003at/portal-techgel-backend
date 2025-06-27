using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

public static class WordBookmarkReplacer
{
    public static void ReplacePlaceholders(Stream wordStream, Dictionary<string, (string Text, bool IsBold)> placeholders)
    {
        if (wordStream == null || !wordStream.CanRead || !wordStream.CanSeek)
            throw new ArgumentException("Stream must be readable and seekable", nameof(wordStream));

        wordStream.Seek(0, SeekOrigin.Begin);

        using (var wordDoc = WordprocessingDocument.Open(wordStream, true))
        {
            var mainPart = wordDoc.MainDocumentPart;
            if (mainPart?.Document?.Body == null)
                throw new InvalidDataException("Invalid Word document structure");

            ReplaceBookmarksInElement(mainPart.Document.Body, placeholders);

            foreach (var header in mainPart.HeaderParts)
            {
                ReplaceBookmarksInElement(header.Header, placeholders);
                header.Header.Save();
            }

            foreach (var footer in mainPart.FooterParts)
            {
                ReplaceBookmarksInElement(footer.Footer, placeholders);
                footer.Footer.Save();
            }

            mainPart.Document.Save();
        }

        wordStream.Seek(0, SeekOrigin.Begin);
    }

    private static void ReplaceBookmarksInElement(OpenXmlElement root, Dictionary<string, (string Text, bool IsBold)> placeholders)
    {
        var bookmarks = root.Descendants<BookmarkStart>();

        foreach (var bookmarkStart in bookmarks)
        {
            if (bookmarkStart.Name == null || !placeholders.ContainsKey(bookmarkStart.Name!))
                continue;

            var (text, isBold) = placeholders[bookmarkStart.Name!];

            // Remove all elements between BookmarkStart and BookmarkEnd
            var current = bookmarkStart.NextSibling();

            while (current != null && !(current is BookmarkEnd be && be.Id == bookmarkStart.Id))
            {
                var next = current.NextSibling();
                current.Remove();
                current = next;
            }

            var runProps = new RunProperties(
                new RunFonts { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                new FontSize { Val = "24" },
                new FontSizeComplexScript { Val = "24" }
            );

            if (isBold)
                runProps.Append(new Bold());

            var run = new Run(runProps, new Text(text) { Space = SpaceProcessingModeValues.Preserve });

            bookmarkStart.Parent?.InsertAfter(run, bookmarkStart);
        }
    }
}
