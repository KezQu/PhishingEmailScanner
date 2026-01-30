using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace PhishingEmailScanner.Tests
{
    [TestClass]
    public class MailItemAdapterTests
    {
        [TestMethod]
        public void Subject_ReturnsMailSubject()
        {
            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.Subject).Returns("Test Subject");
            var sut = new MailItemAdapter(mock_mail.Object);

            Assert.AreEqual("Test Subject", sut.Subject);
        }

        [TestMethod]
        public void Body_ReturnsMailBody()
        {
            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.Body).Returns("Test Body");
            var sut = new MailItemAdapter(mock_mail.Object);

            Assert.AreEqual("Test Body", sut.Body);
        }

        [TestMethod]
        public void SenderEmail_ReturnsSenderEmail()
        {
            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.SenderEmailAddress).Returns("sender@example.com");
            var sut = new MailItemAdapter(mock_mail.Object);

            Assert.AreEqual("sender@example.com", sut.SenderEmail);
        }

        [TestMethod]
        public void Links_ExtractsUrlsFromHtmlBody()
        {
            var html = "Click <a href=\"https://example.com\">here</a> and visit https://test.com";
            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.HTMLBody).Returns(html);
            var sut = new MailItemAdapter(mock_mail.Object);

            var links = sut.Links;
            CollectionAssert.Contains(links, "https://example.com");
            CollectionAssert.Contains(links, "https://test.com");
        }

        [TestMethod]
        public void Links_NormalizesUrls()
        {
            var html = "Visit https://ex\uFF41mple.com for more info.";
            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.HTMLBody).Returns(html);
            var sut = new MailItemAdapter(mock_mail.Object);

            var links = sut.Links;

            Assert.IsTrue(links.Any(link => link == "https://example.com"));
        }

        [TestMethod]
        public void Attachments_ReturnsAttachmentFileNames()
        {
            var mock_attachments = new Mock<Attachments>();
            var mock_attachment1 = new Mock<Attachment>();
            var mock_attachment2 = new Mock<Attachment>();
            mock_attachment1.Setup(a => a.FileName).Returns("file1.pdf");
            mock_attachment2.Setup(a => a.FileName).Returns("file2.docx");

            var attachmentsList = new List<Attachment> { mock_attachment1.Object, mock_attachment2.Object };
            mock_attachments.Setup(a => a.GetEnumerator()).Returns(attachmentsList.GetEnumerator());

            var mock_mail = new Mock<MailItem>();
            mock_mail.Setup(m => m.Attachments).Returns(mock_attachments.Object);

            var sut = new MailItemAdapter(mock_mail.Object);
            var files = sut.Attachments;

            CollectionAssert.Contains(files, "file1.pdf");
            CollectionAssert.Contains(files, "file2.docx");
        }

        [TestMethod]
        public void Headers_ReturnsHeaders_WhenPropertyExists()
        {
            var mock_mail = new Mock<MailItem>();
            var mock_accessor = new Mock<PropertyAccessor>();
            mock_accessor.Setup(a => a.GetProperty(It.IsAny<string>())).Returns("header-content");
            mock_mail.Setup(m => m.PropertyAccessor).Returns(mock_accessor.Object);

            var sut = new MailItemAdapter(mock_mail.Object);
            Assert.AreEqual("header-content", sut.Headers);
        }

        [TestMethod]
        public void Headers_ReturnsEmptyString_WhenPropertyAccessorThrows()
        {
            var mock_mail = new Mock<MailItem>();
            var mock_accessor = new Mock<PropertyAccessor>();
            mock_accessor.Setup(a => a.GetProperty(It.IsAny<string>())).Throws(new System.Exception());
            mock_mail.Setup(m => m.PropertyAccessor).Returns(mock_accessor.Object);

            var sut = new MailItemAdapter(mock_mail.Object);
            Assert.AreEqual(string.Empty, sut.Headers);
        }
    }
}