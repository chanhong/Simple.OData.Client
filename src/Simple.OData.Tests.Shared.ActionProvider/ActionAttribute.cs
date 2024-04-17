using System.Data.Services.Providers;

namespace Simple.OData.Tests.Shared.ActionProvider;

[AttributeUsage(AttributeTargets.Method)]
public class ActionAttribute : Attribute
{
	public ActionAttribute()
	{
		Binding = OperationParameterBindingKind.Always;
	}
	public OperationParameterBindingKind Binding { get; protected set; }
	public string AvailabilityMethodName { get; protected set; }
}
public class OccasionallyBindableAction : ActionAttribute
{
	public OccasionallyBindableAction(string availabilityMethod)
	{
		AvailabilityMethodName = availabilityMethod;
		Binding = OperationParameterBindingKind.Sometimes;
	}
}
public class NonBindableAction : ActionAttribute
{
	public NonBindableAction()
	{
		Binding = OperationParameterBindingKind.Never;
	}
}

[AttributeUsage(AttributeTargets.Method)]
public class SkipCheckForFeeds : Attribute
{

}
