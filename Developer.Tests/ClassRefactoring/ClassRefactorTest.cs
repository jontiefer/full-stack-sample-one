using System.Drawing;
using Developer.Service.ClassRefactoring;

namespace Developer.Service.Tests.ClassRefactoring;

public class ClassRefactorTest
{
    [Theory]
    [InlineData(SwallowType.African, BirdLoad.None, "IndianRed", Gender.Male, 27)]
    [InlineData(SwallowType.African, BirdLoad.Twigs, "Orange", Gender.Male, 25)]
    [InlineData(SwallowType.African, BirdLoad.Coconut, "Indigo", Gender.Female, 17)]
    [InlineData(SwallowType.European, BirdLoad.None, "Yellow",  Gender.Male, 25)]
    [InlineData(SwallowType.European, BirdLoad.Worm, "Chocolate", Gender.Female, 21)]
    [InlineData(SwallowType.European, BirdLoad.Coconut, "Bisque", Gender.Female, 14)]
    public void CreateSwallow_With_SwallowFactory_HasCorrect_Speed_Color_Gender(
        SwallowType swallowType, BirdLoad load, string color, Gender gender, 
        double expectedVelocity)
    {
        // Arrange
        var swallowFactory = new SwallowFactory();


        // Act
        var swallow = (Bird)swallowFactory.CreateSwallow(swallowType, Color.FromName(color), gender);

        swallow.ApplyLoad(load);


        // Assert
        Assert.Equal(expectedVelocity, swallow.AirspeedVelocity);
    }

    [Theory]
    [InlineData(SwallowType.African, BirdLoad.None, "IndianRed", Gender.Male, 27)]
    [InlineData(SwallowType.African, BirdLoad.Twigs, "Orange", Gender.Male, 25)]
    [InlineData(SwallowType.African, BirdLoad.Coconut, "Indigo", Gender.Female, 17)]
    [InlineData(SwallowType.European, BirdLoad.None, "Yellow", Gender.Male, 25)]
    [InlineData(SwallowType.European, BirdLoad.Worm, "Chocolate", Gender.Female, 21)]
    [InlineData(SwallowType.European, BirdLoad.Coconut, "Bisque", Gender.Female, 14)]
    public void CreateSwallow_With_SwallowFactory_Generic_HasCorrect_Speed_Color_Gender(
        SwallowType swallowType, BirdLoad load, string color, Gender gender,
        double expectedVelocity)
    {
        // Arrange
        var swallowFactory = new SwallowFactory();


        // Act
        var swallow = (Bird)(swallowType == SwallowType.African
            ? swallowFactory.CreateSwallow<AfricanSwallow>(Color.FromName(color), gender)
            : swallowFactory.CreateSwallow<EuropeanSwallow>(Color.FromName(color), gender));

        swallow.ApplyLoad(load);


        // Assert
        Assert.Equal(expectedVelocity, swallow.AirspeedVelocity);
    }

    [Theory]
    [InlineData(BirdSpecies.AfricanSwallow, BirdLoad.None, "IndianRed", Gender.Male, 27)]
    [InlineData(BirdSpecies.AfricanSwallow, BirdLoad.Twigs, "Orange", Gender.Male, 25)]
    [InlineData(BirdSpecies.AfricanSwallow, BirdLoad.Coconut, "Indigo", Gender.Female, 17)]
    [InlineData(BirdSpecies.EuropeanSwallow, BirdLoad.None, "Yellow", Gender.Male, 25)]
    [InlineData(BirdSpecies.EuropeanSwallow, BirdLoad.Worm, "Chocolate", Gender.Female, 21)]
    [InlineData(BirdSpecies.EuropeanSwallow, BirdLoad.Coconut, "Bisque", Gender.Female, 14)]
    [InlineData(BirdSpecies.BlueJay, BirdLoad.None, null, Gender.Male, 24)]
    [InlineData(BirdSpecies.BlueJay, BirdLoad.Twigs, null, Gender.Male, 21)]
    [InlineData(BirdSpecies.BlueJay, BirdLoad.Coconut, null, Gender.Female, 2)]
    public void CreateBird_With_BirdAbstractFactory_HasCorrect_Speed_Color_Gender(
        BirdSpecies species, BirdLoad load, string? color, Gender gender,
        double expectedVelocity)
    {
        // Arrange
        var birdAbstractFactory = new BirdAbstractFactory();

        
        // Act
        var bird = birdAbstractFactory.CreateBird(
            species, gender, color != null ? Color.FromName(color) : null);

        bird.ApplyLoad(load);


        // Assert
        Assert.Equal(expectedVelocity, bird.AirspeedVelocity);
    }
}