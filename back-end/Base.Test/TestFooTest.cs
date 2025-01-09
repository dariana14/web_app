using Helpers;

namespace Base.Test;

public class TestFooTest
{
    private TestFoo testFoo = new TestFoo();

    public TestFooTest()
    {
    }

    // TestFoo
    [Fact]
    public void InitialValueShouldBeZero()
    {
        // arrange
        // act
        // assert
        Assert.Equal(0, testFoo.State);
    }

    [Fact]
    public void AddTwoNumbers()
    {
        // arrange
        testFoo.State = -1;
        // act
        testFoo.Add(2);
        // assert
        Assert.Equal(1, testFoo.State);
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(1, 1, 2)]
    public void AddTwoNumbersViaParams(int initialValue, int valueToAdd, int expectedValue)
    {
        // arrange
        testFoo.State = initialValue;
        // act
        testFoo.Add(valueToAdd);
        // assert
        Assert.Equal(expectedValue, testFoo.State);
    }


    [Fact]
    public void DivideByZeroShouldGiveException()
    {
        testFoo.State = 1;
        Assert.Throws<DivideByZeroException>(() => testFoo.Div(0));
    }
}