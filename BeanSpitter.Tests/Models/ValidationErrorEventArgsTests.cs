namespace BeanSpitter.Tests.Models
{
    using BeanSpitter.Models;
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Xml.Schema;

    [TestClass]
    public class ValidationErrorEventArgsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenCtorIsCalledWithNullExceptionMustThrowArgumentNullException()
        {
            new ValidationErrorEventArgs(ex: null);
        }

        [TestMethod]
        public void WhenCtorIsCalledWithValidExceptionAndWithoutSeverityMustSetSeverityToError()
        {
            var res = new ValidationErrorEventArgs(new XmlSchemaException());
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Severity == XmlSeverityType.Error);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenCtorIsCalledWithValidSeverityNullExceptionMustThrowArgumentNullException()
        {
            new ValidationErrorEventArgs(ex: null, XmlSeverityType.Error);
        }

        [TestMethod]
        public void WhenCtorIsCalledWithValidExceptionAndSeverityMustNotThrowAnyExceptions()
        {
            const string testMessage = "testMessage";
            var res = new ValidationErrorEventArgs(new XmlSchemaException(testMessage), XmlSeverityType.Warning);
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Severity == XmlSeverityType.Warning);
            Assert.IsNotNull(res.Exception);
            Assert.IsTrue(res.Exception.Message == testMessage);
        }

        [TestMethod]
        public void EmptyExceptionMessageTest()
        {
            var fakeException = A.Fake<XmlSchemaException>();
            A.CallTo(() => fakeException.Message).Returns(string.Empty);

            var res = new ValidationErrorEventArgs(fakeException, XmlSeverityType.Warning);
            Assert.IsTrue(res.Message == string.Empty);
        }

        [TestMethod]
        public void NullExceptionMessageTest()
        {
            var fakeException = A.Fake<XmlSchemaException>();
            A.CallTo(() => fakeException.Message).Returns(null);

            var res = new ValidationErrorEventArgs(fakeException, XmlSeverityType.Warning);
            Assert.IsTrue(res.Message == string.Empty);
        }

        [TestMethod]
        public void GenericMessageExceptionTest()
        {
            var fakeException = A.Fake<XmlSchemaException>();
            A.CallTo(() => fakeException.Message).Returns("A");

            var res = new ValidationErrorEventArgs(fakeException, XmlSeverityType.Warning);
            Assert.IsTrue(res.Message == "A");
        }
    }
}
