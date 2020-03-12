using GemeindeJubiläen.Models;
using System;
using System.Collections.Generic;
using System.IO;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.Layout.Borders;
using GemeindeJubiläen.Views;
using System.Linq;
using iText.Kernel.Colors;

namespace GemeindeJubilaen.Services
{
    class PDFExport
    {
        //  public static readonly string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
        private PdfFont normalFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

        //     public string Static ExportQuartherAsPDF(
        public FileInfo ExportAsPDF(List<Item> alleGemeindeglieder,List<Monate> monate,Boolean exportGeb = true, Boolean exportTauf = true)
        {
            String path = Path.ChangeExtension(Path.GetTempFileName(), "pdf");
            FileInfo file = new FileInfo(path);
            file.Directory.Create();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(file));
            Document doc = new Document(pdfDoc);
            Table header = new Table((exportTauf && exportGeb)? 2:1).UseAllAvailableWidth();
            for(int i = 0; i < 2; i++)
            {
                if (i == 0 && exportGeb || i == 1 & exportTauf)
                {
                    Paragraph p = new Paragraph();
                    p = new Paragraph();
                    p.SetFont(boldFont);
                    p.SetTextAlignment(TextAlignment.CENTER);
                    p.SetFontSize(30f);
                    p.SetUnderline();
                    p.Add((i == 0) ? "Geburtstage" : "Taufjubiläen");
                    header.AddCell(new Cell().Add(p).SetBorder(Border.NO_BORDER));
                }
            }
            doc.Add(header);
            foreach (var monat in monate)
            {
                var test = GetMonthTable(alleGemeindeglieder, monat, exportGeb, exportTauf);
                doc.Add(test);
                doc.Add(new Paragraph().SetFontSize(15f));
            }
            doc.Close();
            return file;
        }

        public Table GetMonthTable(List<Item> alleGemeindeglieder, Monate monat, bool exportGeb, bool exportTauf)
        {
            var gebJub = alleGemeindeglieder.FindAll(x => x.Geburtsdatum.Length == 10 && x.Geb.Month == (int)monat + 1).OrderBy(x => x.Geb.Day).ToList();
            var taufJub = alleGemeindeglieder.FindAll(x => x.Taufdatum.Length == 10 && x.Tauf.Month == (int)monat + 1).OrderBy(x => x.Tauf.Day).ToList();
            if (!exportGeb) gebJub = null;
            if (!exportTauf) taufJub = null;
            return GetMonthTable(gebJub, taufJub, monat);
        }

        public FileInfo ExportQuartalAsPDF(List<Item> alleGemeindeglieder,int quartal)
        {
            return ExportAsPDF(alleGemeindeglieder,new List<Monate>() { (Monate)(quartal * 3), (Monate)(quartal * 3 + 1), (Monate)(quartal * 3 + 2) });

        }
        public FileInfo ExportMonthAsPDF(List<Item> alleGemeindeglieder, Monate monat)
        {
            return ExportAsPDF(alleGemeindeglieder, new List<Monate>() { monat });
        }
        public FileInfo ExportMonthAsPDF(List<Item> geburtstagskinder, List<Item> taufKinder, Monate monat)
        {
            String path = Path.ChangeExtension(Path.GetTempFileName(), "pdf");
            FileInfo file = new FileInfo(System.IO.Path.Combine(path));
            file.Directory.Create();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(file));
            Document doc = new Document(pdfDoc);
            Table table = GetMonthTable(geburtstagskinder, taufKinder, monat);
            doc.Add(table);
            doc.Close();
            return file;
        }
        private Table GetMonthTable(List<Item> geburtstagskinder, List<Item> taufKinder, Monate month = (Monate)(-1))
        {
            Boolean twoRows = geburtstagskinder != null && taufKinder != null;
            var arrayOfList = new List<List<Item>> { geburtstagskinder, taufKinder };
           
            var outer = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
            if (!twoRows) outer = new Table(1).UseAllAvailableWidth();
            if (month != (Monate)(-1))
            {
                Paragraph p = new Paragraph();
                p = new Paragraph();
                p.SetFont(boldFont);
                p.SetTextAlignment(TextAlignment.LEFT);
                p.SetFontSize(18f);
                p.SetTextAlignment(TextAlignment.CENTER);
                p.Add(month.ToString());
                if (twoRows)
                {
                    Cell c = new Cell(1,2).Add(p).SetBorder(Border.NO_BORDER);
                    outer.AddCell(c);
                }
                else
                {
                    outer.AddCell(new Cell(1, 2).Add(p).SetBorder(Border.NO_BORDER));
                }
            }
            for (int i = 0; i <= 1; i++)
            {
                
                if (arrayOfList[i] != null)
                {

                        Table table = new Table(3).UseAllAvailableWidth();
                    if (arrayOfList[i].Count > 0)
                    {
                        var cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                        cell.Add(new Paragraph("Name").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(12f));
                        table.AddCell(cell);

                        cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                        cell.Add(new Paragraph((i== 0)?"Geburtstag":"Taufjubiläum").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(12f));
                        table.AddCell(cell);

                        cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                        cell.Add(new Paragraph("Status").SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(12f));
                        table.AddCell(cell);
                        foreach (var item in arrayOfList[i])
                        {
                            cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE);
                            cell.Add(new Paragraph(item.Name).SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10f));
                            table.AddCell(cell);

                            cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE);
                            cell.Add(new Paragraph((i == 0) ? item.Geburtsdatum : item.Taufdatum).SetFont(normalFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10f));
                            table.AddCell(cell);

                            cell = new Cell().SetVerticalAlignment(VerticalAlignment.MIDDLE);
                            cell.Add(new Paragraph(item.Status).SetFont(normalFont).SetTextAlignment(TextAlignment.CENTER).SetFontSize(10f));
                            table.AddCell(cell);
                        }
                    }
                    outer.AddCell(new Cell().Add(table).SetBorder(Border.NO_BORDER));
                }
            }
            return outer;
        }
    }
}
