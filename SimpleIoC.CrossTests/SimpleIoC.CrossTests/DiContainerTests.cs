using NUnit.Framework;
using SimpleIoC.CrossTests.Services;
using SimpleIoC.Win8Tests.Services;

namespace SimpleIoC.CrossTests
{
    [TestFixture]
    public class DiContainerTests
    {
        #region Service Instantiations

        [Test]
        public void TypeNotRegistered()
        {
            var ex = Assert.Throws<ContainerException>(() =>
            {
                var di = GetDiContainer();
                di.GetInstance<IFoo>();
            });
            Assert.That(ex.Message, Contains.Substring("not registered"));
        }

        [Test]
        public void DependencyNotRegistered()
        {
            var ex = Assert.Throws<ContainerException>(() =>
            {
                var di = GetDiContainer();
                di.Register<ICalc, CalcWithDependency>();
                di.GetInstance<ICalc>();    // Calc needs IBar
            });
            Assert.That(ex.ProbableCause, Contains.Substring("not registered"));
        }

        [Test]
        public void GetServiceInstance()
        {
            var di = GetDiContainer();
            di.Register<Service>();
            var inst = di.GetInstance<Service>();
            Assert.IsNotNull(inst);
            Assert.IsTrue(inst.Value == 575656);
        }

        [Test]
        public void GetServiceInstanceLikeSingleObject()
        {
            var di = GetDiContainer();
            di.Register<Service>(LifeCycle.SinglePerContainer);
            var inst = di.GetInstance<Service>();
            Assert.IsNotNull(inst);
            inst.Value = 838383333;
            var ints2 = di.GetInstance<Service>();
            Assert.AreSame(ints2, inst);
        }

        [Test]
        public void GetServiceInstanceLikeTransientObject()
        {
            var di = GetDiContainer();
            di.Register<Service>(LifeCycle.Transient);
            var inst = di.GetInstance<Service>();

            Assert.IsNotNull(inst);

            inst.Value = 838383333;

            var ints2 = di.GetInstance<Service>();

            Assert.AreNotSame(ints2, inst);
        }

        [Test]
        public void GetImplementationOfRegisteredService()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>();
            var inst = di.GetInstance<ICalc>();
            Assert.IsNotNull(inst);
            Assert.IsInstanceOf<CalcSimple>(inst);
        }

        [Test]
        public void GetImplementationOfRegisteredServiceAfterTwoRegistrationsOfImplementator()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>();
            di.Register<ICalc, CalcEngineer>();

