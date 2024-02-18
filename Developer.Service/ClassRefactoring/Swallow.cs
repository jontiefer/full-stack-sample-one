using System;
using System.Drawing;

namespace Developer.Service.ClassRefactoring
{
    public class AfricanSwallow : Bird
    {
        public AfricanSwallow()
        {
        }

        public AfricanSwallow(Color color, Gender gender)
            :base(color, gender)
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
                    BirdLoad.None => 27,
                    BirdLoad.Worm => 26,
                    BirdLoad.Twigs => 25,
                    BirdLoad.Coconut => 21,
                    _ => throw new InvalidOperationException()
                };
            }
            else
            {
                AirspeedVelocity = Load switch
                {
                    BirdLoad.None => 24,
                    BirdLoad.Worm => 23,
                    BirdLoad.Twigs => 22,
                    BirdLoad.Coconut => 17,
                    _ => throw new InvalidOperationException()
                };
            }
        }
    }

    public class EuropeanSwallow : Bird
    {
        public EuropeanSwallow()
        {

        }

        public EuropeanSwallow(Color color, Gender gender)
            : base(color, gender)
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
                    BirdLoad.None => 25,
                    BirdLoad.Worm => 24,
                    BirdLoad.Twigs => 23,
                    BirdLoad.Coconut => 19,
                    _ => throw new InvalidOperationException()
                };
            }
            else
            {
                AirspeedVelocity = Load switch
                {
                    BirdLoad.None => 22,
                    BirdLoad.Worm => 21,
                    BirdLoad.Twigs => 20,
                    BirdLoad.Coconut => 14,
                    _ => throw new InvalidOperationException()
                };
            }
        }
    }
}