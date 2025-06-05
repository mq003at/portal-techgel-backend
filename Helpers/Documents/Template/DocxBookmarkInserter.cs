using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using portal.Documents.Props;
using System.Linq;

public class DocxBookmarkInserter
{
    public static MemoryStream InsertEmployeeData(LeaveRequestProps props)
    {
        // Read template into memory (must be available so that in normal or vps environment, the path is still correct)
        byte[] byteArray = File.ReadAllBytes(props.templatePath);
        var memoryStream = new MemoryStream();
        memoryStream.Write(byteArray, 0, byteArray.Length);
        memoryStream.Position = 0;

        using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
        {
            // Insert data into bookmarks
            InsertTextAtBookmark(wordDoc, "today", props.DraftDate.ToString("dd"));
            InsertTextAtBookmark(wordDoc, "tdMonth", props.DraftDate.ToString("MM"));
            InsertTextAtBookmark(wordDoc, "tdYear", props.DraftDate.ToString("yyyy"));
            InsertTextAtBookmark(wordDoc, "employeeName", props.EmployeeName);
            InsertTextAtBookmark(wordDoc, "position", " " + props.Position);
            InsertTextAtBookmark(wordDoc, "department", props.Department);
            InsertTextAtBookmark(wordDoc, "employmentStartDate", props.EmploymentStartDate.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "annualLeavePerYear", props.AnnualLeaveDaysPerYear.ToString());
            InsertTextAtBookmark(wordDoc, "finalEmployeeAnnualLeaveTotalDays", props.FinalEmployeeAnnualLeaveTotalDays.ToString());
            InsertTextAtBookmark(wordDoc, "employeeAnnualLeaveTotalDays", props.TotalDays.ToString());
            InsertTextAtBookmark(wordDoc, "leaveRequestStartHour", props.LeaveRequestStartHour.ToString("HH:mm"));
            InsertTextAtBookmark(wordDoc, "leaveRequestEndHour", props.LeaveRequestEndHour.ToString("HH:mm"));
            InsertTextAtBookmark(wordDoc, "leaveRequestStartDate", props.LeaveRequestStartHour.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "leaveRequestEndDate", props.LeaveRequestEndHour.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "employeeNameTop", props.EmployeeName);
            InsertTextAtBookmark(wordDoc, "birthPlace", props.BirthPlace);

            InsertTextAtBookmark(wordDoc, "address", props.AssigneeAddress);
            InsertTextAtBookmark(wordDoc, "idCardNumber", props.AssigneeIdCardNumber);
            InsertTextAtBookmark(wordDoc, "idCardIssuedLocation", props.AssigneeIdCardIssuedLocation);
            InsertTextAtBookmark(wordDoc, "idCardIssuedDate", props.AssigneeIdCardIssuedDate.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "hrName", props.HrName);
            InsertTextAtBookmark(wordDoc, "generalDirectorName", props.GeneralDirectorName);
            InsertTextAtBookmark(wordDoc, "idCardIssuedDate", props.AssigneeIdCardIssuedDate.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "phoneNumber", props.PhoneNumber);
            InsertTextAtBookmark(wordDoc, "reason", props.Reason);
            InsertTextAtBookmark(wordDoc, "supervisorName", props.SupervisorName);
            InsertTextAtBookmark(wordDoc, "supervisorPosition", props.SupervisorPosition);
            InsertTextAtBookmark(wordDoc, "workAssignedToDateOfBirth", props.WorkAssignedToDateOfBirth.ToString("dd/MM/yyyy"));
            InsertTextAtBookmark(wordDoc, "workAssignedToName", props.WorkAssignedToName);

            if (!string.IsNullOrEmpty(props.EmployeeSignature))
                InsertTextAtBookmark(wordDoc, "employeeSignature", props.EmployeeSignature);
            if (!string.IsNullOrEmpty(props.GeneralDirectorSignature))
                InsertTextAtBookmark(wordDoc, "generalDirectorSignature", props.GeneralDirectorSignature);
            if (!string.IsNullOrEmpty(props.HrSignature))
                InsertTextAtBookmark(wordDoc, "hrSignature", props.HrSignature);
            if (!string.IsNullOrEmpty(props.SupervisorSignature))
                InsertTextAtBookmark(wordDoc, "supervisorSignature", props.SupervisorSignature);

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