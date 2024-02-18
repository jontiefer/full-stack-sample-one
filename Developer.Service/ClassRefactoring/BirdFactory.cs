using System;
using System.Drawing;

namespace Developer.Service.ClassRefactoring;

public enum BirdSpecies
{
    AfricanSwallow, EuropeanSwallow, BlueJay
}

public enum SwallowType
{
    African, European
}

public class BirdAbstractFactory
{
    public Bird CreateBird(BirdSpecies species, Gender gender, Color? color) =>
        species switch
        {
            BirdSpecies.AfricanSwallow => new AfricanSwallow(color ?? Color.Beige, gender),
            BirdSpecies.EuropeanSwallow => new EuropeanSwallow(color ?? Color.Bisque, gender),
            BirdSpecies.BlueJay => new BlueJay(gender),
            _ => throw new InvalidOperationException("Invalid bird type.")
        };
}

public class SwallowFactory
{
    public TSwallow CreateSwallow<TSwallow>(Color color, Gender gender)
        where TSwallow: Bird =>
        typeof(TSwallow).Name switch
        {
            nameof(AfricanSwallow) => (new AfricanSwallow(color, gender) as TSwallow)!,
            nameof(EuropeanSwallow) => (new EuropeanSwallow(color, gender) as TSwallow)!,
            _ => throw new InvalidOperationException("Invalid swallow bird type.")
        };

    public object CreateSwallow(SwallowType swallowType, Color color, Gender gender) =>
        swallowType switch
        {
            SwallowType.African => new AfricanSwallow(color, gender),
            SwallowType.European => new EuropeanSwallow(color, gender),
            _ => throw new InvalidOperationException("Invalid swallow bird type.")
        };
}


