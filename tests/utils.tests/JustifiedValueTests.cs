namespace utils.tests;

public class JustifiedValueTests
{
    [Fact]
    public void FromValue()
    {
        var expected = "ok";
        var res = JustifiedValue<string>.FromValue(expected);

        Assert.True(res);
        Assert.Equal(expected, res.Value);
        Assert.Null(res.Exception);
    }

    [Fact]
    public void FromException()
    {
        var expected = new Exception("an error");
        var res = JustifiedValue<string>.FromException(expected);

        Assert.False(res);
        Assert.Equal(expected, res.Exception);
        Assert.Null(res.Value);
    }
}