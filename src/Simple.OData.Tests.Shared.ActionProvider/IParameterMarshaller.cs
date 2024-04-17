using System.Data.Services;
using System.Data.Services.Providers;

namespace Simple.OData.Tests.Shared.ActionProvider;

public interface IParameterMarshaller
{
	object[] Marshall(DataServiceOperationContext operationContext, ServiceAction serviceAction, object[] parameters);
}
