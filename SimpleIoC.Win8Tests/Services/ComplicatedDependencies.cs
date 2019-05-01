namespace SimpleIoC.Win8Tests.Services
{
    public interface IFoo
    {
        string FooMethod();

        string FooString { get; set; }
    }

    public class Foo : IFoo
    {
        public Foo()
        {
            FooString = "Foo String";
        }

        public string FooMethod()
        {
            return "Foo Method Invoked";
        }

        public string FooString { get; set; }

        public bool Equals(Foo f)
        {
            return FooString == f.FooString;
        }
    }

    public interface IBar
    {
        int BarMethod();

        int BarValue { get; set; }
    }

    public class Bar : IBar
    {
        public Bar()
        {
            BarValue = 999;
        }
        public int BarMethod()
        {
            return 9;
        }

        public int BarValue { get; set; }

        public bool Equals(Bar b)
        {
            return BarValue == b.BarValue;
        }
    }

    public interface ISimple
    {
        IBar Bar { get; }

        IFoo Foo { get; }

        ICalc Calc { get; }
    }

    public class Complicated : ISimple
    {
        #region Ctors.
        public Complicated(ICalc calc)
        {
            Calc = calc;
        }
        public Complicated(IBar bar)
        {
            Bar = bar;
        }

        public Complicated(IFoo foo)
        {
            Foo = foo;
        }

        public Complicated(IBar bar, IFoo foo)
        {
            Bar = bar;
            Foo = foo;
        }
        public Complicated(ICalc calc, IFoo foo)
        {
            Calc = calc;
            Foo = foo;
        }
        public Complicated(ICalc calc, IBar bar)
        {
            Calc = calc;
            Bar = bar;
        }
        public Complicated(ICalc calc, IBar bar, IFoo foo)
        {
            Calc = calc;
            Bar = bar;
            Foo = foo;
        }

#endregion //Ctors.

        public ICalc Calc { get; }

        public IBar Bar { get; }

        public IFoo Foo { get; }
    }

    public class VeryComplicated
    {
        #region Ctors

        public VeryComplicated(ICalc calc)
        {
            Calc = calc;
        }

        public VeryComplicated(IBar bar)
        {
            Bar = bar;
        }

        public VeryComplicated( IFoo foo)
        {
            Foo = foo;
        }
        
        public VeryComplicated(ICalc calc, IBar bar, IFoo foo)
        {
            Calc = calc;
            Bar = bar;
            Foo = foo;
        }

        public VeryComplicated(ICalc calc, IBar bar, IFoo foo, string name, int age, string password)
        {
            Calc = calc;
            Bar = bar;
            Foo = foo;
            Name = name;
            Age = age;
            Password = password;
        }

        #endregion //Ctors
        
        public ICalc Calc { get; set; }

        public IBar Bar { get; set; }

        public IFoo Foo { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Password { get; set; }
    }
}
