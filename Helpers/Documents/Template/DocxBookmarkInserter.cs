using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using portal.Documents.Props;
using System.Linq;

public class DocxBookmarkInserter
{
    public static MemoryStream InsertEmployeeData(LeaveRequestProps props)
    {
        // Read template into memory (must be available so that in normal or vps environment, the path is still correct)
        byte[] byteArray = File.ReadAllBytes(props.TemplatePath);
        var memoryStream = new MemoryStream();
        memoryStream.Write(byteArray, 0, byteArray.Length);
        memoryStream.Position = 0;

        using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
        {
            // Insert data into bookmarks
            InsertTextAtBookmark(wordDoc, "employeeNameTop", props.EmployeeName);
            InsertTextAtBookmark(wordDoc, "leaveRequestStartDate", props.LeaveRequestStartHour.ToString("HH:mm dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "department", props.Department);
            InsertTextAtBookmark(wordDoc, "leaveRequestEndDate", props.LeaveRequestEndHour.ToString("HH:mm dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "position", " " + props.Position);
            InsertTextAtBookmark(wordDoc, "reason", props.Reason);
            InsertTextAtBookmark(wordDoc, "leaveApprovalCategory", props.LeaveApprovalCategory);
            InsertTextAtBookmark(wordDoc, "totalDaysTop", props.TotalDays.ToString() + " ngày");

            InsertTextAtBookmark(wordDoc, "assigneeName", props.AssigneeName);
            InsertTextAtBookmark(wordDoc, "assigneePersonalNumber", props.AssigneePhoneNumber);
            InsertTextAtBookmark(wordDoc, "assigneeEmail", props.AssigneeEmail);
            InsertTextAtBookmark(wordDoc, "assigneeAddress", props.AssigneeAddress);
            InsertTextAtBookmark(wordDoc, "employeeAnnualLeaveTotalDays", props.EmployeeAnnualLeaveTotalDays.ToString() + " ngày");
            InsertTextAtBookmark(wordDoc, "finalEmployeeAnnualLeaveTotalDays", props.FinalEmployeeAnnualLeaveTotalDays.ToString() + " ngày");
            InsertTextAtBookmark(wordDoc, "totalDays", props.TotalDays.ToString() + " ngày");

            InsertTextAtBookmark(wordDoc, "employeeSignDate", props.EmployeeSignDate?.ToString() ?? string.Empty);
            InsertTextAtBookmark(wordDoc, "supervisorSignDate", props.SupervisorSignDate?.ToString() ?? string.Empty);
            InsertTextAtBookmark(wordDoc, "hrSignDate", props.HrSignDate?.ToString() ?? string.Empty);
            InsertTextAtBookmark(wordDoc, "generalDirectorSignDate", props.GeneralDirectorSignDate?.ToString() ?? string.Empty);

            if (!string.IsNullOrEmpty(props.EmployeeSignature))
                InsertTextAtBookmark(wordDoc, "employeeSignature", props.EmployeeSignature);
            if (!string.IsNullOrEmpty(props.GeneralDirectorSignature))
                InsertTextAtBookmark(wordDoc, "generalDirectorSignature", props.GeneralDirectorSignature);
            if (!string.IsNullOrEmpty(props.HrSignature))
                InsertTextAtBookmark(wordDoc, "hrSignature", props.HrSignature);
            if (!string.IsNullOrEmpty(props.SupervisorSignature))
                InsertTextAtBookmark(wordDoc, "supervisorSignature", props.SupervisorSignature);

            InsertTextAtBookmark(wordDoc, "employeeName", props.EmployeeName);
            InsertTextAtBookmark(wordDoc, "hrName", props.HrName);
            InsertTextAtBookmark(wordDoc, "supervisorName", props.SupervisorName);
            InsertTextAtBookmark(wordDoc, "generalDirectorName", props.GeneralDirectorName);

            // Save changes to the document
            wordDoc.MainDocumentPart.Document.Save();
        }

        memoryStream.Position = 0; // Reset stream position for download
        return memoryStream;
    }

    private static void InsertTextAtBookmark(
    WordprocessingDocument wordDoc,
    string bookmarkName,
    string text)
    {
        var bookmark = wordDoc.MainDocumentPart.Document.Body
            .Descendants<BookmarkStart>()
            .FirstOrDefault(b => b.Name == bookmarkName);

        if (bookmark != null)
        {
            // Remove existing text between bookmark if exists (optional)
            var currentElement = bookmark.NextSibling();
            while (currentElement != null && !(currentElement is BookmarkEnd be && be.Id == bookmark.Id))
            {
                var next = currentElement.NextSibling();
                currentElement.Remove();
                currentElement = next;
            }

            // Set font to Arial and size to 12pt (24 half-points)
            var runProps = new RunProperties(
                new RunFonts { Ascii = "Arial", HighAnsi = "Arial" },
                new FontSize { Val = "24" }  // 12pt = 24 half-points
            );

            var run = new Run(runProps, new Text(text));
            bookmark.Parent.InsertAfter(run, bookmark);
        }
    }

}