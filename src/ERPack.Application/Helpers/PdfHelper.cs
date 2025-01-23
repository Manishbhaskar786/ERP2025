using ERPack.Customers;
using ERPack.Departments;
using ERPack.Enquiries;
using ERPack.Enquiries.Dto;
using ERPack.Enquries.Dto;
using ERPack.Estimates;
using ERPack.Estimates.Dto;
using ERPack.MultiTenancy;
using ERPack.Sessions.Dto;
using ERPack.Users;
using ERPack.Workorders;
using ERPack.Workorders.Dto;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Font = iTextSharp.text.Font;

namespace ERPack.Helpers
{
    public class PdfHelper : ERPackAppServiceBase, IPdfHelper
    {
        readonly Font nrmlFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        readonly Font nrmlFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private readonly WorkorderManager _workorderManager;
        private readonly EstimateManager _estimateManager;
        private readonly HostTenantManager _hostTenantManager;
        private readonly CustomerManager _customerManager;
        private readonly EnquiryManager _enquiryManager;
        private readonly EnquiryMaterialManager _enquiryMaterialManager;
        private readonly IUserAppService _userAppService;
        private readonly IUnitAppService _unitAppService;
        private readonly IHostEnvironment _env;

        public PdfHelper(WorkorderManager workorderManager, EstimateManager estimateManager,
            HostTenantManager hostTenantManager,
            CustomerManager customerManager,
            EnquiryManager enquiryManager,
            EnquiryMaterialManager enquiryMaterialManager,
            UserAppService userAppService,
            IUnitAppService unitAppService,
            IHostEnvironment env)
        {
            _workorderManager = workorderManager;
            _estimateManager = estimateManager;
            _hostTenantManager = hostTenantManager;
            _customerManager = customerManager;
            _enquiryManager= enquiryManager;
            _enquiryMaterialManager = enquiryMaterialManager;
            _userAppService = userAppService;
            _unitAppService = unitAppService;
            _env = env;
        }

        public Byte[] ExportTable(string sTitle, string sData)
        {
            // Prepare Doc
            Document doc = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var pgSize = new Rectangle(3000, 2000);
            Document document = new Document(pgSize);

            Rectangle rect = new Rectangle(pgSize);
            rect.BackgroundColor = new BaseColor(242, 238, 235);

            doc.Add(AddParagraph(sTitle, 1, nrmlFontBold));

            string sHtml = string.Format("<table>{0}</table>", sData);
            HtmlNodeCollection hc = HtmlHelper.GetNodeHtmlCollection(sData, "//tr");
            HtmlNodeCollection ths = HtmlHelper.GetNodeHtmlCollection(sData, "//th");

            #region  Table
            PdfPTable pdfPTable = new PdfPTable(ths.Count - 2);
            pdfPTable.SpacingBefore = 10f;
            pdfPTable.SpacingAfter = 20f;

            foreach (HtmlNode nodeTh in ths)
            {
                if (!string.IsNullOrEmpty(nodeTh.InnerText.Trim()) && nodeTh.InnerText.Trim() != "Actions")
                {
                    pdfPTable.AddCell(AddCell(nodeTh.InnerText, 1, 0, nrmlFontBold, 1));
                }
            }

            foreach (HtmlNode node in hc)
            {
                HtmlNodeCollection tds = HtmlHelper.GetNodeHtmlCollection(node.InnerHtml, "//td");
                if (tds == null) continue;
                int iCount = 0;
                foreach (HtmlNode nodeTd in tds)
                {
                    if (iCount > 0 && iCount < tds.Count - 1)
                        pdfPTable.AddCell(AddCell(nodeTd.InnerText.Trim(), 1, 0, nrmlFont, 1));
                    iCount++;
                }
            }
            doc.Add(pdfPTable);

            #endregion

            doc.Close();
            return ms.ToArray();
        }

