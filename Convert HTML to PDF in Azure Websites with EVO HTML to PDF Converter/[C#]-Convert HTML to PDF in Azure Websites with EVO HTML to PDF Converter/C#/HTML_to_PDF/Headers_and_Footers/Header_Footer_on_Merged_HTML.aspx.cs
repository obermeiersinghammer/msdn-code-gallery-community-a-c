﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

// Use EVO PDF Namespace
using EvoPdf.HtmlToPdfClient;

namespace EvoHtmlToPdfDemo.HTML_to_PDF.Headers_and_Footers
{
    public partial class Header_Footer_on_Merged_HTML : System.Web.UI.Page
    {
        protected void convertToPdfButton_Click(object sender, EventArgs e)
        {
            // Get the server IP and port
            String serverIP = textBoxServerIP.Text;
            uint serverPort = uint.Parse(textBoxServerPort.Text);

            // Create a PDF document
            Document pdfDocument = new Document(serverIP, serverPort);

            // Set optional service password
            if (textBoxServicePassword.Text.Length > 0)
                pdfDocument.ServicePassword = textBoxServicePassword.Text;

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "4W9+bn19bn5ue2B+bn1/YH98YHd3d3c=";

            // Add a page to PDF document
            PdfPage firstPdfPage = pdfDocument.AddPage();

            // Create Header
            if (addHeaderCheckBox.Checked)
                CreateHeader(pdfDocument, drawHeaderLineCheckBox.Checked);

            // Create Footer
            if (addFooterCheckBox.Checked)
                CreateFooter(pdfDocument, drawFooterLineCheckBox.Checked, addPageNumbersInFooterCheckBox.Checked);

                // Add First HTML

                // Create the first HTML to PDF element
                HtmlToPdfElement firstHtml = new HtmlToPdfElement(0, 0, firstUrlTextBox.Text);

                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                firstHtml.ConversionDelay = 2;

                // Optionally add a space between header and the content generated by this HTML to PDF element
                // The spacing for first page and the subsequent pages can be set independently
                // Leave this option not set for no spacing
                firstHtml.Y = float.Parse(firstPageSpacingTextBox.Text);
                firstHtml.TopSpacing = float.Parse(headerSpacingTextBox.Text);

                // Optionally add a space between footer and the content generated by this HTML to PDF element
                // Leave this option not set for no spacing
                firstHtml.BottomSpacing = float.Parse(footerSpacingTextBox.Text);

                // Set the header visibility in first, odd and even pages
                if (addHeaderCheckBox.Checked)
                {
                    firstPdfPage.ShowHeader = showHeaderInFirstPageCheckBox.Checked;
                    firstHtml.ShowHeaderInEvenPages = showHeaderInEvenPagesCheckBox.Checked;
                    firstHtml.ShowHeaderInOddPages = showHeaderInOddPagesCheckBox.Checked;
                }

                // Set the footer visibility in first, odd and even pages
                if (addFooterCheckBox.Checked)
                {
                    firstPdfPage.ShowFooter = showFooterInFirstPageCheckBox.Checked;
                    firstHtml.ShowFooterInEvenPages = showFooterInEvenPagesCheckBox.Checked;
                    firstHtml.ShowFooterInOddPages = showFooterInOddPagesCheckBox.Checked;
                }

                // Add the first HTML to PDF element to PDF document
                firstPdfPage.AddElement(firstHtml);
                
                // Add Second HTML
            
                // Create the second HTML to PDF element
                HtmlToPdfElement secondHtml = new HtmlToPdfElement(0, 0, secondUrlTextBox.Text);

                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                secondHtml.ConversionDelay = 2;

                // Optionally add a space between header and the content generated by this HTML to PDF element
                // Leave this option not set for no spacing
                secondHtml.TopSpacing = float.Parse(headerSpacingTextBox.Text);

                // Optionally add a space between footer and the content generated by this HTML to PDF element
                // Leave this option not set for no spacing
                secondHtml.BottomSpacing = float.Parse(footerSpacingTextBox.Text);

                // Set the header visibility in odd and even pages
                if (addHeaderCheckBox.Checked)
                {
                    secondHtml.ShowHeaderInEvenPages = showHeaderInEvenPagesCheckBox.Checked;
                    secondHtml.ShowHeaderInOddPages = showHeaderInOddPagesCheckBox.Checked;
                }

                // Set the footer visibility in odd and even pages
                if (addFooterCheckBox.Checked)
                {
                    secondHtml.ShowFooterInEvenPages = showFooterInEvenPagesCheckBox.Checked;
                    secondHtml.ShowFooterInOddPages = showFooterInOddPagesCheckBox.Checked;
                }

                if (startNewPageCheckBox.Checked)
                {
                    // Create a PDF page where to add the second HTML
                    PdfPage secondPdfPage = pdfDocument.AddPage();
                    // Add the second HTML to PDF element to PDF document at the beginnig of the new PDF page 
                    secondPdfPage.AddElement(secondHtml);
                }
                else
                {
                    // Add the second HTML to PDF element to PDF document write after the last added element
                    pdfDocument.AddElement(secondHtml);
                }

                // Save the PDF document in a memory buffer
                byte[] outPdfBuffer = pdfDocument.Save();

                // Send the PDF as response to browser

                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=Header_Footer_in_Merge_Multipe_HTML.pdf; size={0}", outPdfBuffer.Length.ToString()));

                // Write the PDF document buffer to HTTP response
                Response.BinaryWrite(outPdfBuffer);

                // End the HTTP response and stop the current page processing
                Response.End();            
        }

        /// <summary>
        /// Creates the document header
        /// </summary>
        /// <param name="pdfDocument">The PDF document object</param>
        /// <param name="drawHeaderLine">A flag indicating if a line should be drawn at the bottom of the header</param>
        private void CreateHeader(Document pdfDocument, bool drawHeaderLine)
        {
            string headerHtmlString = System.IO.File.ReadAllText(Server.MapPath("~/DemoAppFiles/Input/HTML_Files/Header_HTML.html"));
            string headerBaseUrl = "http://www.evopdf.com/demo/DemoAppFiles/Input/HTML_Files/";

            // Add a header to document having 60 points in height
            pdfDocument.AddHeaderTemplate(60);

            // Create a HTML element to be added in header
            HtmlToPdfElement headerHtml = new HtmlToPdfElement(headerHtmlString, headerBaseUrl);

            // Set the HTML element to fit the container height
            headerHtml.FitHeight = true;

            // Add HTML element to header
            pdfDocument.Header.AddElement(headerHtml);

            if (drawHeaderLine)
            {
                PdfPage firstPage = pdfDocument.GetPage(0);
                float headerWidth = firstPage.PageSize.Width - firstPage.Margins.Left - firstPage.Margins.Right;
                float headerHeight = pdfDocument.Header.Height;

                // Create a line element for the bottom of the header
                LineElement headerLine = new LineElement(0, headerHeight - 1, headerWidth, headerHeight - 1);

                // Set line color
                headerLine.ForeColor = RgbColor.Gray;

                // Add line element to the bottom of the header
                pdfDocument.Header.AddElement(headerLine);
            }
        }

        /// <summary>
        /// Creates the document footer
        /// </summary>
        /// <param name="htmlToPdfConverter">The HTML to PDF Converter object</param>
        /// <param name="addPageNumbers">A flag indicating if the page numbering is present in footer</param>
        /// <param name="drawFooterLine">A flag indicating if a line should be drawn at the top of the footer</param>
        private void CreateFooter(Document pdfDocument, bool addPageNumbers, bool drawFooterLine)
        {
            string footerHtmlString = System.IO.File.ReadAllText(Server.MapPath("~/DemoAppFiles/Input/HTML_Files/Footer_HTML.html"));
            string footerBaseUrl = "http://www.evopdf.com/demo/DemoAppFiles/Input/HTML_Files/";

            // Add a footer to document having 60 points in height
            pdfDocument.AddFooterTemplate(60);

            // Create a HTML element to be added in footer
            HtmlToPdfElement footerHtml = new HtmlToPdfElement(footerHtmlString, footerBaseUrl);

            // Set the HTML element to fit the container height
            footerHtml.FitHeight = true;

            // Add HTML element to footer
            pdfDocument.Footer.AddElement(footerHtml);

            // Add page numbering
            if (addPageNumbers)
            {
                // Create a text element with page numbering place holders &p; and & P;
                TextElement footerText = new TextElement(0, 30, "Page &p; of &P;  ", new PdfFont("Times New Roman", 10, true));

                // Align the text at the right of the footer
                footerText.TextAlign = HorizontalTextAlign.Right;

                // Set page numbering text color
                footerText.ForeColor = RgbColor.Navy;

                // Embed the text element font in PDF
                footerText.EmbedSysFont = true;

                // Add the text element to footer
                pdfDocument.Footer.AddElement(footerText);
            }

            if (drawFooterLine)
            {
                // Calculate the footer width based on PDF page size and margins
                PdfPage firstPage = pdfDocument.GetPage(0);
                float footerWidth = firstPage.PageSize.Width - firstPage.Margins.Left - firstPage.Margins.Right;

                // Create a line element for the top of the footer
                LineElement footerLine = new LineElement(0, 0, footerWidth, 0);

                // Set line color
                footerLine.ForeColor = RgbColor.Gray;

                // Add line element to the bottom of the footer
                pdfDocument.Footer.AddElement(footerLine);
            }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sampleCodeLiteral.Text = System.IO.File.ReadAllText(Server.MapPath("~/DemoAppFiles/Input/Code_Samples/CSharp/AspNet/HTML_to_PDF/Headers_and_Footers/Header_Footer_on_Merged_HTML.html"));
                descriptionLiteral.Text = System.IO.File.ReadAllText(Server.MapPath("~/DemoAppFiles/Input/Descriptions/AspNet/HTML_to_PDF/Headers_and_Footers/Header_Footer_on_Merged_HTML.html"));

                Master.CollapseAll();
                Master.ExpandNode("HTML_to_PDF");
                Master.ExpandNode("Headers_and_Footers");
                Master.SelectNode("Header_Footer_on_Merged_HTML");
            }
        }

        protected void demoMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "Live_Demo":
                    demoMultiView.SetActiveView(liveDemoView);
                    break;
                case "Description":
                    demoMultiView.SetActiveView(descriptionView);
                    break;
                case "Sample_Code":
                    demoMultiView.SetActiveView(sampleCodeView);
                    break;
                default:
                    break;
            }
        }
    }
}