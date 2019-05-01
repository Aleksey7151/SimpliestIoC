using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace SimpleIoC
{
    /// <summary>
    /// Represents a container exception
    /// </summary>
    public sealed class ContainerException : Exception
    {
        /// <summary>
        /// Probable cause of exception
        /// </summary>
        public string ProbableCause { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="probableCause">Probable cause</param>
        public ContainerException(string message, string probableCause = null) : base(message)
        {
            ProbableCause = probableCause;
        }
    }

    #region IRegistartion

    /// <summary>
    /// Registration services interface
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Registers the <typeparamref name="TService"/> in dependency injection container with Transient lifetime.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <exception cref="ContainerException"></exception>
        void Register<TService>();

        /// <summary>
        /// Registers the <typeparamref name="TService"/> in dependency injection container.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="lifeCycle">Life cycle behavior of the created <typeparamref name="TService"/></param>
        /// <exception cref="ContainerException"></exception>
        void Register<TService>(LifeCycle lifeCycle);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory method <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation</param>
        /// <exception cref="ContainerException"></exception>
        void Register<TService>(Func<IInstantiation, TService> factory);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with corresponding factory method <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation</param>
        /// <param name="lifeCycle">Life cycle behavior of the created <typeparamref name="TService"/></param>
        /// <exception cref="ContainerException"></exception>
        void Register<TService>(Func<IInstantiation, TService> factory, LifeCycle lifeCycle);

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        void Register<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <param name="implementationId">Implementation identifier</param>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        void Register<TService, TImplementation>(string implementationId) where TImplementation : TService;

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<TService, TImplementation>(LifeCycle lifeCycle) where TImplementation : TService;

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<TService, TImplementation>(string implementationId, LifeCycle lifeCycle)
            where TImplementation : TService;

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        void Register<TService, TImplementation>(Func<IInstantiation, TImplementation> factory)
            where TImplementation : TService;

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        void Register<TService, TImplementation>(string implementationId, Func<IInstantiation, TImplementation> factory)
            where TImplementation : TService;

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<TService, TImplementation>(Func<IInstantiation, TImplementation> factory, LifeCycle lifeCycle)
            where TImplementation : TService;

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<TService, TImplementation>(string implementationId, Func<IInstantiation, TImplementation> factory,
            LifeCycle lifeCycle) where TImplementation : TService;

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, TService>(Func<IInstantiation, T1, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);


        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, T2, TService>(Func<IInstantiation, T1, T2, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, T2, T3, TService>(Func<IInstantiation, T1, T2, T3, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, T2, T3, T4, TService>(Func<IInstantiation, T1, T2, T3, T4, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, T2, T3, T4, T5, TService>(Func<IInstantiation, T1, T2, T3, T4, T5, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        void Register<T1, T2, T3, T4, T5, T6, TService>(Func<IInstantiation, T1, T2, T3, T4, T5, T6, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient);

    }

    #endregion //IRegistration


    #region IInstantiation

    /// <summary>
    /// Interface of getting instances
    /// </summary>
    public interface IInstantiation
    {
        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<TService>();

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <param name="implementationId">Implementator identifier</param>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<TService>(string implementationId);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg">First parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, TService>(T1 arg);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, T2, TService>(T1 arg1, T2 arg2);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, T2, T3, TService>(T1 arg1, T2 arg2, T3 arg3);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, T2, T3, T4, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="T5">Type of the fifth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <param name="arg5">Fifth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, T2, T3, T4, T5, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="T5">Type of the fifth parameter</typeparam>
        /// <typeparam name="T6">Type of the sixth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <param name="arg5">Fifth parameter value</param>
        /// <param name="arg6">Sixth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        TService GetInstance<T1, T2, T3, T4, T5, T6, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    }

    #endregion //IInstantiation


    #region IPropertyInjection

    /// <summary>
    /// Properties injection interface
    /// </summary>
    public interface IPropertyInjection
    {
        /// <summary>
        /// Registers the setter for a property injection in <typeparamref name="TService"/>
        /// </summary>
        /// <param name="propertyForInjection">Expression that defines the property for injection. It must be member access expression.</param>
        /// <param name="injectionFactory">Method that determines the injection value</param>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <typeparam name="TProperty">Type of Property</typeparam>
        void RegisterProperty<TService, TProperty>(Expression<Func<TService, TProperty>> propertyForInjection,
            Func<IInstantiation, TProperty> injectionFactory);

        /// <summary>
        /// Registers the value for injection as a value in property
        /// </summary>
        /// <param name="propertyForInjection">Expression that defines the property for injection. It must be member access expression.</param>
        /// <param name="injectionValue">Injected value</param>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <typeparam name="TProperty">Type of Property</typeparam>
        void RegisterProperty<TService, TProperty>(Expression<Func<TService, TProperty>> propertyForInjection,
            TProperty injectionValue);
    }

    #endregion


    /// <summary>
    /// Enumerates the time periods of life of instantiated objects
    /// </summary>
    public enum LifeCycle
    {
        /// <summary>
        /// Only one instance of a object will be created in this container
        /// </summary>
        SinglePerContainer,

        /// <summary>
        /// A new instance of the object always will be created
        /// </summary>
        Transient
    }

    /// <summary>
    /// Represents dependency injection container
    /// </summary>
    public class DiContainer : IRegistration, IPropertyInjection, IInstantiation, IDisposable
    {
        #region Private Fields

        private const string DefaultId = "DEFAULT";

        private readonly SynchronizedDictionary<Type, Dictionary<string, ServiceInfo>> _services;

        private readonly SynchronizedDictionary<Type, PropertyInjections> _properties;

        #endregion //Private Fields


        /// <summary>
        /// Ctor
        /// </summary>
        public DiContainer()
        {
            _services = new SynchronizedDictionary<Type, Dictionary<string, ServiceInfo>>();

            _properties = new SynchronizedDictionary<Type, PropertyInjections>();
        }


        #region Implementation of IRegistration

        /// <summary>
        /// Registers the <typeparamref name="TService"/> in dependency injection container with Transient lifetime.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <exception cref="ContainerException"></exception>
        public void Register<TService>()
        {
            RegisterService<TService, TService>(DefaultId, null, LifeCycle.Transient);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> in dependency injection container.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="lifeCycle">Life cycle behavior of the created <typeparamref name="TService"/></param>
        /// <exception cref="ContainerException"></exception>
        public void Register<TService>(LifeCycle lifeCycle)
        {
            RegisterService<TService, TService>(DefaultId, null, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory method <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation</param>
        /// <exception cref="ContainerException"></exception>
        public void Register<TService>(Func<IInstantiation, TService> factory)
        {
            RegisterService<TService, TService>(DefaultId, factory, LifeCycle.Transient);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with corresponding factory method <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation</param>
        /// <param name="lifeCycle">Life cycle behavior of the created <typeparamref name="TService"/></param>
        /// <exception cref="ContainerException"></exception>
        public void Register<TService>(Func<IInstantiation, TService> factory, LifeCycle lifeCycle)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(DefaultId, null, LifeCycle.Transient);
        }

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <param name="implementationId">Implementation identifier</param>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        public void Register<TService, TImplementation>(string implementationId) where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(implementationId, null, LifeCycle.Transient);
        }

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<TService, TImplementation>(LifeCycle lifeCycle) where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(DefaultId, null, lifeCycle);
        }

        /// <summary>
        /// Register the <typeparamref name="TService"/> with corresponding implementation type <typeparamref name="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<TService, TImplementation>(string implementationId, LifeCycle lifeCycle)
            where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(implementationId, null, lifeCycle);
        }

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        public void Register<TService, TImplementation>(Func<IInstantiation, TImplementation> factory)
            where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(DefaultId, factory, LifeCycle.Transient);
        }

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        public void Register<TService, TImplementation>(string implementationId,
            Func<IInstantiation, TImplementation> factory)
            where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(implementationId, factory, LifeCycle.Transient);
        }

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<TService, TImplementation>(Func<IInstantiation, TImplementation> factory,
            LifeCycle lifeCycle)
            where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the service <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// defines the instantiation method.
        /// </summary>
        /// <typeparam name="TService">Type of registering service</typeparam>
        /// <typeparam name="TImplementation">Type of object that implements <typeparamref name="TService"/></typeparam>
        /// <param name="implementationId">Implementation identifier</param>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation. </param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<TService, TImplementation>(string implementationId,
            Func<IInstantiation, TImplementation> factory,
            LifeCycle lifeCycle) where TImplementation : TService
        {
            RegisterService<TService, TImplementation>(implementationId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, TService>(Func<IInstantiation, T1, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }


        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, T2, TService>(Func<IInstantiation, T1, T2, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, T2, T3, TService>(Func<IInstantiation, T1, T2, T3, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, T2, T3, T4, TService>(Func<IInstantiation, T1, T2, T3, T4, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method. 
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, T2, T3, T4, T5, TService>(Func<IInstantiation, T1, T2, T3, T4, T5, TService> factory,
            LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }

        /// <summary>
        /// Registers the <typeparamref name="TService"/> with the corresponding factory <paramref name="factory"/> that
        /// describes the instantiation method.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter</typeparam>
        /// <typeparam name="T2">The type of the second parameter</typeparam>
        /// <typeparam name="T3">The type of the third parameter</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter</typeparam>
        /// <typeparam name="TService">Registering type of service</typeparam>
        /// <param name="factory">Lambda expression, that is resposible for the service <typeparamref name="TService"/> creation.</param>
        /// <param name="lifeCycle">Life cycle behavior of the created service</param>
        public void Register<T1, T2, T3, T4, T5, T6, TService>(
            Func<IInstantiation, T1, T2, T3, T4, T5, T6, TService> factory, LifeCycle lifeCycle = LifeCycle.Transient)
        {
            RegisterService<TService, TService>(DefaultId, factory, lifeCycle);
        }


        #endregion //Type Registration


        #region Implementation of IInstantiation

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<TService>()
        {
            var args = new object[] {DefaultId, this};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <param name="implementationId">Implementator identifier</param>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<TService>(string implementationId)
        {
            var args = new object[] {implementationId, this};
                // here only one place where we pass string parameter in first position.
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, TService>(T1 arg1)
        {
            var args = new object[] {DefaultId, this, arg1};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, T2, TService>(T1 arg1, T2 arg2)
        {
            var args = new object[] {DefaultId, this, arg1, arg2};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, T2, T3, TService>(T1 arg1, T2 arg2, T3 arg3)
        {
            var args = new object[] {DefaultId, this, arg1, arg2, arg3};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, T2, T3, T4, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var args = new object[] {DefaultId, this, arg1, arg2, arg3, arg4};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="T5">Type of the fifth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <param name="arg5">Fifth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, T2, T3, T4, T5, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var args = new object[] {DefaultId, this, arg1, arg2, arg3, arg4, arg5};
            return InstantiateService<TService>(args);
        }

        /// <summary>
        /// Gets the instance of registered service <typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="T1">Type of the first parameter</typeparam>
        /// <typeparam name="T2">Type of the second parameter</typeparam>
        /// <typeparam name="T3">Type of the third parameter</typeparam>
        /// <typeparam name="T4">Type of the fourth parameter</typeparam>
        /// <typeparam name="T5">Type of the fifth parameter</typeparam>
        /// <typeparam name="T6">Type of the sixth parameter</typeparam>
        /// <typeparam name="TService">Type of service that will be instantiate</typeparam>
        /// <param name="arg1">First parameter value</param>
        /// <param name="arg2">Second parameter value</param>
        /// <param name="arg3">Third parameter value</param>
        /// <param name="arg4">Fourth parameter value</param>
        /// <param name="arg5">Fifth parameter value</param>
        /// <param name="arg6">Sixth parameter value</param>
        /// <returns>The instance of service <typeparamref name="TService"/></returns>
        public TService GetInstance<T1, T2, T3, T4, T5, T6, TService>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5,
            T6 arg6)
        {
            var args = new object[] {DefaultId, this, arg1, arg2, arg3, arg4, arg5, arg6};
            return InstantiateService<TService>(args);
        }


        #endregion //Type Instanceation


        #region Properties registration

        /// <summary>
        /// Registers the setter for a property injection in <typeparamref name="TService"/>
        /// </summary>
        /// <param name="propertyForInjection">Expression that defines the property for injection. It must be member access expression.</param>
        /// <param name="injectionFactory">Method that determines the injection value</param>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <typeparam name="TProperty">Type of Property</typeparam>
        public void RegisterProperty<TService, TProperty>(Expression<Func<TService, TProperty>> propertyForInjection,
            Func<IInstantiation, TProperty> injectionFactory)
        {
            var propertyName = ExpressionTranslator.GetPropertyName(propertyForInjection);

            if (!_properties.ContainsKey(typeof(TService)))
            {
                var injectors = new PropertyInjections();
                injectors.AddPropertyFactory(propertyName, injectionFactory);
                _properties.Add(typeof(TService), injectors);
            }
            else
            {
                var injectors = _properties.Read(typeof(TService));
                injectors.AddPropertyFactory(propertyName, injectionFactory);
            }
        }

        /// <summary>
        /// Registers the value for injection as a value in property
        /// </summary>
        /// <param name="propertyForInjection">Expression that defines the property for injection. It must be member access expression.</param>
        /// <param name="injectionValue">Injected value</param>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <typeparam name="TProperty">Type of Property</typeparam>
        public void RegisterProperty<TService, TProperty>(Expression<Func<TService, TProperty>> propertyForInjection,
            TProperty injectionValue)
        {
            var propertyName = ExpressionTranslator.GetPropertyName(propertyForInjection);

            if (!_properties.ContainsKey(typeof(TService)))
            {
                var injectors = new PropertyInjections();
                injectors.AddPropertyValue(propertyName, injectionValue);
                _properties.Add(typeof(TService), injectors);
            }
            else
            {
                var injectors = _properties.Read(typeof(TService));
                injectors.AddPropertyValue(propertyName, injectionValue);
            }
        }


        #endregion //Properties registration 


        #region Private members

        private TService InstantiateService<TService>(object[] arguments)
        {
            if (arguments == null)
            {
                return default(TService);
            }

            var implementId = (string) arguments[0];

            var serviceInfo = GetServiceInfo(typeof(TService), implementId);

            if (serviceInfo.LifeCycle == LifeCycle.SinglePerContainer && serviceInfo.Instance != null)
            {
                // if type defined as Singleton, and if instance is already created, we just return created instance
                return (TService) serviceInfo.Instance;
            }

            if (serviceInfo.Factory != null)
            {
                var tmp = new object[arguments.Length - 1];
                Array.Copy(arguments, 1, tmp, 0, tmp.Length);

                object inst;
                try
                {
                    inst = serviceInfo.Factory.DynamicInvoke(tmp);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                
                if (serviceInfo.LifeCycle == LifeCycle.SinglePerContainer && serviceInfo.Instance == null)
                    serviceInfo.Instance = inst;

                InjectRegisteredProperties((TService)inst);

                return (TService) inst;
            }

            ConstructorInfo[] ctors = serviceInfo.ImplementationType.GetTypeInfo().DeclaredConstructors?.ToArray();
            if (ctors == null)
                throw new ContainerException($"Type {typeof(TService)} doesn't have decleared constructors.");

            ParameterInfo[] bestParameters = GetConstructorBestParameters(ctors);

            if (bestParameters.Length == 0) // if constructor without parameters
            {
                var inst = Activator.CreateInstance(serviceInfo.ImplementationType);

                if (serviceInfo.LifeCycle == LifeCycle.SinglePerContainer && serviceInfo.Instance == null)
                    serviceInfo.Instance = inst;

                InjectRegisteredProperties((TService)inst);

                return (TService) inst;
            }

            var args = new object[bestParameters.Length]; // array of params, that will be passed into the constructor

            var instantiationMethod = typeof(DiContainer).GetRuntimeMethods()
                    .FirstOrDefault(mi => string.Equals(mi.Name, nameof(InstantiateService)));

            for (var j = 0; j < bestParameters.Length; j++)
            {
                var paramType = bestParameters[j].ParameterType;
                object instanceOfParam;
                try
                {
                    instanceOfParam = instantiationMethod.MakeGenericMethod(paramType).Invoke(this, new object[1]{new object[] { DefaultId}});
                }
                catch (Exception ex)
                {
                    throw new ContainerException(ex.Message, $"Apparently type {typeof(TService)} not registered.");
                }
                
                // pass to generic method Type of parameter
                args[j] = instanceOfParam;
            }
            var instance = Activator.CreateInstance(serviceInfo.ImplementationType, args);
            if (instance == null)
            {
                throw new ContainerException($"Can't create instance of type {typeof(TService)}.");
            }

            if (serviceInfo.LifeCycle == LifeCycle.SinglePerContainer && serviceInfo.Instance == null)
                serviceInfo.Instance = instance;

            InjectRegisteredProperties((TService)instance);

            return (TService)instance;
        }

        private void RegisterService<TService, TImplementation>(string implementatorId, Delegate factory, LifeCycle lifeCycle)
        {
            if (!_services.ContainsKey(typeof(TService)))
            {
                var dict = new Dictionary<string, ServiceInfo>
                {
                    [implementatorId] = new ServiceInfo(typeof(TService), typeof(TImplementation), lifeCycle, factory)
                };
                _services.Add(typeof(TService), dict);
            }
            else
            {
                var dict = _services.Read(typeof(TService));
                if (dict.ContainsKey(implementatorId))
                {
                    var si = dict[implementatorId];
                    si.ImplementationType = typeof(TImplementation);
                    si.Factory = factory;
                    si.LifeCycle = lifeCycle;
                }
                else
                {
                    dict[implementatorId] = new ServiceInfo(typeof(TService), typeof(TImplementation), lifeCycle, factory);
                }
            }
        }


        private ServiceInfo GetServiceInfo(Type serviceType, string implentationId)
        {

            if (!_services.ContainsKey(serviceType))
                throw new ContainerException($"Type {serviceType} not registered.");

            var dict = _services.Read(serviceType);

            if (!dict.ContainsKey(implentationId))
            {
                if(implentationId == DefaultId)
                    throw new ContainerException($"Default implementation of service {serviceType} hasn't been registered.");
                throw new ContainerException($"Implementation ID: {implentationId} not registered.");
            }
            return dict[implentationId];
        }

        private ParameterInfo[] GetConstructorBestParameters(ConstructorInfo[] constructors)
        {
            //The point of this method is to find a constructor, that contains maximum
            //parameters, that have been already registered in service container 

            ParameterInfo[] bestParameters = null;
            var maxCountOfConcurParams = 0; // count of concured parameters 

            for (var ci = 0; ci < constructors.Length; ci++)
            {
                ParameterInfo[] ctorParams = constructors[ci].GetParameters();
                if (ci == 0)
                    bestParameters = ctorParams; // we must have something to compare with

                var cntOfConcurParams = 0;
                foreach (ParameterInfo paramInfo in ctorParams)
                {
                    if (_services.ContainsKey(paramInfo.ParameterType))
                        // determine if we already registered revoces for constructor parameters
                        cntOfConcurParams++;
                }
                if (cntOfConcurParams == ctorParams.Length && cntOfConcurParams > maxCountOfConcurParams)
                // if for all parameters of this constructor we have registered services, we decide, that this constructor is most suitable for us
                {
                    bestParameters = ctorParams;
                    maxCountOfConcurParams = cntOfConcurParams;
                }
            }
            return bestParameters;
        }

        private void InjectRegisteredProperties<TService>(TService service)
        {
            if (!_properties.ContainsKey(typeof(TService)))
                return;

            PropertyInfo[] propertyInfos = typeof(TService).GetRuntimeProperties().ToArray();

            var injections = _properties.Read(typeof(TService));

            foreach (var property in propertyInfos)
            {
                if (!property.CanWrite)
                    continue;

                object instance = null;

                if (injections.PropertySetters.ContainsKey(property.Name))
                {
                    var setter = injections.PropertySetters[property.Name];

                    instance = setter.DynamicInvoke(this);
                }
                else if (injections.PropertyValues.ContainsKey(property.Name))
                {
                    instance = injections.PropertyValues[property.Name];
                }

                if(instance != null)
                    property.SetValue(service, instance);
            }
        }

        #endregion // Private members


        #region Implementation of IDisposable

        /// <summary>
        /// Releases all registered services. If services implement
        /// IDisposable, then they'll be disposed. 
        /// </summary>
        public void Dispose()
        {
            _services?.Dispose();
        }

        #endregion // Implementation of IDisposable

    }
}

