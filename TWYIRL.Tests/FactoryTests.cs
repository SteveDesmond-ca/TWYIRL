using Xunit;

namespace TWYIRL.Tests;

public class FactoryTests
{
	[Fact]
	public void CanCreateApp()
	{
		//arrange/act
		var app = Factory.App();

		//assert
		Assert.IsType<App>(app);
	}
}