using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using log4net;

namespace Tiexue.Framework.Wcf {
    [AttributeUsage(AttributeTargets.Class)]
    public class ErrorHandlerBehaviorAttribute : Attribute, IErrorHandler, IServiceBehavior {
        private readonly ILog _log = LogManager.GetLogger(typeof(ErrorHandlerBehaviorAttribute));

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault) {
            var faultException = error as FaultException ?? new FaultException<InvalidOperationException>(new InvalidOperationException(error.Message), error.Message);
            var messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        public bool HandleError(Exception error) {
            _log.Error(error.Message, error);
            return false;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) {}

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {
            foreach(ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers) {
                channelDispatcher.ErrorHandlers.Add(this);
            }
        }
    }
}
