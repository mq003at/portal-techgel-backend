using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

public static class WordBookmarkReplacer
{
    public static void ReplacePlaceholders(Stream wordStream, Dictionary<string, string> placeholders)
    {
        if (wordStream == null || !wordStream.CanRead || !wordStream.CanSeek)
            throw new ArgumentException("Stream must be readable and seekable", nameof(wordStream));

        // Ensure the stream is at the beginning
        wordStream.Seek(0, SeekOrigin.Begin);

        using (var wordDoc = WordprocessingDocument.Open(wordStream, true))
        {
            var mainPart = wordDoc.MainDocumentPart;
            if (mainPart?.Document?.Body == null)
                throw new InvalidDataException("Invalid Word document structure");

            // Replace in body
            ReplaceBookmarksInElement(mainPart.Document.Body, placeholders);

            // Replace in headers
            foreach (var header in mainPart.HeaderParts)
            {
                ReplaceBookmarksInElement(header.Header, placeholders);
                header.Header.Save();
            }

            // Replace in footers
            foreach (var footer in mainPart.FooterParts)
            {
                ReplaceBookmarksInElement(footer.Footer, placeholders);
                footer.Footer.Save();
            }

            mainPart.Document.Save();
        }

        // Reset stream position if caller needs to read the result
        wordStream.Seek(0, SeekOrigin.Begin);
    }

    private static void ReplaceBookmarksInElement(OpenXmlElement root, Dictionary<string, string> placeholders)
    {
        var bookmarks = root.Descendants<BookmarkStart>();

        foreach (var bookmarkStart in bookmarks)
        {
            if (bookmarkStart.Name == null || !placeholders.ContainsKey(bookmarkStart.Name!))
                continue;

            var replacementText = placeholders[bookmarkStart.Name!];

            // Remove all elements between BookmarkStart and BookmarkEnd
            var current = bookmarkStart.NextSibling();

            while (current != null && !(current is BookmarkEnd && ((BookmarkEnd)current).Id == bookmarkStart.Id))
            {
                var next = current.NextSibling();
                current.Remove();
                current = next;
            }

            var run = new Run(
            new RunProperties(
                new RunFonts { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" },
                new FontSize { Val = "24" },
                new FontSizeComplexScript { Val = "24" }
            ),
            new Text(replacementText) { Space = SpaceProcessingModeValues.Preserve }
            );

            // Insert replacement text
            bookmarkStart.Parent?.InsertAfter(run, bookmarkStart);

        }
    }
}
