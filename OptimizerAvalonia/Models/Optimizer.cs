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
                System.Console.WriteLine($"Calculating optimal heat production for demand from {demand.StartTime} to {demand.EndTime}.");

                double totalProductionCost = 0;
                CalculateProductionCosts(demand); // Calculate production costs for this iteration

                var sortedBoilers = Boilers.OrderBy(b => b.ProductionCost);

                foreach (var boiler in sortedBoilers)
                {
                    //double heatToProduce = Math.Min(boiler.MaxHeat, demand.HeatDemand);

                    if (demand.HeatDemand <= boiler.MaxHeat)
                    {
                        // boiler cannot produce less than one MW
                        if (demand.HeatDemand <= 1)
                        {
                            totalProductionCost += boiler.ProductionCost;
                            demand.HeatDemand = 0;
                            Console.WriteLine($"Turning on {boiler.Name} to produce 1 MW of heat with effective cost {boiler.ProductionCost}.");
                            break;
                        }
                        totalProductionCost += demand.HeatDemand * boiler.ProductionCost;
                        demand.HeatDemand = Math.Round(demand.HeatDemand, 2);
                        Console.WriteLine($"Turning on {boiler.Name} to produce {demand.HeatDemand} MW of heat with effective cost {boiler.ProductionCost}.");
                        demand.HeatDemand = 0;
                        break;
                    }

                    // vyše zreteľne zobraziť, že sa spustí kotol
                    totalProductionCost += boiler.ProductionCost * boiler.MaxHeat;
                    demand.HeatDemand -= boiler.MaxHeat;
                    Console.WriteLine($"Turning on {boiler.Name} to produce {boiler.MaxHeat} MW of heat with effective cost {boiler.ProductionCost}.");
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
                    ActivatedBoilers = Boilers.Where(b => b.MaxHeat > 0).ToList()
                });
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
