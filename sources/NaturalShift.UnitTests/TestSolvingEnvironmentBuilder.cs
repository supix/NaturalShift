﻿//-----------------------------------------------------------------------
// <copyright file="TestSolvingEnvironmentBuilder.cs" company="supix">
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
using Moq;
using NaturalShift.Model.ProblemModel;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestSolvingEnvironmentBuilder
    {
        private Random rnd = new Random();

        [Test]
        public void DefaultNumberOfThreadsIsNull()
        {
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null);

            Assert.That(builder.NumberOfThreads, Is.Null);
        }

        [Test]
        public void DefaultSolvingEnvironmentIsMultiThreaded()
        {
            var problem = new Mock<Problem>().Object;
            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .Build();

            Assert.That(solvingEnvironment, Is.TypeOf<MultiThreadedSolvingEnvironment>());
        }

        [Test]
        [Repeat(100)]
        public void ForcingMoreThanOneThreadBuildsMultiThreadedSolvingEnvironment()
        {
            var problem = new Mock<Problem>().Object;
            var numberOfThreads = rnd.Next(10) + 2;
            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .UsingExactlyANumberOfThreadsEqualTo(numberOfThreads)
                .Build();

            Assert.That(solvingEnvironment, Is.TypeOf<MultiThreadedSolvingEnvironment>());
        }

        [Test]
        public void ForcingOneThreadBuildsSimpleSolvingEnvironment()
        {
            var problem = new Mock<Problem>().Object;
            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .StoppingComputationAfter(1).Minutes
                .UsingExactlyANumberOfThreadsEqualTo(1)
                .Build();

            Assert.That(solvingEnvironment, Is.TypeOf<SimpleSolvingEnvironment>());
        }

        [Test]
        [Repeat(100)]
        public void MaxComputationTimeInMillisecondsIsCorrectlyConfigured()
        {
            var millisecs = rnd.Next(60000);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .StoppingComputationAfter(millisecs).Milliseconds;

            Assert.That(builder.EnvironmentConfig.MaxExecutionTimeMilliseconds, Is.EqualTo(millisecs));
        }

        [Test]
        [Repeat(100)]
        public void MaxComputationTimeInMinutesIsCorrectlyConfigured()
        {
            var minutes = rnd.Next(100);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .StoppingComputationAfter(minutes).Minutes;

            Assert.That(builder.EnvironmentConfig.MaxExecutionTimeMilliseconds, Is.EqualTo(minutes * 1000 * 60));
        }

        [Test]
        [Repeat(100)]
        public void MaxComputationTimeInSecondsIsCorrectlyConfigured()
        {
            var secs = rnd.Next(60);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .StoppingComputationAfter(secs).Seconds;

            Assert.That(builder.EnvironmentConfig.MaxExecutionTimeMilliseconds, Is.EqualTo(secs * 1000));
        }

        [Test]
        [Repeat(10)]
        public void MaxComputationTimeInHoursIsCorrectlyConfigured()
        {
            var hours = rnd.Next(100);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .StoppingComputationAfter(hours).Hours;

            Assert.That(builder.EnvironmentConfig.MaxExecutionTimeMilliseconds, Is.EqualTo(hours * 60 * 60 * 1000));
        }

        [Test]
        [Repeat(100)]
        public void MaxEpochsAreCorrectlyConfigured()
        {
            var epochs = rnd.Next(1000);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .RenewingPopulationAfterEpochs(epochs);

            Assert.That(builder.EnvironmentConfig.MaxEpochs, Is.EqualTo(epochs));
        }

        [Test]
        [Repeat(100)]
        public void MaxEpochsWithoutFitnessImprovementAreCorrectlyConfigured()
        {
            var epochs = rnd.Next(1000);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .RenewingPopulationAfterSameFitnessEpochs(epochs);

            Assert.That(builder.EnvironmentConfig.MaxEpochsWithoutFitnessImprovement, Is.EqualTo(epochs));
        }

        [Test]
        [Repeat(10)]
        public void NumberOfThreadsIsCorrectlyConfigured()
        {
            var threads = rnd.Next(20) + 2;
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .UsingExactlyANumberOfThreadsEqualTo(threads);

            Assert.That(builder.NumberOfThreads, Is.EqualTo(threads));
        }

        [Test]
        [Repeat(100)]
        public void PopulationSizeIsCorrectlyConfigured()
        {
            var population = rnd.Next(500);
            var builder = (SolvingEnvironmentBuilder)SolvingEnvironmentBuilder.Configure()
                .ForProblem(null)
                .WithPopulationSize(population);

            Assert.That(builder.EnvironmentConfig.PopulationSize, Is.EqualTo(population));
        }
    }
}