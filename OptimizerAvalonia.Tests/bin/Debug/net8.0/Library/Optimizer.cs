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
        public readonly List<OptimizedData> OptimizedData = new();

        public Optimizer(List<IBoiler> boilers, List<SourceData> demandData)
        {
            Boilers = boilers;
            DemandData = demandData;
        }

        public void CalculateOptimalHeatProduction(bool OptimizeEmissions)
        {
            foreach (var demand in DemandData)
            {
                Console.WriteLine($"Calculating optimal heat production for demand from {demand.StartTime} to {demand.EndTime}.");

                double totalHeatDemand = demand.HeatDemand;
                double totalProductionCost = 0;
                double totalEmissions = 0;
                var boilerProductions = new List<BoilerProduction>();

                CalculateProductionCosts(demand); // Calculate production costs for this iteration
                
                
                // Sort the boilers based on the OptimizeEmissions
                var sortedBoilers = OptimizeEmissions 
                    // If OptimizeEmissions sort the boilers by the Emissions 
                    ? Boilers.OrderBy(b => b.Emissions) 
                    // Else sort the boilers by the ProductionCost
                    : Boilers.OrderBy(b => b.ProductionCost);

                foreach (var boiler in sortedBoilers)
                {
                    if (demand.HeatDemand <= boiler.MaxHeat)
                    {
                        if (demand.HeatDemand <= 1)
                        {
                            totalProductionCost += boiler.ProductionCost;
                            totalEmissions += boiler.Emissions;
                            demand.HeatDemand = 0;
                            boilerProductions.Add(new BoilerProduction
                            {
                                BoilerName = boiler.Name,
                                HeatProduced = 1
                            });
                            Console.WriteLine($"Turning on {boiler.Name} to produce 1 MW of heat with effective cost {boiler.ProductionCost}.");
                            break;
                        }
                        totalProductionCost += demand.HeatDemand * boiler.ProductionCost;
                        totalEmissions += demand.HeatDemand * boiler.Emissions;
                        demand.HeatDemand = Math.Round(demand.HeatDemand, 2);
                        boilerProductions.Add(new BoilerProduction
                        {
                            BoilerName = boiler.Name,
                            HeatProduced = demand.HeatDemand
                        });
                        Console.WriteLine($"Turning on {boiler.Name} to produce {demand.HeatDemand} MW of heat with effective cost {boiler.ProductionCost} and effective emissions {boiler.Emissions}.");
                        demand.HeatDemand = 0;
                        break;
                    }

                    totalProductionCost += boiler.ProductionCost * boiler.MaxHeat;
                    totalEmissions += boiler.Emissions * boiler.MaxHeat;
                    demand.HeatDemand -= boiler.MaxHeat;
                    boilerProductions.Add(new BoilerProduction
                    {
                        BoilerName = boiler.Name,
                        HeatProduced = boiler.MaxHeat
                    });
                    Console.WriteLine($"Turning on {boiler.Name} to produce {boiler.MaxHeat} MW of heat with effective cost {boiler.ProductionCost} and effective emissions {boiler.Emissions}..");
                }

                if (demand.HeatDemand > 0)
                {
                    // Handle insufficient capacity
                    Console.WriteLine($"Insufficient capacity to meet the demand for interval {demand.StartTime} to {demand.EndTime}");
                }

                Console.WriteLine($"Total production cost  and total Emissions for interval {demand.StartTime} to {demand.EndTime}: {totalProductionCost}, {totalEmissions}");
                Console.WriteLine();

                ResetProductionCosts(demand); // Reset production costs for next iteration

                OptimizedData.Add(new OptimizedData
                {
                    StartTime = demand.StartTime,
                    EndTime = demand.EndTime,
                    TotalProductionCost = Math.Round(totalProductionCost, 2),
                    Emissions = Math.Round(totalEmissions, 2),
                    HeatDemand = totalHeatDemand,
                    BoilerProductions = boilerProductions,
                    ElectricityPrice = demand.ElectricityPrice
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