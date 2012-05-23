Ever been stuck in .NET 2.0 land and wish you had an IoC container that didn't feel like it was built in 2005?

# Features

* Simple
* Code based registration
* Convention based registration
* Autowire support
* Singleton lifecycle
* .NET 2.0 based

# Examples

## Code based registration

XML configuration is great for those of you that like to program in XML.  Go use the .NET 2.0 version of Spring.NET, Castle Windsor, or StructureMap.

For those of you that have self-respect, and are still with me, here is how you do code-based registration like a real programmer.

    var container = new Container();
    
    container.For<ICar>().Use<BmwM3>();
    
    var car = container.Get<ICar>();
    car.Drive();

## Convention based registration

Sometimes parts of your application can be abstracted into a few rules.  That is what the conventions are for.

The following example registers all types in the current assembly that end with `Processor`

    var container = new Container();
    
    container.SetupConvention(x =>
    {
        x.AddAssembly(MethodInfo.GetCurrentMethod.DeclaringType.Assembly);
        x.Where = type => type.Name.EndsWith("Processor");
        x.Do = (type, contextContainer) => contextContainer.RegisterType(type, type);
    });
    
    var fileProcessor = container.Get<FileProcessor>();
    var memoryProcessor = container.Get<MemoryProcessor>();
    var databaseProcessor = container.Get<DatabaseProcessor>();

## Autowire

Like most containers, TwoIoc can autowire dependencies if they're registered in the container.

Consider the following class that has a dependency:

    class Addict
    {
        IDrug _drug;
    
        public Addict(IDrug drug)
        {
            _drug = drug;
        }
    }

Using the below, the dependency is autowired into the class that needs it.

    var container = new Container();
    
    container.For<IDrug>().Use<Tylenol>();
    container.Concrete.Use<Addict>();

    var dependent = container.GetAddict();

## Singletons

Sometimes, you just really need a singleton.  Please please PLEASE use a container for that instead of a static GetInstance method.

    var container = new Container();
    
    container.Concrete.Use<ExpensiveResource>().AsSingleton();
    
    // These are the same reference
    var resource1 = container.Get<ExpensiveResource>();
    var resource2 = container.Get<ExpensiveResource>();

## Specify Constructor Arguments

Need to pass in some constructor arguments based on some data like a config file?  No problem.

Here's the class that needs configured:

    class NeedsConfigured : INeedsConfigured
    {
        public NeedsConfigured(string hostname, int port) { ... }
    }

Use the `WithCtorParams` option, with an anonymous type that has property names that match your constructor parameter names.  Whoa, sweet.

    var container = new Container();
    
    // for concrete usage
    container.Concrete.UseWithCtorParams<NeedsConfigured>(
      new {hostname = "twoioc.com", port = 123});

    // for interface based usage
    container.For<INeedsConfigured>().Use<NeedsConfigured>(
      new {hostname = "twoioc.com", port = 123});
    
    var configured = container.Get<NeedsConfigured>();

You can also pass in constructor args as you're resolving from the container.  These should be passed in order, like you would to `Activator.CreateInstance`.

    var container = new Container();
    
    container.Concrete.Use<NeedsConfigured>();
    
    var configured = container.Get<NeedsConfigured>("twoioc.com", 123);


## Lambda Registration!

Have some other need that doesn't fit the above scenarios?  Use a Lambda to register your object.

In this example, every Veyron gets a different number.

    var container = new Container();
    
    var carNumber = 0;
    container.For<ICar>().UseFunc(() =>
    {
        return new Veyron(++carNumber);
    });

    var veyron1 = container.Get<ICar>(); // car number is 1
    var veyron2 = container.Get<ICar>(); // car number is 2

You can also use the container in your lambda.

    var container = new Container();
    
    container.Concrete.Use<MotorController>().AsSingleton();

    container.For<IInstrumentPanel>().UseFunc(() =>
    {
        return new MobilePanel(container.Get<MotorController>());
    });

