using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using System.Text;

public static class LeaveRequestTemplateHelper
{
    public static byte[] LeaveRequestTemplate()
    {
        using (var ms = new MemoryStream())
        {
            using (var doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());
                var stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();
                stylePart.Styles = new Styles();
                stylePart.Styles.Append(
                    new DocDefaults(
                        new RunPropertiesDefault(
                            new RunPropertiesDefault(
                                new RunPropertiesBaseStyle(
                                    new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" },
                                    new FontSize { Val = "24" }
                                )
                            )
                        )
                    )
                );
                stylePart.Styles.Append(
                    new Style(
                        new StyleName { Val = "Normal" },
                        new BasedOn { Val = "Normal" },
                        new UIPriority { Val = 1 },
                        new PrimaryStyle(),
                        new StyleRunProperties(
                            new RunFonts { Ascii = "Times New Roman", HighAnsi = "Times New Roman", EastAsia = "Times New Roman", ComplexScript = "Times New Roman" },
                            new FontSize { Val = "24" }
                        )
                    )
                    { Type = StyleValues.Paragraph, StyleId = "Normal" }
                );
                Body body = mainPart.Document.Body ?? (mainPart.Document.Body = new Body());

                // 1. TP.HCM, ngày ... (right aligned, editable)
                var dateParagraph = new Paragraph(
                    CreateTabStopProperties(9360),
                    new Run(
                        new TabChar(),
                        new Text("TP.HCM, ngày      tháng     năm      ") { Space = SpaceProcessingModeValues.Preserve }
                    )
                );
                body.Append(dateParagraph);

                // 2. Title (center, bold, big)
                var titleParagraph = new Paragraph(
                    new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                    new Run(
                        new RunProperties(
                            new Bold(),
                            new FontSize() { Val = "36" } // 18pt
                        ),
                        new Text("ĐƠN XIN NGHỈ PHÉP")
                    )
                );
                body.Append(titleParagraph);

                // 3. Kính gửi: ... (indented)
                var kinhGuiParagraph = new Paragraph(
                    new Run(new Text("Kính gửi:  ") { Space = SpaceProcessingModeValues.Preserve }),
                    new Run(new Text("TỔNG GIÁM ĐỐC")),
                    new Break(),
                    new Run(new TabChar()),
                    new Run(new Text("PHÒNG HÀNH CHÍNH – NHÂN SỰ"))
                );
                kinhGuiParagraph.ParagraphProperties = new ParagraphProperties(
                    new Indentation() { Left = "720" } // 0.5 inch
                );
                body.Append(kinhGuiParagraph);

                // 4. Empty line
                body.Append(new Paragraph(new Run(new Text(""))));

                // 5. Ý KIẾN PHÊ DUYỆT... (section header)
                var approvalHeader = new Paragraph(
                    new ParagraphProperties(new Justification() { Val = JustificationValues.Center }),
                    new Run(
                        new Bold(),
                        new Text("Ý KIẾN PHÊ DUYỆT CỦA TỔNG GIÁM ĐỐC")
                    )
                );
                body.Append(approvalHeader);

                // 6. Empty line
                body.Append(new Paragraph(new Run(new Text(""))));

                // 7. Form fields and data
                body.Append(MakeTextParagraph("Tôi tên: SƠN LÀNH"));
                body.Append(MakeChucVuPhongBanParagraph("Bảo vệ", "Hành chính – Nhân sự"));
                body.Append(MakeTextParagraph("Ngày vào Công ty\t: 01/03/2024"));
                body.Append(MakeTextParagraph("Thời gian xin nghỉ phép\t: 04 ngày từ 06h00 ngày 29/01/2025 đến 06h00 ngày 01/02/2025"));
                body.Append(MakeTextParagraph("Lý do nghỉ phép\t: Nghỉ Tết"));
                body.Append(MakeTextParagraph("Trong thời gian nghỉ phép, tôi sẽ bàn giao lại công việc cho:"));

                body.Append(MakeTextParagraph("-Họ và tên\t: TRẦN NGỌC ẨN"));
                body.Append(MakeTextParagraph("-Ngày sinh\t: 10/10/1958\tQuê quán: Bình Định"));
                body.Append(MakeTextParagraph("-Địa chỉ thường trú\t: 234324"));
                body.Append(MakeTextParagraph("-Số CCCD\t: 079058008985\tNgày cấp: 21/12/2021"));
                body.Append(MakeTextParagraph("Nơi cấp\t: Cục trưởng Cục CS QLHC về TTXH"));
                body.Append(MakeTextParagraph("-Số điện thoại\t: 0377 372 909"));
                body.Append(MakeTextParagraph("Thơi gian nghỉ phép còn lại\t: ………. Ngày"));

                body.Append(new Paragraph(new Run(new Text("Kính đề nghị Tổng giám đốc xem xét giải quyết."))));
                body.Append(new Paragraph(new Run(new Text("Trân trọng cảm ơn!"))));

                mainPart.Document.Save();
            }
            return ms.ToArray();
        }
    }

    // Helper for tab stops in a paragraph
    private static ParagraphProperties CreateTabStopProperties(int positionTwips)
    {
        return new ParagraphProperties(
            new Tabs(new TabStop() { Val = TabStopValues.Right, Position = positionTwips })
        );
    }

    // Helper for simple text paragraphs
    private static Paragraph MakeTextParagraph(string text)
    {
        // Use tab char for alignment
        var runs = text.Split('\t').Select((t, i) =>
            i == 0 ? new Run(new Text(t) { Space = SpaceProcessingModeValues.Preserve }) : new Run(new TabChar(), new Text(t) { Space = SpaceProcessingModeValues.Preserve })
        ).ToArray();
        return new Paragraph(runs);
    }

    // Helper for Chức vụ (left) and Phòng/Ban (right)
    private static Paragraph MakeChucVuPhongBanParagraph(string chucVu, string phongBan)
    {
        // Tab stop at 5000 twips (~3.5in), adjust as needed
        return new Paragraph(
            new ParagraphProperties(new Tabs(new TabStop() { Val = TabStopValues.Right, Position = 5000 })),
            new Run(new Text($"Chức vụ: {chucVu}") { Space = SpaceProcessingModeValues.Preserve }),
            new Run(new TabChar()),
            new Run(new Text($"Phòng/Ban: {phongBan}") { Space = SpaceProcessingModeValues.Preserve })
        );
    }
}