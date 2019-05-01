using System;

namespace SimpleIoC.CrossTests.Services
{
    public interface ICalc
    {
        int Add(int a, int b);

        int Value { get; set; }
    }
    public class CalcEngineer : ICalc
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Value { get; set; }

        public CalcEngineer()
        {
            Value = 2;
        }

        public bool Equals(CalcEngineer c)
        {
            return Value == c.Value;
        }
    }

    public class CalcSimple : ICalc
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Value { get; set; }

        public CalcSimple()
        {
            Value = 7;
        }

        public bool Equals(CalcSimple c)
        {
            return Value == c.Value;
        }
    }

    public class CalcProgrammer : ICalc
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Value { get; set; } = 1771;
    }

    public class CalcWithDependency : ICalc
    {
        public IBar Bar { get; set; }
        public CalcWithDependency(IBar dependency)
        {
            Bar = dependency;
        }
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Value { get; set; } = 811;
    }
}
