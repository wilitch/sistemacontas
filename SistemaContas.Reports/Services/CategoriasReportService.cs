using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using OfficeOpenXml;
using SistemaContas.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Reports.Services
{
    public class CategoriasReportService
    {
        /// <summary>
        /// Método para retornar um relatório de categorias em formato Excel
        /// </summary>
        /// <param name="categorias">Lista de categorias</param>
        /// <returns>Arquivo em memória (bytes[]) contendo o relatório</returns>
        public byte[] GerarRelatorioExcel(List<Categoria> categorias)
        {
            //define o tipo de licença para criação do arquivo excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //criando o conteudo do arquivo excel
            using (var excelPackage = new ExcelPackage())
            {
                //nome da planilha
                var sheet = excelPackage.Workbook.Worksheets.Add("Categorias");

                //escrevendo nas células:
                sheet.Cells["A1"].Value = "Relatório de Categorias";

                sheet.Cells["A3"].Value = "Id";
                sheet.Cells["B3"].Value = "Nome da Categoria";

                //imprimindo as categorias
                var linha = 4;

                foreach (var item in categorias)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Id.ToString();
                    sheet.Cells[$"B{linha}"].Value = item.Nome;

                    linha++;
                }

                //formatando as celulas da planilha
                sheet.Cells["A:B"].AutoFitColumns();

                //retornando o arquivo excel em memória
                return excelPackage.GetAsByteArray();
            }
        }

        /// <summary>
        /// Método para retornar um relatório de categorias em formato Pdf
        /// </summary>
        /// <param name="categorias">Lista de categorias</param>
        /// <returns>Arquivo em memória (bytes[]) contendo o relatório</returns>
        public byte[] GerarRelatorioPdf(List<Categoria> categorias)
        { 
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatório de Categorias\n"));
                document.Add(new Paragraph($"Data: {DateTime.Now.ToString("dd/MM/yyyy")}\n"));

                var table = new Table(2);
                //A tabela terá uma largura de 100% do documento
                table.SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell("ID");
                table.AddHeaderCell("Nome da Categoria");

                foreach (var item in categorias)
                {
                    table.AddCell(item.Id.ToString());
                    table.AddCell(item.Nome);
                }

                document.Add(table);
            }

            return memoryStream.ToArray();
        }
    }
}



