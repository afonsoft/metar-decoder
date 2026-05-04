using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace Metar.Decoder.Tests.Entity
{
    [TestFixture, Category("Entity")]
    public class ValueExtendedTest
    {
        [Test]
        public void TestToString()
        {
            var value = new Value(100.5, Value.Unit.Meter);
            ClassicAssert.AreEqual("100.5 Meter", value.ToString());
        }

        [Test]
        public void TestToStringKnot()
        {
            var value = new Value(25, Value.Unit.Knot);
            ClassicAssert.AreEqual("25 Knot", value.ToString());
        }

        [Test]
        public void TestToIntPositive()
        {
            var result = Value.ToInt("1234");
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(1234, result.Value);
        }

        [Test]
        public void TestToIntNegativeWithM()
        {
            var result = Value.ToInt("M05");
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(-5, result.Value);
        }

        [Test]
        public void TestToIntWithP()
        {
            var result = Value.ToInt("P10");
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(10, result.Value);
        }

        [Test]
        public void TestToIntNull()
        {
            var result = Value.ToInt("///");
            ClassicAssert.IsNull(result);
        }

        [Test]
        public void TestConstructor()
        {
            var value = new Value(42.5, Value.Unit.HectoPascal);
            ClassicAssert.AreEqual(42.5, value.ActualValue);
            ClassicAssert.AreEqual(Value.Unit.HectoPascal, value.ActualUnit);
        }

        [Test]
        public void TestUnsupportedConversionThrows()
        {
            var value = new Value(100, Value.Unit.DegreeCelsius);
            ClassicAssert.Throws<ArgumentException>(() =>
            {
                value.GetConvertedValue(Value.Unit.Meter);
            });
        }

        [Test]
        public void TestSameUnitConversion()
        {
            var value = new Value(1013.25, Value.Unit.HectoPascal);
            var converted = value.GetConvertedValue(Value.Unit.HectoPascal);
            ClassicAssert.AreEqual(1013.25f, converted);
        }

        [Test]
        public void TestMeterToStatuteMileConversion()
        {
            var value = new Value(1609.34, Value.Unit.Meter);
            var converted = value.GetConvertedValue(Value.Unit.StatuteMile);
            ClassicAssert.AreEqual(1f, converted);
        }

        [Test]
        public void TestKmhToMeterPerSecondConversion()
        {
            var value = new Value(3.6, Value.Unit.KilometerPerHour);
            var converted = value.GetConvertedValue(Value.Unit.MeterPerSecond);
            ClassicAssert.AreEqual(1f, converted);
        }
    }
}
