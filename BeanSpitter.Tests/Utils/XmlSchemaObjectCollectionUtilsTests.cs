namespace BeanSpitter.Tests.Utils
{
    using BeanSpitter.Utils;
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using static BeanSpitter.Utils.XmlSchemaObjectCollectionUtils;

    [TestClass]
    public class XmlSchemaObjectColletionUtilsTests
    {
        #region Variable_Declarations
        XmlSchemaElement xmlElementWithoutName;
        XmlSchemaElement xmlElementWithoutSchemaTypeName;
        XmlSchemaElement xmlElementWithNamelessSchemaTypeName;
        XmlSchemaElement validXmlElement;

        XmlSchemaComplexType xmlTypeWithNullParticle;
        XmlSchemaComplexType xmlTypeWithWrongParticleType;
        XmlSchemaComplexType xmlTypeNullItemsFromParticle;
        XmlSchemaComplexType xmlTypeEmptyItemsFromParticle;
        XmlSchemaComplexType validXmlType;

        #endregion

        #region Test_Setup
        [TestInitialize]
        public void Setup()
        {
            xmlElementWithoutName = new XmlSchemaElement { Name = string.Empty, SchemaTypeName = new XmlQualifiedName("TestSchemaType") };
            xmlElementWithoutSchemaTypeName = new XmlSchemaElement { Name = "TestElementName", SchemaTypeName = null };
            xmlElementWithNamelessSchemaTypeName = new XmlSchemaElement { Name = "TestElementName", SchemaTypeName = new XmlQualifiedName(string.Empty) };
            validXmlElement = new XmlSchemaElement { Name = "TestElementName", SchemaTypeName = new XmlQualifiedName("TestSchemaType") };

            xmlTypeWithNullParticle = new XmlSchemaComplexType { Particle = null };
            xmlTypeWithWrongParticleType = new XmlSchemaComplexType { Particle = new XmlSchemaAny() };
            xmlTypeEmptyItemsFromParticle = new XmlSchemaComplexType { Particle = new XmlSchemaChoice() };

            var fakeParticleWithValidItems = A.Fake<XmlSchemaChoice>();
            A.CallTo(() => fakeParticleWithValidItems.Items).Returns(new XmlSchemaObjectCollection { validXmlElement });

            var fakeParticleWithNullItemsProperty = A.Fake<XmlSchemaChoice>();
            A.CallTo(() => fakeParticleWithNullItemsProperty.Items).Returns(null);

            validXmlType = new XmlSchemaComplexType { Particle = fakeParticleWithValidItems };

            xmlTypeNullItemsFromParticle = new XmlSchemaComplexType { Particle = fakeParticleWithNullItemsProperty };
        }
        #endregion

        [TestMethod]
        public void WhenMethodGetTagsByTypeIsCalledWithNullSchemaObjectsCollectionMustReturnEmptyDictionary()
        {
            const ICollection<XmlSchemaObject> schemaObjects = null;
            var res = schemaObjects.GetTagsByType();

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void WhenMethodGetTagsByTypeIsCalledWithEmptySchemaObjectsCollectionMustReturnEmptyDictionary()
        {
            ICollection<XmlSchemaObject> schemaObjects = new List<XmlSchemaObject>();
            var res = schemaObjects.GetTagsByType();

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void WhenXmlSchemaElementValidationIsCalledWithNullElementMustReturnFalse()
        {
            var res = ThisXmlSchemaElementIsValid(null);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaElementValidationIsCalledWithElementWithoutNameMustReturnFalse()
        {
            var res = ThisXmlSchemaElementIsValid(xmlElementWithoutName);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaElementValidationIsCalledWithElementWithoutSchemaTypeNameMustReturnFalse()
        {
            var res = ThisXmlSchemaElementIsValid(xmlElementWithoutSchemaTypeName);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaElementValidationIsCalledWithElementNamelessSchemaTypeNameMustReturnFalse()
        {
            var res = ThisXmlSchemaElementIsValid(xmlElementWithNamelessSchemaTypeName);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenGetTagsByTypeMethodIsCalledWithInvalidElementCollectionMustReturnEmptyDictionary()
        {
            var schemaObjects = new List<XmlSchemaObject>();

            schemaObjects.Add(xmlElementWithoutName);
            schemaObjects.Add(xmlElementWithoutSchemaTypeName);
            schemaObjects.Add(xmlElementWithNamelessSchemaTypeName);

            var result = schemaObjects.GetTagsByType();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void WhenGetTagsByTypeMethodIsCalledWithOneValidXmlElementMustReturnDictionaryContainingThisElement()
        {
            var schemaObjects = new List<XmlSchemaObject>();

            schemaObjects.Add(validXmlElement);

            var result = schemaObjects.GetTagsByType();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.ContainsKey(validXmlElement.SchemaTypeName.Name));
            Assert.AreEqual(validXmlElement.Name, result[validXmlElement.SchemaTypeName.Name].FirstOrDefault());
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithNullTypeMustReturnFalse()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(null);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithTypeContainingNullParticleMustReturnFalse()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(xmlTypeWithNullParticle);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithTypeWithWrongParticleTypeMustReturnFalse()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(xmlTypeWithWrongParticleType);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithTypeWithNullParticleItemsTypeMustReturnFalse()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(xmlTypeNullItemsFromParticle);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithTypeWithEmptyParticleItemsTypeMustReturnFalse()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(xmlTypeEmptyItemsFromParticle);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void WhenXmlSchemaTypeValidationMethodIsCalledWithTypeWithValidParticleItemsTypeMustReturnTrue()
        {
            var res = ThisXmlSchemaComplexTypeIsValid(validXmlType);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void WhenGetTagsByTypeMethodIsCalledWithOneValidXmlTypeWithOneElementMustReturnDictionaryContainingThisElement()
        {
            var schemaObjects = new List<XmlSchemaObject>();

            schemaObjects.Add(validXmlType);

            var result = schemaObjects.GetTagsByType();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.ContainsKey(validXmlElement.SchemaTypeName.Name));
            Assert.AreEqual(validXmlElement.Name, result[validXmlElement.SchemaTypeName.Name].FirstOrDefault());
        }
    }
}
