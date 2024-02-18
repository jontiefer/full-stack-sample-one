using System.Drawing;
using Developer.Service.ClassRefactoring;
using Developer.Service.Containers;

namespace Developer.Service.Tests.Containers;

//internal interface IContainerTestInterface
//{
//}

//internal class ContainerTestClass : IContainerTestInterface
//{
//}

public class ContainerTest
{
    [Theory]
    [InlineData(1, ServiceLifetime.Singleton)]
    [InlineData(2, ServiceLifetime.Singleton)]
    [InlineData(1, ServiceLifetime.Transient)]
    [InlineData(2, ServiceLifetime.Transient)]
    public void BindAndGet_OneBinding_Service_With_ValidInterfaceAndImplementationType_Success(
        int testCase, ServiceLifetime lifetime)
    {
        // Arrange
        var container = new Container();

        if (testCase == 1)
            container.Bind<IBird, AfricanSwallow>(lifetime);
        else if (testCase == 2)
            container.Bind<IBird, EuropeanSwallow>(lifetime);
        else
            throw new NotImplementedException();


        // Act
        var swallowInstanceA = container.GetService<IBird>();
        var swallowInstanceB = container.GetService<IBird>();


        // Assert
        Assert.IsType(testCase == 1 ? typeof(AfricanSwallow) : typeof(EuropeanSwallow), swallowInstanceA);

        if(lifetime ==  ServiceLifetime.Singleton)
            Assert.Same(swallowInstanceA, swallowInstanceB);
        else
            Assert.NotSame(swallowInstanceA, swallowInstanceB);
    }

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Transient)]
    public void BindAndGet_MultipleBindings_Service_With_ValidInterfaceAndImplementationType_Success(ServiceLifetime lifetime)
    {
        // Arrange
        var container = new Container();

        container.Bind<IBird, AfricanSwallow>(lifetime);
        container.Bind<IBird, EuropeanSwallow>(lifetime);
        container.Bind<IBird, BlueJay>(lifetime);


        // Act
        var birdInstancesA = container.GetServices<IBird>().ToList();
        var birdInstancesB = container.GetServices<IBird>().ToList();


        // Assert
        Assert.Equal(birdInstancesA.Count, birdInstancesB.Count);

        for (var index = 0; index < birdInstancesA.Count; index++)
        {
            if(lifetime == ServiceLifetime.Singleton)
                Assert.Same(birdInstancesA[index], birdInstancesB[index]);
            else
                Assert.NotSame(birdInstancesA[index], birdInstancesB[index]);
        }

        Assert.Equal(
            birdInstancesA.Select(b => b.GetType()).ToArray(),
            new[] { typeof(AfricanSwallow), typeof(EuropeanSwallow), typeof(BlueJay) });
    }

    [Theory]
    [InlineData(1, ServiceLifetime.Singleton)]
    [InlineData(2, ServiceLifetime.Singleton)]
    [InlineData(1, ServiceLifetime.Transient)]
    [InlineData(2, ServiceLifetime.Transient)]
    public void BindAndGet_OneBinding_Service_With_ValidInterfaceAndImplementationFactory_Success(
        int testCase, ServiceLifetime lifetime)
    {
        // Arrange
        var container = new Container();

        if (testCase == 1)
            container.Bind<IBird>(() => new AfricanSwallow(Color.Orange, Gender.Female), lifetime);
        else if (testCase == 2)
            container.Bind<IBird>(() => new EuropeanSwallow(Color.Purple, Gender.Male), lifetime);
        else
            throw new NotImplementedException();


        // Act
        var swallowInstanceA = container.GetService<IBird>();
        var swallowInstanceB = container.GetService<IBird>();


        // Assert
        Assert.IsType(testCase == 1 ? typeof(AfricanSwallow) : typeof(EuropeanSwallow), swallowInstanceA);

        if(lifetime ==  ServiceLifetime.Singleton)
            Assert.Same(swallowInstanceA, swallowInstanceB);
        else
            Assert.NotSame(swallowInstanceA, swallowInstanceB);

        Assert.Equal(swallowInstanceA.Color, testCase == 1 ? Color.Orange : Color.Purple);

        Assert.Equal(swallowInstanceA.Gender, testCase == 1 ? Gender.Female : Gender.Male);
    }

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Transient)]
    public void BindAndGet_MultiBindings_Service_With_ValidInterfaceAndImplementationFactory_Success(
        ServiceLifetime lifetime)
    {
        // Arrange
        var container = new Container();

        container.Bind<IBird>(() => new AfricanSwallow(Color.Orange, Gender.Female), lifetime);
        container.Bind<IBird>(() => new EuropeanSwallow(Color.Purple, Gender.Male), lifetime);
        container.Bind<IBird>(() => new BlueJay(Gender.Male), lifetime);


        // Act
        var birdInstancesA = container.GetServices<IBird>().ToList();
        var birdInstancesB = container.GetServices<IBird>().ToList();


        // Assert
        Assert.Equal(birdInstancesA.Count, birdInstancesB.Count);

        for (var index = 0; index < birdInstancesA.Count; index++)
        {
            if (lifetime == ServiceLifetime.Singleton)
                Assert.Same(birdInstancesA[index], birdInstancesB[index]);
            else
                Assert.NotSame(birdInstancesA[index], birdInstancesB[index]);
        }

        // 1st Binding
        Assert.Equal(birdInstancesA[0].GetType(), typeof(AfricanSwallow));
        Assert.Equal(birdInstancesA[0].Color, Color.Orange);
        Assert.Equal(birdInstancesA[0].Gender, Gender.Female);

        // 2nd Binding
        Assert.Equal(birdInstancesA[1].GetType(), typeof(EuropeanSwallow));
        Assert.Equal(birdInstancesA[1].Color, Color.Purple);
        Assert.Equal(birdInstancesA[1].Gender, Gender.Male);

        // 3rd Binding
        Assert.Equal(birdInstancesA[2].GetType(), typeof(BlueJay));
        Assert.Equal(birdInstancesA[2].Color, Color.Blue);
        Assert.Equal(birdInstancesA[2].Gender, Gender.Male);
    }
}