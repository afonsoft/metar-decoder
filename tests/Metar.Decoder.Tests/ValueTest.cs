using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;

namespace Metar.Decoder_tests
{
    /// <summary>
    /// ValueTest
    /// </summary>
    [TestFixture, Category("Entity")]
    public class ValueTest
    {
        /// <summary>
        /// TestValueConversion
        /// </summary>
        /// <param name="valueToTest"></param>
        [Test, TestCaseSource("ValidValues")]
        public void TestValueConversion(Tuple<float, Value.Unit, float, Value.Unit> valueToTest)
        {
            var value = new Value(valueToTest.Item1, valueToTest.Item2);
            ClassicAssert.That(value.GetConvertedValue(valueToTest.Item4), Is.EqualTo(valueToTest.Item3));
        }

        /// <summary>
        /// TestValueNoRateException
        /// </summary>
        /// <param name="valueToTest"></param>
        [Test, TestCaseSource("ValueNoRateException")]
        public void TestValueNoRateException(Tuple<float, Value.Unit, float, Value.Unit> valueToTest)
        {
            var value = new Value(valueToTest.Item1, valueToTest.Item2);
            ClassicAssert.Throws(typeof(ArgumentException), () =>
             {
                 value.GetConvertedValue(valueToTest.Item4);
             });
        }

        /// <summary>
        /// TestValueNoRateExceptionMessage
        /// </summary>
        /// <param name="valueToTest"></param>
        [Test, TestCaseSource("ValueNoRateException")]
        public void TestValueNoRateExceptionMessage(Tuple<float, Value.Unit, float, Value.Unit> valueToTest)
        {
            var hasException = false;
            try
            {
                var value = new Value(valueToTest.Item1, valueToTest.Item2);
                value.GetConvertedValue(valueToTest.Item4);
            }
            catch (ArgumentException ex)
            {
                hasException = true;
                ClassicAssert.That(ex.Message, Is.EqualTo($@"Conversion rate between ""{valueToTest.Item4}"" and ""{valueToTest.Item2}"" is not defined."));
            }
            ClassicAssert.IsTrue(hasException, "An exception should have been thrown.");
        }

        #region TestCaseSources

        /// <summary>
        /// ValidValues
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<float, Value.Unit, float, Value.Unit>> ValidValues()
        {
            return new List<Tuple<float, Value.Unit, float, Value.Unit>>()
            {
                new Tuple<float, Value.Unit, float, Value.Unit>(1000f, Value.Unit.HectoPascal, 29.53f, Value.Unit.MercuryInch),
                new Tuple<float, Value.Unit, float, Value.Unit>(2.02f, Value.Unit.MercuryInch, 68.405f, Value.Unit.HectoPascal),
                new Tuple<float, Value.Unit, float, Value.Unit>(800f, Value.Unit.Meter, 2624.672f, Value.Unit.Feet),
                new Tuple<float, Value.Unit, float, Value.Unit>(5000f, Value.Unit.Feet, 1524f, Value.Unit.Meter),
                new Tuple<float, Value.Unit, float, Value.Unit>(2500f, Value.Unit.Meter, 1.553f, Value.Unit.StatuteMile),
                new Tuple<float, Value.Unit, float, Value.Unit>(1f, Value.Unit.MeterPerSecond, 1.944f, Value.Unit.Knot),
                new Tuple<float, Value.Unit, float, Value.Unit>(99f, Value.Unit.Knot, 50.93f, Value.Unit.MeterPerSecond),
                new Tuple<float, Value.Unit, float, Value.Unit>(3f, Value.Unit.KilometerPerHour, 1.620f, Value.Unit.Knot),
                new Tuple<float, Value.Unit, float, Value.Unit>(500f, Value.Unit.Meter, 500f, Value.Unit.Meter),
                new Tuple<float, Value.Unit, float, Value.Unit>(50f, Value.Unit.Feet, 50f, Value.Unit.Feet),
            };
        }

        /// <summary>
        /// ValueNoRateException
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<float, Value.Unit, float, Value.Unit>> ValueNoRateException()
        {
            return new List<Tuple<float, Value.Unit, float, Value.Unit>>()
            {
                new Tuple<float, Value.Unit, float, Value.Unit>(3f, Value.Unit.KilometerPerHour, 50f, Value.Unit.Feet),
            };
        }

        /// <summary>
        /// ValueUnsupportedException
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<string, int, string, string>> ValueUnsupportedException()
        {
            return new List<Tuple<string, int, string, string>>()
            {
                new Tuple<string, int, string, string>("271035Z AAA", 27, "10:35", "AAA"),
                new Tuple<string, int, string, string>("012342Z BBB", 1,  "23:42", "BBB"),
                new Tuple<string, int, string, string>("311200Z CCC", 31, "12:00", "CCC"),
            };
        }

        #endregion TestCaseSources
    }
}