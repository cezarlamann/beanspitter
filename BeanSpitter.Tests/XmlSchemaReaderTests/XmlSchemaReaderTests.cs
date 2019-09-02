namespace BeanSpitter.Tests.XmlSchemaReaderTests
{
    using BeanSpitter.Interfaces;
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO.Abstractions;
    using System.Xml;

    [TestClass]
    public class XmlSchemaReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes = true)]
        public void WhenFileSystemThrowsExceptionTheReaderMustThrowInvalidOperationException()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMsf = A.Fake<IMemoryStreamFactory>();

            A.CallTo(() => fakeFs.File.Exists(string.Empty)).Returns(true);
            A.CallTo(() => fakeFs.FileStream.Create(string.Empty, System.IO.FileMode.Open)).Throws<Exception>();

            var readerclass = new XmlSchemaReader(fakeFs, fakeMsf);

            readerclass.ReadFromPath(string.Empty);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes = true)]
        public void WhenMemoryStreamFactoryThrowsExceptionTheReaderMustThrowInvalidOperationException()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMemoryFactory = A.Fake<IMemoryStreamFactory>();
            var array = new byte[1] { Convert.ToByte(true) };

            A.CallTo(() => fakeMemoryFactory.Create(array, false)).WithAnyArguments().Throws<Exception>();

            var readerclass = new XmlSchemaReader(fakeFs, fakeMemoryFactory);

            readerclass.ReadFromByteArray(array);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenReadFromStreamMethodIsCalledWithEmptyStreamTheReaderMustThrowArgumentException()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMsf = A.Fake<IMemoryStreamFactory>();

            var fakeStream = A.Fake<System.IO.MemoryStream>();

            A.CallTo(() => fakeStream.CanRead).Returns(true);
            A.CallTo(() => fakeStream.Length).Returns(0);

            var readerclass = new XmlSchemaReader(fakeFs, fakeMsf);

            readerclass.ReadFromStream(fakeStream);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenReadFromStreamMethodIsCalledWithNonReadableStreamTheReaderMustThrowArgumentException()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMsf = A.Fake<IMemoryStreamFactory>();

            var fakeStream = A.Fake<System.IO.MemoryStream>();

            A.CallTo(() => fakeStream.CanRead).Returns(false);

            var readerclass = new XmlSchemaReader(fakeFs, fakeMsf);

            readerclass.ReadFromStream(fakeStream);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenUsingEmptyByteArrayTheReaderMustThrowArgumentException()
        {
            var readerclass = new XmlSchemaReader(new FileSystem(), new MemoryStreamFactory());
            var array = new byte[0] { };

            readerclass.ReadFromByteArray(array);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenUsingInvalidPathTheReaderMustThrowArgumentException()
        {
            var readerclass = new XmlSchemaReader(new FileSystem(), new MemoryStreamFactory());

            readerclass.ReadFromPath(string.Empty);

            Assert.Fail("Should not reach this point.");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
        public void WhenUsingNullByteArrayTheReaderMustThrowArgumentNullException()
        {
            var readerclass = new XmlSchemaReader(new FileSystem(), new MemoryStreamFactory());
            const byte[] array = null;

            readerclass.ReadFromByteArray(array);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WhenUsingNullPathTheReaderMustThrowArgumentException()
        {
            var readerclass = new XmlSchemaReader(new FileSystem(), new MemoryStreamFactory());

            readerclass.ReadFromPath(null);

            Assert.Fail("Should not reach this point.");
        }

        [TestMethod]
        public void WhenReadFromStreamMethodIsCalledWithUnpositionedStreamTheReaderMustCallStreamSeekMethod()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMsf = A.Fake<IMemoryStreamFactory>();

            var fakeStream = A.Fake<System.IO.MemoryStream>();

            A.CallTo(() => fakeStream.CanRead).Returns(true);
            A.CallTo(() => fakeStream.Length).Returns(1);
            A.CallTo(() => fakeStream.Position).Returns(1);
            A.CallTo(() => fakeStream.Seek(0, System.IO.SeekOrigin.Begin)).Returns(0);

            var readerclass = new XmlSchemaReader(fakeFs, fakeMsf);

            try
            {
                readerclass.ReadFromStream(fakeStream);
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch (Exception)
            {
                // do nothing
            }
#pragma warning restore CC0004 // Catch block cannot be empty

            A.CallTo(() => fakeStream.Seek(0, System.IO.SeekOrigin.Begin)).MustHaveHappened();
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException), AllowDerivedTypes = true)]
        public void WhenReadFromStreamMethodIsCalledWithNonXmlStreamTheReaderMustThrowXmlException()
        {
            var fakeFs = A.Fake<IFileSystem>();
            var fakeMsf = A.Fake<IMemoryStreamFactory>();

            var fakeStream = A.Fake<System.IO.MemoryStream>();

            A.CallTo(() => fakeStream.CanRead).Returns(true);
            A.CallTo(() => fakeStream.Length).Returns(1);
            A.CallTo(() => fakeStream.Position).Returns(0);

            var readerclass = new XmlSchemaReader(fakeFs, fakeMsf);

            readerclass.ReadFromStream(fakeStream);

            Assert.Fail("Should not reach this point.");
        }
    }
}
