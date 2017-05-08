using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task1.ClassMapper;

namespace Module1.ExpressoonsAndIqueryable.Tests
{
    [TestClass]
    public class Task1ClassMapperTests
    {
        [TestMethod]
        public void CreateMapperSourceToDestination()
        {
            // Arrange
            SourceClass sClass = new SourceClass("SourceClassTestStr", 21, new RefTypeProp());
            MappingGenerator mappingGenerator = new MappingGenerator();
            Mapper<SourceClass, DestinationClass> mapper = mappingGenerator.Generate<SourceClass, DestinationClass>();

            // Act
            DestinationClass dClassResult = mapper.Map(sClass);

            // Assert
            Assert.IsNotNull(dClassResult);
            Assert.AreEqual(sClass.FirstProp, dClassResult.FirstProp);
            Assert.AreNotEqual(sClass.SecondProp, dClassResult.SecondProp);
            Assert.AreEqual(sClass.ThirdProp, dClassResult.ThirdProp);
        }

        [TestMethod]
        public void CreateMapperDestinationToSource()
        {
            // Arrange
            DestinationClass dClass = new DestinationClass("DestinationClassTestStr", true, new RefTypeProp());
            MappingGenerator mappingGenerator = new MappingGenerator();
            Mapper<DestinationClass,SourceClass > mapper = mappingGenerator.Generate<DestinationClass, SourceClass>();

            // Act
            SourceClass sClassResult = mapper.Map(dClass);
            
            // Assert
            Assert.IsNotNull(sClassResult);
            Assert.AreEqual(dClass.FirstProp, sClassResult.FirstProp);
            Assert.AreNotEqual(dClass.SecondProp, sClassResult.SecondProp);
            Assert.AreEqual(dClass.ThirdProp, sClassResult.ThirdProp);
        }

        private class SourceClass
        {
            public SourceClass()
            {
            }

            public SourceClass(string firstProp, int secondProp, RefTypeProp thirdProp)
            {
                FirstProp = firstProp;
                SecondProp = secondProp;
                ThirdProp = thirdProp;
            }

            public string FirstProp { get; set; }
            public int SecondProp { get; set; }
            public RefTypeProp ThirdProp { get; set; }
        }

        private class DestinationClass
        {
            public DestinationClass()
            {

            }

            public DestinationClass(string firstProp, bool secondProp, RefTypeProp thirdProp)
            {
                FirstProp = firstProp;
                SecondProp = secondProp;
                ThirdProp = thirdProp;
            }

            public string FirstProp { get; set; }
            public bool SecondProp { get; set; }
            public RefTypeProp ThirdProp { get; set; }
        }

        private class RefTypeProp
        {
        }
    }
}
