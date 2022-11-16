//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }

        public int TotalTime { get; set; }

        public bool Cooked { get; private set; } = false;

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        // Agregado por Expert
        public int GetCookTime()
        {
            foreach (BaseStep step in steps)
            {
                this.TotalTime += step.Time;
            }
            return this.TotalTime;
        }
        // Se aplica Creator para poder pasar los objetos como parametros al método 
        public void Cook()
        {
            // Con el if me aseguro que la receta se haya cocinado una única vez.
            if (!this.Cooked)
            {
                CountdownTimer timer = new CountdownTimer();
                TimerClient timeClient = new Timed(this);
                timer.Register(this.GetCookTime(), timeClient);
            }
            else
            {
                Console.WriteLine("La receta fue previamente cocinada.");
            }
        }

        private void Finished()
        {
            this.Cooked = true;
        }

        // Aplico ISP 
        private class Timed : TimerClient
        {
            Recipe Recipe { get; set; }
            public Timed(Recipe recipe)
            {
                this.Recipe = recipe;
            }

            public void TimeOut()
            {
                this.Recipe.Finished();
            }
        }
    }
}