            var inst = di.GetInstance<ICalc>();
            Assert.IsNotNull(inst);
            Assert.IsInstanceOf<CalcEngineer>(inst);
        }

        [Test]
        public void GetService_WithKnownDependency()
        {
            var di = GetDiContainer();
            di.Register<IBar, Bar>();
            di.Register<Complicated>();
            var instance = di.GetInstance<Complicated>();
            Assert.NotNull(instance);
            Assert.NotNull(instance.Bar);
            Assert.Null(instance.Calc);
            Assert.Null(instance.Foo);
            Assert.IsInstanceOf(typeof(IBar), instance.Bar);
        }

        [Test]
        public void GetService_WithKnownDependencyAsSingleObject()
        {
            var di = GetDiContainer();
            di.Register<IBar, Bar>();
            di.Register<IFoo, Foo>(LifeCycle.SinglePerContainer);
            di.Register<Complicated>();

            var instance1 = di.GetInstance<Complicated>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance1.Bar);
            Assert.Null(instance1.Calc);
            Assert.NotNull(instance1.Foo);
            Assert.IsInstanceOf(typeof(IBar), instance1.Bar);
            Assert.IsInstanceOf(typeof(IFoo), instance1.Foo);

            var instance2 = di.GetInstance<Complicated>();

            Assert.NotNull(instance2);
            Assert.NotNull(instance2.Bar);
            Assert.Null(instance2.Calc);
            Assert.NotNull(instance2.Foo);

            Assert.AreNotSame(instance2, instance1);
            Assert.AreNotSame(instance1.Bar, instance2.Bar);
            Assert.AreSame(instance1.Foo, instance2.Foo);
        }

        [Test]
        public void GetImplementation_WithKnownDependencyAsSingleObject()
        {
            var di = GetDiContainer();
            di.Register<IBar, Bar>();
            di.Register<IFoo, Foo>(LifeCycle.SinglePerContainer);
            di.Register<ICalc, CalcEngineer>(LifeCycle.SinglePerContainer);
            di.Register<ISimple, Complicated>();

            var instance1 = di.GetInstance<ISimple>();

            Assert.NotNull(instance1);
            Assert.NotNull(instance1.Bar);
            Assert.NotNull(instance1.Calc);
            Assert.NotNull(instance1.Foo);
            Assert.IsInstanceOf(typeof(IBar), instance1.Bar);
            Assert.IsInstanceOf(typeof(IFoo), instance1.Foo);
            Assert.IsInstanceOf(typeof(ICalc), instance1.Calc);

            var instance2 = di.GetInstance<ISimple>();

            Assert.NotNull(instance2);
            Assert.NotNull(instance2.Bar);
            Assert.NotNull(instance2.Calc);
            Assert.NotNull(instance2.Foo);

            Assert.AreNotSame(instance2, instance1);
            Assert.AreNotSame(instance1.Bar, instance2.Bar);
            Assert.AreSame(instance1.Foo, instance2.Foo);
            Assert.AreSame(instance1.Calc, instance2.Calc);
        }

        [Test]
        public void GetImplementation_WithOneFactoryParam()
        {
            var str = "This is Foo string";
            var di = GetDiContainer();
            di.Register<IFoo, ISimple>((factory, foo) => new Complicated(foo));

            var fooArg = new Foo { FooString = str };

            var simple = di.GetInstance<IFoo, ISimple>(fooArg);

            Assert.NotNull(simple);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.IsTrue(simple.Foo.FooString == str);
        }

        [Test]
        public void GetImplementation_WithOneFactoryParam_And_OtherFromContainer()
        {
            var str = "This is Foo string";
            var di = GetDiContainer();
            di.Register<IBar, Bar>();
            di.Register<ICalc, CalcSimple>();
            di.Register<IFoo, ISimple>((factory, foo) => new Complicated(factory.GetInstance<ICalc>(), factory.GetInstance<IBar>(), foo));

            var fooArg = new Foo { FooString = str };

            var simple = di.GetInstance<IFoo, ISimple>(fooArg);

            Assert.NotNull(simple);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.IsTrue(simple.Foo.FooString == str);
            Assert.NotNull(simple.Bar);
            Assert.NotNull(simple.Calc);
        }

        [Test]
        public void GetImplementation_WithTwoFactoryParams()
        {
            var fooStr = "Some Foo String";
            var di = GetDiContainer();
            di.Register<IFoo, IBar, ISimple>((factory, foo, bar) => new Complicated(bar, foo));

            var fooArg = new Foo { FooString = fooStr };
            var barArg = new Bar { BarValue = 123455 };

            var simple = di.GetInstance<IFoo, IBar, ISimple>(fooArg, barArg);

            Assert.NotNull(simple);
            Assert.NotNull(simple.Foo);
            Assert.NotNull(simple.Bar);
            Assert.Null(simple.Calc);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.AreSame(barArg, simple.Bar);
            Assert.IsTrue(simple.Foo.FooString == fooStr);
            Assert.IsTrue(simple.Bar.BarValue == 123455);
        }

        [Test]
        public void GetImplementation_WithTwoFactoryParams_And_OtherFromContainer()
        {
            var fooStr = "Some Foo String";
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>();
            di.Register<IFoo, IBar, ISimple>((factory, foo, bar) => new Complicated(factory.GetInstance<ICalc>(), bar, foo));

            var fooArg = new Foo { FooString = fooStr };
            var barArg = new Bar { BarValue = 123455 };

            var simple = di.GetInstance<IFoo, IBar, ISimple>(fooArg, barArg);

            Assert.NotNull(simple);
            Assert.NotNull(simple.Foo);
            Assert.NotNull(simple.Bar);
            Assert.NotNull(simple.Calc);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.AreSame(barArg, simple.Bar);
            Assert.IsTrue(simple.Foo.FooString == fooStr);
            Assert.IsTrue(simple.Bar.BarValue == 123455);
        }

        [Test]
        public void GetInstance_WithThreeFactoryParams()
        {
            var fooStr = "Some Foo String";

            var di = GetDiContainer();
            di.Register<IBar, ICalc, IFoo, ISimple>((factory, bar, calc, foo) => new Complicated(calc, bar, foo));

            var fooArg = new Foo { FooString = fooStr };
            var barArg = new Bar { BarValue = 123455 };
            var calcArg = new CalcSimple();

            var simple = di.GetInstance<IBar, ICalc, IFoo, ISimple>(barArg, calcArg, fooArg);

            Assert.NotNull(simple);
            Assert.NotNull(simple.Foo);
            Assert.NotNull(simple.Bar);
            Assert.NotNull(simple.Calc);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.AreSame(barArg, simple.Bar);
            Assert.AreSame(calcArg, simple.Calc);
            Assert.IsTrue(simple.Foo.FooString == fooStr);
            Assert.IsTrue(simple.Bar.BarValue == 123455);
        }

        [Test]
        public void GetInstance_WithSixFactoryParams()
        {
            var fooStr = "Some Foo String";
            var di = GetDiContainer();
            di.Register<ICalc, IBar, IFoo, string, int, string, VeryComplicated>((factory, calc, bar, foo, name, age, pass) => new VeryComplicated(calc, bar, foo, name, age, pass));

            var fooArg = new Foo { FooString = fooStr };
            var barArg = new Bar { BarValue = 123455 };
            var calcArg = new CalcSimple();

            var simple = di.GetInstance<ICalc, IBar, IFoo, string, int, string, VeryComplicated>(calcArg, barArg, fooArg, "NameIs", 36363, "PASSSSSSSWWWWWW");

            Assert.NotNull(simple);
            Assert.NotNull(simple.Foo);
            Assert.NotNull(simple.Bar);
            Assert.NotNull(simple.Calc);
            Assert.AreSame(fooArg, simple.Foo);
            Assert.AreSame(barArg, simple.Bar);
            Assert.AreSame(calcArg, simple.Calc);
            Assert.IsTrue(simple.Foo.FooString == fooStr);
            Assert.IsTrue(simple.Bar.BarValue == 123455);
            Assert.IsTrue(simple.Name == "NameIs");
            Assert.IsTrue(simple.Age == 36363);
            Assert.IsTrue(simple.Password == "PASSSSSSSWWWWWW");
        }

        #endregion //Service Instantiations


        #region Property Injections

        [Test]
        public void InjectOnePropertyUsingValue()
        {
            var ioc = GetDiContainer();

            ioc.Register<IBar, Bar>();

            ioc.Register<VeryComplicated>();
            // Here we register for property "Name" injection value "Registered Injection"
            ioc.RegisterProperty<VeryComplicated, string>(complicated => complicated.Name, "Registered Injection");

            var complicatedInst = ioc.GetInstance<VeryComplicated>();

            Assert.NotNull(complicatedInst);
            Assert.NotNull(complicatedInst.Bar);
            Assert.IsTrue(complicatedInst.Name == "Registered Injection");
        }

        [Test]
        public void InjectTwoPropertiesUsingValue()
        {
            var di = GetDiContainer();

            di.Register<IBar, Bar>();

            di.Register<VeryComplicated>();
            // Here we register for property "Name" injection value "Registered Injection"
            di.RegisterProperty<VeryComplicated, string>(complicated => complicated.Name, "Registered Injection");
            di.RegisterProperty<VeryComplicated, int>(complicated => complicated.Age, 144444);
            var complicatedInst = di.GetInstance<VeryComplicated>();

            Assert.NotNull(complicatedInst);
            Assert.NotNull(complicatedInst.Bar);
            Assert.IsTrue(complicatedInst.Name == "Registered Injection");
            Assert.IsTrue(complicatedInst.Age == 144444);
        }

        [Test]
        public void InjectOnePropertyUsingFactory()
        {
            var di = GetDiContainer();

            di.Register<IBar, Bar>();

            di.Register<VeryComplicated>();
            // Here we register for property "Name" injection value "Registered Injection"
            di.RegisterProperty<VeryComplicated, IFoo>(complicated => complicated.Foo, i => new Foo { FooString = "Some string" });

            var complicatedInst = di.GetInstance<VeryComplicated>();

            Assert.NotNull(complicatedInst);
            Assert.NotNull(complicatedInst.Bar);
            Assert.NotNull(complicatedInst.Foo);
            Assert.IsTrue(complicatedInst.Foo.FooString == "Some string");
        }

        [Test]
        public void InjectTwoPropertiesUsingFactoryMethod()
        {
            var di = GetDiContainer();

            di.Register<IBar, Bar>();
            di.Register<ICalc, CalcSimple>();

            di.Register<VeryComplicated>();
            // Here we register for property "Name" injection value "Registered Injection"
            di.RegisterProperty<VeryComplicated, IFoo>(complicated => complicated.Foo, i => new Foo { FooString = "Some string" });
            di.RegisterProperty<VeryComplicated, IBar>(complicated => complicated.Bar, i => i.GetInstance<IBar>());

            var complicatedInst = di.GetInstance<VeryComplicated>();

            Assert.NotNull(complicatedInst);
            Assert.NotNull(complicatedInst.Bar);
            Assert.NotNull(complicatedInst.Foo);
            Assert.NotNull(complicatedInst.Calc);
            Assert.IsTrue(complicatedInst.Foo.FooString == "Some string");
        }

        #endregion //Property Injections


        #region Different Implementations for one Service

        [Test]
        public void ThrowsExceptionWhenPassWrongIdentifier()
        {
            var ex = Assert.Throws<ContainerException>(() =>
            {
                var di = GetDiContainer();
                di.Register<ICalc, CalcSimple>("Simple");
                di.GetInstance<ICalc>("timple");
            });

            Assert.That(ex.Message, Contains.Substring("Implementation ID:"));
            Assert.That(ex.Message, Contains.Substring("not registered."));
        }

        [Test]
        public void ThrowsExceptionWhenThereIsNoDefaultImplementationForService()
        {
            var ex = Assert.Throws<ContainerException>(() =>
            {
                var di = GetDiContainer();
                di.Register<ISimple, Complicated>("Simple", factory => new Complicated(factory.GetInstance<ICalc>()));
                di.GetInstance<ISimple>();
            });

            Assert.That(ex.Message, Contains.Substring("Default implementation of service"));
        }

        [Test]
        public void ThrowsExceptionIfImplementationHasNotRegisteredDependency()
        {
            var ex = Assert.Throws<ContainerException>(() =>
            {
                var di = GetDiContainer();
                di.Register<ISimple, Complicated>("Simple", factory => new Complicated(factory.GetInstance<ICalc>()));
                di.GetInstance<ISimple>("Simple");
            });
            Assert.That(ex.Message, Contains.Substring("not registered."));
        }

        [Test]
        public void TwoImplementationsOfOneService()
        {
            var di = GetDiContainer();

            di.Register<ICalc, CalcSimple>("Simple");
            di.Register<ICalc, CalcEngineer>("Engineer");

            var simple = di.GetInstance<ICalc>("Simple");
            var engineer = di.GetInstance<ICalc>("Engineer");

            Assert.NotNull(simple);
            Assert.NotNull(engineer);

            Assert.IsInstanceOf<CalcSimple>(simple);
            Assert.IsInstanceOf<CalcEngineer>(engineer);
        }

        [Test]
        public void ThreeImplementationsOfOneService()
        {
            var di = GetDiContainer();

            di.Register<ICalc, CalcSimple>("Simple");
            di.Register<ICalc, CalcEngineer>("Engineer");
            di.Register<ICalc, CalcProgrammer>("Programmer");

            var simple = di.GetInstance<ICalc>("Simple");
            var engineer = di.GetInstance<ICalc>("Engineer");
            var programmer = di.GetInstance<ICalc>("Programmer");

            Assert.NotNull(simple);
            Assert.NotNull(engineer);
            Assert.NotNull(programmer);

            Assert.IsInstanceOf<CalcSimple>(simple);
            Assert.IsInstanceOf<CalcEngineer>(engineer);
            Assert.IsInstanceOf<CalcProgrammer>(programmer);
        }

        [Test]
        public void TwoImplementationsOfOneServiceWhenImplementationDependsOnOtherService()
        {
            var di = GetDiContainer();
            di.Register<IBar, Bar>();
            di.Register<ICalc, CalcSimple>("Simple");
            di.Register<ICalc, CalcEngineer>("Engineer");
            di.Register<ICalc, CalcWithDependency>("WithDependency");

            var simple = di.GetInstance<ICalc>("Simple");
            var engineer = di.GetInstance<ICalc>("Engineer");
            var withDependency = di.GetInstance<ICalc>("WithDependency");

            Assert.NotNull(simple);
            Assert.NotNull(engineer);
            Assert.NotNull(withDependency);

            Assert.IsInstanceOf<CalcSimple>(simple);
            Assert.IsInstanceOf<CalcEngineer>(engineer);
            Assert.IsInstanceOf<CalcWithDependency>(withDependency);

            Assert.NotNull(((CalcWithDependency)withDependency).Bar);
        }

        [Test]
        public void GetServiceByIdentifierLikeSingleObject()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>("SimpleSingle", LifeCycle.SinglePerContainer);

            var singl1 = di.GetInstance<ICalc>("SimpleSingle");
            var singl2 = di.GetInstance<ICalc>("SimpleSingle");
            Assert.AreSame(singl1, singl2);
        }

        [Test]
        public void GetServiceByIdentifierLikeTransientObject()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>("Simple");

            var singl1 = di.GetInstance<ICalc>("Simple");
            var singl2 = di.GetInstance<ICalc>("Simple");
            Assert.AreNotSame(singl1, singl2);
        }

        [Test]
        public void GetServiceByIdentifierLikeSingleAndTransientObjects()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>("Single", LifeCycle.SinglePerContainer);
            di.Register<ICalc, CalcSimple>("Transient", LifeCycle.Transient);

            var singl1 = di.GetInstance<ICalc>("Single");
            singl1.Value = 7838383;
            var singl2 = di.GetInstance<ICalc>("Single");
            Assert.AreSame(singl1, singl2);

            var transient1 = di.GetInstance<ICalc>("Transient");
            Assert.AreNotSame(singl1, transient1);
        }

        [Test]
        public void GetServiceByIdentifierUsingFactoryMethod()
        {
            var di = GetDiContainer();
            di.Register<ICalc, CalcSimple>();
            di.Register<ISimple, Complicated>("Simple", factory => new Complicated(factory.GetInstance<ICalc>()));
            var instance = di.GetInstance<ISimple>("Simple");
            Assert.NotNull(instance);
            Assert.NotNull(instance.Calc);
            Assert.IsNull(instance.Bar);
            Assert.IsNull(instance.Foo);
        }

        #endregion //Different Implementations for one Service


        #region Private Functions

        private DiContainer GetDiContainer()
        {
            return new DiContainer();
        }

        #endregion
    }
}
