using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using FinanceSharp.Tests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests.Data {
    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    public abstract partial class DoubleArrayBaseTests {
        //TODO: test property specific Function.
        public abstract DoubleArray CreateDefault();
        public abstract DoubleArray CreateScalar1_1(double value1);
        public abstract DoubleArray CreateScalar1_2(double value1, double value2);
        public abstract DoubleArray CreateArray2_1(double value1, double value2);
        public abstract DoubleArray CreateScalar1_4(double value1, double value2, double value3, double value4);
        public abstract DoubleArray CreateArray4_1(double value1, double value2, double value3, double value4);
        public abstract DoubleArray CreateMatrix2_2(double value1, double value2, double value3, double value4);

        public virtual DoubleArray CreateScalar1_4((double value, double value2, double value3, double value4) data) {
            return CreateScalar1_4(data.value, data.value2, data.value3, data.value4);
        }

        public virtual DoubleArray CreateArray4_1((double value, double value2, double value3, double value4) data) {
            return CreateArray4_1(data.value, data.value2, data.value3, data.value4);
        }

        public virtual DoubleArray CreateMatrix2_2((double value, double value2, double value3, double value4) data) {
            return CreateMatrix2_2(data.value, data.value2, data.value3, data.value4);
        }

        [Test]
        public void DefaultConstructor() {
            CreateDefault()[0].Should().Be(0d);
        }

        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public void Indexing(double expected) {
            var arr = CreateScalar1_1(expected);
            arr[0].Should().Be(expected);
            arr[0, 0].Should().Be(expected);
        }

        [Test]
        [TestCaseSource(nameof(DuoDataSet))]
        public void Indexing(double expected, double expected2) {
            var arr = CreateScalar1_2(expected, expected2);
            arr[0].Should().Be(expected);
            arr[1].Should().Be(expected2);
            arr[0, 0].Should().Be(expected);
            arr[0, 1].Should().Be(expected2);
        }

        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public void Get(double expected) {
            var arr = CreateScalar1_1(expected);
            arr.Get<IndicatorValue>(0).Value.Should().Be(expected);
        }

        [Test]
        [TestCaseSource(nameof(DuoDataSet))]
        public void Get2DScalar(double expected, double expected2) {
            DoubleArray arr;

            arr = CreateScalar1_2(expected, expected2);
            var str = arr.Get<TestStructX2>(0);
            str.Value.Should().Be(expected);
            str.AnotherValue.Should().Be(expected2);
        }

        [Test]
        [TestCaseSource(nameof(DuoDataSet))]
        public void Get2DArray(double expected, double expected2) {
            DoubleArray arr;

            arr = CreateArray2_1(expected, expected2);
            var str = arr.Get<IndicatorValue>(0);
            str.Value.Should().Be(expected);

            var str2 = arr.Get<IndicatorValue>(1);
            str2.Value.Should().Be(expected2);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ToDoubleArray(double value1, double value2, double value3, double value4) {
            var arr = CreateArray4_1(value1, value2, value3, value4);

            arr.ToArray().Should().BeEquivalentTo(value1, value2, value3, value4);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ToStructArray_Reinterpret(double value1, double value2, double value3, double value4) {
            var arr = CreateMatrix2_2(value1, value2, value3, value4);

            arr.ToArray<TestStructX2>(ArrayConversionMethod.Reinterpret).Should()
                .BeEquivalentTo(
                    new TestStructX2(value1, value2),
                    new TestStructX2(value3, value4)
                );
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ToStructArray_Cast(double value1, double value2, double value3, double value4) {
            var arr = CreateMatrix2_2(value1, value2, value3, value4);
            var converted = arr.ToArray<TestStructX2>(ArrayConversionMethod.Cast, 2);
            converted.Length.Should().Be(2);

            converted[0].Value.Should().BeApproximately(value1);
            converted[0].AnotherValue.Should().BeApproximately(value2);
            converted[1].Value.Should().BeApproximately(value3);
            converted[1].AnotherValue.Should().BeApproximately(value4);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ToStructArray_Cast_Hopped(double value1, double value2, double value3, double value4) {
            var arr = CreateMatrix2_2(value1, value2, value3, value4);
            var converted = arr.ToArray<TestStructX2>(ArrayConversionMethod.Cast, 1);
            converted.Length.Should().Be(4);
            converted.All(s => Math.Abs(s.AnotherValue) < Constants.ZeroEpsilon).Should().BeTrue();
            converted[0].Value.Should().BeApproximately(value1);
            converted[1].Value.Should().BeApproximately(value2);
            converted[2].Value.Should().BeApproximately(value3);
            converted[3].Value.Should().BeApproximately(value4);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ForEach(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var subject = CreateMatrix2_2(value1, value2, value3, value4);

            var l = new List<double>();
            subject.ForEach(value => l.Add(value));
            l.Should().BeEquivalentTo(values);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ForEachReference(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var subject = CreateMatrix2_2(value1, value2, value3, value4);

            var l = new List<double>();
            subject.ForEach((ref double value) => l.Add(value));
            l.Should().BeEquivalentTo(values);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Function_Binary(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var arr = CreateMatrix2_2(value1, value2, value3, value4);

            var subject = arr.Function(value => value);
            ReferenceEquals(arr, subject).Should().BeFalse();
            values.ToArray().Should().BeEquivalentTo(subject.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Function_Unary(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var arr = CreateMatrix2_2(value1, value2, value3, value4);

            var subject = arr.Function(arr, (v1, v2) => {
                v1.Should().BeApproximately(v2);
                return v1;
            });
            ReferenceEquals(arr, subject).Should().BeFalse();
            values.ToArray().Should().BeEquivalentTo(subject.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Function_Unary_LhsScalar(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var lhs = CreateMatrix2_2(value1, value2, value3, value4);
            var rhs = CreateScalar1_1(value1);

            var subject = lhs.Function(rhs, (v1, v2) => { return v1; });

            ReferenceEquals(lhs, subject).Should().BeFalse();
            values.ToArray().Should().BeEquivalentTo(subject.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Function_Unary_RhsScalar(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1, value2, value3, value4};
            var lhs = CreateMatrix2_2(value1, value2, value3, value4);
            var rhs = CreateScalar1_1(value1);

            var subject = rhs.Function(lhs, (v1, v2) => { return v2; });

            ReferenceEquals(lhs, subject).Should().BeFalse();
            values.ToArray().Should().BeEquivalentTo(subject.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public void Function_Unary_BothScalar(double value1) {
            var values = new double[] {value1};
            var lhs = CreateScalar1_1(value1);
            var rhs = CreateScalar1_1(value1);

            var subject = rhs.Function(lhs, (v1, v2) => { return v2; });

            ReferenceEquals(lhs, subject).Should().BeFalse();
            values.ToArray().Should().BeEquivalentTo(subject.ToArray());
        }

        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public void Accessors_Scalar(double value1) {
            var values = new double[] {value1};
            var subject = CreateScalar1_1(value1);
            subject[0].Should().BeApproximately(value1);
            subject[0, 0].Should().BeApproximately(value1);

            subject[0] = 1d;
            subject[0].Should().BeApproximately(1d);

            subject[0, 0] = 2d;
            subject[0, 0].Should().BeApproximately(2d);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Accessors_Matrix(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1};
            var subject = CreateMatrix2_2(value1, value2, value3, value4);
            subject[0, 0].Should().BeApproximately(value1);

            subject[0, 1] = 2d;
            subject[0, 1].Should().BeApproximately(2d);

            subject[1, 1] = value1;
            subject[1, 1].Should().BeApproximately(value1);
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void Accessors_SetLinear(double value1, double value2, double value3, double value4) {
            var values = new double[] {value1};
            var subject = CreateMatrix2_2(value1, value2, value3, value4);
            subject[0, 0].Should().BeApproximately(value1);

            subject.SetLinear(1, 2d);
            subject[0, 1].Should().BeApproximately(2d);

            subject.SetLinear(3, value1);
            subject[1, 1].Should().BeApproximately(value1);
        }

        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public void Accessors_SetLinear_Scalar(double value1) {
            var values = new double[] {value1};
            var subject = CreateScalar1_1(value1);

            subject[0, 0].Should().BeApproximately(value1);
            subject.GetLinear(0).Should().BeApproximately(value1);

            subject.SetLinear(0, 2d);
            subject[0, 0].Should().BeApproximately(2d);

            subject.SetLinear(0, value1);
            subject[0].Should().BeApproximately(value1);
        }


        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public unsafe void Fixing(double expected) {
            var arr = CreateScalar1_1(expected);
            fixed (double* ptr = arr) {
                ptr[0].Should().Be(expected);
                ptr[0] = 1d;
            }

            arr[0].Should().BeApproximately(1d);
        }


        [Test]
        [TestCaseSource(nameof(DuoDataSet))]
        public void ReshapeArray2_1(double value1, double value2) {
            DoubleArray arr;

            arr = CreateArray2_1(value1, value2);
            var ret = arr.Reshape(1, 2, copy: true);

            ret.Count.Should().Be(1);
            ret.Properties.Should().Be(2);
            ret.LinearLength.Should().Be(2);
            (arr == ret).Should().BeFalse();
        }

        [Test]
        [TestCaseSource(nameof(QuadDataSet))]
        public void ReshapeMatrix2_2(double value1, double value2, double value3, double value4) {
            DoubleArray arr, ret;

            arr = CreateMatrix2_2(value1, value2, value3, value4);
            ret = arr.Reshape(2, 2, copy: true);

            ret.Count.Should().Be(2);
            ret.Properties.Should().Be(2);
            ret.LinearLength.Should().Be(4);
            (arr == ret).Should().BeTrue();

            ret = arr.Reshape(1, 4, copy: true);

            ret.Count.Should().Be(1);
            ret.Properties.Should().Be(4);
            ret.LinearLength.Should().Be(4);
            (arr == ret).Should().BeFalse();


            ret = arr.Reshape(4, 1, copy: true);

            ret.Count.Should().Be(4);
            ret.Properties.Should().Be(1);
            ret.LinearLength.Should().Be(4);
            (arr == ret).Should().BeFalse();
        }


        [Test]
        [TestCaseSource(nameof(ScalarDataSet))]
        public unsafe void ReshapeScalar(double value) {
            var arr = CreateScalar1_1(value);

            var ret = arr.Reshape(1, 1, copy: true);

            ret.Count.Should().Be(1);
            ret.Properties.Should().Be(1);
            ret.LinearLength.Should().Be(1);
            (arr == ret).Should().BeTrue();
        }
    }
}