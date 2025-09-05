namespace DomainDrivenVerticalSlices.Template.Common.Tests.Models;

using DomainDrivenVerticalSlices.Template.Common.Models;

public class ValueObjectTests
{
    [Fact]
    public void Equals_ReturnsTrue_WhenEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("abc", 123);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenNotEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("def", 456);

        // Act
        var result = vo1.Equals(vo2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenDifferentType()
    {
        // Arrange
        var obj1 = new TestValueObject("value", 1);
        var obj2 = new object();

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_ReturnsSameValue_WhenEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("abc", 123);

        // Act
        var hashCode1 = vo1.GetHashCode();
        var hashCode2 = vo2.GetHashCode();

        // Assert
        Assert.Equal(hashCode2, hashCode1);
    }

    [Fact]
    public void GetHashCode_ReturnsDifferentValue_WhenNotEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("def", 456);

        // Act
        var hashCode1 = vo1.GetHashCode();
        var hashCode2 = vo2.GetHashCode();

        // Assert
        Assert.NotEqual(hashCode2, hashCode1);
    }

    [Fact]
    public void OperatorEquals_ReturnsTrue_WhenEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("abc", 123);

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorEquals_ReturnsFalse_WhenNotEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("abc", 123);
        var vo2 = new TestValueObject("def", 456);

        // Act
        var result = vo1 == vo2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsFalse_WhenEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("value", 1);
        var obj2 = new TestValueObject("value", 1);

        // Act
        var result = obj1 != obj2;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsTrue_WhenNotEqual()
    {
        // Arrange
        var obj1 = new TestValueObject("value", 1);
        var obj2 = new TestValueObject("another-value", 2);

        // Act
        var result = obj1 != obj2;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsFalse_WhenSameInstance()
    {
        // Arrange
        var obj1 = new TestValueObject("value", 1);
        var obj2 = obj1;

        // Act
        var result = obj1 != obj2;

        // Assert
        Assert.False(result);
    }

    private class TestValueObject(string property1, int property2) : ValueObject<TestValueObject>
    {
        public string Property1 { get; set; } = property1;

        public int Property2 { get; set; } = property2;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Property1;
            yield return Property2;
        }
    }
}
