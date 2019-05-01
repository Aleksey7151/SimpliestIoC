using System;
using System.Collections.Generic;

namespace SimpleIoC
{
    internal class PropertyInjections
    {
        public PropertyInjections()
        {
            PropertySetters = new Dictionary<string, Delegate>();

            PropertyValues = new Dictionary<string, object>();
        }
        /// <summary>
        /// Adds property for injection
        /// </summary>
        /// <param name="propertyName">Service property name for injection</param>
        /// <param name="factory">Method that is responsible for creation a value for injection</param>
        public void AddPropertyFactory(string propertyName, Delegate factory)
        {
            // property can be added only to one Dictionary, so we must remove from other dictionary, if it contains the property
            if (PropertyValues.ContainsKey(propertyName))
                PropertyValues.Remove(propertyName);

            PropertySetters[propertyName] = factory;    // here we just update the value
        }

        /// <summary>
        /// Adds property for injection
        /// </summary>
        /// <param name="propertyName">Service property name for injection</param>
        /// <param name="value">value that will be injected</param>
        public void AddPropertyValue(string propertyName, object value)
        {
            // property can be added only to one Dictionary, so we must remove from other dictionary, if it contains the property
            if (PropertySetters.ContainsKey(propertyName))
                PropertySetters.Remove(propertyName);

            PropertyValues[propertyName] = value;       // here we just update the value
        }
        /// <summary>
        /// All registered property setters for injection
        /// </summary>
        public Dictionary<string, Delegate> PropertySetters { get; }

        /// <summary>
        /// All registered property values for injection
        /// </summary>
        public Dictionary<string, object> PropertyValues { get; }
    }



    internal class ServiceInfo : IDisposable
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="serviceType">Type of registered service</param>
        /// <param name="implementationType">Type og implementator</param>
        /// <param name="lifeCycle">Life cycle of implementation</param>
        /// <param name="factory">Factory of service instantiation</param>
        public ServiceInfo(Type serviceType, Type implementationType, LifeCycle lifeCycle, Delegate factory)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeCycle = lifeCycle;
            Factory = factory;
        }

        /// <summary>
        /// Type of registered service
        /// </summary>
        public Type ServiceType { get; private set; }
        
        /// <summary>
        /// Type of object that implements serveice
        /// </summary>
        public Type ImplementationType { get; set; }
        
        /// <summary>
        /// Life cycle of implementation
        /// </summary>
        public LifeCycle LifeCycle { get; set; }
        /// <summary>
        /// Reference to a instance of implementation, if it's already created
        /// </summary>
        public object Instance { get; set; }
        
        /// <summary>
        /// Method that is resposible for implementation creation
        /// </summary>
        public Delegate Factory { get; set; }
        
        public void Dispose()
        {
            var disposable = Instance as IDisposable;
            disposable?.Dispose();
            Instance = null;
            Factory = null;
            ServiceType = null;
        }
    }
}
