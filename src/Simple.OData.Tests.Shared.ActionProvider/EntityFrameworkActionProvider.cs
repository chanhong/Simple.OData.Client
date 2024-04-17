using System.Data.Entity;

namespace Simple.OData.Tests.Shared.ActionProvider;

public class EntityFrameworkActionProvider(DbContext dbContext) : ActionProvider(dbContext, new EntityFrameworkParameterMarshaller())
{
}
