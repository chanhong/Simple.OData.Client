using BenchmarkDotNet.Attributes;
#if !BENCHMARKS
using Xunit;
#endif

namespace Simple.OData.Benchmarks.Client;

public class he_employee
{
	public int he_employeenumber { get; set; }
}

public class CrmEmployee
{
	[Benchmark]
	public async static Task GetAll()
	{
		var result = await Utils.GetClient("crm_schema.xml", "crm_result_10.json")
			.For<he_employee>()
			.FindEntriesAsync().ConfigureAwait(false);

#if !BENCHMARKS
		Assert.Equal(10, result.ToList().Count);
#endif
	}

	[Benchmark]
	public async static Task GetSingle()
	{
		var result = await Utils.GetClient("crm_schema.xml", "crm_result_1.json")
			.For<he_employee>()
			.Filter(x => x.he_employeenumber == 123456)
			.FindEntryAsync().ConfigureAwait(false);

#if !BENCHMARKS
		Assert.Equal(123456, result.he_employeenumber);
#endif
	}
}
