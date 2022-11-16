namespace Full_GRASP_And_SOLID
{
    // Se aplica ISP
    public class Timed : TimerClient
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