        public async Task<byte[]> GetWorkorder(long workorderId, string imageURL)
        {
            var entity = await _workorderManager.GetAsync(workorderId);
            var workorder = ObjectMapper.Map<WorkorderDto>(entity);

            var workorderTasks = await _workorderManager.GetWorkorderTasksAsync(workorderId);

            var workorderTasksList = ObjectMapper.Map<List<WorkorderTaskDto>>(workorderTasks);
            var Units = await _unitAppService.GetUnitsAsync();
            // Prepare Doc
            Document doc = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            Rectangle rect = new Rectangle(doc.PageSize)
            {
                BackgroundColor = new BaseColor(242, 238, 235)
            };

            doc.Add(AddParagraph("Workorder for " + workorder.WorkorderId, 1, nrmlFontBold));

            PdfPTable logoTable = new PdfPTable(8);
            logoTable.SpacingBefore = 20f;
            logoTable.AddCell(AddCell("", 4, 0, nrmlFont));
            logoTable.AddCell(AddCell("", 2, 0, nrmlFont));
            doc.Add(logoTable);

            #region mat Table
            PdfPTable matTable = new PdfPTable(7);
            matTable.SpacingAfter = 20f;

            matTable.AddCell(AddCell("S.No", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Material", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Unit", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Qty", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Issue Date", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Complete Date", 1, 1, nrmlFontBold, 1));
            matTable.AddCell(AddCell("Actual Complete Data", 1, 1, nrmlFontBold, 1));

            int serialNumber = 1;
            foreach (var item in workorderTasksList)
            {
                string unitname = "";
                if(item.UnitId > 0)
                {
                    unitname = Units.Where(x => x.Id == item.UnitId).Select(x => x.UnitName).FirstOrDefault();
                }
                
                matTable.AddCell(AddCell(serialNumber.ToString(), 1, 1, nrmlFont, 1));
                matTable.AddCell(AddCell(item.MaterialName, 1, 1, nrmlFont, 1));
                matTable.AddCell(AddCell(unitname, 1, 1, nrmlFont, 1));
                matTable.AddCell(AddCell(item.Quantity.ToString(), 1, 1, nrmlFont, 1));
                matTable.AddCell(AddCell(item.TaskIssueDate.ToString(), 1, 1, nrmlFont, 1)); ;
                matTable.AddCell(AddCell(item.TaskIssueCompleteDate.ToString(), 1, 1, nrmlFont, 1));
                matTable.AddCell(AddCell(item.TaskIssueActualCompleteDate.ToString(), 1, 1, nrmlFont, 1));
                serialNumber++;
            }
            doc.Add(matTable);
            #endregion
            doc.Close();
            return ms.ToArray();
        }

        public async Task<byte[]> GetEstimate(long estimateId, GetCurrentLoginInformationsOutput loginInfo)
        {
            var hostTenantInfo = await _hostTenantManager.GetAsync();

            var entity = await _estimateManager.GetAsync(estimateId);
            var estimate = ObjectMapper.Map<EstimateDto>(entity);

            var customer = await  _customerManager.GetAsync(estimate.CustomerId);

            var estimateTasks = await _estimateManager.GetEstimateTasksAsync(estimateId);

            var estimateTasksList = ObjectMapper.Map<List<EstimateTaskDto>>(estimateTasks);
            // Prepare Doc
            Document doc = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            Image logo;
            if (loginInfo.Tenant != null)
            {
                logo = Image.GetInstance(loginInfo.Tenant.Logo);
            }
            else
            {
                logo = Image.GetInstance(Path.Combine(_env.ContentRootPath, "wwwroot\\img\\logo.png"));
            }
            logo.ScaleToFit(96f, 96f);
            logo.Alignment = Image.TEXTWRAP | Image.ALIGN_CENTER;
            doc.Add(logo);

            #region Tenant Table
            PdfPTable tableTenant = new PdfPTable(4);
            tableTenant.DefaultCell.Border = 0;
            tableTenant.WidthPercentage = 100;

            var titleFont = new Font(Font.FontFamily.UNDEFINED, 10);
            var subTitleFont = new Font(Font.FontFamily.UNDEFINED, 8);

            PdfPCell cell11 = new PdfPCell();
            cell11.Colspan = 2;
            cell11.Rowspan = 3;
            if (loginInfo.Tenant != null)
            {
                cell11.AddElement(new Paragraph(loginInfo.Tenant.Name, titleFont));
            }
            else
            {
                cell11.AddElement(new Paragraph("ERPack", titleFont));
            }

            if (loginInfo.Tenant != null)
            {
                cell11.AddElement(new Paragraph(loginInfo.Tenant.Address1 + " "
                + loginInfo.Tenant.Address2 + ","
                + loginInfo.Tenant.City + ","
                + loginInfo.Tenant.State + ","
                + loginInfo.Tenant.Country + ","
                + loginInfo.Tenant.PinCode
                , subTitleFont));
            }
            else
            {
                cell11.AddElement(new Paragraph(hostTenantInfo.Address1 + " "
                + hostTenantInfo.Address2 + ","
                + hostTenantInfo.City + ","
                + hostTenantInfo.State + ","
                + hostTenantInfo.Country + ","
                + hostTenantInfo.PinCode
                , subTitleFont));
            }

            cell11.VerticalAlignment = Element.ALIGN_LEFT;

            tableTenant.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell();

            cell12.VerticalAlignment = Element.ALIGN_CENTER;
            cell12.AddElement(new Paragraph("Invoice No./Estimate No.", subTitleFont));
            cell12.AddElement(new Paragraph(estimate.EstimateId, subTitleFont));

            tableTenant.AddCell(cell12);

            PdfPCell cell13 = new();

            cell13.AddElement(new Paragraph("Dated", subTitleFont));
            cell13.AddElement(new Paragraph(DateTime.Now.ToShortDateString(), subTitleFont));

            tableTenant.AddCell(cell13);

            PdfPCell cell22 = new();

            cell22.AddElement(new Paragraph("Delivery Note", subTitleFont));

            tableTenant.AddCell(cell22);

            PdfPCell cell23 = new();

            cell23.AddElement(new Paragraph("Mode/Terms of Payment", subTitleFont));

            tableTenant.AddCell(cell23);

            PdfPCell cell32 = new();

            cell32.AddElement(new Paragraph("Supplier’s Ref.", subTitleFont));

            tableTenant.AddCell(cell32);

            PdfPCell cell33 = new();

            cell33.AddElement(new Paragraph("Other Reference(s)", subTitleFont));

            tableTenant.AddCell(cell33);

            PdfPCell cell41 = new();
            cell41.Rowspan = 4;
            cell41.Colspan = 2;

            cell41.AddElement(new Paragraph("Customer: " + customer.Name, subTitleFont));
            cell41.AddElement(new Paragraph("Phone: " + customer.ContactNo, subTitleFont));
            cell41.AddElement(new Paragraph("GSTIN/UIN: " + customer.GSTNo, subTitleFont));
            cell41.AddElement(new Paragraph("State: " + customer.StateId, subTitleFont));
            cell41.AddElement(new Paragraph("Pincode:" + customer.PinCode, subTitleFont));
            tableTenant.AddCell(cell41);

            PdfPCell cell42 = new();

            cell42.AddElement(new Paragraph("Buyer’s Order No.", subTitleFont));

            tableTenant.AddCell(cell42);

            PdfPCell cell43 = new();

            cell43.AddElement(new Paragraph("Dated", subTitleFont));
            cell43.AddElement(new Paragraph(DateTime.Now.ToShortDateString(), subTitleFont));

            tableTenant.AddCell(cell43);

            PdfPCell cell52 = new();

            cell52.AddElement(new Paragraph("Despatch Document No.", subTitleFont));

            tableTenant.AddCell(cell52);

            PdfPCell cell53 = new();

            cell53.AddElement(new Paragraph("Delivery Note Date", subTitleFont));

            tableTenant.AddCell(cell53);

            PdfPCell cell62 = new();

            cell62.AddElement(new Paragraph("Despatched through", subTitleFont));

            tableTenant.AddCell(cell62);

            PdfPCell cell63 = new();

            cell63.AddElement(new Paragraph("Destination", subTitleFont));

            tableTenant.AddCell(cell63);

            PdfPCell cell72 = new();
            cell72.Colspan = 2;
            cell72.AddElement(new Paragraph("Terms of Delivery", subTitleFont));

            tableTenant.AddCell(cell72);

            doc.Add(tableTenant);

            #endregion

            #region Items Table

            PdfPTable tableItems = new PdfPTable(7);
            tableItems.DefaultCell.Border = 0;
            tableItems.WidthPercentage = 100;

            PdfPCell itemCell11 = new();
            itemCell11.AddElement(new Paragraph("S No.", subTitleFont));
            tableItems.AddCell(itemCell11);

            PdfPCell itemCell12 = new();
            itemCell12.AddElement(new Paragraph("Description of Goods", subTitleFont));
            tableItems.AddCell(itemCell12);

            PdfPCell itemCell13 = new();
            itemCell13.AddElement(new Paragraph("HSN/SAC", subTitleFont));
            tableItems.AddCell(itemCell13);

            PdfPCell itemCell14 = new();
            itemCell14.AddElement(new Paragraph("Quantity", subTitleFont));
            tableItems.AddCell(itemCell14);

            PdfPCell itemCell15 = new();
            itemCell15.AddElement(new Paragraph("Rate", subTitleFont));
            tableItems.AddCell(itemCell15);

            PdfPCell itemCell16 = new();
            itemCell16.AddElement(new Paragraph("Per", subTitleFont));
            tableItems.AddCell(itemCell16);

            PdfPCell itemCell17 = new();
            itemCell17.AddElement(new Paragraph("Amount", subTitleFont));
            tableItems.AddCell(itemCell17);

            int serialNumber = 1;
            decimal totalQty = 0;
            decimal totalAmount = 0;
            foreach (var item in estimateTasksList)
            {
                tableItems.AddCell(AddCell(serialNumber.ToString(), 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.MaterialName, 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.SellingUnitName, 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.Qty.ToString(), 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.Price.ToString(), 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.DiscountPercentage.ToString(), 1, 1, nrmlFont, 1));
                tableItems.AddCell(AddCell(item.Amount.ToString(), 1, 1, nrmlFont, 1));
                totalQty = totalQty + item.Qty;
                totalAmount = totalAmount + item.Amount;
                serialNumber++;
            }

            PdfPCell itemCell21 = new();
            itemCell21.AddElement(new Paragraph("", subTitleFont));
            tableItems.AddCell(itemCell21);

            PdfPCell itemCell22 = new();
            itemCell22.AddElement(new Paragraph("Total", subTitleFont));
            tableItems.AddCell(itemCell22);

            PdfPCell itemCell23 = new();
            itemCell13.AddElement(new Paragraph("", subTitleFont));
            tableItems.AddCell(itemCell23);

            PdfPCell itemCell24 = new();
            var totalQtyElement = new Paragraph(totalQty.ToString(), subTitleFont);
            totalQtyElement.Alignment = Element.ALIGN_CENTER;
            itemCell24.AddElement(totalQtyElement);
            tableItems.AddCell(itemCell24);

            PdfPCell itemCell25 = new();
            itemCell25.AddElement(new Paragraph("", subTitleFont));
            tableItems.AddCell(itemCell25);

            PdfPCell itemCell26 = new();
            itemCell26.AddElement(new Paragraph("", subTitleFont));
            tableItems.AddCell(itemCell26);

            PdfPCell itemCell27 = new();
            var totalAmountElement = new Paragraph(totalAmount.ToString(), subTitleFont);
            totalAmountElement.Alignment = Element.ALIGN_CENTER;
            itemCell27.AddElement(totalAmountElement);
            tableItems.AddCell(itemCell27);

            doc.Add(tableItems);

            #endregion

            #region Bottom Table

            PdfPTable tableBottom = new PdfPTable(7);
            tableBottom.DefaultCell.Border = 0;
           // tableBottom.SpacingBefore = 200f;
            tableBottom.WidthPercentage = 100;

            PdfPCell bottomCell11 = new();
            bottomCell11.Colspan = 3;
            bottomCell11.AddElement(new Paragraph("Amount Chargeable (in words)", subTitleFont));
            tableBottom.AddCell(bottomCell11);

            PdfPCell bottomCell12 = new();
            bottomCell12.Colspan = 4;
            bottomCell12.AddElement(new Paragraph("E. & O.E", subTitleFont));
            tableBottom.AddCell(bottomCell12);

            PdfPCell bottomCell21 = new();
            bottomCell21.Rowspan = 2;
            bottomCell21.AddElement(new Paragraph("HSN/SAC", subTitleFont));
            tableBottom.AddCell(bottomCell21);

            PdfPCell bottomCell22 = new();
            bottomCell22.Rowspan = 2;
            bottomCell22.AddElement(new Paragraph("Taxable Value", subTitleFont));
            tableBottom.AddCell(bottomCell22);

            PdfPCell bottomCell23 = new();
            bottomCell23.Colspan = 2;
            bottomCell23.AddElement(new Paragraph("Central Tax", subTitleFont));
            tableBottom.AddCell(bottomCell23);

            PdfPCell bottomCell24 = new();
            bottomCell24.Colspan = 2;
            bottomCell24.AddElement(new Paragraph("State Tax", subTitleFont));
            tableBottom.AddCell(bottomCell24);

            PdfPCell bottomCell25 = new();
            bottomCell25.Rowspan = 2;
            bottomCell25.AddElement(new Paragraph("Total Tax Amount", subTitleFont));
            tableBottom.AddCell(bottomCell25);

            PdfPCell bottomCell33 = new();
            bottomCell33.AddElement(new Paragraph("Rate", subTitleFont));
            tableBottom.AddCell(bottomCell33);

            PdfPCell bottomCell34 = new();
            bottomCell34.AddElement(new Paragraph("Amount", subTitleFont));
            tableBottom.AddCell(bottomCell34);

            PdfPCell bottomCell35 = new();
            bottomCell35.AddElement(new Paragraph("Rate", subTitleFont));
            tableBottom.AddCell(bottomCell35);

            PdfPCell bottomCell36 = new();
            bottomCell36.AddElement(new Paragraph("Amount", subTitleFont));
            tableBottom.AddCell(bottomCell36);

            doc.Add(tableBottom);

            #endregion

            #region BankDetails Table

            PdfPTable tableBank = new PdfPTable(2);
            tableBank.DefaultCell.Border = 0;
            tableBank.WidthPercentage = 100;

            PdfPCell bankCell11 = new();
            bankCell11.AddElement(new Paragraph("Tax Amount (in words):", subTitleFont));
            bankCell11.AddElement(new Paragraph("Company's VAT TIN:", subTitleFont));
            bankCell11.AddElement(new Paragraph("Company's CST No:", subTitleFont));
            tableBank.AddCell(bankCell11);

            if(loginInfo.Tenant != null)
            {
                PdfPCell bankCell12 = new();
                bankCell12.AddElement(new Paragraph("Bank Name :" + loginInfo.Tenant.BankName, subTitleFont));
                bankCell12.AddElement(new Paragraph("A/c No. :" + loginInfo.Tenant.AccountNumber, subTitleFont));
                bankCell12.AddElement(new Paragraph("Branch :" + loginInfo.Tenant.Branch, subTitleFont));
                bankCell12.AddElement(new Paragraph("IFSC Code:" + loginInfo.Tenant.IFSCCode, subTitleFont));
                tableBank.AddCell(bankCell12);
            }
            else
            {
                PdfPCell bankCell12 = new();
                bankCell12.AddElement(new Paragraph("Bank Name :" + hostTenantInfo.BankName, subTitleFont));
                bankCell12.AddElement(new Paragraph("A/c No. :" + hostTenantInfo.AccountNumber, subTitleFont));
                bankCell12.AddElement(new Paragraph("Branch :" + hostTenantInfo.Branch, subTitleFont));
                bankCell12.AddElement(new Paragraph("IFSC Code:" + hostTenantInfo.IFSCCode, subTitleFont));
                tableBank.AddCell(bankCell12);
            }

            PdfPCell bankCell21 = new();
            bankCell21.AddElement(new Paragraph("Declaration", subTitleFont));
            bankCell21.AddElement(new Paragraph("Terms and Conditions", subTitleFont));
            bankCell21.AddElement(new Paragraph("If any Defect in material inform us within 30 Days. After that no Claim will be entertain. 18% Interest will be charged on late payments.", subTitleFont));
            tableBank.AddCell(bankCell21);

            PdfPCell bankCell22 = new();
            var p1 = new Paragraph("for", subTitleFont);
            p1.Alignment = Element.ALIGN_RIGHT;
            bankCell22.AddElement(p1);

            var p2 = new Paragraph("Authorised Signatory", subTitleFont);
            p2.Alignment = Element.ALIGN_RIGHT;
            bankCell22.AddElement(p2);
            tableBank.AddCell(bankCell22);

            doc.Add(tableBank);

            #endregion

            doc.Close();
            return ms.ToArray();
        }
        public async Task<byte[]> GetDesignJobCard(long enquiryId, GetCurrentLoginInformationsOutput loginInfo)
        {
            var hostTenantInfo = await _hostTenantManager.GetAsync();

            var entity = await _enquiryManager.GetAsync(enquiryId);
            var enquiry = ObjectMapper.Map<EnquiryDto>(entity);

          
            var user = await _userAppService.GetByIdAsync((long)enquiry.CreatorUserId);

            var enquiryMaterials = await _enquiryMaterialManager.GetAllByEnquiryIdAsync((int)enquiryId);

            var enquiryMaterialsList = ObjectMapper.Map<List<EnquiryMaterialDto>>(enquiryMaterials);
            // Prepare Doc
            Document doc = new Document();
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            // Get the PdfContentByte object
            PdfContentByte canvas = writer.DirectContent;

            // Set the line width for the border
            canvas.SetLineWidth(1f);

            // Create a rectangle that fits within the margins
            Rectangle borderRectangle = new Rectangle(
                doc.LeftMargin,
                doc.BottomMargin,
                doc.PageSize.Width - doc.RightMargin,
                doc.PageSize.Height - doc.TopMargin
            );

            // Draw the rectangle border
            canvas.Rectangle(borderRectangle.Left, borderRectangle.Bottom, borderRectangle.Width, borderRectangle.Height);
            canvas.Stroke();



            #region Design Table
            PdfPTable tableDesign = new PdfPTable(3);
            tableDesign.DefaultCell.BorderWidth = 0;
            tableDesign.WidthPercentage = 100;

            var font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
           
            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLUE);
            var subTitleFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
            PdfPCell cell11 = new PdfPCell();

            Image logo;
            if (loginInfo.Tenant != null)
            {
                logo = Image.GetInstance(loginInfo.Tenant.Logo);
            }
            else
            {
                logo = Image.GetInstance(Path.Combine(_env.ContentRootPath, "wwwroot\\img\\logo.png"));
            }
            logo.ScaleToFit(95f, 95f);
            logo.Alignment = Image.TEXTWRAP | Image.ALIGN_CENTER;
            cell11.AddElement(logo);
            cell11.AddElement(new Paragraph("\n"));
            Paragraph tenantName;

            if (loginInfo.Tenant != null)
            {
                // Create the Paragraph with the tenant name and font
                tenantName = new Paragraph(10f, loginInfo.Tenant.Name.ToUpper(), subTitleFont);
            }
            else
            {
                // Create the Paragraph with the default text and font
                tenantName = new Paragraph(10f, "ERPACK", subTitleFont);
            }

            // Set the alignment of the paragraph to center
            tenantName.Alignment = Element.ALIGN_CENTER;

            // Add the paragraph to the cell
            cell11.AddElement(tenantName);

            cell11.VerticalAlignment = Element.ALIGN_CENTER;
          
            cell11.BorderWidth = 2f;
            cell11.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell11.Border = Rectangle.BOX; // Enables all borders
           
            cell11.BorderColor = BaseColor.BLACK; // Sets the border color

            tableDesign.AddCell(cell11);
           
            PdfPCell cell12 = new PdfPCell();
            cell12.Colspan = 2;
            cell12.BorderWidth = 0;
            cell12.AddElement(new Paragraph("\n"));
            cell12.PaddingTop = 20f; // Adjust padding as needed
            cell12.PaddingBottom = 20f;
            cell12.PaddingLeft = 50f;
            Chunk underlinedText = new Chunk("Design Job Card", titleFont);
            underlinedText.SetUnderline(0.5f, -1.5f); // Set the thickness and position of the underline

            // Add the Chunk to a Paragraph
            Paragraph paragraph = new Paragraph();
            paragraph.Add(underlinedText);

            cell12.AddElement(paragraph);
            cell12.HorizontalAlignment = Element.ALIGN_CENTER; // Center horizontally
            cell12.VerticalAlignment = Element.ALIGN_MIDDLE;  // Center vertically


            tableDesign.AddCell(cell12);

            PdfPCell cell21 = new();

            Phrase combinedDate = new Phrase();
            combinedDate.Add(new Paragraph("Date : ", subTitleFont));
            combinedDate.Add(new Paragraph(enquiry.CreationTime.ToString(), font));

            cell21.AddElement(combinedDate);
            cell21.BorderWidth = 0;
            cell21.MinimumHeight = 30f;
            cell21.FixedHeight = 40f;
            tableDesign.AddCell(cell21);

            PdfPCell cell22 = new();

            cell22.MinimumHeight = 30f;
            cell22.FixedHeight = 40f;
            cell22.BorderWidth = 0;
            Phrase postedby = new Phrase();
            postedby.Add(new Paragraph("Posted By : ", subTitleFont));
            postedby.Add(new Paragraph(user.UserName, font));

            cell22.AddElement(postedby);

          
            tableDesign.AddCell(cell22);

            PdfPCell cell23 = new();


            cell23.MinimumHeight = 30f;
            cell23.FixedHeight = 40f;
            cell23.BorderWidth = 0;
            Phrase enqid = new Phrase();
            enqid.Add(new Paragraph("Enquiry Id : ", subTitleFont));
            enqid.Add(new Paragraph(enquiry.EnquiryId, font));

            cell23.AddElement(enqid);

            tableDesign.AddCell(cell23);

            PdfPCell cell31 = new();

            cell31.Colspan = 3;
            cell31.BorderWidth = 1f;
            cell31.PaddingBottom = 2f;


            Phrase comments = new Phrase();
            comments.Add(new Paragraph("Comments: ", subTitleFont));
            comments.Add(new Paragraph(enquiry.Comments, font));

            cell31.AddElement(comments);
            cell31.AddElement(new Paragraph("\n"));
            cell31.HorizontalAlignment = Element.ALIGN_LEFT;

            tableDesign.AddCell(cell31);

            

            PdfPCell cell41 = new();
            cell41.BorderWidth = 0;
            Phrase StyleNamePhrase = new Phrase();
            StyleNamePhrase.Add(new Chunk("Style Name : ", subTitleFont));
            StyleNamePhrase.Add(new Chunk(enquiry.DesignName != null ? enquiry.DesignName.ToString() : "NA", font));
            cell41.AddElement(StyleNamePhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase StyleNumberPhrase = new Phrase();
            StyleNumberPhrase.Add(new Chunk("Style Number : ", subTitleFont));
            StyleNumberPhrase.Add(new Chunk(enquiry.DesignNumber != null ? enquiry.DesignNumber.ToString() : "NA", font));
            cell41.AddElement(StyleNumberPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase BoxLengthPhrase = new Phrase();
            BoxLengthPhrase.Add(new Chunk("Box Length : ", subTitleFont));
            BoxLengthPhrase.Add(new Chunk(enquiry.BoxLength != null ? enquiry.BoxLength.ToString() : "NA", font));
            cell41.AddElement(BoxLengthPhrase);           
            cell41.AddElement(new Paragraph("\n"));

            Phrase BoxHeightPhrase = new Phrase();
            BoxHeightPhrase.Add(new Chunk("Box Height : ", subTitleFont));
            BoxHeightPhrase.Add(new Chunk(enquiry.BoxHeight != null ? enquiry.BoxHeight.ToString() : "NA", font));
            cell41.AddElement(BoxHeightPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase BoxWidthPhrase = new Phrase();
            BoxWidthPhrase.Add(new Chunk("Box Width : ", subTitleFont));
            BoxWidthPhrase.Add(new Chunk(enquiry.BoxWidth != null ? enquiry.BoxWidth.ToString() : "NA", font));
            cell41.AddElement(BoxWidthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase BraileLengthPhrase = new Phrase();
            BraileLengthPhrase.Add(new Chunk("Braile Length : ", subTitleFont));
            BraileLengthPhrase.Add(new Chunk(enquiry.BraileLength != null ? enquiry.BraileLength.ToString() : "NA", font));
            cell41.AddElement(BraileLengthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase BraileWidthPhrase = new Phrase();
            BraileWidthPhrase.Add(new Chunk("Braile Width : ", subTitleFont));
            BraileWidthPhrase.Add(new Chunk(enquiry.BraileWidth != null ? enquiry.BraileWidth.ToString() : "NA",font));
            cell41.AddElement(BraileWidthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase EmbossLengthPhrase = new Phrase();
            EmbossLengthPhrase.Add(new Chunk("Emboss Length : ", subTitleFont));
            EmbossLengthPhrase.Add(new Chunk(enquiry.EmbossLength != null ? enquiry.EmbossLength.ToString() : "NA",font));
            cell41.AddElement(EmbossLengthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase EmbossWidthPhrase = new Phrase();
            EmbossWidthPhrase.Add(new Chunk("Emboss Width : ", subTitleFont));
            EmbossWidthPhrase.Add(new Chunk(enquiry.EmbossWidth != null ? enquiry.EmbossWidth.ToString() : "NA", font));
            cell41.AddElement(EmbossWidthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase SheetSizeLengthPhrase = new Phrase();
            SheetSizeLengthPhrase.Add(new Chunk("SheetSize Length : ", subTitleFont));
            SheetSizeLengthPhrase.Add(new Chunk(enquiry.SheetSizeLength != null ? enquiry.SheetSizeLength.ToString() : "NA", font));
            cell41.AddElement(SheetSizeLengthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase SheetSizeWidthPhrase = new Phrase();
            SheetSizeWidthPhrase.Add(new Chunk("SheetSize Width : ", subTitleFont));
            SheetSizeWidthPhrase.Add(new Chunk(enquiry.SheetSizeWidth!=null?enquiry.SheetSizeWidth.ToString() : "NA",font)) ;
            cell41.AddElement(SheetSizeWidthPhrase);
            cell41.AddElement(new Paragraph("\n"));

            Phrase BoardTypePhrase = new Phrase();
            BoardTypePhrase.Add(new Chunk("Board Type : ", subTitleFont));
            BoardTypePhrase.Add(new Chunk(enquiry.BoardTypeName != null ? enquiry.BoardTypeName.ToString() : "NA",font));
            cell41.AddElement(BoardTypePhrase);
            cell41.AddElement(new Paragraph("\n"));

          
            
            cell41.Padding = 5f;
            cell41.VerticalAlignment = Element.ALIGN_LEFT;
            tableDesign.AddCell(cell41);

            PdfPCell cell42 = new();
            cell42.Colspan =2;

            Image design;
            if (enquiry.DesignImage!= null)
                {
                if (!enquiry.DesignImage.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                   
                        design = Image.GetInstance(Path.Combine( _env.ContentRootPath, "wwwroot", enquiry.DesignImage.TrimStart('\\', '/')));
                    }
            else
                    {
                        PdfReader reader = new PdfReader(Path.Combine(_env.ContentRootPath, "wwwroot" , enquiry.DesignImage.TrimStart('\\', '/')));

                        // Get the first page (for example) from the source PDF
                        PdfImportedPage importedPage = writer.GetImportedPage(reader, 1);

                        // Create a PdfTemplate for the imported page
                        PdfTemplate template = writer.GetImportedPage(reader, 1);

                        // Create an Image from the imported PDF page
                        Image pdfAsImage = Image.GetInstance(template);

                        // Adjust the image size to fit within the cell


                        design = pdfAsImage;
                    }
                }
            else
            { 
            design = Image.GetInstance(Path.Combine("wwwroot/img","default-Image.jpg"));
                }
            design.ScaleToFit(300f,300f);
            design.Alignment = Image.TEXTWRAP | Image.ALIGN_CENTER;
            cell42.HorizontalAlignment = Element.ALIGN_CENTER; // Center horizontally
            cell42.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell42.UseBorderPadding = true;
            cell42.BorderWidth = 2f;
            cell42.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell42.Border = Rectangle.BOX; // Enables all borders

            cell42.BorderColor = BaseColor.BLACK; // S
            cell42.AddElement(design);

            

            tableDesign.AddCell(cell42);
            doc.Add(tableDesign);
            #endregion
            PdfPTable tableItems = new PdfPTable(3);
            tableItems.WidthPercentage = 100;
            #region Items Table
            if (enquiryMaterialsList.Count>0)
            { 
            // Create a table with 3 columns
            
            PdfPCell headerCell = new PdfPCell(new Phrase("Materials", subTitleFont));
            headerCell.Padding = 10f;
            headerCell.Colspan = 3;
            headerCell.BorderWidth = 0;

            tableItems.AddCell(headerCell);
            // Style the table headers
            PdfPCell headerCell1 = new PdfPCell(new Phrase("S No.", subTitleFont));
            headerCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
            headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell1.Padding = 8f;
            headerCell1.BorderColor = BaseColor.BLACK;
            headerCell1.BorderWidth = 1f;
            tableItems.AddCell(headerCell1);

            PdfPCell headerCell2 = new PdfPCell(new Phrase("ItemCode", subTitleFont));
            headerCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
            headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell2.Padding = 8f;
            headerCell2.BorderColor = BaseColor.BLACK;
            headerCell2.BorderWidth = 1f;
            tableItems.AddCell(headerCell2);

            PdfPCell headerCell3 = new PdfPCell(new Phrase("MaterialName", subTitleFont));
            headerCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
            headerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell3.Padding = 8f;
            headerCell3.BorderColor = BaseColor.BLACK;
            headerCell3.BorderWidth = 1f;
            tableItems.AddCell(headerCell3);

            // Add data rows
            int serialNumber = 1;
            foreach (var item in enquiryMaterialsList)
            {
                PdfPCell serialCell = new PdfPCell(new Phrase(serialNumber.ToString(), font));
                serialCell.HorizontalAlignment = Element.ALIGN_CENTER;
                serialCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                serialCell.Padding = 8f;
                serialCell.BorderColor = BaseColor.BLACK;
                serialCell.BorderWidth = 1f;
                tableItems.AddCell(serialCell);

                PdfPCell codeCell = new PdfPCell(new Phrase(item.ItemCode, font));
                codeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                codeCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                codeCell.Padding = 8f;
                codeCell.BorderColor = BaseColor.BLACK;
                codeCell.BorderWidth = 1f;
                tableItems.AddCell(codeCell);

                PdfPCell nameCell = new PdfPCell(new Phrase(item.MaterialName, font));
                nameCell.HorizontalAlignment = Element.ALIGN_LEFT;
                nameCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                nameCell.Padding = 8f;
                nameCell.BorderColor = BaseColor.BLACK;
                nameCell.BorderWidth = 1f;
                tableItems.AddCell(nameCell);

                serialNumber++;
            }

           
            }

            else
            {
                // Create a table with 3 columns
               
                PdfPCell headerCell = new PdfPCell();
                headerCell.Padding = 10f;
                headerCell.Colspan = 3;
                headerCell.BorderWidth = 0;

                Phrase MaterialsPhrase = new Phrase();
                MaterialsPhrase.Add(new Chunk("Materials : ", subTitleFont));
                MaterialsPhrase.Add(new Chunk("NA", font));
                headerCell.AddElement(MaterialsPhrase);
                headerCell.AddElement(new Paragraph("\n"));

                tableItems.AddCell(headerCell);
                
               
            }
            // Add the table to the document
            doc.Add(tableItems);
            #endregion



            doc.Close();
            return ms.ToArray();
        }

        Paragraph AddParagraph(string sData, int iAlign = 0, Font font = null)
        {
            if (font == null) font = nrmlFont;
            Paragraph para = new Paragraph(sData, font);
            para.Alignment = iAlign;
            return para;
        }

        Image AddImage(string imageURL)
        {
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            jpg.ScaleToFit(50f, 50f);
            jpg.SpacingBefore = 10f;
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_LEFT;
            return jpg;
        }

        PdfPCell AddCell(string sdata, int icolspan = 1, int iAlign = 0, Font font = null, int border = 0)
        {
            if (font == null) font = nrmlFont;
            PdfPCell cell = new PdfPCell(new Phrase(sdata, font));
            cell.Colspan = icolspan;
            cell.HorizontalAlignment = iAlign;
            if (border == 0)
                cell.Border = 0;
            return cell;
        }
    }
}
