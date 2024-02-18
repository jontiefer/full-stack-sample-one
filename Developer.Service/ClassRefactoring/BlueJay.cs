using System;
using System.Drawing;

namespace Developer.Service.ClassRefactoring;

public enum BlueJayGenus
{
    None, Coconut
}

public class BlueJay : Bird
{
    public BlueJay()
        :base(Color.Blue, Gender.None)
    {

    }

    public BlueJay(Gender gender)
        : base(Color.Blue, gender)
    {

    }

    public override void ApplyLoad(BirdLoad load)
    {
        base.ApplyLoad(load);

        CalculateAirspeedVelocity();
    }

    protected override void CalculateAirspeedVelocity()
    {
        if (Gender == Gender.Male)
        {
            AirspeedVelocity = Load switch
            {
                BirdLoad.None => 24,
                BirdLoad.Worm => 23,
                BirdLoad.Twigs => 21,
                BirdLoad.Coconut => 5,
                _ => throw new InvalidOperationException()
            };
        }
        else
        {
            AirspeedVelocity = Load switch
            {
                BirdLoad.None => 21,
                BirdLoad.Worm => 20,
                BirdLoad.Twigs => 19,
                BirdLoad.Coconut => 2,
                _ => throw new InvalidOperationException()
            };
        }
    }
}
