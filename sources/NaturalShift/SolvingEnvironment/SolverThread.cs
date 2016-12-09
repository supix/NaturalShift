// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using System.Threading;

namespace NaturalShift.SolvingEnvironment
{
    internal class SolverThread
    {
        private readonly SimpleSolvingEnvironment solvingEnvironment;
        private Thread thread;
        public ISolution bestSolution;

        public bool IsActive { get { return thread != null; } }
        public SimpleSolvingEnvironment SolvingEnvironment { get { return solvingEnvironment; } }
        public ISolution BestSolution { get { return this.bestSolution; } }

        internal SolverThread(Problem problem, EnvironmentConfig environmentConfig)
        {
            this.solvingEnvironment = new SimpleSolvingEnvironment(problem, environmentConfig, false);
            this.solvingEnvironment.OnFitnessImprovement += SolvingEnvironment_OnFitnessImprovement;
            this.thread = null;
        }

        private void SolvingEnvironment_OnFitnessImprovement(double bestFitness, double averageFitness)
        {
            OnFitnessImprovement?.Invoke(bestFitness, averageFitness);
        }

        internal void Solve()
        {
            thread = new Thread(new ThreadStart(DoIt));
            thread.Start();
        }

        internal void DoIt()
        {
            this.bestSolution = solvingEnvironment.Solve();
            this.thread = null;
        }

        public delegate void FitnessImprovement(double bestFitness, double averageFitness);

        public event FitnessImprovement OnFitnessImprovement;
    }
}