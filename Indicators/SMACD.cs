//
// Copyright (C) 2020, Neil McGuire
// MIT License
//
#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

namespace NinjaTrader.NinjaScript.Indicators
{
	/// <summary>
	/// The SMACD (Simple Moving Average Convergence/Divergence) is a trend following momentum indicator
	/// that shows the relationship between two moving averages of prices. It is based on the NinjaTrader MACD,
    /// with the exception that it uses a Fast and Slow SMA instead of an EMA.
	/// </summary>
	public class SMACD : Indicator
	{
		private	SMA fastMA;
		private	SMA slowMA;
		private SMA signal;
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					= "SMA-based MACD";
				Name						= "SMACD";
				Fast						= 10;
				IsSuspendedWhileInactive	= true;
				Slow						= 30;
				Smooth						= 5;

				AddPlot(Brushes.DarkCyan,									NinjaTrader.Custom.Resource.NinjaScriptIndicatorNameMACD);
				AddPlot(Brushes.Crimson,									NinjaTrader.Custom.Resource.NinjaScriptIndicatorAvg);
				AddPlot(new Stroke(Brushes.DodgerBlue, 2),	PlotStyle.Bar,	NinjaTrader.Custom.Resource.NinjaScriptIndicatorDiff);
				AddLine(Brushes.DarkGray,					0,				NinjaTrader.Custom.Resource.NinjaScriptIndicatorZeroLine);
			}
			else if (State == State.DataLoaded)
			{
				fastMA = SMA(Fast);
				slowMA = SMA(Slow);
				signal = SMA(Value, Smooth);
			}
		}

		protected override void OnBarUpdate()
		{
			double input0	= Input[0];

			if (CurrentBar < Slow + Smooth)
			{
				Value[0]		= 0;
				Avg[0]			= 0;
				Diff[0]			= 0;
			}
			else
			{
				Value[0]		= fastMA[0] - slowMA[0];
				Avg[0]			= signal[0];
				Diff[0]			= Value[0] - Avg[0];
			}
		}

		#region Properties
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Avg
		{
			get { return Values[1]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Default
		{
			get { return Values[0]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Diff
		{
			get { return Values[2]; }
		}

		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Fast", GroupName = "NinjaScriptParameters", Order = 0)]
		public int Fast
		{ get; set; }

		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Slow", GroupName = "NinjaScriptParameters", Order = 1)]
		public int Slow
		{ get; set; }

		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Smooth", GroupName = "NinjaScriptParameters", Order = 2)]
		public int Smooth
		{ get; set; }
		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private SMACD[] cacheSMACD;
		public SMACD SMACD(int fast, int slow, int smooth)
		{
			return SMACD(Input, fast, slow, smooth);
		}

		public SMACD SMACD(ISeries<double> input, int fast, int slow, int smooth)
		{
			if (cacheSMACD != null)
				for (int idx = 0; idx < cacheSMACD.Length; idx++)
					if (cacheSMACD[idx] != null && cacheSMACD[idx].Fast == fast && cacheSMACD[idx].Slow == slow && cacheSMACD[idx].Smooth == smooth && cacheSMACD[idx].EqualsInput(input))
						return cacheSMACD[idx];
			return CacheIndicator<SMACD>(new SMACD(){ Fast = fast, Slow = slow, Smooth = smooth }, input, ref cacheSMACD);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.SMACD SMACD(int fast, int slow, int smooth)
		{
			return indicator.SMACD(Input, fast, slow, smooth);
		}

		public Indicators.SMACD SMACD(ISeries<double> input , int fast, int slow, int smooth)
		{
			return indicator.SMACD(input, fast, slow, smooth);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.SMACD SMACD(int fast, int slow, int smooth)
		{
			return indicator.SMACD(Input, fast, slow, smooth);
		}

		public Indicators.SMACD SMACD(ISeries<double> input , int fast, int slow, int smooth)
		{
			return indicator.SMACD(input, fast, slow, smooth);
		}
	}
}

#endregion
