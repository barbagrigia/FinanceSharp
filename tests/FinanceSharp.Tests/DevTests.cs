using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FinanceSharp.Indicators;
using FinanceSharp;
using FluentAssertions;
using NUnit.Framework;

namespace FinanceSharp.Tests {
    public class DevTests {
        [Test]
        public unsafe void Dev() {
            const int size = 50_000_000;
            var a = new DoubleArrayUnmanaged(60_000_000, 1, true);
            for (int i = 0; i < size; i++) {
                a.Address[i] = i;
            }
            
            Unsafe.CopyBlock(a.Address + 10_000_000, a.Address, (uint) (sizeof(double) * (size) * a.Properties));

            for (int i = 10_000_000; i < 60_000_000; i++) {
                a.Address[i].Should().Be(i - 10_000_000, "index " + i);
            }
        }

        void Print(DoubleArray arr) {
            if (arr.Properties == 1) {
                Console.Write($"[");
                for (int i = 0; i < arr.Count; i++) {
                    Console.Write($"{arr[i, 0]}");
                    if (i != arr.Count - 1) {
                        Console.Write($", ");
                    }
                }

                Console.Write($"]");
            } else {
                Console.Write($"[");
                for (int i = 0; i < arr.Count; i++) {
                    Console.Write($"[");
                    for (int j = 0; j < arr.Properties; j++) {
                        Console.Write($"{arr[i, j]}");
                        if (i != arr.Count - 1) {
                            Console.Write($", ");
                        }
                    }

                    Console.Write($"]");

                    if (i != arr.Count - 1) {
                        Console.Write($", ");
                    }
                }

                Console.Write($"]");
            }

            Console.WriteLine();
        }
    }
}