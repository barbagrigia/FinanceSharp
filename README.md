# FinanceSharp
<p align="center">
  <img align="center" src="https://i.imgur.com/6601cb9.png" height=200  />
</p> 
Financial computation for technical analysis with a versatile and productive piping and consolidation techniques.
The library designed towards performance and complitability with running machine-learning algorithms in Python (via Pythonnet).

With FinanceSharp you can easily design a model within minutes to manipulate, normalize, consolidate the 
most complex technical indicator combinations models by hand with over 100 fully-tested indicators available.

## Notice!
The library is an on-going development and is before alpha. Classes and architecture might be changed and differ from docs.

## Key points
Our indicators follow a similar model to Pytorch, having a `Forward` method, input shape and output shape (2 dimensional).<br>
Our data architecture is:
- always pinnable and ready to be passed to Python as a [numpy](https://github.com/numpy/numpy) without cloning.
- Is always made of two dimensions, `(Count, Properties)`
    - Count represents number of items in the array with `N` Properties.
    - Every property is a single `double` so 4 Properties can be a `BarValue`. Most indicators return has 1 property.
- Supports `unmanaged structs`, `double[]`, `double[,]`, `double[][]`, fast scalar double, fast scalar struct and more.<br>
Our graphing techniques are pretty straight forward with C#'s built in `event` system. Although IL builder is planned.

With all the abilities mentioned above, this makes FinanceSharp performance-ready for all the tasks required
to execute a model and pass observation data to Python for machine-learning algorithm to work with or process.

## Getting Started
Have a look at [/examples](https://github.com/Nucs/FinanceSharp/blob/master/examples/FinanceSharp.Examples/Program.cs) directory for plotted examples.

## Architecture
FinanceSharp favors high productivity and complitability while still maintaining performant computation alogrithms.
We use an 2D array to represent both scalar, indicator value, trade value and tick.

#### Time
We use `long` to represent time as *1970-epoch-milliseconds* as baseline. 
Unlike in QuantConnect/Lean, our time value is always passed seperatly from the values of our data type.

All time processing are always interpreted as UTC. local time is ignored completely and always swallen and interpreted as UTC.
Converting methods are available in static `Time` class.

#### Values - DoubleArray
DoubleArray is an abstract representation of a 2 dimensional array shaped `(Count, Properties)` and is used in all of our implemented algorithms (e.g. indicators, crunchers).<br>
  `Count` - how many items are in the DoubleArray<br>
  `Properties` - how many `double` fields for every item (count) in the DoubleArray

The class aims to support in as many ways as possible data is stored in C#:
  - `double` scalar via `DoubleArrayScalar`
  - `double[]`, `double[,]`, `double[][]` via `DoubleArrayManaged` and `DoubleArray2DManaged`
  - `double*` and `length` via `DoubleArrayUnmanaged`
  - Structs - `where TStruct : unmanaged, DataStruct, ICloneable` via `DoubleArrayStruct` and `DoubleArrayStructScalar`
    - Any structure can be used to represent data in `DoubleArray` as long as all of it's fields are double (verified staticly during runtime).

The classes derived from DoubleArray can be initialized via their constructor or via `DoubleArray.From`.

`struct BarValue, struct TradeBarValue` are ordered `CHLO` instead of `OHLC` to make `Close` (index 0) complitable with `IndicatorValue` which uses its `Value` at index 0 making it possible 
to pass `CHLO` values to a indicators that accept `IndicatorValue` that has only one property.

#### IIndicator : IUpdatable
Every indicator implements both IIndicator and IUpdatable. 
//TODO: elaborate

## Implemented Algorithms
<details> 
  <summary>Indicators List</summary>
 - AbsolutePriceOscillator<br>
 - AccelerationBands<br>
 - AccumulationDistribution<br>
 - AccumulationDistributionOscillator<br>
 - ArnaudLegouxMovingAverage<br>
 - AroonOscillator<br>
 - AverageDirectionalIndex<br>
 - AverageDirectionalMovementIndexRating<br>
 - AverageTrueRange<br>
 - BalanceOfPower<br>
 - Belash<br>
 - BollingerBands<br>
 - ChandeMomentumOscillator<br>
 - CommodityChannelIndex<br>
 - CompositeIndicator<br>
 - ConstantIndicator<br>
 - CoppockCurve<br>
 - Delay<br>
 - DetrendedPriceOscillator<br>
 - DonchianChannel<br>
 - DoubleExponentialMovingAverage<br>
 - ExponentialMovingAverage<br>
 - FilteredIdentity<br>
 - FisherTransform<br>
 - FractalAdaptiveMovingAverage<br>
 - FunctionalComparingIndicator<br>
 - FunctionalIndicator<br>
 - HeikinAshi<br>
 - HullMovingAverage<br>
 - IchimokuKinkoHyo<br>
 - IndicatorExtensions<br>
 - IndicatorResult<br>
 - IndicatorStatus<br>
 - IntradayVwap<br>
 - KaufmanAdaptiveMovingAverage<br>
 - KeltnerChannels<br>
 - LeastSquaresMovingAverage<br>
 - LinearWeightedMovingAverage<br>
 - LogReturn<br>
 - MassIndex<br>
 - Maximum<br>
 - MaximumDeltaIndicator<br>
 - MeanAbsoluteDeviation<br>
 - MidPoint<br>
 - MidPrice<br>
 - Minimum<br>
 - MinimumDeltaIndicator<br>
 - Momentum<br>
 - MomentumPercent<br>
 - Momersion<br>
 - MoneyFlowIndex<br>
 - MovingAverageConvergenceDivergence<br>
 - MovingAverageType<br>
 - MovingAverageTypeExtensions<br>
 - NormalizedAverageTrueRange<br>
 - OnBalanceVolume<br>
 - ParabolicStopAndReverse<br>
 - PercentagePriceOscillator<br>
 - PeriodlessMaximum<br>
 - PeriodlessMinimum<br>
 - RateOfChange<br>
 - RateOfChangePercent<br>
 - RateOfChangeRatio<br>
 - RegressionChannel<br>
 - RelativeStrengthIndex<br>
 - SimpleMovingAverage<br>
 - StandardDeviation<br>
 - Stochastics<br>
 - Sum<br>
 - SwissArmyKnife<br>
 - T3MovingAverage<br>
 - TriangularMovingAverage<br>
 - TripleExponentialMovingAverage<br>
 - Trix<br>
 - TrueRange<br>
 - UltimateOscillator<br>
 - Variance<br>
 - VolumeWeightedAveragePriceIndicator<br>
 - WilderMovingAverage<br>
 - WilliamsPercentR
</details>
<details> 
  <summary>Consolidators List</summary>
 - FilteredIdentityDataConsolidator<br>
 - RenkoConsolidator<br>
 - SequentialConsolidator<br>
 - TickConsolidator<br>
 - TradeBarConsolidator
</details>

<details> 
  <summary>Candlestick Patterns List</summary>
 - AbandonedBaby<br>
 - AdvanceBlock<br>
 - BeltHold<br>
 - Breakaway<br>
 - CandleEnums<br>
 - CandleSettings<br>
 - ClosingMarubozu<br>
 - ConcealedBabySwallow<br>
 - Counterattack<br>
 - DarkCloudCover<br>
 - Doji<br>
 - DojiStar<br>
 - DragonflyDoji<br>
 - Engulfing<br>
 - EveningDojiStar<br>
 - EveningStar<br>
 - GapSideBySideWhite<br>
 - GravestoneDoji<br>
 - Hammer<br>
 - HangingMan<br>
 - Harami<br>
 - HaramiCross<br>
 - HighWaveCandle<br>
 - Hikkake<br>
 - HikkakeModified<br>
 - HomingPigeon<br>
 - IdenticalThreeCrows<br>
 - InNeck<br>
 - InvertedHammer<br>
 - Kicking<br>
 - KickingByLength<br>
 - LadderBottom<br>
 - LongLeggedDoji<br>
 - LongLineCandle<br>
 - Marubozu<br>
 - MatchingLow<br>
 - MatHold<br>
 - MorningDojiStar<br>
 - MorningStar<br>
 - OnNeck<br>
 - Piercing<br>
 - RickshawMan<br>
 - RiseFallThreeMethods<br>
 - SeparatingLines<br>
 - ShootingStar<br>
 - ShortLineCandle<br>
 - SpinningTop<br>
 - StalledPattern<br>
 - StickSandwich<br>
 - Takuri<br>
 - TasukiGap<br>
 - ThreeBlackCrows<br>
 - ThreeInside<br>
 - ThreeLineStrike<br>
 - ThreeOutside<br>
 - ThreeStarsInSouth<br>
 - ThreeWhiteSoldiers<br>
 - Thrusting<br>
 - Tristar<br>
 - TwoCrows<br>
 - UniqueThreeRiver<br>
 - UpDownGapThreeMethods<br>
 - UpsideGapTwoCrows
</details>

## I Come From QuantConnect/Lean
We support all indicators and consolidators from QuantConnect/Lean. <br>
Key differences are:
  - IndicatorBase and DataConsolidator are non generic.
  - Time is passed separately from data.
  - Instead of IndicatorDataPoint/TradeBar/RenkoBar/Tick, DoubleArray is used.
  - decimal has been replaced with double to allow interopability with other languages without casting from decimal to other type.
  - DataConsolidator inheriets IIndicator
  - All original indicator tests from Lean are passing so any consolidation code from Lean can be used with FinanceSharp.

### Copyrights
The library is based on modified code from [QuantConnect](https://github.com/QuantConnect/Lean), incredible library for full-stack trading algorithm development.
