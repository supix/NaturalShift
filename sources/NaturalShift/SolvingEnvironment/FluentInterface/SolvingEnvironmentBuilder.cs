using NaturalShift.Model.ProblemModel;
using System;

namespace NaturalShift.SolvingEnvironment
{
    public class SolvingEnvironmentBuilder :
        IConfigurableSolvingEnvironment,
        ISolvingEnvironmentWithoutProblem,
        IConfiguringTime
    {
        private Problem problem;
        private EnvironmentConfig environmentConfig;
        private int elapsed;
        private int? numberOfThreads;

        IConfigurableSolvingEnvironment IConfiguringTime.Milliseconds
        {
            get
            {
                this.environmentConfig.MaxExecutionTimeMilliseconds = elapsed;

                return this;
            }
        }

        IConfigurableSolvingEnvironment IConfiguringTime.Seconds
        {
            get
            {
                this.environmentConfig.MaxExecutionTimeMilliseconds = (int)new TimeSpan(0, 0, elapsed).TotalMilliseconds;

                return this;
            }
        }

        IConfigurableSolvingEnvironment IConfiguringTime.Minutes
        {
            get
            {
                this.environmentConfig.MaxExecutionTimeMilliseconds = (int)new TimeSpan(0, elapsed, 0).TotalMilliseconds;

                return this;
            }
        }

        IConfigurableSolvingEnvironment IConfiguringTime.Hours
        {
            get
            {
                this.environmentConfig.MaxExecutionTimeMilliseconds = (int)new TimeSpan(elapsed, 0, 0).TotalMilliseconds;

                return this;
            }
        }

        private SolvingEnvironmentBuilder()
        {
            this.environmentConfig = new EnvironmentConfig();
        }

        public static ISolvingEnvironmentWithoutProblem Configure()
        {
            return new SolvingEnvironmentBuilder();
        }

        IConfigurableSolvingEnvironment ISolvingEnvironmentWithoutProblem.ForProblem(Problem problem)
        {
            this.problem = problem;

            return this;
        }

        IConfigurableSolvingEnvironment IConfigurableSolvingEnvironment.WithPopulationSize(int size)
        {
            this.environmentConfig.PopulationSize = size;

            return this;
        }

        IConfigurableSolvingEnvironment IConfigurableSolvingEnvironment.RenewingPopulationAfterEpochs(int epochs)
        {
            this.environmentConfig.MaxEpochs = epochs;

            return this;
        }

        IConfigurableSolvingEnvironment IConfigurableSolvingEnvironment.RenewingPopulationAfterSameFitnessEpochs(int epochs)
        {
            this.environmentConfig.MaxEpochsWithoutFitnessImprovement = epochs;

            return this;
        }

        IConfiguringTime IConfigurableSolvingEnvironment.StoppingComputationAfter(int after)
        {
            this.elapsed = after;

            return this;
        }

        IConfigurableSolvingEnvironment IConfigurableSolvingEnvironment.UsingExactlyANumberOfThreadsEqualTo(int threads)
        {
            if (threads < 1)
                throw new ArgumentException("Number of threads must be greater than 0");

            this.numberOfThreads = threads;

            return this;
        }

        ISolvingEnvironment IConfigurableSolvingEnvironment.Build()
        {
            if (!this.numberOfThreads.HasValue)
                return new MultiThreadedSolvingEnvironment(this.problem, this.environmentConfig);
            else if (this.numberOfThreads.Value == 1)
                return new SimpleSolvingEnvironment(this.problem, this.environmentConfig);
            else
                return new MultiThreadedSolvingEnvironment(this.problem, this.environmentConfig, this.numberOfThreads.Value);
        }

        internal EnvironmentConfig EnvironmentConfig { get { return environmentConfig; } }
        internal int? NumberOfThreads { get { return numberOfThreads; } }
    }
}