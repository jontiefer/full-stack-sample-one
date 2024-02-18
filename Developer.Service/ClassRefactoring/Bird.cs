using System.Collections.Generic;
using System.Drawing;

namespace Developer.Service.ClassRefactoring;

public abstract class Bird : IBird
{
    public Color Color { get; set; } = Color.White;

    public Gender Gender { get; set; } = Gender.None;

    public BirdState State { get; protected set; } = BirdState.Nest;

    public IList<IBird> Flock { get; } = new List<IBird>();

    protected BirdLoad Load { get; set; }

    protected Bird()
    {

    }

    protected Bird(Color color, Gender gender)
    {
        Color = color;
        Gender = gender;
    }

    public double AirspeedVelocity { get; protected set; }

    public virtual void ApplyLoad(BirdLoad load) => Load = load;
    
    public virtual void SetState(BirdState state) => State = state;
    
    protected abstract void CalculateAirspeedVelocity();
}
