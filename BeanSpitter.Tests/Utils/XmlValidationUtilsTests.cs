namespace BeanSpitter.Tests.Utils
{
    using BeanSpitter.Interfaces;
    using BeanSpitter.Utils;
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO.Abstractions;
    using System.Xml.Schema;

    [TestClass]
    public class XmlValidationUtilsTests
    {
        private IFileSystem fakeFileSystem;
        private IXmlValidationUtils xmlValidationUtils;

        private const string testMessage = "<test message>";
        private const string path = "A";

        [TestInitialize]
        public void Setup()
        {
            fakeFileSystem = A.Fake<IFileSystem>();
            xmlValidationUtils = new XmlValidationUtils();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenPathValidationMethodIsCalledWithEmptyPathMustThrowArgumentNullException()
        {
            xmlValidationUtils.ValidateFilePath(string.Empty, fakeFileSystem);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenPathValidationMethodIsCalledWithNullPathMustThrowArgumentNullException()
        {
            xmlValidationUtils.ValidateFilePath(null, fakeFileSystem);
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithMessageAndEmptyPathMustThrowArgumentNullException()
        {
            try
            {
                xmlValidationUtils.ValidateFilePath(string.Empty, fakeFileSystem, testMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(testMessage));
            }
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithMessageAndNullPathMustThrowArgumentNullException()
        {
            try
            {
                xmlValidationUtils.ValidateFilePath(null, fakeFileSystem, testMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(testMessage));
            }
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithNoMessageAndEmptyPathMustThrowArgumentNullExceptionWithDefaultMessage()
        {
            try
            {
                xmlValidationUtils.ValidateFilePath(string.Empty, fakeFileSystem);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.emptyPathMsg));
            }
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithNoMessageAndNullPathMustThrowArgumentNullExceptionWithDefaultMessage()
        {
            try
            {
                xmlValidationUtils.ValidateFilePath(null, fakeFileSystem);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.emptyPathMsg));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenPathValidationMethodIsCalledWithInexistantPathMustThrowArgumentNullException()
        {
            A.CallTo(() => fakeFileSystem.File.Exists(path)).WithAnyArguments().Returns(false);
            xmlValidationUtils.ValidateFilePath(path, fakeFileSystem);
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithCustomMessageInexistantPathMustThrowArgumentNullExceptionWithCustomMessage()
        {

            A.CallTo(() => fakeFileSystem.File.Exists(path)).WithAnyArguments().Returns(false);
            try
            {
                xmlValidationUtils.ValidateFilePath(path, fakeFileSystem, testMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(testMessage));
            }
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledWithNoMessageAndInexistantPathMustThrowArgumentNullExceptionWithDefaultMessage()
        {
            A.CallTo(() => fakeFileSystem.File.Exists(path)).WithAnyArguments().Returns(false);
            try
            {
                xmlValidationUtils.ValidateFilePath(path, fakeFileSystem);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.nonExistantFilePathMsg));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void WhenPathValidationMethodIsCalledAndFileOpenThrowsExceptionItMustThrowException()
        {
            A.CallTo(() => fakeFileSystem.File.Exists(path)).WithAnyArguments().Returns(true);
            A.CallTo(() => fakeFileSystem.File.OpenRead(path)).WithAnyArguments().Throws<Exception>();
            xmlValidationUtils.ValidateFilePath(path, fakeFileSystem);
        }

        [TestMethod]
        public void WhenPathValidationMethodIsCalledAndFileOpenThrowsExceptionItMustThrowExceptionWithFormattedMessage()
        {
            A.CallTo(() => fakeFileSystem.File.Exists(path)).WithAnyArguments().Returns(true);
            A.CallTo(() => fakeFileSystem.File.OpenRead(path)).WithAnyArguments().Throws<Exception>();
            try
            {
                xmlValidationUtils.ValidateFilePath(path, fakeFileSystem);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.fileCannotBeReadMsg));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenSchemaSetValidatorIsCalledWithNullSchemaSetMustThrowArgumentNullException()
        {
            xmlValidationUtils.ValidateXmlSchemaSet(null);
        }

        [TestMethod]
        public void WhenSchemaSetValidatorIsCalledWithCustomMessageAndNullSchemaSetMustThrowArgumentNullExceptionWithCustomMessage()
        {
            try
            {
                xmlValidationUtils.ValidateXmlSchemaSet(null, testMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(testMessage));
            }
        }

        [TestMethod]
        public void WhenSchemaSetValidatorIsCalledWithNoMessageAndNullSchemaSetMustThrowArgumentNullExceptionWithDefaultMessage()
        {
            try
            {
                xmlValidationUtils.ValidateXmlSchemaSet(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.schemaNullMsg));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenSchemaSetValidatorIsCalledWithEmptySchemaSetMustThrowArgumentException()
        {
            xmlValidationUtils.ValidateXmlSchemaSet(new XmlSchemaSet());
        }

        [TestMethod]
        public void WhenSchemaSetValidatorIsCalledWithCustomMessageAndEmptySchemaSetMustThrowArgumentExceptionWithCustomMessage()
        {
            try
            {
                xmlValidationUtils.ValidateXmlSchemaSet(new XmlSchemaSet(), testMessage);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(testMessage));
            }
        }

        [TestMethod]
        public void WhenSchemaSetValidatorIsCalledWithNoMessageAndEmptySchemaSetMustThrowArgumentExceptionWithDefaultMessage()
        {
            try
            {
                xmlValidationUtils.ValidateXmlSchemaSet(new XmlSchemaSet());
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains(XmlValidationUtils.schemaEmptyMsg));
            }
        }
    }
}
