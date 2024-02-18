using System.Collections.Generic;
using System.Drawing;

namespace Developer.Service.ClassRefactoring;

public enum Gender
{
    None, Male, Female
}

public enum BirdState
{
    Nest, Flying, Walking, Chirping
}

public enum BirdLoad
{
    None, Twigs, Worm, Coconut
}

public interface IBird
{
    Color Color { get; }

    Gender Gender { get; }

    BirdState State { get; }    

    IList<IBird> Flock { get; }

    double AirspeedVelocity { get; }

    void ApplyLoad(BirdLoad load);

    void SetState(BirdState state);
}
