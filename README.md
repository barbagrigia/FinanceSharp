# FinanceSharp
High Performance Financial Graph Computation for Quant-Trading.

### Terminology
- DataValue: An object holding value/s.
- DataPoint: An object holding time (in milliseconds-epoch) and value/s.
- DataArray: An array holding a serie of DataValues or DataPoints.
- DataWindow: An 2d array of DataValues or DataPoints.
- Indicator: Transforms and returns new value/s. e.g. Tick/TradeBar to EMA.
- Consolidator: Consolidates values into new value/s. e.g. Ticks to TradeBar

### Copyrights
The library is based on modified code from [QuantConnect](https://github.com/QuantConnect/Lean), incredible library for full-stack trading algorithm development.
