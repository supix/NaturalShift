﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="supix">
//
// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
//
// This file is part of NaturalShift.
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// NaturalShift is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System;
using NaturalShift.Model.ProblemModel.FluentInterfaces;
using NaturalShift.SolvingEnvironment;

namespace ProblemExamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            //log4net.Config.XmlConfigurator.Configure();

            var problem = ProblemBuilder.Configure()
                .WithDays(30)
                .WithSlots(14)
                .WithItems(18)
                .WithMaxConsecutiveWorkingDaysEqualTo(5)
                .RestAfterMaxWorkingDaysReached(2)
                //.Making.Item(0).UnavailableForSlots().From(10).To(13).Always()
                //.Making.Items().From(0).To(4)
                //    .UnavailableForSlots().From(10).To(13)
                //    .InDays().From(3).To(6)
                //.AssigningAptitude(5).ToItem(0).ForSlots().From(0).To(4).Always()
                .AssigningLength(2).ToSlots().From(10).To(13)
                //.AssigningWeight(1 / 7 * 10).ToSlots().From(10).To(13)
                //.AssigningValue(4).ToSlots().From(0).To(3)
                //.AssigningValue(1).ToSlot(4)
                //.AssigningValue(4).ToSlots().From(5).To(8)
                //.AssigningValue(0.25F).ToSlot(9)
                //.AssigningValue(10).ToSlots().From(10).To(13)
                //.Multiplying.AptitudeBy(0.05F).WhenSlots().From(5).To(9).IsFollowedBySlots().From(0).To(4)
                //.Multiplying.AptitudeBy(1.5F).WhenSlots().From(10).To(13).IsFollowedBySlots().From(0).To(4)
                //.Making.AllItems().UnavailableForAllSlots().InDay(10)
                //.AssigningAptitude(0).ToItem(1).ForSlots().From(6).To(12).InDays().From(14).To(29)
                //.AssigningAptitude(0).ToItem(2).ForSlots().From(7).To(13).InDays().From(0).To(19)
                //.AssigningAptitude(0).ToItem(3).ForSlots().From(8).To(12).InDays().From(5).To(20)
                //.AssigningAptitude(0).ToItem(4).ForSlots().From(9).To(11).InDays().From(14).To(20)
                //.AssigningValue(2).ToSlot(2)
                //.AssigningValue(2).ToSlot(6)
                //.AssigningWeight(2.1F).ToSlot(7)
                //.AssigningWeight(2.5F).ToSlot(8)
                .Closing.Slots().From(8).To(9).InDays().From(5).To(6)
                .Closing.Slots().From(8).To(9).InDays().From(12).To(13)
                .Closing.Slots().From(8).To(9).InDays().From(19).To(20)
                .Closing.Slots().From(8).To(9).InDays().From(26).To(27)
                //.Closing.AllSlots().InDay(4)
                //.ConsideringThat.WhenItem(2).CoversSlot(3).AptitudeIsMultipliedBy(2).ForItem(3).CoveringSlot(5)
                //.ConsideringThat.WhenItem(3).CoversSlot(4).AptitudeIsMultipliedBy(3).ForItem(4).CoveringSlot(4)
                //.ConsideringThat.WhenItem(4).CoversSlot(5).AptitudeIsMultipliedBy(3).ForItem(5).CoveringSlot(3)
                //.ConsideringThat.WhenItem(5).CoversSlot(6).AptitudeIsMultipliedBy(3).ForItem(6).CoveringSlot(2)
                //.ConsideringThat.WhenItem(6).CoversSlot(7).AptitudeIsMultipliedBy(2).ForItem(7).CoveringSlot(1)
                //.ConsideringThat.WhenItem(7).CoversSlot(8).AptitudeIsMultipliedBy(2).ForItem(8).CoveringSlot(0)
                //.Making.Items().From(4).To(10).UnavailableForSlots().From(2).To(6).InDays().From(2).To(25)
                .Build();

            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .WithPopulationSize(100)
                //.RenewingPopulationAfterEpochs(0)
                .RenewingPopulationAfterSameFitnessEpochs(10)
                .StoppingComputationAfter(2).Minutes
                //.UsingExactlyANumberOfThreadsEqualTo(4)
                //.UsingExactlyANumberOfThreadsEqualTo(1)
                .Build();

            var solution = solvingEnvironment.Solve();

            Console.WriteLine(solution);
            Console.ReadLine();
        }
    }
}