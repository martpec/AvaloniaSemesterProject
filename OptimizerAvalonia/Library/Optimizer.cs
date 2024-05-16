using HeatProductionOptimization.Models;
using HeatProductionOptimization.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HeatProductionOptimization
{
public class Optimizer
{
    private readonly List<IBoiler> Boilers;
    private readonly List<SourceData> DemandData;
    private readonly List<OptimizedData> OptimizedData = new List<OptimizedData>();

    public Optimizer(List<IBoiler> boilers, List<SourceData> demandData)
    {
        Boilers = boilers;
        DemandData = demandData;
    }

    public void CalculateOptimalHeatProduction()
    {
        foreach (var demand in DemandData)
        {
            Console.WriteLine($"Calculating optimal heat production for demand from {demand.StartTime} to {demand.EndTime}.");

            double totalProductionCost = 0;
            var boilerProductions = new List<BoilerProduction>();

            CalculateProductionCosts(demand); // Calculate production costs for this iteration

            var sortedBoilers = Boilers.OrderBy(b => b.ProductionCost);

            foreach (var boiler in sortedBoilers)
            {
                double heatToProduce = Math.Min(boiler.MaxHeat, demand.HeatDemand);

                if (heatToProduce > 0)
                {
                    double cost = heatToProduce * boiler.ProductionCost;
                    totalProductionCost += cost;

                    boilerProductions.Add(new BoilerProduction
                    {
                        BoilerName = boiler.Name,
                        HeatProduced = Math.Round(heatToProduce, 2)
                    });

                    Console.WriteLine($"Turning on {boiler.Name} to produce {Math.Round(heatToProduce, 2)} MW of heat with effective cost {boiler.ProductionCost}.");
                    demand.HeatDemand -= heatToProduce;
                }

                if (demand.HeatDemand <= 0)
                {
                    break;
                }
            }

            if (demand.HeatDemand > 0)
            {
                // Handle insufficient capacity
                Console.WriteLine($"Insufficient capacity to meet the demand for interval {demand.StartTime} to {demand.EndTime}");
            }

            Console.WriteLine($"Total production cost for interval {demand.StartTime} to {demand.EndTime}: {totalProductionCost}");
            Console.WriteLine();

            ResetProductionCosts(demand); // Reset production costs for next iteration

            OptimizedData.Add(new OptimizedData
            {
                StartTime = demand.StartTime,
                EndTime = demand.EndTime,
                TotalProductionCost = Math.Round(totalProductionCost, 2),
                Emissions = Boilers.Sum(b => b.Emissions),
                BoilerProductions = boilerProductions
            });
            foreach (var boiler in boilerProductions)
            {
                Console.WriteLine($"{boiler.BoilerName} produced: {boiler.HeatProduced}");
            }
        }
    }
        
        private void CalculateProductionCosts(SourceData demand)
        {
            foreach (var boiler in Boilers)
            {
                if (boiler is ElectricBoiler electricBoiler)
                {
                    electricBoiler.ProductionCost += demand.ElectricityPrice;
                    electricBoiler.ProductionCost = Math.Round(electricBoiler.ProductionCost, 2);
                }

                else if (boiler is GasMotor gasMotor)
                {
                    gasMotor.ProductionCost -= (gasMotor.ElectricityProduced / gasMotor.MaxHeat) * demand.ElectricityPrice;
                    gasMotor.ProductionCost = Math.Round(gasMotor.ProductionCost, 2);
                }
            }
        }

        private void ResetProductionCosts(SourceData demand)
        {
            foreach (var boiler in Boilers)
            {
                if (boiler is ElectricBoiler electricBoiler)
                {
                    electricBoiler.ProductionCost -= demand.ElectricityPrice;
                    electricBoiler.ProductionCost = Math.Round(electricBoiler.ProductionCost, 2);
                }

                if (boiler is GasMotor gasMotor)
                {
                    gasMotor.ProductionCost += (gasMotor.ElectricityProduced / gasMotor.MaxHeat) * demand.ElectricityPrice;
                    gasMotor.ProductionCost = Math.Round(gasMotor.ProductionCost, 2);
                }
            }
        }
    }
}
