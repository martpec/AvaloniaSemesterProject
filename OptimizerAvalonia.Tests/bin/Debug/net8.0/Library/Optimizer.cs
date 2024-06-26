using HeatProductionOptimization.Models;
using HeatProductionOptimization.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HeatProductionOptimization;

public class Optimizer
{
    // Activated _boilers
    private readonly List<IBoiler> _boilers;

    // Stores data used for optimization
    private readonly List<SourceData> _demandData;

    // Stores final calculated data
    public List<OptimizedData> OptimizedData = new();

    public Optimizer(List<IBoiler> boilers, List<SourceData> demandData)
    {
        _boilers = boilers;
        _demandData = demandData;
    }

    public void CalculateOptimalHeatProduction(bool optimizeEmissions)
    {
        // Goes through each time slice
        foreach (var demand in _demandData)
        {
            Console.WriteLine(
                $"Calculating optimal heat production for demand from {demand.StartTime} to {demand.EndTime}.");

            double totalHeatDemand = demand.HeatDemand;
            double totalProductionCost = 0;
            double totalEmissions = 0;
            var boilerProductions = new List<BoilerProduction>();

            CalculateProductionCosts(demand); // Calculate production costs for this iteration


            // Sort the boilers based on the OptimizeEmissions
            var sortedBoilers = optimizeEmissions
                // If OptimizeEmissions sort the boilers by the Emissions 
                ? _boilers.OrderBy(b => b.Emissions)
                // Else sort the boilers by the ProductionCost
                : _boilers.OrderBy(b => b.ProductionCost);

            int counter = 0;
            foreach (var boiler in sortedBoilers)
            {
                // Checks if boiler can produce enough heat for the demand
                if (demand.HeatDemand <= boiler.MaxHeat)
                {
                    // Checks if the demand is less than 1 (Boiler cannot produce less than 1 MW)
                    if (demand.HeatDemand <= 1)
                    {
                        // How much should be subtracted from previous boiler to make the current boiler produce 1 MW
                        double decreaseFromPreviousBoiler = 1 - demand.HeatDemand;

                        // Handling IndexOutOfRange exception and the previous boiler cannot produce less than 1 MW
                        if (counter != 0 && boilerProductions[counter - 1].HeatProduced - decreaseFromPreviousBoiler >=
                            1)
                        {
                            // Decreasing attributes from and according to previous boiler
                            boilerProductions[counter - 1].HeatProduced -= decreaseFromPreviousBoiler;
                            totalProductionCost -= sortedBoilers.ToList()[counter - 1].ProductionCost *
                                                   decreaseFromPreviousBoiler;
                            totalEmissions -= sortedBoilers.ToList()[counter - 1].Emissions *
                                              decreaseFromPreviousBoiler;
                        }

                        // Calculate all attributes accordingly 
                        totalProductionCost += boiler.ProductionCost;
                        totalEmissions += boiler.Emissions;
                        // HeatDemand = 0.5 ->                         demand.HeatDemand = 0;
                        boilerProductions.Add(new BoilerProduction
                        {
                            BoilerName = boiler.Name,
                            HeatProduced = 1
                        });
                        Console.WriteLine(
                            $"Turning on {boiler.Name} to produce {demand.HeatDemand} MW of heat with effective cost {boiler.ProductionCost} and effective emissions {boiler.Emissions}.");
                        demand.HeatDemand = 0;
                        counter++;
                        break;
                    }

                    // Calculate all attributes accordingly
                    totalProductionCost += demand.HeatDemand * boiler.ProductionCost;
                    totalEmissions += demand.HeatDemand * boiler.Emissions;
                    demand.HeatDemand = Math.Round(demand.HeatDemand, 2);
                    boilerProductions.Add(new BoilerProduction
                    {
                        BoilerName = boiler.Name,
                        HeatProduced = demand.HeatDemand
                    });
                    Console.WriteLine(
                        $"Turning on {boiler.Name} to produce {demand.HeatDemand} MW of heat with effective cost {boiler.ProductionCost} and effective emissions {boiler.Emissions}.");
                    demand.HeatDemand = 0;
                    counter++;
                    break;
                }

                // Calculate all attributes accordingly
                totalProductionCost += boiler.ProductionCost * boiler.MaxHeat;
                totalEmissions += boiler.Emissions * boiler.MaxHeat;
                demand.HeatDemand -= boiler.MaxHeat;
                boilerProductions.Add(new BoilerProduction
                {
                    BoilerName = boiler.Name,
                    HeatProduced = boiler.MaxHeat
                });
                counter++;
                Console.WriteLine(
                    $"Turning on {boiler.Name} to produce {boiler.MaxHeat} MW of heat with effective cost {boiler.ProductionCost} and effective emissions {boiler.Emissions}..");
            }

            if (demand.HeatDemand > 0)
            {
                // Handle insufficient capacity
                Console.WriteLine(
                    $"Insufficient capacity to meet the demand for interval {demand.StartTime} to {demand.EndTime}");
            }

            Console.WriteLine(
                $"Total production cost  and total Emissions for interval {demand.StartTime} to {demand.EndTime}: {totalProductionCost}, {totalEmissions}");
            Console.WriteLine();

            ResetProductionCosts(demand); // Reset production costs for next iteration

            // Final calculated Time slice added to OptimizedData List
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

    // Calculates production cost for the EK and GM (price of electricity is always changing)
    private void CalculateProductionCosts(SourceData demand)
    {
        foreach (var boiler in _boilers)
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

    // Resets production cost for the EK and GM 
    private void ResetProductionCosts(SourceData demand)
    {
        foreach (var boiler in _boilers)
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