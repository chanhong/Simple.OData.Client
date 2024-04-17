using System.Reflection;

namespace Simple.OData.Tests.Shared.ActionProvider;

public static class ActionFinder
{
	public static IEnumerable<ActionInfo> GetActionsFromType(Type type)
	{
		var actionInfos = type
			.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.Select(m => new
			{
				Method = m,
				Attribute = Attribute.GetCustomAttribute(m, typeof(ActionAttribute)) as ActionAttribute
			})
			.Where(u => u.Attribute is not null)
			.Select(u => new ActionInfo(
				u.Method,
				u.Attribute.Binding,
				u.Attribute.AvailabilityMethodName)
			).ToArray();
		return actionInfos;
	}
}
