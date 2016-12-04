namespace NaturalShift.SolvingEnvironment
{
    public interface IConfigurableSolvingEnvironment
    {
        /// <summary>
        /// Build the solving environment
        /// </summary>
        /// <returns>The configured solving environment</returns>
        ISolvingEnvironment Build();

        /// <summary>
        /// Close current population and start a new one after a number of epochs
        /// </summary>
        /// <param name="epochs">Maximum number of epochs</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment RenewingPopulationAfterEpochs(int epochs);

        /// <summary>
        /// Close current population and start a new one after a consecutive epochs without fitness improvement
        /// </summary>
        /// <param name="epochs">Maximum number of epochs</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment RenewingPopulationAfterSameFitnessEpochs(int epochs);

        /// <summary>
        /// Start configuring duration of computation
        /// </summary>
        /// <param name="after">Elapsed time</param>
        /// <returns>The fluent interface</returns>
        IConfiguringTime StoppingComputationAfter(int after);

        /// <summary>
        /// Start configuring multithreading
        /// </summary>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment UsingExactlyANumberOfThreadsEqualTo(int threads);

        /// <summary>
        /// Set population size
        /// </summary>
        /// <param name="size">Number of individuals</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment WithPopulationSize(int size);
    }
}